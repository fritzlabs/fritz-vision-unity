using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class FritzVisionUnity : MonoBehaviour
{

    protected static string debugPoseMessage = "[[[0,180.67385578155518,171.22436904907227,0.20715008676052094],[1,176.22251681983471,170.16234302520752,0.12665387988090515],[2,180.38571119308472,169.48479127883911,0.31717085838317871],[3,169.78597116470337,172.18780732154846,0.36716234683990479],[4,176.92858785390854,170.53509187698364,0.84875857830047607],[5,160.12279987335205,182.33350086212158,0.95805686712265015],[6,180.52317190170288,183.13732814788818,0.99377453327178955],[7,158.10193943977356,196.39807319641116,0.74261933565139771],[8,195.79326581954956,192.81923198699951,0.90857023000717163],[9,142.70438385009766,203.86272954940796,0.54645127058029175],[10,204.6772677898407,197.00697612762451,0.87599426507949829],[11,172.86505317687988,218.25026512145996,0.3790256679058075],[12,185.68683433532715,215.4071378707886,0.98059260845184326],[13,178.38080716133118,235.5998330116272,0.88386666774749756],[14,182.5034646987915,235.51912498474118,0.89248514175415039],[15,189.0296745300293,256.92245519161224,0.71080482006072998],[16,192.38013252615929,257.30943143367767,0.90547037124633789]]]";
    #region Declare external C interface

#if UNITY_IOS && !UNITY_EDITOR
        
    [DllImport("__Internal")]
    private static extern string _configure();

    [DllImport("__Internal")]
    private static extern void _startCamera();

    [DllImport("__Internal")]
    private static extern void _stopCamera();

    [DllImport("__Internal")]
    private static extern void _setMinPartThreshold(double threshold);

    [DllImport("__Internal")]
    private static extern void _setMinPoseThreshold(double threshold);

    [DllImport("__Internal")]
    private static extern void _setNumPoses(int poses);

    [DllImport("__Internal")]
    private static extern string _getLatestPose();


#endif

    #endregion

    #region Wrapped methods and properties

    public static void Configure()
    {
#if UNITY_IOS && !UNITY_EDITOR
        _configure();
#else

#endif
    }

    public static void StartCamera()
    {
#if UNITY_IOS && !UNITY_EDITOR
        _startCamera();
#else
        
#endif
    }

    public static void StopCamera()
    {
#if UNITY_IOS && !UNITY_EDITOR
        _stopCamera();
#else
#endif
    }

    public static void SetNumPoses(int numPoses)
    {
#if UNITY_IOS && !UNITY_EDITOR
        _setNumPoses(numPoses);
#else
#endif
    }

    public static List<FritzPose> GetLatestPose()
    {
        string message;
#if UNITY_IOS && !UNITY_EDITOR
        message = _getLatestPose();
#else
        message = debugPoseMessage;
#endif
        List<List<List<float>>> poses = JsonConvert.DeserializeObject<List<List<List<float>>>>(message);
        if (poses == null)
        {
            return new List<FritzPose>();
        }
        List<FritzPose> decoded = new List<FritzPose>();
        foreach (List<List<float>> pose in poses)
        {
            decoded.Add(new FritzPose(pose));
        }
        return decoded;
    }

    #endregion

    #region Singleton implementation

    private static FritzVisionUnity _instance;

    public static FritzVisionUnity Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("FritzVisionUnity");
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
        Configure();
    }

    private void Start()
    {
        StartCamera();
    }

    private void Update()
    {
        var poses = GetLatestPose();
        //foreach (FritzPose pose in poses)
        //{
        //    foreach (Keypoint keypoint in pose.keypoints)
        //    {
        //        Debug.Log(keypoint.position);
        //    }
        //}
        
    }
}
