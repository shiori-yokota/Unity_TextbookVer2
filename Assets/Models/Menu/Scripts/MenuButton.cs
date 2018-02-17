using System.Collections;
using UnityEngine;


public class MenuButton : MonoBehaviour {

    public GameObject SubMenuPanel;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickMenuButton()
    {
        if (!SubMenu.isOpen)
        {
            SubMenuPanel.SetActive(true);
            SubMenu.isOpen = true;
        }
    }

}
