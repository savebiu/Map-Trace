using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActorControler : MonoBehaviour
{
    //����ģ��
    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public float WalkSpeed = 3.0f;
    public float RunSpeed = 4.0f;
    private CapsuleCollider col;
    //��Ծ����
    public float JumpVelocity = 4.0f;
    //��������
    public float jbMultiplier = 3.0f;
    //��������
    public float RollVelocity = 3.0f;
    //��������
    //private float lerpTarget;
    //Root Motion��ֵ
    private Vector3 deltaPos;



    [SerializeField]
    //����
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 PlanarVec;
    private Vector3 thrustVec;
    //λ����
    public bool lockPlanar = false;
    //�����ж�
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
        //��
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), (pi.run ? 2.0f : 1.0f), 0.5f));

        //����ת����Ϊ��,�Ͳ��������ת
        if(camcon.lockState == false){

            if (pi.Dmag > 0.1f)
            {
                //������ֵ���͵��������Լ������            
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.1f);//��ת
            }
            if(lockPlanar == false)
            {
                PlanarVec = pi.Dmag * model.transform.forward * WalkSpeed * (pi.run ? RunSpeed : 1.0f);//transform.forward��ǰ(z��)�ƶ�
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
        
        //falseδ��λ��
        if (lockPlanar == false)
        {
        }

        //��
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        //����
        if (pi.jumpback)
        {
            anim.SetTrigger("jumpBack");
            canAttack = false;
        }
        //����
        if (pi.roll)//������������
        {
            anim.SetTrigger("roll");
            canAttack = false ;
        }
        //����
        //�ȼ���Ƿ�������״̬
        if (pi.attack && (CheckState("ground") || CheckStateTag("attack") ) && canAttack)
        {
            anim.SetTrigger("attack");

        }
        //����
        anim.SetBool("defense", pi.defense);

        //�ӽ�����
        if(pi.lockon == true)
        camcon.LockUp();
 
        
    }
    //1/50�����һ��
    void FixedUpdate()
    {
        //��rigibody���ϸ��˶��Ĳ�ֵ
        rigid.position += deltaPos; 
        //�����ƶ�����
        //rigid.position += moveingVec * Time.fixedDeltaTime;//�޸�λ��.��Ҫ�����ٶ�
        rigid.velocity = new Vector3(PlanarVec.x, rigid.velocity.y, PlanarVec.z) + thrustVec;//velocity�Ǹ�����ٶ�����
        //������Ծ����
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;//��ֵ����
    }

    //����ͼ��ͱ�ǩ
    private bool CheckState(string stateName, string layerName = "Base Layer"){
        //��ȡҪ���ҵ�״̬
        int layerIndex = anim.GetLayerIndex(layerName);
        //��ȡ����animation��״̬,���ȶ��Ƿ������������ͬ
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }
    private bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        //��ȡҪ���ҵ�״̬
        int layerIndex = anim.GetLayerIndex(layerName);
        //��ȡ����animation��״̬,���ȶ��Ƿ������������ͬ
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
        return result;
    }
    /// <summary>
    /// Message processing block
    /// </summary>


    //��Ծʱ�̶����� falseʱtargetDup,targetDright��Ϊ0,�̶�����,����Ҳ��λ�ù̶�ס��
    public void OnJumpEnter()
    {
        //����������
        pi.InputEnable = false;
        //λ��������
        lockPlanar = true;
        //��ֵ����
        thrustVec = new Vector3(0, JumpVelocity, 0);
    }
    /*public void OnJumpExit()
    {
        pi.InputEnable = true;
        lockPlanar = false;
    }*/

    public void IsGround()
    {
        //print("���ڵ���Ŷ");
        anim.SetBool("isGround", true);
        col.material = frictionOne;

    }
    public void IsAir()
    {
        //print("�ߺ������");
        anim.SetBool("isGround", false);
        col.material = frictionZero;
    }

    //�����������ٶ�����
    public void OnGroundEnter()
    {
        canAttack = true;
        pi.InputEnable = true;
        lockPlanar = false;
    }
    //�뿪����
    public void OnGroundExit()
    {

    }
    //�������״̬
    public void OnFallEnter()
    {
        pi.InputEnable = false;
        lockPlanar = true;
    }
    //���뷭��
    public void OnRollEnter()
    {
        //����������
        pi.InputEnable = false;
        //λ��������
        lockPlanar = true;
        //��ֵ����
        thrustVec = new Vector3(0, RollVelocity, 0);
    }
    //�������״̬
    public void OnJumpBackEnter() 
    {
        //����������
        pi.InputEnable = false;
        //λ��������
        lockPlanar = true;
    }  
    //������Ծ״̬
    public void OnJumpUpdate()
    {
        //��ֵ����
        thrustVec = model.transform.forward * anim.GetFloat("jbVelocity") * jbMultiplier;
    }
    //���빥��״̬
    public void OnAttack1HandEnter()
    {
        pi.InputEnable = false;
        //lockPlanar = true;
        //��ȡ����Ȩ��
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 1.0f);
        //lerpTarget = 1.0f;

    }
    //����״̬ʵʱ����
    public void OnAttack1HandUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1HandVelocity");
        ////��ȡ��ǰ����Ȩ��
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        ////��������
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.4f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);//��ֵ������

    }
    //���뾲ֹ״̬
    //public void OnAttackIdleEnter()
    //{
    //    pi.InputEnable = true;
    //    //lockPlanar = false;
    //    //anim.SetLayerWeight(1, 0);
    //    //lerpTarget = 0;
    //}

    ////�������
    //public void OnDefenseEnter()
    //{
    //    pi.InputEnable = false;
    //    //��ȡͼ��
    //    float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Defense"));

    //    //����Ȩ��
    //    anim.SetLayerWeight(anim.GetLayerIndex("Defense"), 1.0f);
    //}
    public void OnAttackIdleUpdate()
    {
        ////����Idle�Ժ󽫻�����Ϊ��
        ////��ȡ��ǰ����Ȩ��
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        ////��������
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.4f);
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);//��ֵ������
    }
    public void OnUpdateRootMotion(object _deltalPosition)
    {
        if(CheckState("attack3Hand"))
        {
            deltaPos += (Vector3)_deltalPosition;

        }
    }

}
