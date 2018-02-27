using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ModeratorOfChap10 : MonoBehaviour {

    public Slider sliderComp;

    public GameObject robot;
    public GameObject walls;
    public GameObject prefab;
    public MenuButton mb;

    private ExecutePanel ep;
    private ControllerOfChap10 controller;
    private ModeratorOfTreasures treasures;
    public GameSettings gs;

    private int MazeSize = new int();

    public bool isExecute = false;
    public bool isRunning = false;

    private List<GameObject> TreasuresList = new List<GameObject>();


    // Use this for initialization
    void Start () {
        ep = FindObjectOfType<ExecutePanel>();
        controller = FindObjectOfType<ControllerOfChap10>();
        treasures = FindObjectOfType<ModeratorOfTreasures>();


        sliderComp.value = 0.7f;

        GetSettings();          // 設定情報の取得
        InitRobotPosition();    // ロボットの初期位置設定
        SetEnvironment();       // 俯瞰図用カメラ・ライトの位置設定
        SetMazeOuterWall();     // 迷路外壁の設定
        SetMazeInterWall();     // 迷路内壁の設定

        //try
        //{
        //    TreasuresList = treasures.InitializeAndGetTreasures();
        //}
        //catch (Exception exception)
        //{
        //    Debug.LogError(exception);
        //    ApplicationQuitAfter1sec();
        //}

        //PreProcess();
    }

    void Update() {
        if (mb.clickedSettingButton)
        {
            mb.clickedSettingButton = false;
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


        if (ep.StopOrder)
        {
            ep.StopOrder = false;
            isRunning = false;
            isExecute = false;
            controller.StopController();
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

    private void GetSettings()
    {
        MazeSize = gs.MazeSize;
    }
    
    public void LightChange()
    {
        GameObject light = GameObject.Find("Directional Light");
                
        Light lightComp = light.GetComponent<Light>();
        lightComp.intensity = (sliderComp.value) + 0.3f;
    }
    
    public void InitRobotPosition()
    {
        int row = UnityEngine.Random.Range(0, MazeSize);
        int col = UnityEngine.Random.Range(0, MazeSize);

        robot.transform.position = new Vector3((col * 2) + 1, 1, -((row * 2) + 1));
    }

    private void PreProcess()
    {
        Dictionary<TreasurePositionsInfo, GameObject> treasuresPositionMap = null;
        treasuresPositionMap = treasures.CreateTreasuresPositionMap();

        foreach (KeyValuePair<TreasurePositionsInfo, GameObject> pair in treasuresPositionMap)
        {
            pair.Value.transform.position = pair.Key.position;
            pair.Value.transform.eulerAngles = pair.Key.eulerAngles;
        }

    }

    private void SetEnvironment()
    {
        // 俯瞰図用カメラ
        GameObject cam = GameObject.Find("Main Camera");
        cam.transform.position = new Vector3(MazeSize, MazeSize * 2.5f, -MazeSize);
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

    private void ApplicationQuitAfter1sec()
    {
        Thread.Sleep(1000);
        Application.Quit();
    }

}
