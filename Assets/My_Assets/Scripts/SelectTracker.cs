using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTracker : MonoBehaviour {

	public bool isSelected;
    public bool isBeingScaled;
    public bool isBeingTranslated;

    public Color normalColor; //specified in editor; color of this object when it is not being translated or scaled
    public Color selectedColor; //specified in editor; color of this object when it is being " "

	// Use this for initialization
	void Start () {
		isSelected = false;
        isBeingScaled = false;
        isBeingTranslated = false;
        //deactivateHighlight();

        //DELETE THIS AFTER TESTING WITH COLOR PAINTING IS FINISHED
        /*
        Material newMaterial = GetComponent<Renderer>().material;
        newMaterial.color = normalColor;
        GetComponent<Renderer>().material = newMaterial;*/
        ContinueRotation();
        //print(GetComponent<Renderer>().material.color.ToString());

	}

    // Update is called once per frame
    void Update()
    {
        isSelected = (isBeingScaled || isBeingTranslated);
        // USED FOR EDITOR TESTING
        /*
        if(!isSelected){
            activateHighlight();
            isBeingScaled = true;
        }*/

	}

    //important to create new unique material when highlighting/dehighlighting, or else altering one object's material changes all of them
    public void activateHighlight(){
        //add a baseline shade of selectedColor on top of whatever the vertex colors are (maintains whatever the user has painted)
        /*Material newMaterial = GetComponent<Renderer>().material;
        newMaterial.color = selectedColor;
        GetComponent<Renderer>().material = newMaterial;*/
        //GetComponent<MeshInitializer>().TurnTransparent();
        FreezeRotation();
    }

    public void deactivateHighlight(){
        //return the baseline shade to plain white (maintains whatever the user has painted)
        /*Material newMaterial = GetComponent<Renderer>().material;
        newMaterial.color = normalColor;
        GetComponent<Renderer>().material = newMaterial;*/
        //GetComponent<MeshInitializer>().TurnOpaque();
        ContinueRotation();
    }

    //called on an object when it is selected; this was to fix the bug that caused objects to rotate infinitely when knocking into something during translation
	private void FreezeRotation(){
        GetComponent<Rigidbody>().freezeRotation = true;
	}

    private void ContinueRotation(){
        GetComponent<Rigidbody>().freezeRotation = false;
        
    }

}
