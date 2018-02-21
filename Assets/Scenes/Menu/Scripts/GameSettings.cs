using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour {

    public class Parameters : MonoBehaviour
    {
        public static int MazeSize = 5;
        public static int GoalCol = 5;
        public static int GoalRow = 5;
    }

    public static Dictionary<string, Vector3> MazeState = new Dictionary<string, Vector3>
    {
        {   "S",    new Vector3(-1f, 0f, -1f)   },
        {   "G",    new Vector3(11f, 0f, -9f)   },
        {   "S1",   new Vector3(3f, 0f, -1f)    },
        {   "S2",   new Vector3(9f, 0f, -1f)    },
        {   "S3",   new Vector3(1f, 0f, -3f)    },
        {   "S4",   new Vector3(7f, 0f, -3f)    },
        {   "S5",   new Vector3(3f, 0f, -5f)    },
        {   "S6",   new Vector3(9f, 0f, -5f)    },
        {   "S7",   new Vector3(3f, 0f, -7f)    },
        {   "S8",   new Vector3(5f, 0f, -7f)    },
        {   "S9",   new Vector3(1f, 0f, -9f)    },
        {   "S10",  new Vector3(5f, 0f, -9f)    },
    };
    
    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Dictionary<string, Vector3> SetMazeState(List<double> state)
    {
        Dictionary<string, Vector3> tmp = new Dictionary<string, Vector3>
        {
            { state[0].ToString("F2"), new Vector3(1.0f, 0f, -0.5f) },    // 1
            { state[1].ToString("F2"), new Vector3(1.5f, 0f, -1.0f) },
            { state[2].ToString("F2"), new Vector3(1.0f, 0f, -1.5f) },
            { state[3].ToString("F2"), new Vector3(0.5f, 0f, -1.0f) },
            { state[4].ToString("F2"), new Vector3(1.0f, 0f, -0.5f) },    // 2
            { state[5].ToString("F2"), new Vector3(1.5f, 0f, -1.0f) },
            { state[6].ToString("F2"), new Vector3(1.0f, 0f, -1.5f) },
            { state[7].ToString("F2"), new Vector3(0.5f, 0f, -1.0f) },
        };
        return tmp;
    }
}
