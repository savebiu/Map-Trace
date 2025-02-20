using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    //����ʱ�������״̬
    public enum STATE { 
        IDLE,
        RUN,
        FINISHED
    }
    public STATE state;
    //�����ȴ�ʱ��
    public float duration = 1.0f;
    //��ʼʱ��
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
