﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {
    
    public static bool isOpen;
    public static bool clickedSettingButton = false;

    public GameObject SubMenuPanel;
    public GameObject menuButton;
    public GameObject ChapterSettingPanel;
    private GameObject StateList;

    private List<RectTransform> ChapterSettings = new List<RectTransform>();

    // Use this for initialization
    void Start () {
        StateList = GameObject.Find("---- State ----");
        SubMenuPanel.SetActive(false);
        isOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (SubMenuPanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void OnClickMenuButton()
    {
        if (!MenuButton.isOpen)
        {
            SubMenuPanel.SetActive(true);
            MenuButton.isOpen = true;
            menuButton.SetActive(false);
        }
    }

    public void OnClickBack2MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickSetSettingButton()
    {
        foreach (Transform state in StateList.transform)
        {
            GameObject.Destroy(state.gameObject);
        }
        foreach (RectTransform fields in ChapterSettingPanel.transform)
        {
            ChapterSettings.Add(fields);
        }

        if (MenuButton.isOpen)
        {
            SubMenuPanel.SetActive(false);
            MenuButton.isOpen = false;
            menuButton.SetActive(true);
            clickedSettingButton = true;
        }
    }

    public List<RectTransform> GetChapterSettings()
    {
        return ChapterSettings;
    }

}
