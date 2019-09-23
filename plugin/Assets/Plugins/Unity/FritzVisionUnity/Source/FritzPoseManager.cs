using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FritzPoseManager
{
    protected static string debugPoseMessage = "[[[0,180.67385578155518,171.22436904907227,0.20715008676052094],[1,176.22251681983471,170.16234302520752,0.12665387988090515],[2,180.38571119308472,169.48479127883911,0.31717085838317871],[3,169.78597116470337,172.18780732154846,0.36716234683990479],[4,176.92858785390854,170.53509187698364,0.84875857830047607],[5,160.12279987335205,182.33350086212158,0.95805686712265015],[6,180.52317190170288,183.13732814788818,0.99377453327178955],[7,158.10193943977356,196.39807319641116,0.74261933565139771],[8,195.79326581954956,192.81923198699951,0.90857023000717163],[9,142.70438385009766,203.86272954940796,0.54645127058029175],[10,204.6772677898407,197.00697612762451,0.87599426507949829],[11,172.86505317687988,218.25026512145996,0.3790256679058075],[12,185.68683433532715,215.4071378707886,0.98059260845184326],[13,178.38080716133118,235.5998330116272,0.88386666774749756],[14,182.5034646987915,235.51912498474118,0.89248514175415039],[15,189.0296745300293,256.92245519161224,0.71080482006072998],[16,192.38013252615929,257.30943143367767,0.90547037124633789]]]";

    public static void Configure()
    {
#if UNITY_IOS
        FritziOSPoseManager.Configure();
#elif UNITY_ANDROID
        FritzAndroidPoseManager.Configure();
#endif
    }

    public static bool Processing()
    {
#if UNITY_IOS
        return FritziOSPoseManager.Processing();
#elif UNITY_ANDROID
        return FritzAndroidPoseManager.Processing();
#endif
    }

    public static void SetNumPoses(int numPoses)
    {
#if UNITY_IOS
        FritziOSPoseManager.SetNumPoses(numPoses);
#elif UNITY_ANDROID
        FritzAndroidPoseManager.SetNumPoses(numPoses);
#endif
    }

    public static void SetCallbackTarget(string name)
    {
#if UNITY_IOS
        FritziOSPoseManager.SetCallbackTarget(name);
#elif UNITY_ANDROID
        FritzAndroidPoseManager.SetCallbackTarget(name);
#endif
    }

    public static void SetCallbackFunctionTarget(string name)
    {
#if UNITY_IOS
        FritziOSPoseManager.SetCallbackFunctionTarget(name);
#elif UNITY_ANDROID
        FritzAndroidPoseManager.SetCallbackFunctionTarget(name);
#endif
    }

    public static List<FritzPose> ProcessPose(IntPtr buffer)
    {
        if (buffer == IntPtr.Zero)
        {
            Debug.LogError("buffer is NULL!");
            return null;
        }

#if UNITY_IOS
        string message = FritziOSPoseManager.ProcessPose(buffer);
        return ProcessEncodedPoses(message);
#endif
        return null;
    }

    public static void ProcessPoseAsync(IntPtr buffer)
    {
        if (buffer == IntPtr.Zero)
        {
            Debug.LogError("buffer is NULL!");
            return;
        }

#if UNITY_IOS
        FritziOSPoseManager.ProcessPoseAsync(buffer);
#endif
    }

    public static List<FritzPose> ProcessImage(XRCameraImage cameraImage)
    {
#if UNITY_ANDROID
        string message = FritzAndroidPoseManager.ProcessImage(cameraImage);
        return ProcessEncodedPoses(message);
#endif
    }

    public static void ProcessImageAsync(XRCameraImage cameraImage)
    {
#if UNITY_ANDROID
        FritzAndroidPoseManager.ProcessImageAsync(cameraImage);
#endif
    }

    public static List<FritzPose> ProcessEncodedPoses(string message)
    {
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

    public static void LogMessage(string message)
    {
#if UNITY_ANDROID
        FritzAndroidPoseManager.LogMessage(message);
#endif
    }
}