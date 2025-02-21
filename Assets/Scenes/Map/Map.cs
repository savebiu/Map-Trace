using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
目标:在地图上显示玩家位置
    1.Canva和小地图
    2.在Resources文件夹中存储玩家图标预制体
    3.实例化玩家图标
    3.固定图标位置
    4.设置图标跟随玩家旋转(玩家旋转值绕y轴,但是小地图为平面,旋转值绕z轴,角色旋转y赋值给图标旋转z)

目标:增设敌人位置
    1.使用NavMeshAgent组件建跟踪敌人
    2.在Map_Track脚本中调取Map从而获取怪物位置
    3.判断地图上是否应该显示敌人
    4.更新敌人位置
*/

public class Map : MonoBehaviour
{
    private RectTransform rect; //Canvas组件
    private Transform player;       //玩家位置
    private GameObject playerIconPrefab;    //小地图上的玩家图标
    private GameObject playerImage;    //玩家图标
    private GameObject enemyImage;    //敌人图标

    private GameObject enemyIconPrefab;    //敌人图标预制体
    private Dictionary<Transform, GameObject> enemyIcons = new Dictionary<Transform, GameObject>();    //敌人图标字典


    void Start()
    {
        rect = GetComponent<RectTransform>();

        //加载玩家和敌人图标
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerIconPrefab = Resources.Load<GameObject>("PlayerPrefab");   //加载玩家图标
        enemyIconPrefab = Resources.Load<GameObject>("EnemyPrefab");     //加载敌人图标        
        
        playerImage = Instantiate(playerIconPrefab, transform);        //实例化玩家图标预制体
        enemyImage = Instantiate(enemyIconPrefab, transform);        //实例化敌人图标预制体
        playerImage.transform.localPosition = Vector3.zero;     //固定玩家到小地图中间

    }    
    void Update()
    {
        ShowPlayer();   //显示玩家位置
        UpdateEnemies();    //更新敌人位置
    }

    private void ShowPlayer(){
       
        playerImage.transform.localPosition = Vector3.zero;     //固定玩家到小地图中间
        //playerImage.transform.SetParent(transform, false );    //设置玩家图标的父物体
        playerImage.transform.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);    //设置玩家图标的旋转角度
    }

    //判断地图上是否显示敌人    
    public void RegisterEnemy(Transform enemy){  //怪物位置, 地图上怪物的显示状况
        //需要显示则创建怪物图标
        if (!enemyIcons.ContainsKey(enemy))
        {     //如果敌人图标字典中没有该敌人,则创建
            GameObject enemyImage = Instantiate(enemyIconPrefab, transform);    //实例化敌人图标
            
            //enemyImage.transform.SetParent(transform, false);    //设置敌人图标的父物体
            enemyIcons.Add(enemy, enemyImage);    //添加到敌人图标字典
        }
    }                   
    private void UpdateEnemies(){

        Vector2 mapSize = rect.sizeDelta;    //获取小地图大小
        float worldToMapRatio = mapSize.x / 10f;    //计算世界坐标到小地图坐标的比例

        foreach (var enemy in enemyIcons.Keys){
            //该类写法是将enemy的位置生成在世界坐标上
            //Vector3 enemyPos = enemy.position;    //获取敌人位置
            //enemyIcons[enemy].transform.position = new Vector3(enemyPos.x, enemyPos.z, 0); //设置敌人图标的位置
            
            Vector3 relativePos = enemy.position - player.position;    //获取相对位置
            //Vector3 playerWorldPos = player.position;  //获取玩家位置

            // **调整比例**: 这里假设世界坐标的 10 个单位映射到小地图的 mapSize.x

            Vector2 miniMapPos = new Vector2(relativePos.x, relativePos.z) / worldToMapRatio;    //计算敌人在小地图上的位置

            //Vector3 relativePos = enemyWorldPos - playerWorldPos; // 计算相对位置

            enemyIcons[enemy].GetComponent<RectTransform>().anchoredPosition = miniMapPos; // 更新图标位置
            enemyIcons[enemy].transform.rotation = Quaternion.Euler(0, 0, -enemy.eulerAngles.y); // 旋转


        }
    }

}
