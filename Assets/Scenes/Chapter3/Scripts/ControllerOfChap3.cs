using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class ControllerOfChap3 : MonoBehaviour {

    private ScriptEngine    scriptEngine;       // スクリプト実行用
    private ScriptScope     scriptScope;        // スクリプトに値を渡す
    private ScriptSource    scriptSource;       // スクリプトのソースを指定する

    public GameObject robot;
    private ModeratorOfChap3 moderator;
    public GameSettings gs;

    private string script = string.Empty;
    
    private Dictionary<List<string>, List<int>> StateAction = new Dictionary<List<string>, List<int>>();
    private Dictionary<string, Dictionary<Vector3, string>> Definitions = new Dictionary<string, Dictionary<Vector3, string>>();
    private List<int> actionList = new List<int>();
    private List<string> stateList = new List<string>();
    private Vector3 startPos = new Vector3();
    private Vector3 endPos = new Vector3();
    private string FilePath = string.Empty;
    private bool iswalking = false;
    private string ClosedList = string.Empty;
    public  bool isFinishing = false;
    public  bool isStopping = false;

    // Use this for initialization
    void Start () {
        moderator = FindObjectOfType<ModeratorOfChap3>();
    }
	
	// Update is called once per frame
	void Update () {
		if (moderator.isExecute)
        {
            Debug.Log("Execute");
            moderator.isExecute = false;
            isStopping = false;

            StateAction = SetStateAction();

            startPos = robot.transform.position;

            Definitions = moderator.GetDefinition();
            SetDefinitions();

            FilePath = moderator.SetPythonFilePath();
            StartPythonSouce(FilePath);
        }
        if (iswalking)
        {
            if (robot.transform.position == endPos)
            {
                iswalking = false;
                startPos = robot.transform.position;
                SetEndPosition();

            }
            robot.transform.position = Vector3.MoveTowards(robot.transform.position, endPos, Time.deltaTime * 3.0f);
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

        var Result = scriptScope.GetVariable<IronPython.Runtime.List>(ClosedList);
        stateList = Result.Cast<string>().ToList();
        WalkingTheRobot();
    }

    private void SetDefinitions()
    {
        ClosedList = string.Empty;
        foreach (KeyValuePair<Vector3, string> pair in Definitions["クローズドリスト："])
        {
            ClosedList = pair.Value;
        }
    }

    private Dictionary<List<string>, List<int>> SetStateAction()
    {
        var tmp = new Dictionary<List<string>, List<int>>(gs.StateAction);

        return tmp;
    }

    private void WalkingTheRobot()
    {
        if (stateList.Count > 1)
        {
            List<string> NowAndNext = new List<string> { stateList[0], stateList[1] };
            Debug.Log("Now:"+ stateList[0] + ", Next:" + stateList[1]);
            actionList = getActionNum(NowAndNext);
            SetEndPosition();
            stateList.RemoveAt(0);
        }
        else isFinishing = true;
    }

    private List<int> getActionNum(List<string> name)
    {
        List<int> act = new List<int>();
        int count = 0;
        foreach(List<string> key in StateAction.Keys)
        {
            if (key.SequenceEqual(name))
            {
                foreach (int i in StateAction[key])
                {
                    act.Add(i);
                }
                break;
            }
            else count++;
        }
        if (count >= StateAction.Count) act.Add(-1);

        return act;
    }

    private void SetEndPosition()
    {
        if (actionList.Count > 0)
        {
            int action = actionList[0];
            if (action == 0)
            {
                endPos = new Vector3(startPos.x, robot.transform.position.y, startPos.z + 2f);
                iswalking = true;
            }
            else if (action == 1)
            {
                endPos = new Vector3(startPos.x + 2f, robot.transform.position.y, startPos.z);
                iswalking = true;
            }
            else if (action == 2)
            {
                endPos = new Vector3(startPos.x, robot.transform.position.y, startPos.z - 2f);
                iswalking = true;
            }
            else if (action == 3)
            {
                endPos = new Vector3(startPos.x - 2f, robot.transform.position.y, startPos.z);
                iswalking = true;
            }
            else
            {
                iswalking = false;
            }
            actionList.RemoveAt(0);
        }
        else
        {
            WalkingTheRobot();
        }
    }

    public void StopController()
    {
        Debug.Log("Stop");

        iswalking = false;
        isFinishing = false;

        stateList = new List<string>();
        actionList = new List<int>();

    }
}
