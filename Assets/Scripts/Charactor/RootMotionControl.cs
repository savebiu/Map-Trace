using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //OnAnimationMove�����ڿ���RootMotion ��Unity API
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion", (object)anim.deltaPosition);//deltaPosition ��ȡλ�ò�ֵ
    }
}
