using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeftArmAnimFix : MonoBehaviour
{
    private Animator anim;
    public Vector3 rotation;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void OnAnimatorIK()
    {
        if(anim.GetBool("defense") == false)
        {
            //获取对应骨骼
            Transform leftArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftArm.localEulerAngles = rotation;
            //赋值IK角
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftArm.localEulerAngles));//Quaternion.Euler将欧拉转换为四元数
        }
    }
}
