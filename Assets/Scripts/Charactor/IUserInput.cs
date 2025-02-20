using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("____Output____")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public bool run = false;
    public bool jump = false;
    public bool jumpback = false;
    public bool roll;
    public float Jup;
    public float Jright;
    public bool attack;
    public bool defense = false;
    public bool lockon = false;
    


    [Header("____Other____")]
    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;

    //方向锁
    public bool InputEnable = true;

    //调整斜方向向量 
    protected Vector2 SquareToCircle(Vector2 input)

    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2);

        return output;
    }
}
