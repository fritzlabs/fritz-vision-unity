using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class FritzPoseManager
{
	#region Declare external C interface

#if UNITY_IOS && !UNITY_EDITOR
        
    [DllImport("__Internal")]
    private static extern string _configure();

    [DllImport("__Internal")]
    private static extern void _setMinPartThreshold(double threshold);

    [DllImport("__Internal")]
    private static extern void _setMinPoseThreshold(double threshold);

    [DllImport("__Internal")]
    private static extern void _setNumPoses(int poses);

    [DllImport("__Internal")]
    private static extern string _processPose(IntPtr buffer);

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

	public static void SetNumPoses(int numPoses)
	{
#if UNITY_IOS && !UNITY_EDITOR
        _setNumPoses(numPoses);
#else
#endif
	}

	public static List<FritzPose> ProcessPose(IntPtr buffer)
	{
		var message = "";
		if (buffer == IntPtr.Zero)
		{
			Debug.LogError("buffer is NULL!");
			return null;
		}

#if UNITY_IOS && !UNITY_EDITOR
           message = _processPose(buffer);
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
}