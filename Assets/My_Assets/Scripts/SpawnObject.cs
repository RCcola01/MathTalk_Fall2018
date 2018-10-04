﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class SpawnObject : MonoBehaviour 
{

	public Transform SpawnedObject;
	public float maxRayDistance = 30.0f;
	public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
	private Rigidbody rb;


	bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
                    Debug.Log ("Got hit!");
					Transform obj = Instantiate(SpawnedObject,UnityARMatrixOps.GetPosition (hitResult.worldTransform),UnityARMatrixOps.GetRotation (hitResult.worldTransform));
					obj.GetComponent<Rigidbody>().isKinematic = false;
					obj.GetComponent<Rigidbody>().useGravity = true;


                    //SpawnedObject.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    //SpawnedObject.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", SpawnedObject.position.x, SpawnedObject.position.y, SpawnedObject.position.z));
                    return true;
                }
            }
            return false;
        }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			#if UNITY_EDITOR   //we will only use this script on the editor side, though there is nothing that would prevent it from working on device
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				
				//we'll try to hit one of the plane collider gameobjects that were generated by the plugin
				//effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
				if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) {
					//we're going to get the position from the contact point
					Instantiate(SpawnedObject,hit.point,hit.transform.rotation);
					//SpawnedObject.position = hit.point;
					Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", SpawnedObject.position.x, SpawnedObject.position.y, SpawnedObject.position.z));

					//and the rotation from the transform of the plane collider
					//SpawnedObject.rotation = hit.transform.rotation;
				}
			}
			#else
			if (Input.touchCount > 0 && SpawnedObject != null)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

                    // prioritize reults types
                    ARHitTestResultType[] resultTypes = {
						//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
						//ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
						//ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    }; 
					
                    foreach (ARHitTestResultType resultType in resultTypes)
                    {
                        if (HitTestWithResultType (point, resultType))
                        {
                            return;
                        }
                    }
				}
			}
			#endif

	}
}