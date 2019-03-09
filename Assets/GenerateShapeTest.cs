using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateShapeTest : MonoBehaviour
{


    public GameObject can;
    public GameObject sphere;
    public GameObject box;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            print("making box");
            GameObject b = Instantiate(box) as GameObject;
            b.transform.position = new Vector3(0, 0, 8);
        }
        
    }
}
