using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeratorOfChap7 : MonoBehaviour {
    
    private GameObject robot;
    public  GameObject walls;
    public  GameObject prefab;
    public  GameObject state;
    public  GameObject StatePrefab;
    public  GameObject valSetting;
    public  GameObject VariablePrefab;
    public  GameObject ConstantPrefab;

    public MenuButton mb;
    
    private int MazeSize;
    private int GoalCol;
    private int GoalRow;
    private List<RectTransform> pythonVals = new List<RectTransform> { };
    private Dictionary<string, Dictionary<Vector3, string>> VariableList = new Dictionary<string, Dictionary<Vector3, string>>
    {
        {"ゴールにたどり着いた時の報酬：", new Dictionary<Vector3, string> { {new Vector3(-150f, 120f, 0f),   string.Empty} } },
        {"壁にぶつかった時の報酬：", new Dictionary<Vector3, string> { {new Vector3(-150f, 40f, 0f),    string.Empty} } },
        {"壁にぶつからなかった時の報酬：", new Dictionary<Vector3, string> { {new Vector3(-150f, -40f, 0f),    string.Empty} } },
        {"Epsilon-greedy法のパラメータ：", new Dictionary<Vector3, string> { {new Vector3(-150f, -120f, 0f),    string.Empty} } },
        {"割引率：", new Dictionary<Vector3, string> { {new Vector3(-150f, -200f, 0f),    string.Empty} } },
        {"学習率：", new Dictionary<Vector3, string> { {new Vector3(-150f, -280f, 0f),    string.Empty} } },
    };
    private Dictionary<string, Dictionary<Vector3, int>>ConstantList = new Dictionary<string, Dictionary<Vector3, int>>
    {
        {"ゴール座標タテ：", new Dictionary<Vector3, int> { { new Vector3(-150f, 280f, 0f),   GameSettings.Parameters.GoalCol} } },
        {"ゴール座標ヨコ：", new Dictionary<Vector3, int> { { new Vector3(-150f, 200f, 0f),   GameSettings.Parameters.GoalRow} } },
    };


    // Use this for initialization
    void Start () {
        robot = GameObject.Find("Robot");        

        GetSettings();          // 設定情報の取得
        InitRobotPosition();    // ロボットの初期位置設定
        SetEnvironment();       // 俯瞰図用カメラ・ライトの位置設定
        SetMazeOuterWall();     // 迷路外壁の設定
        SetMazeInterWall();     // 迷路内壁の設定
        SetVariable();          // 変数の設定

    }
	
	// Update is called once per frame
	void Update () {
		if (MenuButton.clickedSettingButton)
        {
            MenuButton.clickedSettingButton = false;
            pythonVals = mb.GetChapterSettings();
            SetDefinition();            // 変数の設定
            SetGoalPos();               // ゴールの設定
            string FILE_NAME = Application.dataPath + "/Python/qvalues.txt";
            if (File.Exists(FILE_NAME))
            {
                File.Delete(FILE_NAME);
            }
            File.Create(FILE_NAME).Close();
        }

        if (ControllerOfChap3.isFinishing)
        {
            ControllerOfChap3.isFinishing = false;
            ExecutePanel.isRunning = false;
        }
	}

    public void InitRobotPosition()
    {
        int row = UnityEngine.Random.Range(0, MazeSize);
        int col = UnityEngine.Random.Range(0, MazeSize);

        robot.transform.position = new Vector3((col * 2) + 1, 1, -((row * 2) + 1));
        if (row == GoalRow && col == GoalCol) InitRobotPosition();  // スタート位置とゴール位置が一緒
        else SetRobotPosition(row, col);
    }

    private void SetRobotPosition(int row, int col)
    {
        // ロボットに初期位置を伝える?
    }

    private void GetSettings()
    {
        MazeSize = GameSettings.Parameters.MazeSize;
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
        foreach (Vector3 key in ConstantList["ゴール座標タテ："].Keys)
        {
            GoalCol = ConstantList["ゴール座標タテ："][key];
        }
        foreach (Vector3 key in ConstantList["ゴール座標ヨコ："].Keys)
        {
            GoalRow = ConstantList["ゴール座標ヨコ："][key];
        }

        float PosX = GoalCol * 2 - 1;
        float PosZ = -GoalRow * 2 + 1;

        Vector3 pos = new Vector3(PosX, 0f, PosZ);
        Quaternion quat = Quaternion.identity;
        quat.eulerAngles = new Vector3(90f, 0f, 0f);

        var obj = MyInstantiate(pos, quat, "G") as GameObject;
        
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
        foreach(KeyValuePair<string, Dictionary<Vector3, string>> pair in VariableList)
        {
            foreach (KeyValuePair<Vector3, string> inner in pair.Value)
            {
                GameObject obj = MyValInstantiate(pair.Key, inner.Key);
            }
        }

        foreach(KeyValuePair<string, Dictionary<Vector3, int>> pair in ConstantList)
        {
            foreach (KeyValuePair<Vector3, int> inner in pair.Value)
            {
                GameObject obj = MyConstInstantiate(pair.Key, inner.Key);
            }
        }
    }

    private GameObject MyValInstantiate(string text, Vector3 localPos)
    {
        GameObject obj = Instantiate(VariablePrefab, localPos, Quaternion.identity);
        obj.transform.SetParent(valSetting.transform, false);
        obj.name = text;
        obj.GetComponent<Text>().text = text;
        obj.transform.localPosition = localPos;
        return obj;
    }

    private GameObject MyConstInstantiate(string text, Vector3 localPos)
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
        return obj;
    }

    private void SetDefinition()
    {
        foreach(RectTransform trans in pythonVals)
        {
            if (VariableList.ContainsKey(trans.name))
            {
                List<Vector3> list = new List<Vector3>(VariableList[trans.name].Keys);
                foreach(Vector3 key in list)
                {
                    InputField input = trans.GetComponentInChildren<InputField>();
                    VariableList[trans.name][key] = input.text;
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
        }
    }

    public Dictionary<string, Dictionary<Vector3, string>> GetDefinition()
    {
        return VariableList;
    }
}
