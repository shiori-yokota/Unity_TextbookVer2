using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ModeratorOfChap9 : MonoBehaviour {

    public GameObject robot;
    public GameObject walls;
    public GameObject prefab;
    public GameObject state;
    public GameObject StatePrefab;
    public MenuButton mb;

    private ExecutePanel ep;
    private ControllerOfChap9 controller;
    public GameSettings gs;

    private int MazeSize = new int();
    private Vector3 reSetPos = new Vector3();

    public bool isExecute = false;
    public bool isRunning = false;


    // Use this for initialization
    void Start () {
        ep = FindObjectOfType<ExecutePanel>();
        controller = FindObjectOfType<ControllerOfChap9>();

        GetSettings();          // 設定情報の取得
        InitRobotPosition();    // ロボットの初期位置設定
        SetEnvironment();       // 俯瞰図用カメラ・ライトの位置設定
        SetMazeOuterWall();     // 迷路外壁の設定
        SetMazeInterWall();     // 迷路内壁の設定

    }

    // Update is called once per frame
    void Update () {
        if (mb.clickedSettingButton)
        {
            mb.clickedSettingButton = false;
        }

        if (ep.Execute)
        {
            isExecute = true;
            InitRobotPosition();
            reSetPos = robot.transform.position;
            ep.Execute = false;
            isRunning = true;
        }

        if (isRunning)
            ep.isRunning = true;
        else
            ep.isRunning = false;

        if (controller.canViewProb)
            GetProb();

        if (controller.Collision)
        {
            controller.Collision = false;
            ReSetRobotPosition();
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

    private void ReSetRobotPosition()
    {
        robot.transform.position = reSetPos;
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


    private void GetSettings()
    {
        MazeSize = gs.MazeSize;
    }

    public void InitRobotPosition()
    {
        int row = UnityEngine.Random.Range(0, MazeSize);
        int col = UnityEngine.Random.Range(0, MazeSize);

        robot.transform.position = new Vector3((col * 2) + 1, 1, -((row * 2) + 1));
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

    private void GetProb()
    {
        List<float> stateVal = new List<float>();
        IronPython.Runtime.List tmp = new IronPython.Runtime.List();

        tmp = controller.PRTCL;

        var tmpList = tmp.Cast<double>().ToList();
        stateVal = tmpList.ConvertAll(x => (float)(double)x);
        ViewProb(stateVal);
    }

    private void ViewProb(List<float> stateVal)
    {
        var stateDef = new[]
        {
            new {
                Name = stateVal[0].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -0.5f),
            },
            new {
                Name = stateVal[1].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = stateVal[2].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = stateVal[3].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = stateVal[4].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = stateVal[5].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = stateVal[6].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = stateVal[7].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = stateVal[8].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = stateVal[9].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = stateVal[10].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = stateVal[11].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = stateVal[12].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = stateVal[13].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = stateVal[14].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = stateVal[15].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = stateVal[16].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = stateVal[17].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = stateVal[18].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = stateVal[19].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = stateVal[20].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = stateVal[21].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = stateVal[22].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = stateVal[23].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = stateVal[24].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -8.5f),
            },
        };

        foreach (Transform trans in state.transform)
        {
            if (trans.name != "G") Destroy(trans.gameObject);
        }
        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(90, 0, 0);
        foreach (var states in stateDef)
        {
            var obj = MyQValInstantiate(states.Position, rot, states.Name) as GameObject;
        }

    }

    private GameObject MyQValInstantiate(Vector3 pos, Quaternion quat, string text)
    {
        GameObject obj = Instantiate(StatePrefab, pos, quat) as GameObject;
        obj.name = text;
        obj.GetComponent<TextMesh>().text = text;
        obj.GetComponent<TextMesh>().fontSize = 20;
        obj.GetComponent<TextMesh>().characterSize = 0.15f;
        obj.transform.SetParent(state.transform);

        return obj;
    }

    public int SetMazeState(Vector3 pos)
    {
        int index;

        int col = (Convert.ToInt32(pos.x) - 1) / 2;
        int row = (-(Convert.ToInt32(pos.z)) - 1) / 2;

        if (row == 0)
            index = col;
        else
            index = MazeSize * row + col;

        return index;
    }


}
