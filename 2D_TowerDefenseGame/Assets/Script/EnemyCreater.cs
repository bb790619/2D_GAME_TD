using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//產生怪物的腳本，放在"EnemyCreater"上
public class EnemyCreater : MonoBehaviour
{
    public GameObject EnemyPrefab;//放置"敵人"的Prefab

    //開場階段15秒，每波為30秒，每波出怪20隻(其餘為等待時間)
    public static float TimeDelay = 20f;      //遊戲開場等待階段，讓<UIControl>來使用
    public static float EnemyWaveTime = 30f; //每一波怪的時間，讓<UIControl>來使用
    int EnemyNum = 20;                       //每波出怪數量

    float Waiter = 0.5f;                     //出怪的間隔時間
    public static int EnemyWave = 0;         //目前是第幾波怪(0是開場，1是第一波，以此類推)，讓<UIControl>來使用
    public static int EnemyEnd = 2;         //出現幾波怪就結束遊戲
    int EenmySerialNum;                      //怪物的流水號

    // Start is called before the first frame update
    void Start()
    {
        //重新開始時，參數復歸
        EnemyWave = 0;
        TimeDelay = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("難度視窗") == null) //難度視窗消失才能執行
        {
            if (EnemyWave < EnemyEnd)//限制出怪波數
            {
                //開場階段倒數，之後每段時間就執行下個指令(一段時間會出一次怪)
                if (TimeDelay <= 0f)
                {
                    StartCoroutine(EnemyLevel());//延遲的特殊指令(1/3)
                    TimeDelay = EnemyWaveTime;
                }
                TimeDelay -= Time.deltaTime;
            }
        }
    }

    IEnumerator EnemyLevel()//延遲的特殊指令(2/3)
    {
        //每一波出現固定數量的怪
        EnemyWave++;  //每一波的等級，其他腳本來使用增加怪的強度
        for (int i = 0; i < EnemyNum; i++)
        {
            EenmySerialNum = i + 1;
            EnemyAppear();
            yield return new WaitForSeconds(Waiter);//延遲的特殊指令(3/3)
        }
    }


    //產生怪的預置物
    void EnemyAppear()
    {
        //產生怪物，命名為怪物+波數+_流水號
        Instantiate(EnemyPrefab, GameObject.Find("Start").transform.position, Quaternion.identity).name = "怪物" + EnemyWave + "_" + EenmySerialNum;
        //更換怪物圖片
        for (int i = 1; i < EnemyWave + 1; i++)
        {
            if (EnemyWave == i) GameObject.Find("怪物" + EnemyWave + "_" + EenmySerialNum).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Monster/" + i);
            //GameObject.Find("怪物" + EnemyWave + "_" + EenmySerialNum).GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Monster/怪物")[i-1]; //讀取切割的圖片(LoadAll)
        }

    }



}
