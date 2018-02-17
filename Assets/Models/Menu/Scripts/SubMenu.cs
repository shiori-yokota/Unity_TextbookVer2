using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubMenu : MonoBehaviour {
        
    public static bool isOpen;

    public GameObject SubMenuPanel;
    public Text MazeSize;

    // Use this for initialization
    void Start () {
        if (SceneManager.GetSceneByName("Chapter2").IsValid())
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == "SettingButton")
                {
                    Button btn = child.GetComponent<Button>();
                    btn.interactable = false;
                }
            }
        }
        SubMenuPanel.SetActive(false);
        isOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (SubMenuPanel.activeSelf)
        {
            MazeSize.text = GameSettings.Parameters.MazeSize.ToString() + " マス";
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
	}

    public void OnClickCloseBotton()
    {
        if (SubMenu.isOpen)
        {
            SubMenuPanel.SetActive(false);
            SubMenu.isOpen = false;
        }
    }

    public void OnClickRestartButton()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnClickBack2MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickGameSettingButton()
    {
        SceneManager.LoadScene("SettingMenu", LoadSceneMode.Additive);
    }

}
