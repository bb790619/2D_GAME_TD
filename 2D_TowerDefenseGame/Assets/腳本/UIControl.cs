using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//控制遊戲場景的UI及勝利和失敗視窗，放在"UI"上
public class UIControl : MonoBehaviour
{
    ////參數設定//
    public static int PlayerHp = 30;      //玩家血量
    public static int PlayerMoney = 150;  //玩家金錢
    ////////////////
    float NowTime = 0f;                   //下波怪出現的時間
    float Wave= 0f;                       //出怪的波數 
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
            Wave = EnemyCreater.EnemyWave+1;
            GameObject.Find("時間TXT").GetComponent<Text>().text = "第"+ Wave + "波怪倒數" + NowTime.ToString("F0") + "秒";  //顯示下一關開始的時間
            GameObject.Find("金錢TXT").GetComponent<Text>().text = PlayerMoney.ToString();  //顯示金錢
            GameObject.Find("生命TXT").GetComponent<Text>().text = PlayerHp.ToString();  //顯示玩家生命
        }
        //失敗條件
        if (PlayerHp <= 0)
        {
            Time.timeScale = 0;
            Invoke("GoodGame", 1f);//如果輸了，延遲1秒出現失敗視窗
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
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
        Time.timeScale = 0;
        OptionWindow.transform.gameObject.SetActive(true);
    }

    ////勝利視窗////
    public void Victory()//遊戲勝利
    {
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);//畫面恢復
        Time.timeScale = 0;
        VictoryWindow.transform.gameObject.SetActive(true);
        GGWindow.transform.gameObject.SetActive(false);
    }

    ////失敗視窗////
    public void GoodGame()//遊戲失敗
    {
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);//畫面恢復
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
    public void EasyMode()//簡單模式，怪物血量90%
    {
        ModeWindow.transform.gameObject.SetActive(false);//關閉視窗，開始遊戲
        StartGame();

    }
    public void NormalMode()//正常模式，怪物血量100%
    {
        ModeWindow.transform.gameObject.SetActive(false);
        StartGame();
    }
    public void HardMode()//困難模式，怪物血量110%
    {
        ModeWindow.transform.gameObject.SetActive(false);
        StartGame();
    }


}
