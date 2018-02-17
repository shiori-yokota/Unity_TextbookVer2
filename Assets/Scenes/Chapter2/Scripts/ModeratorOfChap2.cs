using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeratorOfChap2 : MonoBehaviour {

    private GameObject robot;
    public  GameObject prefab;
    public  GameObject StatePrefab;

    private int MazeSize;

    // Use this for initialization
    void Start () {
        robot = GameObject.Find("Robot");

        GetSettings();          // 設定情報の取得
        InitRobotPosition();    // ロボットの初期位置設定
        SetEnvironment();       // 俯瞰図用カメラ・ライトの位置設定
        SetMazeOuterWall();     // 迷路外壁の設定
        SetMazeInterWall();     // 迷路内壁の設定
        SetState();             // 状態空間の設定


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitRobotPosition()
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
        GameSettings.Parameters.MazeSize = 5;
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
            OuterWalls[i + 5] = Instantiate(prefab, new Vector3(1f + i * 2f, 1.5f, -10f), quat) as GameObject;
        }
        
        quat.eulerAngles = new Vector3(0f, 90f, 0f);
        OuterWalls = new GameObject[MazeSize * 2 - 2];
        Array.Copy(OuterWallFabs, 0, OuterWalls, 0, 8);

        for (int i = 0; i < OuterWalls.Length / 2; i++)
        {
            OuterWalls[i] = Instantiate(prefab, new Vector3(0f, 1.5f, -3f + i * -2f), quat) as GameObject;
            OuterWalls[i + 4] = Instantiate(prefab, new Vector3(10f, 1.5f, -1f + i * -2f), quat) as GameObject;
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
        }
    }

    private void SetState()
    {
        var stateDef = new[] {
            new
            {
                Name = "S",
                Position = new Vector3(-1f, 0f, -1f),
            },
            new
            {
                Name = "G",
                Position = new Vector3(11f, 0f, -9f),
            },new
            {
                Name = "S1",
                Position = new Vector3(3f, 0f, -1f),
            },
            new
            {
                Name = "S2",
                Position = new Vector3(9f, 0f, -1f),
            },
            new
            {
                Name = "S3",
                Position = new Vector3(1f, 0f, -3f),
            },
            new
            {
                Name = "S4",
                Position = new Vector3(7f, 0f, -3f),
            },
            new
            {
                Name = "S5",
                Position = new Vector3(3f, 0f, -5f),
            },
            new
            {
                Name = "S6",
                Position = new Vector3(9f, 0f, -5f),
            },
            new
            {
                Name = "S7",
                Position = new Vector3(3f, 0f, -7f),
            },
            new
            {
                Name = "S8",
                Position = new Vector3(5f, 0f, -7f),
            },
            new
            {
                Name = "S9",
                Position = new Vector3(1f, 0f, -9f),
            },
            new
            {
                Name = "S10",
                Position = new Vector3(5f, 0f, -9f),
            },
        };

        GameObject[] states = new GameObject[stateDef.Length];
        Quaternion quat = Quaternion.identity;
        quat.eulerAngles = new Vector3(90f, 0f, 0f);
        for (int i = 0; i < stateDef.Length; i++)
        {
            states[i] = MyInstantiate(stateDef[i].Position, quat, stateDef[i].Name) as GameObject;
        }
    }

    private GameObject MyInstantiate(Vector3 pos, Quaternion quat, string text)
    {
        GameObject obj = Instantiate(StatePrefab, pos, quat) as GameObject;
        obj.name = text;
        obj.GetComponent<TextMesh>().text = text;
        obj.GetComponent<TextMesh>().fontSize = 45;
        obj.GetComponent<TextMesh>().characterSize = 0.15f;

        return obj;
    }

}
