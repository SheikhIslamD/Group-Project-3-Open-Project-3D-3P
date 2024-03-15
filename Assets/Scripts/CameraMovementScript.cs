using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{


    public Transform player;
    public Transform camera;
    public Transform targetTransform;

    public CameraPathSegment[] path;
    public int currentSegmentID;
    CameraPathSegment currentPathSegment => path[currentSegmentID];
    BoxCollider currentSegmentCollider => path[currentSegmentID].collider;
    public float transitionDuration = 1;
    float transitionTime = 1;


    private void Start()
    {
        BeginSegmentTransition(0);
    }

    private void Update()
    {

        targetTransform.position = player.position;
        targetTransform.localPosition = new Vector3(
            Mathf.Clamp(targetTransform.localPosition.x, 
                currentSegmentCollider.center.x - ((currentSegmentCollider.size.x / 2) - 0.5f), 
                currentSegmentCollider.center.x + ((currentSegmentCollider.size.x / 2) - 0.5f)
                ),
            Mathf.Clamp(targetTransform.localPosition.y, 
                currentSegmentCollider.center.y - ((currentSegmentCollider.size.y / 2) - 0.5f), 
                currentSegmentCollider.center.y + ((currentSegmentCollider.size.y / 2) - 0.5f)
                ),
            Mathf.Clamp(targetTransform.localPosition.z, 
                currentSegmentCollider.center.z - ((currentSegmentCollider.size.z / 2) - 0.5f), 
                currentSegmentCollider.center.z + ((currentSegmentCollider.size.z / 2) - 0.5f)
                )
            );

        Vector3 targetPos = targetTransform.position + currentPathSegment.cameraOffset;

        if(transitionTime < 1)
        {
            transitionTime += Time.deltaTime / transitionDuration;

            camera.position = Vector3.Lerp(transitionBeginPos, targetPos, transitionTime);
            camera.rotation = Quaternion.Lerp(Quaternion.Euler(transitionBeginRot), Quaternion.Euler(currentPathSegment.cameraDirection), transitionTime);
        }
        if(transitionTime >= 1)
        {
            camera.position = targetPos;
            camera.eulerAngles = currentPathSegment.cameraDirection;
        }

    }

    Vector3 transitionBeginPos;
    Vector3 transitionBeginRot;

    [Serializable]
    public struct CameraPathSegment
    {
        public BoxCollider collider;
        public Vector3 cameraOffset;
        public Vector3 cameraDirection;
    }

    public void BeginSegmentTransition(int endSegment)
    {
        if (endSegment == currentSegmentID) return;

        currentSegmentID = endSegment;
        targetTransform.parent = currentSegmentCollider.transform;
        transitionBeginPos = camera.position;
        transitionBeginRot = camera.eulerAngles;
        transitionTime = 0;

    }

}


/*

#if UNITY_EDITOR

public class CamPathHandlesEnidtor : Editor
{
    public void OnSceneGUI()
    {
        var linkedObject = target as CameraMovementScript;


        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < linkedObject.path.Length; i++)
        {
            linkedObject.path[i] = Handles.PositionHandle(linkedObject.path[i], Quaternion.identity);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(linkedObject, "Change Path");
        }
    }
}
#endif
 */