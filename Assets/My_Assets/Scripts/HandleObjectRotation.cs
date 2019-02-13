using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObjectRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TouchRotation();
    }

    /// Flag set to true if the user currently makes an rotation gesture, otherwise false
    private bool rotating = false;
    /// The squared rotation width determining an rotation
    public const float TOUCH_ROTATION_WIDTH = 1; // Always
    /// The threshold in angles which must be exceeded so a touch rotation is recogniced as one
    public const float TOUCH_ROTATION_MINIMUM = 1;
    /// Start vector of the current rotation
    Vector2 startVector = Vector2.zero;

    /// Processes input for touch rotation, only the first two touches are used
    private void TouchRotation()
    {
        if (Input.touchCount == 2)
        {
            if (!rotating)
            {
                startVector = Input.touches[1].position - Input.touches[0].position;
                rotating = startVector.sqrMagnitude > TOUCH_ROTATION_WIDTH;
            }
            else
            {
                Vector2 currVector = Input.touches[1].position - Input.touches[0].position;
                float angleOffset = Vector2.Angle(startVector, currVector);

                if (angleOffset > TOUCH_ROTATION_MINIMUM)
                {
                    Vector3 LR = Vector3.Cross(startVector, currVector); // z > 0 left rotation, z < 0 right rotation

                    if (LR.z > 0)
                    {
                        //CSharpscaling.ScaleTransform.eulerAngles = new Vector3(0f, CSharpscaling.ScaleTransform.eulerAngles.y - angleOffset, 0f);
                        //CSharpscaling.ScaleTransform.Rotate(new Vector3(0f, -1f * angleOffset, 0f));
                        CSharpscaling.ScaleTransform.RotateAround(CSharpscaling.ScaleTransform.position, new Vector3(0f, 1f, 0f), -1f * angleOffset);
                    }
                    else if (LR.z < 0)
                    {
                        //CSharpscaling.ScaleTransform.eulerAngles = new Vector3(0f, CSharpscaling.ScaleTransform.eulerAngles.y + angleOffset, 0f);
                        //CSharpscaling.ScaleTransform.Rotate(new Vector3(0f, angleOffset, 0f));
                        CSharpscaling.ScaleTransform.RotateAround(CSharpscaling.ScaleTransform.position, new Vector3(0f, 1f, 0f), angleOffset);
                    }

                    //mouseLook.y = Mathf.Clamp(mouseLook.y, 0, 180F); // Clamp looking down and up

                    //GameController.Instance.mainCamera.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                    startVector = currVector;
                }
            }
        }
        else
            rotating = false;
    }
}
