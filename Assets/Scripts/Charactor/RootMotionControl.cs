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

    //OnAnimationMove是用于控制RootMotion 的Unity API
    private void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRootMotion", (object)anim.deltaPosition);//deltaPosition 获取位置差值
    }
}
