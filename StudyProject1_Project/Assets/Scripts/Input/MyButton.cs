using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    private bool isPressing = false;
    private bool onPressed = false;
    private bool onReleased = false;

    private bool curState = false;  //false means release
    private bool lastState = false;

    public void Tick(bool input)
    {
        curState = input;

        isPressing = curState;

        if(curState != lastState)
        {
            onPressed = curState;
            onReleased = !curState;
            //Debug.Log(onPressed ? "On Pressed." : "On Released.");
        }

        lastState = curState;
    }




    public bool IsPressing
    {
        get
        {
            return this.isPressing;
        }
    }

    public bool OnPressed
    {
        get
        {
            return this.onPressed;
        }
    }

    public bool OnReleased
    {
        get
        {
            return this.onReleased;
        }
    }
}
