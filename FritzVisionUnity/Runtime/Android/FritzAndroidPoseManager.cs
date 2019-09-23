using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;


public class FritzAndroidPoseManager : MonoBehaviour
{
    AndroidJavaClass ajc;

	static AndroidJavaObject poseManager;
	static bool processing = false;

	void Start()
    {

    }

    public static bool IsProcessing()
	{
		return processing;
	}

    public static void SetProcessing()
	{
		processing = true;
	}

    public static void Configure()
    {
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass fritzCore = new AndroidJavaClass("ai.fritz.core.Fritz");

        fritzCore.CallStatic("configure", currentActivityObject, "4c6e348aedaa40e48b46b0a19ef4e5dd");
		fritzCore.CallStatic("enableModelUpdates", false);

		poseManager = new AndroidJavaObject("ai.fritz.fritzvisionunity.FritzPoseUnityManager");
    }

    public static bool Processing()
    {
        return poseManager.Call<bool>("isProcessing");
    }

    public static void SetNumPoses(int numPoses)
    {
		poseManager.Call("setNumPoses", numPoses);
    }

    public static void SetCallbackTarget(string name)
    {
        poseManager.Call("setCallbackTarget", name);
    }

    public static void SetCallbackFunctionTarget(string name)
    {
        poseManager.Call("setCallbackFunctionTarget", name);
    }

    public static string ProcessImage(XRCameraImage image)
	{
        object[] methodParams = extractYUVFromImage(image);
        string message = poseManager.Call<string>("processPose", methodParams);
        return message;
	}

	public static void LogMessage(string message)
	{
		poseManager.Call("logMessage", message);
	}


	public static void ProcessImageAsync(XRCameraImage image)
    {

        object[] methodParams = extractYUVFromImage(image);
        poseManager.Call("processPoseAsync", methodParams);
    }

    private static object[] extractYUVFromImage(XRCameraImage image)
    {
        // Consider each image plane
        XRCameraImagePlane plane = image.GetPlane(0);
        var yRowStride = plane.rowStride;
        var y = plane.data;

        XRCameraImagePlane plane2 = image.GetPlane(1);
        var uvRowStride = plane2.rowStride;
        var uvPixelStride = plane2.pixelStride;
        var u = plane2.data;

        XRCameraImagePlane plane3 = image.GetPlane(2);
        var v = plane3.data;

        byte[] yDst = new byte[y.Length];
        byte[] uDst = new byte[u.Length];
        byte[] vDst = new byte[v.Length];

        object[] objParams = new object[8];
        NativeArray<byte>.Copy(y, yDst);
        NativeArray<byte>.Copy(u, uDst);
        NativeArray<byte>.Copy(v, vDst);

        objParams[0] = yDst;
        objParams[1] = uDst;
        objParams[2] = vDst;
        objParams[3] = yRowStride;
        objParams[4] = uvRowStride;
        objParams[5] = uvPixelStride;
        objParams[6] = image.width;
        objParams[7] = image.height;

        return objParams;
    }
}