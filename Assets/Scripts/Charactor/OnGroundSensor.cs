using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capsule;

    private Vector3 point1;
    private Vector3 point2;

    private float radius = 0.1f;
    public float offset =1.2f;
    void Awake()
    {
        radius = capsule.radius;
    }
    void FixedUpdate()
    {
        point1 = transform.position + transform.up * radius;
        point2 = transform.position + transform.up * capsule.height - transform.up * radius - transform.up * offset;
        
        //�����ײ,����������ͼ��뾶
        Collider[] outputcollider = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));//����Ƿ��Groundͼ������
        if (outputcollider.Length != 0)
        {
            //��ӡ��ײ��
            /*foreach (var collider in outputcollider)
            {
                print(collider.name);
            }*/
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsAir");
        }
    }
    void OawGizmos()
    {        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point1, radius);
    }
}
