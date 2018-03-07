using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class ControllerOfChap10 : MonoBehaviour {

    private ScriptEngine scriptEngine;       // スクリプト実行用
    private ScriptScope scriptScope;        // スクリプトに値を渡す
    private ScriptSource scriptSource;       // スクリプトのソースを指定する

    public Camera arCamera;
    public GameObject robot;
    public GameSettings gs;
    private ModeratorOfChap10 moderator;

    private string datetimeStr;
    private string script = string.Empty;
    private string FilePath = string.Empty;
    private string PythonLibPath = string.Empty;


    private bool canInput = false;
    private bool pushForward = false;
    private bool pushRight = false;
    private bool pushLeft = false;
    private float speed = 0.03f;

    // Use this for initialization
    void Start () {
        moderator = FindObjectOfType<ModeratorOfChap10>();

    }

    // Update is called once per frame
    void Update () {
        datetimeStr = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Day.ToString().PadLeft(2, '0') + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Minute.ToString().PadLeft(2, '0') + System.DateTime.Now.Second.ToString().PadLeft(2, '0');

        if (moderator.isExecute)
        {
            Debug.Log("Execute");
            moderator.isExecute = false;

            PythonLibPath = moderator.SetPythonLibPath();
            FilePath = moderator.SetPythonFilePath();

            ExecutePythonSouce();
        }


        if (canInput)
        {
            if (Input.GetKey(KeyCode.UpArrow) || pushForward)
            {
                UpArrow();
            }
            else if (Input.GetKey(KeyCode.RightArrow) || pushRight)
            {
                TurnRight();
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || pushLeft)
            {
                TurnLeft();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("*** Capture ***");
                saveCameraImage();
            }
        }
    }

    private void ExecutePythonSouce()
    {
        using (StreamReader sr = new StreamReader(FilePath, System.Text.Encoding.UTF8))
        {
            script = sr.ReadToEnd();
        }
        scriptEngine = Python.CreateEngine();                               // Pythonスクリプト実行エンジン
        scriptScope = scriptEngine.CreateScope();                           // 実行エンジンに渡す値を設定する
        scriptSource = scriptEngine.CreateScriptSourceFromString(script);   // Pythonのソースを設定

        scriptScope.SetVariable("PYTHON_LIB_PATH", PythonLibPath);

        scriptSource.Execute(scriptScope);      // ソースを実行する

        canInput = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Wall(Clone)")
        {
            Debug.Log("Collision");
        }
    }

    public void PushForwardDown()
    {
        pushForward = true;
    }

    public void PushForwardUp()
    {
        pushForward = false;
    }

    public void PushRightDown()
    {
        pushRight = true;
    }

    public void PushRightUp()
    {
        pushRight = false;
    }

    public void PushLeftDown()
    {
        pushLeft = true;
    }

    public void PushLeftUp()
    {
        pushLeft = false;
    }

    private void UpArrow()
    {
        robot.transform.position += robot.transform.forward * speed;
    }

    private void TurnRight()
    {
        robot.transform.Rotate(new Vector3(0, 1f, 0));
    }

    private void TurnLeft()
    {
        robot.transform.Rotate(new Vector3(0, -1f, 0));
    }

    public void saveCameraImage()
    {
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.Resize(320, 240);
        RenderTexture rt = new RenderTexture(screenShot.width, screenShot.height, 24);
        RenderTexture prev = arCamera.targetTexture;
        arCamera.targetTexture = rt;
        arCamera.Render();
        arCamera.targetTexture = prev;
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        Destroy(screenShot);

        File.WriteAllBytes(Application.dataPath + "/../Python/ScreenShots/" + datetimeStr + ".png", bytes);
    }


    public void StopController()
    {
        Debug.Log("Stop");

        canInput = false;

    }

}
