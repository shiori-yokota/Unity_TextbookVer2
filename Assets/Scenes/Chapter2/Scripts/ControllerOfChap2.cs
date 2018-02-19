using UnityEngine;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class ControllerOfChap2 : MonoBehaviour {

    private ScriptEngine    scriptEngine;       // スクリプト実行用
    private ScriptScope     scriptScope;        // スクリプトに値を渡す
    private ScriptSource    scriptSource;       // スクリプトのソースを指定する

    public GameObject robot;

    private string script = string.Empty;
    
    private Dictionary<List<string>, List<int>> StateAction = new Dictionary<List<string>, List<int>>();
    private Vector3 startPos = new Vector3();
    private string FilePath = string.Empty;

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


            FilePath = FindObjectOfType<ExecutePanel>().GetExecuteFilePath();
            StartPythonSouce(FilePath);
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

        // IronPythonで実装されているリスト

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

}
