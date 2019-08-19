using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class FritzVisionUnity : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events.")]
    ARCameraManager m_CameraManager;

    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }

    [SerializeField]
    Camera m_Cam;

    #region Singleton implementation

    private static FritzVisionUnity _instance;

    public static FritzVisionUnity Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("FritzPoseUnity");
                _instance = obj.AddComponent<FritzVisionUnity>();
            }

            return _instance;
        }
    }


    #endregion

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        FritzPoseManager.Configure();

    }

    private void Update()
    {
        if (Time.frameCount % 10 != 0) { return;  }

        var cameraParams = new XRCameraParams
        {
            zNear = m_Cam.nearClipPlane,
            zFar = m_Cam.farClipPlane,
            screenWidth = Screen.width,
            screenHeight = Screen.height,
            screenOrientation = Screen.orientation
        };
        
        XRCameraFrame frame;
        if (!cameraManager.subsystem.TryGetLatestFrame(cameraParams, out frame))
        {
            return;
        }

        var poses = FritzPoseManager.ProcessPose(frame.nativePtr);
        foreach (FritzPose pose in poses)
        {
            foreach (Keypoint keypoint in pose.keypoints)
            {
                Debug.Log(keypoint.position);
            }
        }

    }
}
