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
    public Transform backCollider;


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

        if(transitionTime < 1) transitionTime += Time.deltaTime / transitionDuration;
        if (transitionTime > 1) transitionTime = 1;

        camera.position = Vector3.Lerp(transitionBeginPos, targetPos, transitionTime);
        camera.rotation = Quaternion.Lerp(Quaternion.Euler(transitionBeginRot), Quaternion.Euler(currentPathSegment.cameraRotation), transitionTime);

        Vector3 targetBackPos = (currentPathSegment.backFromPlayer ? player.position : camera.position) + currentPathSegment.backOffset;

        backCollider.transform.position = Vector3.Lerp(transitionBackOffset, targetBackPos, transitionTime);
        backCollider.transform.rotation = Quaternion.Lerp(Quaternion.Euler(transitionBackRotation), Quaternion.Euler(currentPathSegment.backRotation), transitionTime);
        backCollider.transform.eulerAngles += new Vector3(-90, 0, 0);
    }

    Vector3 transitionBeginPos;
    Vector3 transitionBeginRot;
    Vector3 transitionBackOffset;
    Vector3 transitionBackRotation;

    [Serializable]
    public struct CameraPathSegment
    {
        public BoxCollider collider;
        public Vector3 cameraOffset;
        public Vector3 cameraRotation;
        public Vector3 backOffset;
        public Vector3 backRotation;
        public bool backFromPlayer;
    }

    public void BeginSegmentTransition(int endSegment)
    {
        transitionBeginPos = camera.position;
        transitionBeginRot = camera.eulerAngles;
        transitionBackOffset = backCollider.position;
        transitionBackRotation = backCollider.eulerAngles - new Vector3(-90, 0, 0);

        if (endSegment == currentSegmentID) return;

        currentSegmentID = endSegment;
        targetTransform.parent = currentSegmentCollider.transform;
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