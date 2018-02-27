using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class ControllerOfChap9 : MonoBehaviour {

    private ScriptEngine scriptEngine;       // スクリプト実行用
    private ScriptScope scriptScope;        // スクリプトに値を渡す
    private ScriptSource scriptSource;       // スクリプトのソースを指定する

    public GameObject robot;
    public GameSettings gs;
    private ModeratorOfChap9 moderator;

    private string script = string.Empty;
    public string robotState = string.Empty;

    private string FilePath = string.Empty;
    private string PythonLibPath = string.Empty;

    private IronPython.Runtime.List WallsState = new IronPython.Runtime.List();
    public IronPython.Runtime.List PRTCL = new IronPython.Runtime.List();

    private Vector3 startPos = new Vector3();
    private Vector3 endPos = new Vector3();
    private Vector3 prevPos = new Vector3();

    public bool iswalking = false;
    private bool collided = false;
    public bool isStopping = false;
    public bool Collision = false;

    private bool canInput = false;
    public bool canViewProb = false;

    private int action = -1;

    // Use this for initialization
    void Start () {
        moderator = FindObjectOfType<ModeratorOfChap9>();
        WallsState = gs.WALLS;
    }

    // Update is called once per frame
    void Update () {
        if (moderator.isExecute)
        {
            Debug.Log("Execute");
            moderator.isExecute = false;
            isStopping = false;

            startPos = robot.transform.position;
            prevPos = robot.transform.position;

            PythonLibPath = moderator.SetPythonLibPath();
            FilePath = moderator.SetPythonFilePath();

            robotState = "INITIAL";
            ExecutePythonSouce();
        }

        if (canInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                action = 0;
                canInput = false;
                SetEndPosition(action);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                action = 1;
                canInput = false;
                SetEndPosition(action);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                action = 2;
                canInput = false;
                SetEndPosition(action);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                action = 3;
                canInput = false;
                SetEndPosition(action);
            }
        }

        if (iswalking)
        {
            if (robot.transform.position == endPos)
            {
                iswalking = false;
                StartCoroutine(FinishWalking(0.5f));
            }
            robot.transform.position = Vector3.MoveTowards(robot.transform.position, endPos, Time.deltaTime * 2.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wall(Clone)")
        {
            Debug.Log("Collision");
            Collision = true;
            iswalking = false;
            StartCoroutine(FinishWalking(0.5f));
        }
    }


    private void ExecutePythonSouce()
    {
        string WallList = GetWallStatus(robot.transform.position);

        using (StreamReader sr = new StreamReader(FilePath, System.Text.Encoding.UTF8))
        {
            script = sr.ReadToEnd();
        }
        scriptEngine = Python.CreateEngine();                               // Pythonスクリプト実行エンジン
        scriptScope = scriptEngine.CreateScope();                           // 実行エンジンに渡す値を設定する
        scriptSource = scriptEngine.CreateScriptSourceFromString(script);   // Pythonのソースを設定

        scriptScope.SetVariable("PYTHON_LIB_PATH", PythonLibPath);
        scriptScope.SetVariable("ROBOTSTATE", robotState);
        scriptScope.SetVariable("PRTCL", PRTCL);
        scriptScope.SetVariable("ACTION", action);
        scriptScope.SetVariable("WALL", WallList);
        scriptScope.SetVariable("tmpWALLS", WallsState);

        scriptSource.Execute(scriptScope);      // ソースを実行する

        PRTCL = scriptScope.GetVariable<IronPython.Runtime.List>("PRTCL");

        canInput = true;
        canViewProb = true;
    }

    private IEnumerator FinishWalking(float waitTime)
    {
        if (isStopping) { yield break; }
        yield return new WaitForSeconds(waitTime);

        prevPos = startPos;
        startPos = robot.transform.position;

        robotState = "FROMTHESECONDTIME";
        ExecutePythonSouce();
    }

    private void SetEndPosition(int action)
    {
        robotState = "WALKING";
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

    private string GetWallStatus(Vector3 pos)
    {
        int state = moderator.SetMazeState(pos);
        string walls = WallsState[state].ToString();

        return walls;
    }

    public void StopController()
    {
        Debug.Log("Stop");

        iswalking = false;
        collided = false;

    }

}
