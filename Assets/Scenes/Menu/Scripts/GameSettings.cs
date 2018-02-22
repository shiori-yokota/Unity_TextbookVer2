using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour {

    public  int MazeSize = 5;
    public  int GoalCol = 5;
    public  int GoalRow = 5;

    public  Dictionary<string, Vector3> MazeState = new Dictionary<string, Vector3>
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

    public  Dictionary<List<string>, List<int>> StateAction = new Dictionary<List<string>, List<int>>
    {
        {new List<string> { "S", "S3" }, new List<int> { 1, 2 } },
        {new List<string> { "S3", "S4" }, new List<int> { 1, 1, 1 } },
        {new List<string> { "S4", "S1" }, new List<int> { 0, 3, 3 } },
        {new List<string> { "S4", "S6" }, new List<int> { 2, 1 } },
        {new List<string> { "S6", "S2" }, new List<int> { 0, 0 } },
        {new List<string> { "S6", "G" }, new List<int> { 2, 3, 2, 1, 1 } },
        {new List<string> { "S3", "S7" }, new List<int> { 2, 2, 1 } },
        {new List<string> { "S7", "S8" }, new List<int> { 1 } },
        {new List<string> { "S7", "S9" }, new List<int> { 2, 3 } },
        {new List<string> { "S8", "S5" }, new List<int> { 0, 3 } },
        {new List<string> { "S8", "S10" }, new List<int> { 2 } },

        {new List<string> { "S3", "S" }, new List<int> { 0, 3 } },
        {new List<string> { "S4", "S3" }, new List<int> { 3, 3, 3 } },
        {new List<string> { "S1", "S4" }, new List<int> { 1, 1, 2 } },
        {new List<string> { "S6", "S4" }, new List<int> { 3, 0 } },
        {new List<string> { "S2", "S6" }, new List<int> { 2, 2 } },
        {new List<string> { "G", "S6" }, new List<int> { 3, 3, 0, 1, 0 } },
        {new List<string> { "S7", "S3" }, new List<int> { 3, 0, 0 } },
        {new List<string> { "S8", "S7" }, new List<int> { 3 } },
        {new List<string> { "S9", "S7" }, new List<int> { 1, 0 } },
        {new List<string> { "S5", "S8" }, new List<int> { 1, 2 } },
        {new List<string> { "S10", "S8" }, new List<int> { 0 } },
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
        Dictionary<string, Vector3> tmp = new Dictionary<string, Vector3> { };
        return tmp;
    }
    
}
