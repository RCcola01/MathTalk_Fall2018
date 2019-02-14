using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

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
        HandleManualUIClosing();
        
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

    public void ToggleUIMenuShow(){
        if(UIVisible){
            CloseUIMenu();
        }
        else{
            OpenUIMenu();
        }
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

    Vector2 startPos = new Vector2();
    private bool swiping = false;
    private void HandleManualUIClosing(){
        if (Input.touchCount == 1){
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startPos = Input.GetTouch(0).position;
                swiping = true;
            }
            else if ((Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended) && swiping)
            {
                //if the swipe was more tall than wide and down on the lower third of the screen and the swipe is long enough and the UI is open
                swiping = false;
                bool swipedLongEnough = Mathf.Abs(Vector2.Distance(Input.GetTouch(0).position, startPos)) > Screen.height/5f;
                bool swipedDown = startPos.y > Input.GetTouch(0).position.y;
                bool swipedLowerScreen = (Input.GetTouch(0).position.y < Screen.height / 3f);
                bool moreTallThanWide = (Mathf.Abs(startPos.y - Input.GetTouch(0).position.y) > Mathf.Abs(startPos.x - Input.GetTouch(0).position.x));
                if (swipedDown && swipedLongEnough && swipedLowerScreen && moreTallThanWide)
                {
                    CloseUIMenu();
                }
            }
            else if ((Input.GetTouch(0).phase == TouchPhase.Moved)){
                if ((Input.GetTouch(0).deltaPosition.y > 0f)){
                    startPos = Input.GetTouch(0).position; //start position is now at the maximum y of the touch
                }
            }
        }
        else{
            swiping = false;
            
        }

    }

}
