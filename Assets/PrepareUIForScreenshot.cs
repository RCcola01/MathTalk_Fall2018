using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareUIForScreenshot : MonoBehaviour
{

    private List<GameObject> gameObjectsToReappear;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectsToReappear = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //clear all of the UI except for the company logo
    public void ScreenshotUI(){
        gameObjectsToReappear.RemoveRange(0, gameObjectsToReappear.Count);
        for (int i = 0; i < transform.parent.childCount; i++){
            if (transform.parent.GetChild(i).gameObject.activeInHierarchy)
            {
                gameObjectsToReappear.Add(transform.parent.GetChild(i).gameObject);
            }
            transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.SetActive(true); //keep this object (the company logo) active

    }

    //make all previously active objects reappear in the UI
    public void ReappearUI(){
        foreach (GameObject go in gameObjectsToReappear){
            go.SetActive(true);
        }
        //depending on current mode, trigger the correct icon animation in top left corner
    }
}
