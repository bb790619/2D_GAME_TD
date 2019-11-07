﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//控制遊戲場景的UI及勝利和失敗視窗，放在"UI"上
public class UIControl : MonoBehaviour
{
    ////參數設定//
    public static int PlayerHp;    //玩家血量
    public static int PlayerHpMax;  //玩家血量最大值
    public static int PlayerMoney; //玩家初始金錢  
    //[0-2]=角色1_LV1-LV3，[3-5]=角色2_LV1-LV3，[6-8]=角色3_LV1-LV3，[9-11]=角色4_LV1-LV3，以此類推
    //修改這邊的金額，<SpaceControl>會自動修改
    public static int[] Player_Price = { 45, 85, 140,
                                         35, 65, 120,
                                         35, 65, 120,
                                         30, 50, 100,
                                         25, 40,  80,
                                         40, 80, 130  };
    float NowTime = 0f;                   //下波怪出現的時間
    float Wave = 0f;                       //出怪的波數 
    public Image VictoryWindow;           //勝利視窗("勝利視窗")
    public Image GGWindow;                //失敗視窗("失敗視窗")
    public Image OptionWindow;            //暫停視窗("暫停視窗")

    public Animator EndAni;                //守門人的動畫

    public static int Chap, Level;  //這場遊戲的章節和關卡

    // Start is called before the first frame update
    void Start()
    {
        PlayerHpMax = 10 + StandByScene.TalentPoint[4] * 2;         //<StandByScene>的技能每升一級，玩家生命+2
        PlayerHp = PlayerHpMax;
        PlayerMoney = 200 + StandByScene.TalentPoint[2] * 50; ;//<StandByScene>的技能每升一級，玩家金錢+50

        //transform.Find 可以找到隱藏的物件
        VictoryWindow.transform.gameObject.SetActive(false);
        GGWindow.transform.gameObject.SetActive(false);
        OptionWindow.transform.gameObject.SetActive(false);

        Time.timeScale = 1;   //遊戲開始
        //GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
        //Invoke("Opening", 1f);        //遊戲場景會先淡出，開啟難度視窗，1秒後淡出消失，同時讓時間暫停

        NowTime = EnemyCreater.TimeDelay; //下波怪出現的時間
        Chap = 0; Level = 0; //先初始化，如果過關再讀取這場遊戲的章節和關卡
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("金錢TXT").GetComponent<Text>().text = PlayerMoney.ToString();  //顯示金錢
        GameObject.Find("生命TXT").GetComponent<Text>().text = PlayerHp.ToString();     //顯示玩家生命
                                                                                      //開場提示消失才能執行
        if (GameObject.FindWithTag("Window") == null)
        {
            NowTime = EnemyCreater.TimeDelay; //下波倒數時間
            Wave = EnemyCreater.EnemyWave + 1;
            GameObject.Find("時間TXT").GetComponent<Text>().text = "第" + Wave + "波怪倒數" + NowTime.ToString("F0") + "秒";  //顯示下一關開始的時間
            if (Wave > EnemyCreater.EnemyEnd) //關卡就不會顯示
            {
                GameObject.Find("時間TXT").GetComponent<Text>().text = "最後一波怪!!!";
            }
        }
        //失敗條件
        if (PlayerHp <= 0)
        {
            Invoke("GoodGame", 5f);       //如果輸了，延遲1秒出現失敗視窗
            EndAni.SetBool("結束", true);  //執行玩家輸了的動畫
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;//關閉背景音樂
        }
        //勝利條件，撐過所有波數，血量大於0，而且怪全都消失了會出現勝利視窗
        if (Wave > EnemyCreater.EnemyEnd && PlayerHp > 0 && GameObject.FindWithTag("Enemy") == null)
        {
            Invoke("Victory", 3f);//如果贏了，延遲3秒出現勝利視窗
            GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;//關閉背景音樂
        }
    }

    /// <summary>
    /// 遊戲加速
    /// </summary>
    public void SpeedUp()
    {
        if (GameObject.Find("加速按鍵").transform.GetChild(0).GetComponent<Text>().text == "x1")
        {
            GameObject.Find("加速按鍵").transform.GetChild(0).GetComponent<Text>().text = "x2";
            Time.timeScale = 2;
        }
        else if (GameObject.Find("加速按鍵").transform.GetChild(0).GetComponent<Text>().text == "x2")
        {
            GameObject.Find("加速按鍵").transform.GetChild(0).GetComponent<Text>().text = "x1";
            Time.timeScale = 1;
        }

    }

    /// <summary>
    /// 執行玩家扣血後的動畫
    /// </summary>
    public void EndControl()
    {
        EndAni.SetTrigger("攻擊");
    }

    public void Opening()//開場1秒後(剛好淡出結束)，讓時間暫停
    {
        Time.timeScale = 0;
    }

    ////暫停視窗////
    public void StartGame()//遊戲開始
    {
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);//畫面恢復
        Time.timeScale = 1;
        OptionWindow.transform.gameObject.SetActive(false);
    }
    public void PauseGame()//遊戲暫停
    {
        if (GameObject.Find("難度視窗") == null)
        {
            GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
            Time.timeScale = 0;
            OptionWindow.transform.gameObject.SetActive(true);
        }
    }

    ////勝利視窗////
    public void Victory()//遊戲勝利
    {
        int Grade = 0;
        if (PlayerHp >= PlayerHpMax * 9 / 10) Grade = 3; //剩餘血量有9成得3顆星
        else if (PlayerHp >= PlayerHpMax * 7 / 10 && PlayerHp < PlayerHpMax * 9 / 10) Grade = 2;//剩餘血量有7成得2顆星
        else Grade = 1;                                  //剩餘血量有7成以下得1顆星

        Chap = StandByScene.ChapterNow; Level = StandByScene.ChapterLevelNow; //如果過關了，就記錄這關的章節和關卡
        if (StandByScene.HardMode == false)//普通模式時
        {  
            //初次過關會送能量，1顆星星會+1能量，假設第一次只有1顆就+1能量，第二次3顆就再+2能量。
            if (Grade > StandByScene.StarsNum[Chap - 1, Level - 1]) StandByScene.EnergyNow += (Grade - StandByScene.StarsNum[Chap - 1, Level - 1]);
            StandByScene.StarsNum[Chap - 1, Level - 1] = Grade;                    //記錄這關的星星數
        }
        else if (StandByScene.HardMode == true)//困難模式時
        {
            if (Grade > StandByScene.StarsNumHard[Chap - 1, Level - 1]) StandByScene.EnergyNow += (Grade - StandByScene.StarsNumHard[Chap - 1, Level - 1]);
            StandByScene.StarsNumHard[Chap - 1, Level - 1] = Grade;                    //記錄這關的星星數
        }


        for (int i = 0; i < Grade; i++) //勝利視窗的星星數
            VictoryWindow.transform.GetChild(0).GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255); //過關幾顆星，就變亮幾顆星星

        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
        VictoryWindow.transform.gameObject.SetActive(true);
    }

    ////失敗視窗////
    public void GoodGame()//遊戲失敗
    {
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
        VictoryWindow.transform.gameObject.SetActive(false);
        GGWindow.transform.gameObject.SetActive(true);
    }

    ////勝利或失敗的按鍵////
    public void Window_Yes()//再來一場，延遲一秒後回到開始場景
    {
        Time.timeScale = 1;
        Invoke("Window_YesNow", 1f);
    }
    public void Window_NO()//離開遊戲，關閉遊戲
    {
        Application.Quit(); ;
    }
    public void Window_YesNow()//回到開始場景
    {
        SceneManager.LoadScene("準備場景");
    }



}
