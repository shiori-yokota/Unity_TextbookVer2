using UnityEngine;
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
    private ExecutePanel ep;

    private string script = string.Empty;

    private string FilePath = string.Empty;
    //    private Dictionary<List<string>, List<int>> StateAction = new Dictionary<List<string>, List<int>>();

    private Dictionary<string, Dictionary<Vector3, double>> Definitions = new Dictionary<string, Dictionary<Vector3, double>>();
    private int EPISODE;
    private double GOALREWARD;
    private double HitPENALTY;
    private double ONESTEPPENALTY;
    private double EPSILON;
    private double GAMMA;
    private double BETA;

    private int episode;

    private Vector3 startPos = new Vector3();
    private Vector3 endPos = new Vector3();
    private float distance = 0f;

    private bool iswalking = false;
    private bool collided = false;
    public  bool isFinishing = false;

    //    // Use this for initialization
    void Start()
    {
        ep = FindObjectOfType<ExecutePanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ep.Execute)
        {
            ep.Execute = false;
            ep.isRunning = true;
            startPos = robot.transform.position;

            Definitions = FindObjectOfType<ModeratorOfChap7>().GetDefinition();
            SetDefinitions();

            FilePath = FindObjectOfType<ExecutePanel>().GetExecuteFilePath();
            StartPythonSouce(FilePath);
        }
        if (iswalking)
        {
            distance += Time.deltaTime * 3.0f;
            robot.transform.position = Vector3.MoveTowards(startPos, endPos, distance);
            if (Vector3.Distance(robot.transform.position, endPos) < 0.1)
            {
                iswalking = false;
                distance = 0f;
                startPos = endPos;
                //SetEndPosition();
            }
        }
    }

    private void StartPythonSouce(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
        {
            script = sr.ReadToEnd();
        }
        scriptEngine = Python.CreateEngine();                               // Pythonスクリプト実行エンジン
        scriptScope = scriptEngine.CreateScope();                           // 実行エンジンに渡す値を設定する
        scriptSource = scriptEngine.CreateScriptSourceFromString(script);   // Pythonのソースを設定

        scriptSource.Execute(scriptScope);      // ソースを実行する

        episode = 0;

        StartQLearning();

        //WalkingTheRobot();
    }

    private void SetDefinitions()
    {
        foreach (KeyValuePair<Vector3, double> pair in Definitions["ゴールにたどり着いた時の報酬："])
        {
            GOALREWARD = pair.Value;
        }
        foreach (KeyValuePair<Vector3, double> pair in Definitions["壁にぶつかった時の報酬："])
        {
            HitPENALTY = pair.Value;
        }
        foreach (KeyValuePair<Vector3, double> pair in Definitions["壁にぶつからなかった時の報酬："])
        {
            ONESTEPPENALTY = pair.Value;
        }
        foreach (KeyValuePair<Vector3, double> pair in Definitions["Epsilon-greedy法のパラメータ："])
        {
            EPSILON = pair.Value;
        }
        foreach (KeyValuePair<Vector3, double> pair in Definitions["割引率："])
        {
            GAMMA = pair.Value;
        }
        foreach (KeyValuePair<Vector3, double> pair in Definitions["学習率："])
        {
            BETA = pair.Value;
        }
    }

    private void StartQLearning()
    {
        episode += 1;

    }

    //    private void WalkingTheRobot()
    //    {
    //        if (stateList.Count > 1)
    //        {
    //            List<string> NowAndNext = new List<string> { stateList[0], stateList[1] };
    //            actionList = getActionNum(NowAndNext);
    //            SetEndPosition();
    //            stateList.RemoveAt(0);
    //        }
    //        else isFinishing = true;
    //    }

    //    private List<int> getActionNum(List<string> name)
    //    {
    //        List<int> act = new List<int>();
    //        int count = 0;
    //        foreach(List<string> key in StateAction.Keys)
    //        {
    //            if (key.SequenceEqual(name))
    //            {
    //                act = StateAction[key];
    //                break;
    //            }
    //            else count++;
    //        }
    //        if (count >= StateAction.Count) act.Add(-1);
    //        return act;
    //    }

    //    private void SetEndPosition()
    //    {
    //        if (actionList.Count > 0)
    //        {
    //            int action = actionList[0];
    //            if (action == 0)
    //            {
    //                endPos = new Vector3(startPos.x, startPos.y, startPos.z + 2f);
    //                iswalking = true;
    //            }
    //            else if (action == 1)
    //            {
    //                endPos = new Vector3(startPos.x + 2f, startPos.y, startPos.z);
    //                iswalking = true;
    //            }
    //            else if (action == 2)
    //            {
    //                endPos = new Vector3(startPos.x, startPos.y, startPos.z - 2f);
    //                iswalking = true;
    //            }
    //            else if (action == 3)
    //            {
    //                endPos = new Vector3(startPos.x - 2f, startPos.y, startPos.z);
    //                iswalking = true;
    //            }
    //            else
    //            {
    //                iswalking = false;
    //            }
    //            actionList.RemoveAt(0);
    //        }
    //        else
    //        {
    //            WalkingTheRobot();
    //        }
    //    }

}
