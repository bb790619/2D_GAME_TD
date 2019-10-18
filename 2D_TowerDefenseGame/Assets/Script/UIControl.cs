﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//控制遊戲場景的UI及勝利和失敗視窗，放在"UI"上
public class UIControl : MonoBehaviour
{
    ////參數設定//
    public static int PlayerHp = 30;      //玩家血量
    public static int PlayerMoney;      //玩家金錢                                         
                                        //[0-2]=角色1_LV1-LV3，[3-5]=角色2_LV1-LV3，[6-8]=角色3_LV1-LV3，[9-11]=角色4_LV1-LV3，
    public static int[] Player_Price = { 30, 50, 100, 40, 75, 125, 35, 60, 115, 45, 80, 140 };//建造和升級的金額，LV1，LV2，LV3。(和視窗顯示的是獨立分開的)

    public static float Mode;             //選擇難度後，血量的比例，給<EnemyControl>使用
    float NowTime = 0f;                   //下波怪出現的時間
    float Wave = 0f;                       //出怪的波數 
    public Image VictoryWindow;           //勝利視窗("勝利視窗")
    public Image GGWindow;                //失敗視窗("失敗視窗")
    public Image OptionWindow;            //暫停視窗("暫停視窗")
    public Image ModeWindow;              //難度視窗("難度視窗")

    string NextName;                      //開頭的場景名稱

    // Start is called before the first frame update
    void Start()
    {
        //transform.Find 可以找到隱藏的物件
        VictoryWindow.transform.gameObject.SetActive(false);
        GGWindow.transform.gameObject.SetActive(false);
        OptionWindow.transform.gameObject.SetActive(false);

        Time.timeScale = 1;   //遊戲開始
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊

        //遊戲場景會先淡出，開啟難度視窗，1秒後淡出消失，同時讓時間暫停
        ModeWindow.transform.gameObject.SetActive(true); //開啟難度視窗
        Invoke("Opening", 1f);

        NowTime = EnemyCreater.TimeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //難度視窗消失才能執行
        if (GameObject.Find("難度視窗") == null)
        {
            NowTime = EnemyCreater.TimeDelay; //下波倒數時間
            Wave = EnemyCreater.EnemyWave + 1;
            GameObject.Find("時間TXT").GetComponent<Text>().text = "第" + Wave + "波怪倒數" + NowTime.ToString("F0") + "秒";  //顯示下一關開始的時間
            if (Wave > EnemyCreater.EnemyEnd) //關卡就不會顯示
            {
                GameObject.Find("時間TXT").GetComponent<Text>().text = "最後一波怪!!!";
            }
            GameObject.Find("金錢TXT").GetComponent<Text>().text = PlayerMoney.ToString();  //顯示金錢
            GameObject.Find("生命TXT").GetComponent<Text>().text = PlayerHp.ToString();     //顯示玩家生命
        }
        //失敗條件
        if (PlayerHp <= 0)
        {
            Time.timeScale = 0;
            Invoke("GoodGame", 1f);//如果輸了，延遲1秒出現失敗視窗
        }
        //勝利條件，撐過所有波數，血量大於0，而且怪全都消失了會出現勝利視窗
        if (Wave >= EnemyCreater.EnemyEnd && PlayerHp > 0 && GameObject.FindWithTag("Enemy") == null)
        {
            Invoke("Victory", 5f);//如果贏了，延遲10秒出現勝利視窗
        }
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
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
        Time.timeScale = 0;
        VictoryWindow.transform.gameObject.SetActive(true);
    }

    ////失敗視窗////
    public void GoodGame()//遊戲失敗
    {
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
        Time.timeScale = 0;
        VictoryWindow.transform.gameObject.SetActive(false);
        GGWindow.transform.gameObject.SetActive(true);
    }

    ////勝利或失敗的按鍵////
    public void Window_Yes(string NextName)//再來一場，回到開始場景(按鍵的Function要輸入"開始場景")
    {
        SceneManager.LoadScene(NextName);
    }
    public void Window_NO()//離開遊戲，關閉遊戲
    {
        Application.Quit();
    }

    ////難度視窗////
    public void EasyMode()//簡單模式，金錢200，怪物血量90%
    {
        ModeWindow.transform.gameObject.SetActive(false);//關閉視窗，開始遊戲
        StartGame();
        PlayerMoney = 200;
        Mode = 0.9f;
    }
    public void NormalMode()//正常模式，金錢150，怪物血量100%
    {
        ModeWindow.transform.gameObject.SetActive(false);
        StartGame();
        PlayerMoney = 150;
        Mode = 1f;
    }
    public void HardMode()//困難模式，金錢100，怪物血量110%
    {
        ModeWindow.transform.gameObject.SetActive(false);
        StartGame();
        PlayerMoney = 100;
        Mode = 1.1f;
    }


}
