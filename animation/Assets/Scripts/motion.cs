using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum PositionIndex : int
{
    lShldrBend = 0,
    rShldrBend,
    lForearmBend,
    rForearmBend,
    lHand,
    rHand,
    lPinky,
    rPinky,
    lMid,
    rMid,
    lThumb,
    rThumb,
    lThighBend,
    rThighBend,
    lKnee,
    rKnee,
    lFoot,
    rFoot,
    lToe,
    rToe,

    //Calculated coordinates
    hip,
    spine,
    neck,
    head,

    nose,
    lEar,
    rEar,

    Count,
}



public static partial class EnumExtend
{
    public static int Int(this PositionIndex i)
    {
        return (int)i;
    }
}

public class motion : MonoBehaviour
{
    private Animator anim;
    private float charaHei;
    private float charaWid;

    List<string> lines;
    int counter = 0;
    string updateBone = "";

    private Dictionary<int, string> BoneMap;

    public class JointPoint
    {
        public Transform Transform = null;
        public Vector3 NextPos = Vector3.zero;
        public JointPoint Child = null;
        public Quaternion MidRotation = Quaternion.identity;
    }

    private JointPoint[] jointPoints;
    
    public JointPoint[] InitJoints()
    {
        jointPoints = new JointPoint[PositionIndex.Count.Int()];

        for (var i = 0; i < PositionIndex.Count.Int(); i++)
        {
            jointPoints[i] = new JointPoint();
        }
        
        jointPoints[PositionIndex.lShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        jointPoints[PositionIndex.rShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        jointPoints[PositionIndex.lForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        jointPoints[PositionIndex.rForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
        jointPoints[PositionIndex.lHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
        jointPoints[PositionIndex.rHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);
        jointPoints[PositionIndex.lPinky.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLittleDistal);
        jointPoints[PositionIndex.rPinky.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLittleDistal);
        jointPoints[PositionIndex.lMid.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftMiddleDistal);
        jointPoints[PositionIndex.rMid.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightMiddleDistal);
        jointPoints[PositionIndex.lThumb.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftThumbDistal);
        jointPoints[PositionIndex.rThumb.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightThumbDistal);
        jointPoints[PositionIndex.lThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        jointPoints[PositionIndex.rThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        jointPoints[PositionIndex.lKnee.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        jointPoints[PositionIndex.rKnee.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        jointPoints[PositionIndex.lFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        jointPoints[PositionIndex.rFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        jointPoints[PositionIndex.lToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftToes);
        jointPoints[PositionIndex.rToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightToes);
        jointPoints[PositionIndex.head.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex.hip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
        jointPoints[PositionIndex.neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Neck);
        jointPoints[PositionIndex.spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);

        // Face
        jointPoints[PositionIndex.nose.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex.lEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex.rEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        

        
        jointPoints[PositionIndex.neck.Int()].Child = jointPoints[PositionIndex.head.Int()];
        jointPoints[PositionIndex.spine.Int()].Child = jointPoints[PositionIndex.neck.Int()];
        jointPoints[PositionIndex.lShldrBend.Int()].Child = jointPoints[PositionIndex.lForearmBend.Int()];
        jointPoints[PositionIndex.rShldrBend.Int()].Child = jointPoints[PositionIndex.rForearmBend.Int()];
        jointPoints[PositionIndex.lForearmBend.Int()].Child = jointPoints[PositionIndex.lHand.Int()];
        jointPoints[PositionIndex.rForearmBend.Int()].Child = jointPoints[PositionIndex.rHand.Int()];
        jointPoints[PositionIndex.lThighBend.Int()].Child = jointPoints[PositionIndex.lKnee.Int()];
        jointPoints[PositionIndex.rThighBend.Int()].Child = jointPoints[PositionIndex.rKnee.Int()];
        jointPoints[PositionIndex.lKnee.Int()].Child = jointPoints[PositionIndex.lFoot.Int()];
        jointPoints[PositionIndex.rKnee.Int()].Child = jointPoints[PositionIndex.rFoot.Int()];
        jointPoints[PositionIndex.lFoot.Int()].Child = jointPoints[PositionIndex.lToe.Int()];
        jointPoints[PositionIndex.rFoot.Int()].Child = jointPoints[PositionIndex.rToe.Int()];

        Vector3 forward = TriangleNormal(jointPoints[PositionIndex.head.Int()].Transform.position, jointPoints[PositionIndex.lThighBend.Int()].Transform.position, jointPoints[PositionIndex.rThighBend.Int()].Transform.position);

        for (var i = 0; i < PositionIndex.Count.Int() - 4; i++)
        {
            if (jointPoints[i].Child != null)
            {
                jointPoints[i].MidRotation = Quaternion.Inverse(jointPoints[i].Transform.rotation) * Quaternion.LookRotation(jointPoints[i].Transform.position - jointPoints[i].Child.Transform.position, forward);
            }
        }

        jointPoints[PositionIndex.head.Int()].MidRotation = Quaternion.Inverse(jointPoints[PositionIndex.head.Int()].Transform.rotation) * Quaternion.LookRotation(jointPoints[PositionIndex.nose.Int()].Transform.position - jointPoints[PositionIndex.head.Int()].Transform.position, forward);
        jointPoints[PositionIndex.hip.Int()].MidRotation = Quaternion.Inverse(jointPoints[PositionIndex.hip.Int()].Transform.rotation) * Quaternion.LookRotation(forward);

        return jointPoints;
    }

    Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 d1 = a - b;
        Vector3 d2 = a - c;

        Vector3 dd = Vector3.Cross(d1, d2);
        dd.Normalize();

        return dd;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        Vector3 charaMidToe = (anim.GetBoneTransform(HumanBodyBones.LeftToes).position + anim.GetBoneTransform(HumanBodyBones.RightToes).position) * 0.5f;
        charaHei = Vector3.Distance(anim.GetBoneTransform(HumanBodyBones.Head).position, charaMidToe);
        charaWid = Vector3.Distance(anim.GetBoneTransform(HumanBodyBones.LeftLowerArm).position, anim.GetBoneTransform(HumanBodyBones.RightLowerArm).position);

        BoneMap = new Dictionary<int, string>()
        {
            { 0, "nose" },
            { 7, "lEar" },
            { 8, "rEar" },
            { 11, "lShldrBend" },
            { 12, "rShldrBend" },
            { 13, "lForearmBend" },
            { 14, "rForearmBend" },
            { 15, "lHand" },
            { 16, "rHand" },
            { 17, "lPinky" },
            { 18, "rPinky" },
            { 19, "lMid" },
            { 20, "rMid" },
            { 21, "lThumb" },
            { 22, "rThumb" },
            { 23, "lThighBend" },
            { 24, "rThighBend" },
            { 25, "lKnee" },
            { 26, "rKnee" },
            { 27, "lFoot" },
            { 28, "rFoot" },
            { 31, "lToe" },
            { 32, "rToe" }
        };

        lines = System.IO.File.ReadLines("Assets/MotionFile.txt").ToList();
        jointPoints = InitJoints();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        string[] points = lines[counter].Split(',');

        for (int i = 0; i <= 32; i++)
        {
            if (BoneMap.ContainsKey(i))
            {
                updateBone = BoneMap[i];
                float x = float.Parse(points[0 + (i * 3)]) * charaWid;
                float y = float.Parse(points[1 + (i * 3)]) * charaHei + 1f;
                float z = float.Parse(points[2 + (i * 3)]) / 3000;

                Vector3 tar = new Vector3(x, y, z) + transform.position;

                if (Enum.TryParse<PositionIndex>(updateBone, out PositionIndex positionIndex))
                {
                    jointPoints[positionIndex.Int()].NextPos = tar;
                }
            }

        }
        
        Vector3 midShoulder = (jointPoints[PositionIndex.lShldrBend.Int()].NextPos + jointPoints[PositionIndex.rShldrBend.Int()].NextPos) * 0.5f;
        Vector3 midThigh = (jointPoints[PositionIndex.lThighBend.Int()].NextPos + jointPoints[PositionIndex.rThighBend.Int()].NextPos) * 0.5f;
        jointPoints[PositionIndex.head.Int()].NextPos = (jointPoints[PositionIndex.lEar.Int()].NextPos + jointPoints[PositionIndex.rEar.Int()].NextPos) * 0.5f + new Vector3(0f, 0.1f, 0.4f);
        jointPoints[PositionIndex.neck.Int()].NextPos = (midShoulder + jointPoints[PositionIndex.head.Int()].NextPos) * 0.5f + new Vector3(0f, 0f, 0.05f);
        jointPoints[PositionIndex.spine.Int()].NextPos = midThigh * 0.6f + midShoulder * 0.4f;
        jointPoints[PositionIndex.hip.Int()].NextPos = midThigh * 0.8f + midShoulder * 0.2f;

        Vector3 forward = TriangleNormal(jointPoints[PositionIndex.head.Int()].NextPos, jointPoints[PositionIndex.lThighBend.Int()].NextPos, jointPoints[PositionIndex.rThighBend.Int()].NextPos);
        jointPoints[PositionIndex.hip.Int()].Transform.rotation = Quaternion.LookRotation(forward) * Quaternion.Inverse(jointPoints[PositionIndex.hip.Int()].MidRotation);

        for (var i = 0; i < PositionIndex.Count.Int() - 5; i++)
        {
            if (jointPoints[i].Child != null)
            {
                jointPoints[i].Transform.rotation = Quaternion.LookRotation(jointPoints[i].NextPos - jointPoints[i].Child.NextPos, forward) * Quaternion.Inverse(jointPoints[i].MidRotation);
            }
            jointPoints[i].Transform.position = jointPoints[i].NextPos;

        }
        jointPoints[PositionIndex.neck.Int()].Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex.neck.Int()].NextPos - jointPoints[PositionIndex.neck.Int()].Child.NextPos, forward) * Quaternion.Inverse(jointPoints[PositionIndex.neck.Int()].MidRotation);
        jointPoints[PositionIndex.head.Int()].Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex.nose.Int()].NextPos - jointPoints[PositionIndex.head.Int()].NextPos, forward) * Quaternion.Inverse(jointPoints[PositionIndex.head.Int()].MidRotation);
        jointPoints[PositionIndex.head.Int()].Transform.position = jointPoints[PositionIndex.head.Int()].NextPos;

        counter += 1;
        if (counter == lines.Count) { counter = 0; }
    }
}
