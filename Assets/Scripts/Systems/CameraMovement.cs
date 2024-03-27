using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{


    public Transform player;
    public new Transform camera;
    public Transform targetTransform;

    //public CameraPathSegment[] path;
    //public int currentSegmentID;
    //CameraPathSegment currentPathSegment => path[currentSegmentID];
    CameraBoundry currentPath;
    [SerializeField] CameraBoundry defaultPath;
    BoxCollider currentSegmentCollider => currentPath.boundry;
    public float transitionDuration = 1;
    float transitionTime = 1;
    public Transform backCollider;


    private void Start()
    {
        defaultPath = currentPath;
    }

    private void Update()
    {
        if (targetTransform == null || currentSegmentCollider == null) return;

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

        Vector3 targetPos = targetTransform.position + currentPath.cameraOffset;

        if (transitionTime < 1) transitionTime += Time.deltaTime / transitionDuration;
        if (transitionTime > 1) transitionTime = 1;

        camera.position = Vector3.Lerp(transitionBeginPos, targetPos, transitionTime);
        camera.rotation = Quaternion.Lerp(Quaternion.Euler(transitionBeginRot), Quaternion.Euler(currentPath.cameraRotation), transitionTime);

        Vector3 targetBackPos = (currentPath.backFromPlayer ? player.position : camera.position) + currentPath.backOffset;

        backCollider.transform.position = Vector3.Lerp(transitionBackOffset, targetBackPos, transitionTime);
        backCollider.transform.rotation = Quaternion.Lerp(Quaternion.Euler(transitionBackRotation), Quaternion.Euler(currentPath.backRotation), transitionTime);
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

    /*
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
     */

    public void BeginSegmentTransition(CameraBoundry newPath)
    {
        transitionBeginPos = camera.position;
        transitionBeginRot = camera.eulerAngles;
        transitionBackOffset = backCollider.position;
        transitionBackRotation = backCollider.eulerAngles - new Vector3(-90, 0, 0);

        if (newPath == currentPath) return;

        currentPath = newPath;
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