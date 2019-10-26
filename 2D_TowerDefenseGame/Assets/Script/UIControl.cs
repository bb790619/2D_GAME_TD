using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//控制遊戲場景的UI及勝利和失敗視窗，放在"UI"上
public class UIControl : MonoBehaviour
{
    ////參數設定//
    public static int PlayerHp = 4;    //玩家血量
    public static int PlayerMoney;      //玩家金錢                                        
    //[0-2]=角色1_LV1-LV3，[3-5]=角色2_LV1-LV3，[6-8]=角色3_LV1-LV3，[9-11]=角色4_LV1-LV3，以此類推
    //修改這邊的金額，<SpaceControl>會自動修改
    public static int[] Player_Price = { 45, 85, 140,
                                         35, 65, 120,
                                         35, 65, 120,
                                         30, 50, 100,
                                         25, 40,  80,
                                         40, 80, 130  };

    public static float Mode;             //選擇難度後，血量的比例，給<EnemyControl>使用
    float NowTime = 0f;                   //下波怪出現的時間
    float Wave = 0f;                       //出怪的波數 
    public Image VictoryWindow;           //勝利視窗("勝利視窗")
    public Image GGWindow;                //失敗視窗("失敗視窗")
    public Image OptionWindow;            //暫停視窗("暫停視窗")
    //public Image ModeWindow;            //難度視窗("難度視窗")

    public Animator EndAni;                //守門人
    public static bool EndHit = false;      //是否有被扣血
    public static float EndTime = 2f;       //倒數計時

    // Start is called before the first frame update
    void Start()
    {
        //transform.Find 可以找到隱藏的物件
        VictoryWindow.transform.gameObject.SetActive(false);
        GGWindow.transform.gameObject.SetActive(false);
        OptionWindow.transform.gameObject.SetActive(false);

        Time.timeScale = 1;   //遊戲開始
        //GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
        //ModeWindow.transform.gameObject.SetActive(true); //開啟難度視窗
        //Invoke("Opening", 1f);        //遊戲場景會先淡出，開啟難度視窗，1秒後淡出消失，同時讓時間暫停

        NowTime = EnemyCreater.TimeDelay; //下波怪出現的時間
        PlayerMoney = 200;                //玩家初始金錢
        Mode = 1f;                        //1倍(現在無使用)
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
        }
        //勝利條件，撐過所有波數，血量大於0，而且怪全都消失了會出現勝利視窗
        if (Wave >= EnemyCreater.EnemyEnd && PlayerHp > 0 && GameObject.FindWithTag("Enemy") == null)
            Invoke("Victory", 3f);//如果贏了，延遲3秒出現勝利視窗

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
        GameObject.Find("變暗背景").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);//畫面變模糊
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
        SceneManager.LoadScene("開始場景");
    }

    /*
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
    */


}
