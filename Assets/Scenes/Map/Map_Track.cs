using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
/*
目标:
如何实现怪物在某区域巡逻，当Player标签进入区域后再通过NavMeshAgent组件进行追踪。
由于角色速度高于怪物移动速度，当角色离开怪物的检测区域以后怪物返回巡逻区域

1.导入NaveMeshAgent并设置好跟踪对象
2.调整怪物SphereCollider的半径，设置为检测范围,并设置为trigger
3.设置巡逻点
4.通过状态机控制怪物的不同行为
*/



public class Map_Track: MonoBehaviour
{
    //设置NavMeshAgent组件
    public NavMeshAgent agent;      //调用NavMeshAgent组件
    public Transform player;

    //设置巡逻点
    public Transform[] patrolPoints;        //巡逻点数组
    private int currentPatrolIndex;     //当前巡逻点索引
    public float patrolWaitTime = 2f;       // 每次巡逻点的等待时间

    public float chaseRange = 1f;      //追逐范围
    public float returnDelay = 3f;            //失去目标后返回延迟
    public bool isChasing = false;     //是否追踪
    private bool returning = false;     //是否返回巡逻点

    private Map map;        //调用Map脚本


    void Start(){
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        map = FindObjectOfType<Map>();       //调用Map脚本
        currentPatrolIndex = 0;     //初始化巡逻点索引
        // StopAllCoroutines();        //停止所有协程
        StartCoroutine(Patrol());       //开始巡逻
    }

    void Update(){
        //追踪
        if(isChasing){
            agent.SetDestination(player.position);//SetDestination设置目标位置
            map.UpdateEnemyPosition(transform, true);        //更新怪物位置,并传入isVisiable为true
        }
        else{
            map.UpdateEnemyPosition(transform, false);       //更新怪物位置,并传入isVisiable为false
        }
        
    }

    //角色进入检测范围后开始追踪
    void OnTriggerEnter(Collider other){
        //Debug.Log($"检测到 {other.gameObject.name} 进入范围！");
        if(other.CompareTag("Player")){
            //Debug.Log("玩家进入检测范围，开始追踪！");
            isChasing = true;       
            returning = false;      //追踪时不返回巡逻点
            StopAllCoroutines();        //停止所有协程
            StopCoroutine(Patrol());        //StopAllCoroutines停止所有协同程序--与之对应的是StartCoroutine
        }
    }
    //角色离开范围后停止追踪
    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            isChasing = false;
            returning = true;
            StopAllCoroutines();        //停止所有协程//ReturnToPatrol开启巡逻协程
            StartCoroutine(ReturnToPatrol());  
        }
    }

    //返回巡逻点
    IEnumerator ReturnToPatrol()
    {
        yield return new WaitForSeconds(returnDelay);       //迭代, WaitForSeconds()等待指定秒数后开始执行
        returning = false;    //返回巡逻点
        StopAllCoroutines();        //停止所有协程
        StartCoroutine(Patrol());

    }
    
    IEnumerator Patrol()
    {
        while(!isChasing){
            
            while (agent.remainingDistance > agent.stoppingDistance) {     // 等待怪物到达目标点  // remainingDistance是目前位置与目标位置的剩余距离            
            //Debug.Log($"到达巡逻点{currentPatrolIndex}的距离为{agent.remainingDistance}");
                yield return null;      // stoppingDistance是到达目标点的刹车距离
            }
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);        //前往巡逻点
            yield return new WaitForSeconds(patrolWaitTime); 
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // 切换到下一个巡逻点
            //break;
            //Debug.Log($"切换到巡逻点{currentPatrolIndex}");
        }
    }
}
