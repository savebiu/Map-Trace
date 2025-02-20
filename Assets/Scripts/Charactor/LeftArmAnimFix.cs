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
            //��ȡ��Ӧ����
            Transform leftArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftArm.localEulerAngles = rotation;
            //��ֵIK��
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftArm.localEulerAngles));//Quaternion.Euler��ŷ��ת��Ϊ��Ԫ��
        }
    }
}
