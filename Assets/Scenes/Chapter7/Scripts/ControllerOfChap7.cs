using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class ControllerOfChap7 : MonoBehaviour {

    private ScriptEngine scriptEngine;       // スクリプト実行用
    private ScriptScope scriptScope;        // スクリプトに値を渡す
    private ScriptSource scriptSource;       // スクリプトのソースを指定する

    public GameObject robot;
    public GameSettings gs;
    private ModeratorOfChap7 moderator;
    
    private string script = string.Empty;

    private string FilePath = string.Empty;
    private string PythonLibPath = string.Empty;
    private Dictionary<string, Dictionary<Vector3, double>> Definitions = new Dictionary<string, Dictionary<Vector3, double>>();
    private int EPISODE;
    private double REWARD;
    
    //private int episodeCount = 0;
    //private int stepCount = 0;
    private string robotState = string.Empty;
    
    private Vector3 startPos = new Vector3();
    private Vector3 endPos = new Vector3();

    private bool iswalking = false;
    private bool collided = false;
    public  bool isFinishing = false;
    public  bool isStopping = false;
    public  bool Collision = false;

    //    // Use this for initialization
    void Start()
    {
        moderator = FindObjectOfType<ModeratorOfChap7>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moderator.isExecute)
        {
            Debug.Log("Execute");
            moderator.isExecute = false;
            isStopping = false;

            startPos = robot.transform.position;

            Definitions = moderator.GetDefinition();
            SetDefinitions();

            PythonLibPath = moderator.SetPythonLibPath();
            FilePath = moderator.SetPythonFilePath();
            robotState = "INITIAL";

            //episodeCount = 0;
            ExecutePythonSouce(FilePath);
        }
        if (iswalking)
        {
            if (robot.transform.position == endPos)
            {
                iswalking = false;
                FinishWalking();
            }
            robot.transform.position = Vector3.MoveTowards(robot.transform.position, endPos, Time.deltaTime * 2.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wall(Clone)")
        {
            Collision = true;
            iswalking = false;
            FinishWalking();
        }
    }


    private void ExecutePythonSouce(string filePath)
    {
        int mazeState = moderator.SetMazeState();

        using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
        {
            script = sr.ReadToEnd();
        }
        scriptEngine = Python.CreateEngine();                               // Pythonスクリプト実行エンジン
        scriptScope = scriptEngine.CreateScope();                           // 実行エンジンに渡す値を設定する
        scriptSource = scriptEngine.CreateScriptSourceFromString(script);   // Pythonのソースを設定

        scriptScope.SetVariable("PYTHON_LIB_PATH", PythonLibPath);
        scriptScope.SetVariable("ROBOTSTATE", robotState);
        scriptScope.SetVariable("MAZESTATE", mazeState);

        scriptSource.Execute(scriptScope);      // ソースを実行する

        //var message = scriptScope.GetVariable<bool>("MESSAGE");
        //Debug.Log(message);

        var action = scriptScope.GetVariable<int>("ACTION");
        Debug.Log(action);

        SetEndPosition(action);
    }

    private void SetDefinitions()
    {
        foreach (KeyValuePair<Vector3, double> pair in Definitions["エピソード"])
        {
            EPISODE = Convert.ToInt32(pair.Value);
        }
    }

    //private void WalkingTheRobot(int action)
    //{
    //    //stepCount += 1;

    //    if (stateList.Count > 1)
    //    {
    //        List<string> NowAndNext = new List<string> { stateList[0], stateList[1] };
    //        actionList = getActionNum(NowAndNext);
    //        SetEndPosition();
    //        stateList.RemoveAt(0);
    //    }
    //    else isFinishing = true;
    //}

    private void SetEndPosition(int action)
    {
        robotState = "WALKING";
        Debug.Log(robotState);
        if (action == 0)
        {
            endPos = new Vector3(startPos.x, startPos.y, startPos.z + 2f);
            iswalking = true;
        }
        else if (action == 1)
        {
            endPos = new Vector3(startPos.x + 2f, startPos.y, startPos.z);
            iswalking = true;
        }
        else if (action == 2)
        {
            endPos = new Vector3(startPos.x, startPos.y, startPos.z - 2f);
            iswalking = true;
        }
        else if (action == 3)
        {
            endPos = new Vector3(startPos.x - 2f, startPos.y, startPos.z);
            iswalking = true;
        }
        else
            iswalking = false;
    }

    private void FinishWalking()
    {
        startPos = robot.transform.position;
        Debug.Log("FINISH walking");
        Debug.Log(moderator.GameMode);

        if (moderator.GameMode == "学習フェーズ")
        {
            REWARD = moderator.SetReward();
            Debug.Log(REWARD);
            robotState = "GETREWARD";
        }
        else if (moderator.GameMode == "行動フェーズ")
        {

        }

    }

    public void StopController()
    {
        Debug.Log("Stop");

        iswalking = false;
        isFinishing = false;
        collided = false;
        isStopping = false;

        //episodeCount = 0;
        robotState = string.Empty;

    }

}
