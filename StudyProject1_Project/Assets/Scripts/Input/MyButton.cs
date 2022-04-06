using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    private bool isPressing = false;        //持续按下
    private bool isLongPressing = false;    //是否长按
    private bool onPressed = false;         //单击
    private bool onDoublePressed = false;   //双击
    private bool onTriplePressed = false;   //三击
    private bool onReleased = false;        //抬起
    private bool isExtending = false;       //可连击状态
    private bool isDelaying = false;        //等待长按
    private bool onTap = false;             //轻击

    private bool curState = false;  //false means release
    private bool lastState = false;

    private MyTimer extendTimer = new MyTimer();
    private float extendingDuration = 0.3f;            //可连击时间

    private MyTimer delayTimer = new MyTimer();
    private float delayingDuration = 0.3f;

    private int doubleCount = 0;
    private int tripleCount = 0;

    public void Tick(bool input)
    {
        extendTimer.Tick();
        delayTimer.Tick();

        //当前状态
        curState = input;

        //是否持续按下
        isPressing = curState;


         onPressed = false;
        onReleased = false;
        onDoublePressed = false;
        onTriplePressed = false;
        onTap = false;

        if (curState != lastState)
        {
            //是否按下
            onPressed = curState;
            if (onPressed)
            {
                extendTimer.Go(extendingDuration);
                delayTimer.Go(delayingDuration);
                //连击
                doubleCount++;
                tripleCount++;
            }

            //是否抬起
            onReleased = !curState;
            if (onReleased && delayTimer.IsRun())
            {
                delayTimer.Reset();
                onTap = true;
            }

            //是否双、三击
            if(doubleCount == 2)
            {
                onDoublePressed = true;
                doubleCount = 0;
            }
            if (tripleCount == 3)
            {
                onTriplePressed = true;
                tripleCount = 0;
            }


            //Debug.Log(onPressed ? "On Pressed." : "On Released.");
            //if (onDoublePressed || onTriplePressed)
            //    Debug.Log(onDoublePressed ? "On Double Pressed." : "On Triple Pressed.");
        }

        //可连击状态
        isExtending = extendTimer.IsRun();
        if (extendTimer.IsFinished())
        {
            //重置连击
            doubleCount = 0;
            tripleCount = 0;
        }

        //可长按状态
        isDelaying = extendTimer.IsFinished();

        //长按
        bool tmp = IsLongPressing;
        isLongPressing = isDelaying && IsPressing;

        lastState = curState;
    }

    /// <summary>
    /// 按下（持续）
    /// </summary>
    public bool IsPressing
    {
        get
        {
            return this.isPressing;
        }
    }

    /// <summary>
    /// 长按
    /// </summary>
    public bool IsLongPressing
    {
        get
        {
            return this.isLongPressing;
        }
    }

    /// <summary>
    /// 按下
    /// </summary>
    public bool OnPressed
    {
        get
        {
            return this.onPressed;
        }
    }

    /// <summary>
    /// 双击
    /// </summary>
    public bool OnDoublePressed
    {
        get
        {
            return this.onDoublePressed;
        }
    }

    /// <summary>
    /// 三击
    /// </summary>
    public bool OnTriplePressed
    {
        get
        {
            return this.onTriplePressed;
        }
    }

    /// <summary>
    /// 抬起
    /// </summary>
    public bool OnReleased
    {
        get
        {
            return this.onReleased;
        }
    }

    /// <summary>
    /// 是否可连击
    /// </summary>
    public bool IsExtending
    {
        get
        {
            return this.isExtending;
        }
    }

    /// <summary>
    /// 是否可长按
    /// </summary>
    public bool IsDelaying
    {
        get
        {
            return this.isDelaying;
        }
    }
    
    /// <summary>
    /// 轻击
    /// </summary>
    public bool OnTap
    {
        get
        {
            return this.onTap;
        }
    }
}
