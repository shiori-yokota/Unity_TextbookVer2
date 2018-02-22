using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExecutePanel : MonoBehaviour {

    public Dropdown     PythonFileList;
    public GameObject   FileError;
    public Text         ErrorText;
    public Button   PlayButton;
    public Button   StopButton;
    public Button       MenuButton;

    private bool        canStart = false;
    private string      FilePathName = string.Empty;
    private string      FileName = string.Empty;
    private int         chapter = new int();

    public bool  Execute = false;
    public bool  isRunning = false;
    public bool  StopOrder = false;

    // Use this for initialization
    void Start () {
        //StopButton.interactable = false;

        FilePathName = Application.dataPath + "/Python/Sources/";
        if (PythonFileList)
        {
            PythonFileList.ClearOptions();      // 現在の要素をクリアする
            List<string> list = new List<string>();
            list = SetPythonFileList();
            PythonFileList.AddOptions(list);
            PythonFileList.value = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            PlayButton.interactable = false;
            PythonFileList.interactable = false;
            MenuButton.interactable = false;
        }
        else
        {
            PlayButton.interactable = true;
            PythonFileList.interactable = true;
            MenuButton.interactable = true;
        }
	}

    private List<string> SetPythonFileList()
    {
        List<string> Files = new List<string>();
        string[] AllFile = Directory.GetFiles(FilePathName, "*.py", SearchOption.AllDirectories);
        foreach (string s in AllFile)
        {
            string fileName = s.Remove(0, FilePathName.Length);
            Files.Add(fileName);
        }
        Files.Insert(0, "----");
        return Files;
    }

    public void OnClickStartButton()
    {
        Debug.Log("clicked");
        if (canStart && !isRunning)
        {
            Execute = true;
            FileError.SetActive(false);
            Debug.Log("Start!! " + FileName);
        }
        else if (canStart && isRunning)
        {
            ErrorText.text = "*ファイル実行中です";
            FileError.SetActive(true);
        }
        else
        {
            ErrorText.text = "*ファイルを選んでください";
            FileError.SetActive(true);
        }
    }

    public void OnClickStopButton()
    {
        Debug.Log("一時停止");
        StopOrder = true;
        isRunning = false;
    }

    public void OnPythonFileChanged(int val)
    {
        FileName = string.Empty;
        if (val != 0)
        {
            FileName = PythonFileList.options[val].text;
            canStart = true;
        }
        else canStart = false;
    }

    public string GetExecuteFilePath()
    {
        string tmp = FilePathName + FileName;
        return tmp;
    }
}
