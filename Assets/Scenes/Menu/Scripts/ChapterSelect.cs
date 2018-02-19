using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelect : MonoBehaviour {
        
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickChapter2()
    {
        SceneManager.LoadScene("Chapter2");
    }

    public void OnClickChapter3()
    {
        SceneManager.LoadScene("Chapter3");
    }

    public void OnClickChapter7()
    {
        SceneManager.LoadScene("Chapter7");
    }

    public void OnClickChapter8()
    {
        SceneManager.LoadScene("Chapter8");
    }

    public void OnClickChapter9()
    {
        SceneManager.LoadScene("Chapter9");
    }

    public void OnClickChapter10()
    {
        SceneManager.LoadScene("Chapter10");
    }

}
