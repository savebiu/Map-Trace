using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActorControler : MonoBehaviour
{
    //人物模型
    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public float WalkSpeed = 3.0f;
    public float RunSpeed = 4.0f;
    private CapsuleCollider col;
    //跳跃冲量
    public float JumpVelocity = 4.0f;
    //后跳冲量
    public float jbMultiplier = 3.0f;
    //翻滚冲量
    public float RollVelocity = 3.0f;
    //攻击缓动
    //private float lerpTarget;
    //Root Motion差值
    private Vector3 deltaPos;



    [SerializeField]
    //动画
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 PlanarVec;
    private Vector3 thrustVec;
    //位置锁
    public bool lockPlanar = false;
    //攻击判断
    private bool canAttack;

    [Header("friction Setting")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;


    void Awake()
    {
        pi = GetComponent<IUserInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

    }

    void Update()
    {
        //跑
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), (pi.run ? 2.0f : 1.0f), 0.5f));

        //当旋转长度为正,就不会向后旋转
        if(camcon.lockState == false){

            if (pi.Dmag > 0.1f)
            {
                //将浮点值发送到动画器以激活过渡            
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.1f);//旋转
            }
            if(lockPlanar == false)
            {
                PlanarVec = pi.Dmag * model.transform.forward * WalkSpeed * (pi.run ? RunSpeed : 1.0f);//transform.forward向前(z轴)移动
            }
        }
        else
        {
            model.transform.forward = transform.forward;
            if(lockPlanar == false)
            {
                PlanarVec = pi.Dvec * WalkSpeed * ((pi.run) ? 2.0f : 1.0f);
            }
        }
        
        //false未锁位置
        if (lockPlanar == false)
        {
        }

        //跳
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        //后跳
        if (pi.jumpback)
        {
            anim.SetTrigger("jumpBack");
            canAttack = false;
        }
        //翻滚
        if (pi.roll)//返回向量长度
        {
            anim.SetTrigger("roll");
            canAttack = false ;
        }
        //攻击
        //先检测是否有其他状态
        if (pi.attack && (CheckState("ground") || CheckStateTag("attack") ) && canAttack)
        {
            anim.SetTrigger("attack");

        }
        //防御
        anim.SetBool("defense", pi.defense);

        //视角锁定
        if(pi.lockon == true)
        camcon.LockUp();
 
        
    }
    //1/50秒更新一次
    void FixedUpdate()
    {
        //给rigibody加上根运动的差值
        rigid.position += deltaPos; 
        //两种移动方法
        //rigid.position += moveingVec * Time.fixedDeltaTime;//修改位置.需要乘以速度
        rigid.velocity = new Vector3(PlanarVec.x, rigid.velocity.y, PlanarVec.z) + thrustVec;//velocity是刚体的速度向量
        //归零跳跃冲量
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;//差值清零
    }

    //查找图层和标签
    private bool CheckState(string stateName, string layerName = "Base Layer"){
        //获取要查找的状态
        int layerIndex = anim.GetLayerIndex(layerName);
        //获取现在animation的状态,并比对是否与我所查的相同
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }
    private bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        //获取要查找的状态
        int layerIndex = anim.GetLayerIndex(layerName);
        //获取现在animation的状态,并比对是否与我所查的相同
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
        return result;
    }
    /// <summary>
    /// Message processing block
    /// </summary>


    //跳跃时固定方向 false时targetDup,targetDright都为0,固定方向,但是也把位置固定住了
    public void OnJumpEnter()
    {
        //方向锁锁死
        pi.InputEnable = false;
        //位置锁解锁
        lockPlanar = true;
        //赋值冲量
        thrustVec = new Vector3(0, JumpVelocity, 0);
    }
    /*public void OnJumpExit()
    {
        pi.InputEnable = true;
        lockPlanar = false;
    }*/

    public void IsGround()
    {
        //print("我在地上哦");
        anim.SetBool("isGround", true);
        col.material = frictionOne;

    }
    public void IsAir()
    {
        //print("芜湖，起飞");
        anim.SetBool("isGround", false);
        col.material = frictionZero;
    }

    //进入地面清除速度向量
    public void OnGroundEnter()
    {
        canAttack = true;
        pi.InputEnable = true;
        lockPlanar = false;
    }
    //离开地面
    public void OnGroundExit()
    {

    }
    //进入掉落状态
    public void OnFallEnter()
    {
        pi.InputEnable = false;
        lockPlanar = true;
    }
    //进入翻滚
    public void OnRollEnter()
    {
        //方向锁锁死
        pi.InputEnable = false;
        //位置锁解锁
        lockPlanar = true;
        //赋值冲量
        thrustVec = new Vector3(0, RollVelocity, 0);
    }
    //进入后跳状态
    public void OnJumpBackEnter() 
    {
        //方向锁锁死
        pi.InputEnable = false;
        //位置锁解锁
        lockPlanar = true;
    }  
    //进入跳跃状态
    public void OnJumpUpdate()
    {
        //赋值冲量
        thrustVec = model.transform.forward * anim.GetFloat("jbVelocity") * jbMultiplier;
    }
    //进入攻击状态
    public void OnAttack1HandEnter()
    {
        pi.InputEnable = false;
        //lockPlanar = true;
        //获取动画权重
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 1.0f);
        //lerpTarget = 1.0f;

    }
    //攻击状态实时跟新
    public void OnAttack1HandUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1HandVelocity");
        ////获取当前动作权重
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        ////缓动处理
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.4f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);//赋值给动画

    }
    //进入静止状态
    //public void OnAttackIdleEnter()
    //{
    //    pi.InputEnable = true;
    //    //lockPlanar = false;
    //    //anim.SetLayerWeight(1, 0);
    //    //lerpTarget = 0;
    //}

    ////进入防御
    //public void OnDefenseEnter()
    //{
    //    pi.InputEnable = false;
    //    //获取图层
    //    float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Defense"));

    //    //设置权重
    //    anim.SetLayerWeight(anim.GetLayerIndex("Defense"), 1.0f);
    //}
    public void OnAttackIdleUpdate()
    {
        ////进入Idle以后将缓动变为零
        ////获取当前动作权重
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        ////缓动处理
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.4f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);//赋值给动画
    }
    public void OnUpdateRootMotion(object _deltalPosition)
    {
        if(CheckState("attack3Hand"))
        {
            deltaPos += (Vector3)_deltalPosition;

        }
    }

}
