using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChildrenVisibility : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleVisibility(){
        for (int i = 0; i < transform.childCount; i++){
            if (UIVisibilityManager.UIVisible) //if UI is visible, handle opening and closing of individual panels normally. User not allowed to open/close panels once closing animation begins
            {
                bool visibility = transform.GetChild(i).gameObject.activeInHierarchy;
                transform.GetChild(i).gameObject.SetActive(!visibility);
            }
        }

    }
}
