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

    private string script = string.Empty;

    private string FilePath = string.Empty;
    private string QValFile = string.Empty;
    //    private Dictionary<List<string>, List<int>> StateAction = new Dictionary<List<string>, List<int>>();

    private Dictionary<string, Dictionary<Vector3, string>> Definitions = new Dictionary<string, Dictionary<Vector3, string>>();
    private string GoalRewardStr = string.Empty;
    private double GOALREWARD;
    private string HitPenaltyStr = string.Empty;
    private string OneStepPenaltyStr = string.Empty;
    private string EpsilonStr = string.Empty;
    private double EPSILON;
    private string GammaStr = string.Empty;
    private double GAMMA;
    private string BetaStr = string.Empty;
    private double BETA;

    private int episode;

    private Vector3 startPos = new Vector3();
    private Vector3 endPos = new Vector3();
    private float distance = 0f;

    private bool iswalking = false;
    private bool collided = false;
    public static bool isFinishing = false;

    //    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ExecutePanel.Execute)
        {
            ExecutePanel.Execute = false;
            ExecutePanel.isRunning = true;
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

        GOALREWARD = scriptScope.GetVariable<double>(GoalRewardStr);
        EPSILON = scriptScope.GetVariable<double>(EpsilonStr);
        GAMMA = scriptScope.GetVariable<double>(GammaStr);
        BETA = scriptScope.GetVariable<double>(BetaStr);

        episode = 0;

        StartQLearning();

        //WalkingTheRobot();
    }

    private void SetDefinitions()
    {
        foreach (KeyValuePair<Vector3, string> pair in Definitions["ゴールにたどり着いた時の報酬："])
        {
            GoalRewardStr = pair.Value;
        }
        foreach (KeyValuePair<Vector3, string> pair in Definitions["壁にぶつかった時の報酬："])
        {
            HitPenaltyStr = pair.Value;
        }
        foreach (KeyValuePair<Vector3, string> pair in Definitions["壁にぶつからなかった時の報酬："])
        {
            OneStepPenaltyStr = pair.Value;
        }
        foreach (KeyValuePair<Vector3, string> pair in Definitions["Epsilon-greedy法のパラメータ："])
        {
            EpsilonStr = pair.Value;
        }
        foreach (KeyValuePair<Vector3, string> pair in Definitions["割引率："])
        {
            GammaStr = pair.Value;
        }
        foreach (KeyValuePair<Vector3, string> pair in Definitions["学習率："])
        {
            BetaStr = pair.Value;
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
