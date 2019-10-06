using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//產生怪物的腳本
public class EnemyCreater : MonoBehaviour
{
    public GameObject EnemyPrefab;//放置敵人的預置物

    //開場階段60秒，每波為30秒，每波出怪20隻(其餘為等待時間)
    float NowTime = 0f;        //遊戲時間
    float TimeDelay = 0f;      //遊戲開場等待階段
    float EnemyWaveTime = 10f;  //每一波怪的時間
    int EnemyNum = 10;          //每波出怪數量
    float Waiter =0.5f;        //出怪的間隔時間
    int EnemyWave = 0;         //目前是第幾波怪(0是開場，1是第一波，以此類推)，讓其他腳本來使用增加怪的強度

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //開場階段倒數，之後每段時間就執行下個指令(一段時間會出一次怪)
        if (TimeDelay <= 0f )
        {
            StartCoroutine(EnemyLevel());//延遲的特殊指令(1/3)
            TimeDelay = EnemyWaveTime;
        }
        TimeDelay -= Time.deltaTime;
    }


   
    IEnumerator EnemyLevel()//延遲的特殊指令(2/3)
    {
        //每一波出現固定數量的怪
        EnemyWave++;  //每一波的等級，其他腳本來使用增加怪的強度
        for (int i = 0; i < EnemyNum; i++)
        {
            EnemyAppear();
            yield return new WaitForSeconds(Waiter);//延遲的特殊指令(3/3)
        }
    }


    //產生怪的預置物
    void EnemyAppear()
    {  
            Instantiate(EnemyPrefab, GameObject.Find("Start").transform.position, Quaternion.identity);
    }


    
}
