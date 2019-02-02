using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisibilityManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void OpenUIMenu(){
        //trigger opening animation
        GetComponent<Animator>().SetBool("Close", false);
        GetComponent<Animator>().SetBool("Open", true);
    }
}
