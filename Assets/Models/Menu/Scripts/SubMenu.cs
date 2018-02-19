using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubMenu : MonoBehaviour {
        
    public static bool isOpen;
    public static bool clickedSettingButton = false;

    public GameObject SubMenuPanel;
    public InputField MazeSize;
    public GameObject menuButton;
    public GameObject ChapterSettingPanel;

    private List<RectTransform> ChapterSettings;

    // Use this for initialization
    void Start () {
        if (SceneManager.GetSceneByName("Chapter2").IsValid())
        {
            MazeSize.interactable = false;
        }
        SubMenuPanel.SetActive(false);
        isOpen = false;
    }
	
	// Update is called once per frame
    void Update () {
        if (SubMenuPanel.activeSelf)
        {
            MazeSize.text = GameSettings.Parameters.MazeSize.ToString();
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void OnClickBack2MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickSetSettingButton()
    {
        foreach (RectTransform fields in ChapterSettingPanel.transform)
        {
            Debug.Log(fields);
            ChapterSettings.Add(fields);
        }

        if (SubMenu.isOpen)
        {
            SubMenuPanel.SetActive(false);
            SubMenu.isOpen = false;
            menuButton.SetActive(true);
            clickedSettingButton = true;
        }
    }

    public List<RectTransform> GetChapterSettings()
    {
        return ChapterSettings;
    }

}
