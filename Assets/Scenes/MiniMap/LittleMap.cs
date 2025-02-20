using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    * 如何创建一个如图Apex中的小地图？
    * 1. 使用Camera实时地图，适用于开放世界和大型地图
    创建Camera
    
    * 
*/


public class LittleMap : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if(player != null){
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;    //保持小地图的y轴不变
            transform.position = newPosition;       //更新小地图的位置
        }
    }

}
