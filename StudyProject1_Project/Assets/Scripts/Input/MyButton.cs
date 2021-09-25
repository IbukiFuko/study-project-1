using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    private bool isPressing = false;        //��������
    private bool isLongPressing = false;    //�Ƿ񳤰�
    private bool onPressed = false;         //����
    private bool onDoublePressed = false;   //˫��
    private bool onTriplePressed = false;   //����
    private bool onReleased = false;        //̧��
    private bool isExtending = false;       //������״̬
    private bool isDelaying = false;        //�ȴ�����
    private bool onTap = false;             //���

    private bool curState = false;  //false means release
    private bool lastState = false;

    private MyTimer extendTimer = new MyTimer();
    private float extendingDuration = 0.3f;            //������ʱ��

    private MyTimer delayTimer = new MyTimer();
    private float delayingDuration = 0.3f;

    private int doubleCount = 0;
    private int tripleCount = 0;

    public void Tick(bool input)
    {
        extendTimer.Tick();
        delayTimer.Tick();

        //��ǰ״̬
        curState = input;

        //�Ƿ��������
        isPressing = curState;


         onPressed = false;
        onReleased = false;
        onDoublePressed = false;
        onTriplePressed = false;
        onTap = false;

        if (curState != lastState)
        {
            //�Ƿ���
            onPressed = curState;
            if (onPressed)
            {
                extendTimer.Go(extendingDuration);
                delayTimer.Go(delayingDuration);
                //����
                doubleCount++;
                tripleCount++;
            }

            //�Ƿ�̧��
            onReleased = !curState;
            if (onReleased && delayTimer.IsRun())
            {
                delayTimer.Reset();
                onTap = true;
            }

            //�Ƿ�˫������
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
            if (onDoublePressed || onTriplePressed)
                Debug.Log(onDoublePressed ? "On Double Pressed." : "On Triple Pressed.");
        }

        //������״̬
        isExtending = extendTimer.IsRun();
        if (extendTimer.IsFinished())
        {
            //��������
            doubleCount = 0;
            tripleCount = 0;
        }

        //�ɳ���״̬
        isDelaying = extendTimer.IsFinished();

        //����
        bool tmp = IsLongPressing;
        isLongPressing = isDelaying && IsPressing;

        lastState = curState;
    }

    /// <summary>
    /// ���£�������
    /// </summary>
    public bool IsPressing
    {
        get
        {
            return this.isPressing;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public bool IsLongPressing
    {
        get
        {
            return this.isLongPressing;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public bool OnPressed
    {
        get
        {
            return this.onPressed;
        }
    }

    /// <summary>
    /// ˫��
    /// </summary>
    public bool OnDoublePressed
    {
        get
        {
            return this.onDoublePressed;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public bool OnTriplePressed
    {
        get
        {
            return this.onTriplePressed;
        }
    }

    /// <summary>
    /// ̧��
    /// </summary>
    public bool OnReleased
    {
        get
        {
            return this.onReleased;
        }
    }

    /// <summary>
    /// �Ƿ������
    /// </summary>
    public bool IsExtending
    {
        get
        {
            return this.isExtending;
        }
    }

    /// <summary>
    /// �Ƿ�ɳ���
    /// </summary>
    public bool IsDelaying
    {
        get
        {
            return this.isDelaying;
        }
    }
    
    /// <summary>
    /// ���
    /// </summary>
    public bool OnTap
    {
        get
        {
            return this.onTap;
        }
    }
}
