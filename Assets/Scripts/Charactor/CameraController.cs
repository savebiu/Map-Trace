using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public IUserInput pi;
    //�������µ���ת�ٶ�
    public float horizontalSpeed = 140.0f;
    public float verticalSpeed = 10.0f;
    public float cameraDampValue = 0.5f;


    private GameObject playerHandle;
    private GameObject cameraHandle;
    private GameObject model;
    public bool lockState;
    private GameObject cameras;

    public Image lockDot;

    //������
    [SerializeField]
    private GameObject lockTarget;

    private Vector3 cameraDampVelocity;
    private float tempEulerX = 10.0f;


    private void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        //��ȡ��ɫ�ĳ�ʼ����λ��
        model = playerHandle.GetComponent<ActorControler>().model;
        //��ͷ׼��
        //lockDot.enabled = false;
        lockState = false;
        cameras = Camera.main.gameObject;

    }

    private void FixedUpdate()
    {
        if (lockTarget == null)
        {
            //��ȡ��ɫ��ʼŷ����
            Vector3 tempModelEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
            //����ŷ���Ƕȵ��ٶ�
            tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
            //����ŷ���Ƕ�
            tempEulerX = Mathf.Clamp(tempEulerX, -20, 30);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
            //����ɫŷ��ʼ���Ǹ�ֵ��ȥ
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
        }
        

        cameras.transform.position = Vector3.SmoothDamp(cameras.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        //cameras.transform.eulerAngles = transform.eulerAngles;
        cameras.transform.LookAt(cameraHandle.transform);

        //�������
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LockUp()
    {
        //print("LockUp");
        //����
        //if(lockTarget == null)
        //{
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.position * 2f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f,10f), model.transform.rotation, LayerMask.GetMask("Enemy"));
        if(cols.Length == 0)
        {
            lockTarget = null;
            lockDot.enabled = false;
            lockState = false;
        }
        else
        {
            foreach (var col in cols)
            {
                if(lockTarget == col.gameObject)
                {
                    lockTarget = null;
                    lockDot.enabled = false;
                    lockState = false;
                    break;
                }
                //print(col.name);
                lockTarget = col.gameObject;
                lockDot.enabled = true;
                lockState = true;
                break;
            }
        }
        
        //}
        //����
        //else
        //{
        //    lockTarget = null;
        //}
    }
}
