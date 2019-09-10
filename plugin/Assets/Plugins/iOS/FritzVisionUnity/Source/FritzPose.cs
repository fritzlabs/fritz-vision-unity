using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Pose Parts 
public enum FritzPoseParts
{
    Nose = 0,
    LeftEye = 1,
    RightEye = 2,
    LeftEar = 3,
    RightEar = 4,
    LeftShoulder = 5,
    RightShoulder = 6,
    LeftElbow = 7,
    RightElbow = 8,
    LeftWrist = 9,
    RightWrist = 10,
    LeftHip = 11,
    RightHip = 12,
    LeftKnee = 13,
    RightKnee = 14,
    LeftAnkle = 15,
    RightAnkle = 16
}


public class Keypoint
{
    public FritzPoseParts part;
    public Vector2 position;
    public double confidence;

    public Keypoint(FritzPoseParts part, Vector2 position, double confidence)
    {
        this.part = part;
        this.position = position;
        this.confidence = confidence;
    }
}


public class FritzPose
{
    public List<Keypoint> keypoints;

    int PART_INDEX = 0;
    int X_POS_INDEX = 1;
    int Y_POS_INDEX = 2;
    int CONFIDENCE_INDEX = 3;

    public FritzPose(List<List<float>> rawPose)
    {
        keypoints = new List<Keypoint>();

        foreach (List<float> item in rawPose)
        {
            int part = (int)item[PART_INDEX];

            Keypoint keypoint = new Keypoint(
                (FritzPoseParts)part,
                new Vector2(item[X_POS_INDEX], item[Y_POS_INDEX]),
                (double)item[CONFIDENCE_INDEX]);
            keypoints.Add(keypoint);
        }
    }
}
