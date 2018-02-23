using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeratorOfChap3 : MonoBehaviour {
    
    public  GameObject robot;
    public  GameObject walls;
    public  GameObject prefab;
    public  GameObject state;
    public  GameObject StatePrefab;
    public  GameObject valSetting;
    public  GameObject VariablePrefab;
    public  MenuButton mb;

    private ExecutePanel ep;
    private ControllerOfChap3 controller;
    public GameSettings gs;

    private int MazeSize = new int();
    private List<RectTransform> pythonVals = new List<RectTransform> { };
    private Dictionary<string, Dictionary<Vector3, string>> VariableList = new Dictionary<string, Dictionary<Vector3, string>>
    {
        {"クローズドリスト：",   new Dictionary<Vector3, string> { {new Vector3(0f, 300f, 0f),    "CLOSEDLIST"} } },
    };

    public bool isExecute = false;
    public bool isRunning = false;
    

    // Use this for initialization
    void Start () {

        ep = FindObjectOfType<ExecutePanel>();
        controller = FindObjectOfType<ControllerOfChap3>();

        GetSettings();          // 設定情報の取得
        InitRobotPosition();    // ロボットの初期位置設定
        SetEnvironment();       // 俯瞰図用カメラ・ライトの位置設定
        SetMazeOuterWall();     // 迷路外壁の設定
        SetMazeInterWall();     // 迷路内壁の設定
        SetState();             // 状態空間の設定
        SetVariable();          // 変数の設定

    }
	
	// Update is called once per frame
	void Update () {
        if (mb.clickedSettingButton)
        {
            mb.clickedSettingButton = false;
            pythonVals = mb.GetChapterSettings();
            SetDefinition();
            SetState();             // 状態空間の設定
        }

        if (ep.Execute)
        {
            isExecute = true;
            InitRobotPosition();
            ep.Execute = false;
            isRunning = true;
        }

        if (isRunning)
            ep.isRunning = true;
        else
            ep.isRunning = false;

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

    public void InitRobotPosition()
    {
        int row = 0;
        int col = -1;

        robot.transform.position = new Vector3((col * 2) + 1, 1, -((row * 2) + 1));
        Quaternion quat = Quaternion.identity;
        quat.eulerAngles = new Vector3(0f, 90f, 0f);
        robot.transform.rotation = quat;
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
        OuterWalls = new GameObject[MazeSize * 2 - 2];
        Array.Copy(OuterWallFabs, 0, OuterWalls, 0, 8);

        for (int i = 0; i < OuterWalls.Length / 2; i++)
        {
            OuterWalls[i] = Instantiate(prefab, new Vector3(0f, 1.5f, -3f + i * -2f), quat) as GameObject;
            OuterWalls[i].transform.SetParent(walls.transform);
            OuterWalls[i + 4] = Instantiate(prefab, new Vector3(10f, 1.5f, -1f + i * -2f), quat) as GameObject;
            OuterWalls[i + 4].transform.SetParent(walls.transform);
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

    private void SetState()
    {                
        Quaternion quat = Quaternion.identity;
        quat.eulerAngles = new Vector3(90f, 0f, 0f);
        Dictionary<string, Vector3> MazeState = FindObjectOfType<GameSettings>().MazeState;
        foreach (KeyValuePair<string, Vector3> pair in MazeState)
        {
            var obj = MyInstantiate(pair.Value, quat, pair.Key) as GameObject;
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

    public void SetVariable()
    {
        foreach(KeyValuePair<string, Dictionary<Vector3, string>> pair in VariableList)
        {
            foreach (KeyValuePair<Vector3, string> inner in pair.Value)
            {
                GameObject obj = MyValInstantiate(pair.Key, inner.Key, inner.Value);
            }
        }
    }

    private GameObject MyValInstantiate(string text, Vector3 localPos, string val)
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

        return obj;
    }

    private void SetDefinition()
    {
        foreach(RectTransform trans in pythonVals)
        {
            List<Vector3> list = new List<Vector3>(VariableList[trans.name].Keys);
            foreach(Vector3 key in list)
            {
                InputField input = trans.GetComponentInChildren<InputField>();
                if (input.text == string.Empty)
                {
                    VariableList[trans.name][key] = input.placeholder.GetComponent<Text>().text;
                }
                else
                {
                    VariableList[trans.name][key] = input.text;
                }
            }
        }
    }

    public Dictionary<string, Dictionary<Vector3, string>> GetDefinition()
    {
        return VariableList;
    }

    public string SetPythonFilePath()
    {
        string PythonFilePath = ep.GetExecuteFilePath();
        return PythonFilePath;
    }
}
