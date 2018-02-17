using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecutePanel : MonoBehaviour {

    public Dropdown     PythonFileList;
    public GameObject   FileError;

    private bool    canStart = false;
    private string  FilePathName = string.Empty;
    private string  FileName = string.Empty;

    // Use this for initialization
    void Start () {
        FilePathName = Application.dataPath + "/Python/";
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
        if (canStart)
        {
            FileError.SetActive(false);
            Debug.Log("Start!! " + FileName);
        } else
        {
            FileError.SetActive(true);
        }
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
}
