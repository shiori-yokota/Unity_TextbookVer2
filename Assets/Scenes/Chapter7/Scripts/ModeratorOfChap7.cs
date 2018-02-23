using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeratorOfChap7 : MonoBehaviour {
    
    public  GameObject robot;
    public  GameObject walls;
    public  GameObject prefab;
    public  GameObject state;
    public  GameObject StatePrefab;
    public  GameObject valSetting;
    public  GameObject VariablePrefab;
    public  GameObject ConstantPrefab;
    public  MenuButton mb;

    private ExecutePanel ep;
    private ControllerOfChap7 controller;
    public  GameSettings gs;
    
    private int MazeSize = new int();
    private int GoalCol = new int();
    private int GoalRow = new int();
    private Vector3 reSetPos = new Vector3();
    private Vector3 GoalPos = new Vector3();
    private string QValFile = string.Empty;

    private double GoalReward;
    private double HitPenalty;
    private double OneStepPenalty;

    public string GameMode = string.Empty;

    private List<RectTransform> pythonVals = new List<RectTransform> { };
    private Dictionary<string, Dictionary<Vector3, double>> ParameterList = new Dictionary<string, Dictionary<Vector3, double>>
    {
        {"エピソード", new Dictionary<Vector3, double> { { new Vector3(-300f, 60f, 0f), 10d } } },
        {"ゴールにたどり着いた時の報酬", new Dictionary<Vector3, double> { {new Vector3(300f, 60f, 0f), 100.0d } } },
        {"壁にぶつかった時の報酬", new Dictionary<Vector3, double> { {new Vector3(-300f, -60f, 0f), -10.0d } } },
        {"壁にぶつからなかった時の報酬", new Dictionary<Vector3, double> { {new Vector3(300f, -60f, 0f), -1.0d } } },
    };
    private Dictionary<string, Dictionary<Vector3, int>>ConstantList = new Dictionary<string, Dictionary<Vector3, int>>
    {
        {"ゴール座標タテ", new Dictionary<Vector3, int> { { new Vector3(-300f, 310f, 0f),   5} } },
        {"ゴール座標ヨコ", new Dictionary<Vector3, int> { { new Vector3(300f, 310f, 0f),   5} } },
    };
    private Dictionary<string, Dictionary<Vector3, string>> GameModeList = new Dictionary<string, Dictionary<Vector3, string>>
    {
        {"実行モード", new Dictionary<Vector3, string>{ {new Vector3(-300f, 190f, 0f), "学習フェーズ"} } },
    };

    public bool isExecute = false;
    public bool isRunning = false;

    private bool isCollision = false;
    
    // Use this for initialization
    void Start () {

        ep = FindObjectOfType<ExecutePanel>();
        controller = FindObjectOfType<ControllerOfChap7>();

        GetSettings();          // 設定情報の取得
        InitRobotPosition();    // ロボットの初期位置設定
        SetEnvironment();       // 俯瞰図用カメラ・ライトの位置設定
        SetMazeOuterWall();     // 迷路外壁の設定
        SetMazeInterWall();     // 迷路内壁の設定
        SetVariable();          // 変数の設定

    }
	
	// Update is called once per frame
	void Update () {
		if (mb.clickedSettingButton)
        {
            mb.clickedSettingButton = false;
            GameSettings();
        }

        if(ep.Execute)
        {
            isExecute = true;
            GameSettings();
            InitRobotPosition();
            reSetPos = robot.transform.position;
            ep.Execute = false;
            isRunning = true;
        }

        if (isRunning)
            ep.isRunning = true;
        else
            ep.isRunning = false;

        if (controller.Collision)
        {
            controller.Collision = false;
            isCollision = true;
            ReSetRobotPosition();
        }

        if (controller.isFinishing)
        {
            Debug.Log("Finish!!");
            controller.StopController();

            isRunning = false;
        }

        if (ep.StopOrder)
        {
            ep.StopOrder = false;
            controller.isStopping = true;
            isRunning = false;
            isExecute = false;
            controller.StopController();
        }
	}

    private void GameSettings()
    {
        pythonVals = mb.GetChapterSettings();
        SetGameMode();              // ゲームモードの設定
        SetDefinition();            // 変数の設定
        SetRewardVal();             // 報酬の設定
        SetGoalPos();               // ゴールの設定
        string FILE_NAME = ep.FilePathName + "qvalues.txt";
        if (File.Exists(FILE_NAME))
        {
            File.Delete(FILE_NAME);
        }
        File.Create(FILE_NAME).Close();
    }

    public void InitRobotPosition()
    {
        int row = UnityEngine.Random.Range(0, MazeSize);
        int col = UnityEngine.Random.Range(0, MazeSize);
        
        robot.transform.position = new Vector3((col * 2) + 1, 1, -((row * 2) + 1));
        if (row == GoalRow && col == GoalCol) InitRobotPosition();  // スタート位置とゴール位置が一緒
    }

    private void ReSetRobotPosition()
    {
        robot.transform.position = reSetPos;
    }

    private void GetSettings()
    {
        MazeSize = gs.MazeSize;
    }

    private void SetEnvironment()
    {
        // 俯瞰図用カメラ
        GameObject cam = GameObject.Find("Main Camera");
        cam.transform.position = new Vector3(MazeSize, MazeSize * 2.5f, -(MazeSize - 1));
        // 俯瞰図用ライト
        GameObject light = GameObject.Find("Directional Light");
        light.transform.position = new Vector3(MazeSize, MazeSize * 2f, -MazeSize);
    }

    private void SetMazeOuterWall()
    {
        Quaternion quat = Quaternion.identity;
        GameObject[] OuterWallFabs = new GameObject[MazeSize * 2];

        GameObject[] OuterWalls = new GameObject[MazeSize * 2];
        Array.Copy(OuterWallFabs, 0, OuterWalls, 0, 10);
        
        for (int i = 0; i < OuterWalls.Length / 2; i++)
        {
            OuterWalls[i] = Instantiate(prefab, new Vector3(1f + i * 2f, 1.5f, 0f), quat) as GameObject;
            OuterWalls[i].transform.SetParent(walls.transform);
            OuterWalls[i + 5] = Instantiate(prefab, new Vector3(1f + i * 2f, 1.5f, -10f), quat) as GameObject;
            OuterWalls[i + 5].transform.SetParent(walls.transform);
        }
        
        quat.eulerAngles = new Vector3(0f, 90f, 0f);
        OuterWalls = new GameObject[MazeSize * 2];
        Array.Copy(OuterWallFabs, 0, OuterWalls, 0, 10);

        for (int i = 0; i < OuterWalls.Length / 2; i++)
        {
            OuterWalls[i] = Instantiate(prefab, new Vector3(0f, 1.5f, -1f + i * -2f), quat) as GameObject;
            OuterWalls[i].transform.SetParent(walls.transform);
            OuterWalls[i + 5] = Instantiate(prefab, new Vector3(10f, 1.5f, -1f + i * -2f), quat) as GameObject;
            OuterWalls[i + 5].transform.SetParent(walls.transform);
        }
    }

    private void SetMazeInterWall()
    {
        int index = 8;
        Quaternion quat = Quaternion.identity;
        GameObject[] InnerWallFab = new GameObject[index];
        Vector3[] wallPos = new Vector3[index];

        wallPos[0] = new Vector3(3f, 1.5f, -2);
        wallPos[1] = new Vector3(5f, 1.5f, -2);

        wallPos[2] = new Vector3(3f, 1.5f, -4);
        wallPos[3] = new Vector3(5f, 1.5f, -4);

        wallPos[4] = new Vector3(3f, 1.5f, -6);
        wallPos[5] = new Vector3(7f, 1.5f, -6);

        wallPos[6] = new Vector3(1f, 1.5f, -8);
        wallPos[7] = new Vector3(9f, 1.5f, -8);

        for (int i = 0; i < InnerWallFab.Length; i++)
        {
            InnerWallFab[i] = Instantiate(prefab, wallPos[i], quat) as GameObject;
            InnerWallFab[i].transform.SetParent(walls.transform);
        }

        quat.eulerAngles = new Vector3(0f, 90f, 0f);
        wallPos[0] = new Vector3(2f, 1.5f, -1);
        wallPos[1] = new Vector3(2f, 1.5f, -5);

        wallPos[2] = new Vector3(4f, 1.5f, -9);

        wallPos[3] = new Vector3(6f, 1.5f, -5);
        wallPos[4] = new Vector3(6f, 1.5f, -7);
        wallPos[5] = new Vector3(6f, 1.5f, -9);

        wallPos[6] = new Vector3(8f, 1.5f, -1);
        wallPos[7] = new Vector3(8f, 1.5f, -3);

        for (int i = 0; i < InnerWallFab.Length; i++)
        {
            InnerWallFab[i] = Instantiate(prefab, wallPos[i], quat) as GameObject;
            InnerWallFab[i].transform.SetParent(walls.transform);
        }
    }

    private void SetGoalPos()
    {
        foreach (Vector3 key in ConstantList["ゴール座標タテ"].Keys)
        {
            GoalCol = ConstantList["ゴール座標タテ"][key];
        }
        foreach (Vector3 key in ConstantList["ゴール座標ヨコ"].Keys)
        {
            GoalRow = ConstantList["ゴール座標ヨコ"][key];
        }

        float PosX = GoalCol * 2 - 1;
        float PosZ = -GoalRow * 2 + 1;

        Vector3 pos = new Vector3(PosX, 0f, PosZ);
        Quaternion quat = Quaternion.identity;
        quat.eulerAngles = new Vector3(90f, 0f, 0f);

        GoalPos = pos;

        var obj = MyInstantiate(pos, quat, "G") as GameObject;
    }

    private void SetGameMode()
    {
        foreach(Vector3 key in GameModeList["実行モード"].Keys)
        {
            GameMode = GameModeList["実行モード"][key];
        }
    }

    private GameObject MyInstantiate(Vector3 pos, Quaternion quat, string text)
    {
        GameObject obj = Instantiate(StatePrefab, pos, quat) as GameObject;
        obj.name = text;
        obj.GetComponent<TextMesh>().text = text;
        obj.GetComponent<TextMesh>().fontSize = 45;
        obj.GetComponent<TextMesh>().characterSize = 0.15f;
        obj.transform.SetParent(state.transform);

        return obj;
    }

    private void SetVariable()
    {
        foreach(KeyValuePair<string, Dictionary<Vector3, double>> pair in ParameterList)
        {
            foreach (KeyValuePair<Vector3, double> inner in pair.Value)
            {
                MyValInstantiate(pair.Key, inner.Key, inner.Value);
            }
        }

        foreach(KeyValuePair<string, Dictionary<Vector3, int>> pair in ConstantList)
        {
            foreach (KeyValuePair<Vector3, int> inner in pair.Value)
            {
                MyConstInstantiate(pair.Key, inner.Key);
            }
        }

        foreach(KeyValuePair<string, Dictionary<Vector3, string>> pair in GameModeList)
        {
            foreach(KeyValuePair<Vector3, string> inner in pair.Value)
            {
                MyGameModeInstantiate(pair.Key, inner.Key);
            }
        }
    }

    private void MyValInstantiate(string text, Vector3 localPos, double val)
    {
        GameObject obj = Instantiate(VariablePrefab, localPos, Quaternion.identity);
        obj.transform.SetParent(valSetting.transform, false);
        obj.name = text;
        obj.GetComponent<Text>().text = text;
        foreach (RectTransform rect in obj.transform)
        {
            rect.GetComponent<InputField>().placeholder.GetComponent<Text>().text = val.ToString();
        }

        obj.transform.localPosition = localPos;
        
    }

    private void MyConstInstantiate(string text, Vector3 localPos)
    {
        GameObject obj = Instantiate(ConstantPrefab, localPos, Quaternion.identity);
        obj.transform.SetParent(valSetting.transform, false);
        obj.name = text;
        obj.GetComponent<Text>().text = text;
        obj.transform.localPosition = localPos;
        
        foreach (RectTransform trans in obj.transform)
        {
            Dropdown dropdown = trans.GetComponentInChildren<Dropdown>();
            if (dropdown)
            {
                dropdown.ClearOptions();
                List<string> list = new List<string>();
                list.Add("1");
                list.Add("2");
                list.Add("3");
                list.Add("4");
                list.Add("5");
                dropdown.AddOptions(list);
                dropdown.value = list.Count;
            }
        }
    }

    private void MyGameModeInstantiate(string text, Vector3 localPos)
    {
        GameObject obj = Instantiate(ConstantPrefab, localPos, Quaternion.identity);
        obj.transform.SetParent(valSetting.transform, false);
        obj.name = text;
        obj.GetComponent<Text>().text = text;
        obj.transform.localPosition = localPos;

        foreach(RectTransform rect in obj.transform)
        {
            Dropdown dropdown = rect.GetComponentInChildren<Dropdown>();
            if (dropdown)
            {
                dropdown.ClearOptions();
                List<string> list = new List<string>();
                list.Add("学習フェーズ");
                list.Add("行動フェーズ");
                dropdown.AddOptions(list);
                dropdown.value = 0;
            }
        }
    }

    private void SetDefinition()
    {
        foreach(RectTransform trans in pythonVals)
        {
            if (ParameterList.ContainsKey(trans.name))
            {
                List<Vector3> list = new List<Vector3>(ParameterList[trans.name].Keys);
                foreach(Vector3 key in list)
                {
                    InputField input = trans.GetComponentInChildren<InputField>();
                    if (input.text == string.Empty)
                    {
                        ParameterList[trans.name][key] = double.Parse(input.placeholder.GetComponent<Text>().text);
                    }
                    else
                    {
                        ParameterList[trans.name][key] = double.Parse(input.text);
                    }
                }
            }
            if (ConstantList.ContainsKey(trans.name))
            {
                List<Vector3> list = new List<Vector3>(ConstantList[trans.name].Keys);
                foreach(Vector3 key in list)
                {
                    Dropdown dropdown = trans.GetComponentInChildren<Dropdown>();
                    ConstantList[trans.name][key] = Int32.Parse(dropdown.options[dropdown.value].text);
                }
            }
            if (GameModeList.ContainsKey(trans.name))
            {
                List<Vector3> list = new List<Vector3>(GameModeList[trans.name].Keys);
                foreach(Vector3 key in list)
                {
                    Dropdown dropdown = trans.GetComponentInChildren<Dropdown>();
                    GameModeList[trans.name][key] = dropdown.options[dropdown.value].text;
                }
            }
        }
    }

    private void SetRewardVal()
    {
        foreach (KeyValuePair<Vector3, double> pair in ParameterList["ゴールにたどり着いた時の報酬"])
        {
            GoalReward = pair.Value;
        }
        foreach (KeyValuePair<Vector3, double> pair in ParameterList["壁にぶつかった時の報酬"])
        {
            HitPenalty = pair.Value;
        }
        foreach (KeyValuePair<Vector3, double> pair in ParameterList["壁にぶつからなかった時の報酬"])
        {
            OneStepPenalty = pair.Value;
        }

    }

    public Dictionary<string, Dictionary<Vector3, double>> GetDefinition()
    {
        return ParameterList;
    }

    private void GetProb()
    {
        string[] text = File.ReadAllLines(QValFile);

        List<double> stateVal = new List<double>();
        for (int i = 0; i < text.Length; i++)
        {
            string[] subStrings = text[i].Split(':');
            stateVal.Add(double.Parse(subStrings[1]));
        }

        ViewProb(stateVal);
    }

    private void ViewProb(List<double> stateVal)
    {
        Dictionary<string, Vector3> State = FindObjectOfType<GameSettings>().SetMazeState(stateVal);

        foreach (Transform trans in state.transform)
        {
            if (trans.name != "G") GameObject.Destroy(trans.gameObject);
        }
        foreach (KeyValuePair<string, Vector3> pair in State)
        {
            GameObject obj = Instantiate(StatePrefab, pair.Value, Quaternion.identity) as GameObject;
            obj.name = pair.Key;
            obj.GetComponent<TextMesh>().text = pair.Key;
            obj.GetComponent<TextMesh>().fontSize = 20;
            obj.GetComponent<TextMesh>().characterSize = 0.15f;
            obj.transform.SetParent(state.transform);
        }
    }

    public string SetPythonFilePath()
    {
        string PythonFilePath = ep.GetExecuteFilePath();
        return PythonFilePath;
    }

    public string SetPythonLibPath()
    {
        string PythonLibPath = ep.PythonLibPath;
        return PythonLibPath;
    }

    public int SetMazeState()
    {
        int index;
        
        int col = (Convert.ToInt32(robot.transform.position.x) - 1) / 2;
        int row = (-(Convert.ToInt32(robot.transform.position.z)) - 1) / 2;
        
        if (row == 0)
            index = col;
        else
            index = MazeSize * row + col;

        return index;
    }

    public double SetReward()
    {
        double reward;

        if (robot.transform.position == GoalPos)
            reward = GoalReward;        
        else
        {
            if (isCollision)
            {
                isCollision = false;
                reward = HitPenalty;
            }
            else
                reward = OneStepPenalty;
        }

        return reward;
    }
}
