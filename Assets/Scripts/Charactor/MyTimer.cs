using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    //监听时间的三个状态
    public enum STATE { 
        IDLE,
        RUN,
        FINISHED
    }
    public STATE state;
    //监听等待时间
    public float duration = 1.0f;
    //初始时间
    private float elapsedTime = 0f;
    public void Tick()
    {
        //Debug.Log(Time.deltaTime);
        if(state == STATE.IDLE)
        {
            
        }
        else if(state == STATE.RUN)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= duration)
            {
                state = STATE.FINISHED;
            }
            
        }
        else if(state == STATE.FINISHED){

        }
        else {
            Debug.Log("MyTimer error");
        }
    }
    public void Go()
    {
        elapsedTime = 0f;
        state = STATE.RUN; 
        
    }

}
