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
    }

    public InputField SizeInput;

    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }


    // Use this for initialization
    void Start () {
        SizeInput.text = Parameters.MazeSize.ToString();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickRegistorationButton()
    {
        Parameters.MazeSize = Int32.Parse(SizeInput.text);
        SceneManager.UnloadSceneAsync(1);
    }
}
