using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MyButton
{
    //三种按钮状态
    public bool IsPressing = false;
    public bool OnPressed = false;
    public bool OnReleased = false;

    //当前状态
    private bool curState = false;
    //最后状态
    private bool lastState = false;
    private MyTimer extTimer = new MyTimer();

    [Header("Setting")]
    private float extendingDuration = 0.3f;
    

    public void Tick(bool input)
    {        
        curState = input;
        IsPressing = curState;

        OnPressed = false;
        //先判断当前状态是否等于结束状态
        if(curState != lastState)
        {
            if(IsPressing == true)
            {
                OnPressed = true;
            }
            else
            {
                OnReleased = true;
                StartTimer(extTimer, extendingDuration);
            }

        }
        lastState = curState;
        if(extTimer.state == MyTimer.STATE.RUN)
        {
            IsPressing = true;
        }
        else
        {
            IsPressing = false;
        }
    }
    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.Go();
    }

}
