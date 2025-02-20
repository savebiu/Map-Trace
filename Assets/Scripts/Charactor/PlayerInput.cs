using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IUserInput
{
    
    //�ֱ���ťţ����
    //��ΪMyButton���е��������
    //public MyButton btnW = new MyButton();
    //public MyButton btnS = new MyButton();
    //public MyButton btnA = new MyButton();
    //public MyButton btnD = new MyButton();
    //public MyButton btnAttack = new MyButton();
    //public MyButton btnDefense = new MyButton();
    //public MyButton btnRun = new MyButton();
    //public MyButton btnRoll = new MyButton();
    //public MyButton btnJump = new MyButton();
    //public MyButton btnJumpBack = new MyButton();

    [Header("___Key Setting____")]
    //��ɫ�ƶ�
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyRight = "d";
    public string KeyLeft = "a";
    public string KeyRun = "left shift";
    public string KeyJump = "space";
    public string KeyJumpBack = "v";
    public string KeyRoll = "c";
    public string KeyAttack = "mouse 0";
    public string KeyDefense = "mouse 1";
    public string KeyLockOn = "q";

    //�ӽ���ת
    public string KeyJUp;
    public string KeyJDown;
    public string KeyJLeft;
    public string KeyJRight;
   

    //������л�
    public bool mouseEnable;
    public float mouseSpeed;

    private MyTimer extTimer = new MyTimer();


    // Update is called once per frame
    void Update()
    {
        //�������
        targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);

        if (mouseEnable == true)
        {
            Jup = mouseSpeed * 2 * Input.GetAxis("Mouse Y");
            Jright = mouseSpeed * 3 *Input.GetAxis("Mouse X");
        }
        else
        {
            //��ת�ӽ�
            Jup = (Input.GetKey(KeyJUp) ? 1.0f : 0f) - (Input.GetKey(KeyJDown) ? 1.0f : 0f);
            Jright = (Input.GetKey(KeyJRight) ? 1.0f : 0f) - (Input.GetKey(KeyJLeft) ? 1.0f : 0f);
        }

        //�ٶ���������
        if (InputEnable == false)
        {
            targetDup = 0.0f;
            targetDright = 0.0f;

        }
        //ƽ��ҡ��
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        //ƽ��б�����ٶ�
        Vector2 tempAxis = SquareToCircle(new Vector2(Dup, Dright));
        float Dup2 = tempAxis.x;
        float Dright2 = tempAxis.y;

        Dmag = Mathf.Sqrt(Dup2 * Dup2) + (Dright2 * Dright2);//Dmag��ʾ���򳤶�ֵ
        Dvec = Dright * transform.right + Dup * transform.forward;

        //��ʱ��
        if (Input.GetKeyDown(KeyCode.P))
        {
            extTimer.duration = 1.0f;
            extTimer.Go();
         }
        extTimer.Tick();
        //Debug.Log(extTimer.state);

        ////�ֱ�
        ////��
        //btnRun.Tick(Input.GetButton(KeyRun));
        //run = btnRun.IsPressing;

        ////��        
        //btnJump.Tick(Input.GetButton(KeyJump));
        //jump = btnJump.OnPressed;

        ////����
        //btnJumpBack.Tick(Input.GetButton(KeyJumpBack));
        //jumpback = btnJumpBack.OnPressed;

        ////����
        //btnRoll.Tick(Input.GetButton(KeyRoll));
        //roll = btnRoll.IsPressing;

        ////����
        //btnAttack.Tick(Input.GetButton(KeyAttack));
        //attack = btnAttack.OnPressed;

        ////����
        //btnDefense.Tick(Input.GetButton(KeyDefense));
        //defense = btnDefense.OnPressed;

        //�������
        //��
        run = Input.GetKey(KeyRun);

        //��        
        jump = Input.GetKeyDown(KeyJump);

        //����
        jumpback = Input.GetKeyDown(KeyJumpBack);

        //����
        roll = Input.GetKeyDown(KeyRoll);        
        
        //����
        attack = Input.GetKeyDown(KeyAttack);

        //����
        defense = Input.GetKey(KeyDefense);

        //�����ӽ�
        lockon = Input.GetKeyDown(KeyLockOn);
        //print("lockon is " + lockon);
    }
}
