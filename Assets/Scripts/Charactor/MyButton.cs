using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MyButton
{
    //���ְ�ť״̬
    public bool IsPressing = false;
    public bool OnPressed = false;
    public bool OnReleased = false;

    //��ǰ״̬
    private bool curState = false;
    //���״̬
    private bool lastState = false;
    private MyTimer extTimer = new MyTimer();

    [Header("Setting")]
    private float extendingDuration = 0.3f;
    

    public void Tick(bool input)
    {        
        curState = input;
        IsPressing = curState;

        OnPressed = false;
        //���жϵ�ǰ״̬�Ƿ���ڽ���״̬
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
