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

    private string script = string.Empty;
    
    private Dictionary<List<string>, List<int>> StateAction = new Dictionary<List<string>, List<int>>();
    private Dictionary<string, Dictionary<Vector3, string>> Definitions = new Dictionary<string, Dictionary<Vector3, string>>();
    private List<int> actionList = new List<int>();
    private List<string> stateList = new List<string>();
    private Vector3 startPos = new Vector3();
    private Vector3 endPos = new Vector3();
    private string FilePath = string.Empty;
    private bool iswalking = false;
    private float distance = 0f;
    private string OpenList = string.Empty;
    private string ClosedList = string.Empty;
    public static bool isFinishing = false;

    // Use this for initialization
    void Start () {
        SetStateAction();
	}
	
	// Update is called once per frame
	void Update () {
		if (ExecutePanel.Execute)
        {
            ExecutePanel.Execute = false;
            ExecutePanel.isRunning = true;
            startPos = robot.transform.position;

            Definitions = FindObjectOfType<ModeratorOfChap3>().GetDefinition();
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
                SetEndPosition();
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

        var Result = scriptScope.GetVariable<IronPython.Runtime.List>(ClosedList);
        stateList = Result.Cast<string>().ToList();
        WalkingTheRobot();
    }

    private void SetStateAction()
    {
        StateAction.Add(new List<string> { "S"  , "S3"   }, new List<int> { 1, 2 });
        StateAction.Add(new List<string> { "S3" , "S4"   }, new List<int> { 1, 1, 1 });
        StateAction.Add(new List<string> { "S4" , "S1"   }, new List<int> { 0, 3, 3 });
        StateAction.Add(new List<string> { "S4" , "S6"   }, new List<int> { 2, 1 });
        StateAction.Add(new List<string> { "S6" , "S2"   }, new List<int> { 0, 0 });
        StateAction.Add(new List<string> { "S6" , "G"    }, new List<int> { 2, 3, 2, 1, 1 });
        StateAction.Add(new List<string> { "S3" , "S7"   }, new List<int> { 2, 2, 1 });
        StateAction.Add(new List<string> { "S7" , "S8"   }, new List<int> { 1 });
        StateAction.Add(new List<string> { "S7" , "S9"   }, new List<int> { 2, 3 });
        StateAction.Add(new List<string> { "S8" , "S5"   }, new List<int> { 0, 3 });
        StateAction.Add(new List<string> { "S8" , "S10"  }, new List<int> { 2 });

        StateAction.Add(new List<string> { "S3" , "S"    }, new List<int> { 0, 3 });
        StateAction.Add(new List<string> { "S4" , "S3"   }, new List<int> { 3, 3, 3 });
        StateAction.Add(new List<string> { "S1" , "S4"   }, new List<int> { 1, 1, 2 });
        StateAction.Add(new List<string> { "S6" , "S4"   }, new List<int> { 3, 0 });
        StateAction.Add(new List<string> { "S2" , "S6"   }, new List<int> { 2, 2 });
        StateAction.Add(new List<string> { "G"  , "S6"   }, new List<int> { 3, 3, 0, 1, 0 });
        StateAction.Add(new List<string> { "S7" , "S3"   }, new List<int> { 3, 0, 0 });
        StateAction.Add(new List<string> { "S8" , "S7"   }, new List<int> { 3 });
        StateAction.Add(new List<string> { "S9" , "S7"   }, new List<int> { 1, 0 });
        StateAction.Add(new List<string> { "S5" , "S8"   }, new List<int> { 1, 2 });
        StateAction.Add(new List<string> { "S10", "S8"   }, new List<int> { 0 });
    }

    private void SetDefinitions()
    {
        foreach(KeyValuePair<Vector3, string> pair in Definitions["オープンリスト："])
        {
            OpenList = pair.Value;
        }
        foreach (KeyValuePair<Vector3, string> pair in Definitions["クローズドリスト："])
        {
            ClosedList = pair.Value;
        }
    }

    private void WalkingTheRobot()
    {
        if (stateList.Count > 1)
        {
            List<string> NowAndNext = new List<string> { stateList[0], stateList[1] };
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
                act = StateAction[key];
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
}
