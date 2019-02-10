using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisibilityManager : MonoBehaviour
{
    public float closingTime; //defined in the inspector, represents max time inactive before closing
    public static bool UIVisible;
    private float inactiveTime; //counts how long the UI has been inactive

    // Start is called before the first frame update
    void Start()
    {
        UIVisible = true;
        inactiveTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInactiveTime();
        
    }


    public void CloseUIMenu(){
        //compress all of the buttons in the UI before closing
        for (int i = 0; i < transform.childCount; i++){
            for (int j = 0; j < transform.GetChild(i).childCount; j++){
                transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
            } 
        }
        //trigger closing animation
        GetComponent<Animator>().SetBool("Close", true);
        GetComponent<Animator>().SetBool("Open", false);
        UIVisible = false;
    }

    public void OpenUIMenu(){
        //trigger opening animation
        ResetClosingTimer();
        GetComponent<Animator>().SetBool("Close", false);
        GetComponent<Animator>().SetBool("Open", true);
        UIVisible = true;
    }

    //called whenever a button on the UI is tapped 
    public void ResetClosingTimer(){
        inactiveTime = 0f;
    }

    private void HandleInactiveTime(){
        inactiveTime += Time.deltaTime;
        if (inactiveTime > closingTime && UIVisible){
            CloseUIMenu();
        }
    }
}
