using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    public enum STATE
    {
        IDLE,
        RUN,
        FINISHED,
    }

    private STATE state;

    private float duration = 1.0f;  //计时时长

    private float elapsedTime = 0;  //经过的时间

    public void Tick()
    {
        switch (state)
        {
            case STATE.IDLE:

                break;
            case STATE.RUN:
                elapsedTime += Time.deltaTime;
                if(elapsedTime >= duration)
                {
                    state = STATE.FINISHED;
                }
                break;
            case STATE.FINISHED:

                break;
            default:
                Debug.Log("MyTime error.");
                break;
        }
    }

    public void Go(float _duration)
    {
        duration = _duration;
        elapsedTime = 0;
        state = STATE.RUN;
    }

    public void Stop()
    {
        state = STATE.FINISHED;
    }

    public void Reset()
    {
        state = STATE.IDLE;
    }

    public bool IsRun()
    {
        return state == STATE.RUN;
    }

    public bool IsFinished()
    {
        return state == STATE.FINISHED;
    }


    public STATE State
    {
        get
        {
            return this.state;
        }
    }

    public float Duration
    {
        get
        {
            return this.duration;
        }
    }

    public float ElapsedTime
    {
        get
        {
            return this.elapsedTime;
        }
    }
}
