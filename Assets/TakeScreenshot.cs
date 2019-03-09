using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.iOS;

public class TakeScreenshot : MonoBehaviour
{

    public PrepareUIForScreenshot CompanyLogo; //defined in editor

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //called by button
    public void InitiateScreenshot(){
        StartCoroutine("Screenshot");
    }

    IEnumerator Screenshot(){
        CompanyLogo.ScreenshotUI();

        //wait for everything in the frame to render before taking screenshot
        yield return new WaitForEndOfFrame();
        //FindObjectOfType<UIVisibilityManager>().CloseUIMenu();
        yield return new WaitForSeconds(1.25f);
        print("screenshot taken");

        //wait 1.25 seconds before taking screenshot so we know the UI is closed
        NativeToolkit.SaveScreenshot("MathTalk", "MathTalk Screenshots");


        /*
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(new Vector2(0, 0), new Vector2(Screen.width, Screen.height)), 0, 0, false);
        screenshot.Apply();

        byte[] screenshotBytes = screenshot.EncodeToPNG();
        Object.Destroy(screenshot);

        File.WriteAllBytes("Desktop", screenshotBytes);*/
        yield return new WaitForEndOfFrame();

        CompanyLogo.ReappearUI();
    }

}
