using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//控制遊戲場景的UI及勝利和失敗視窗，放在"UI"上
public class UIControl : MonoBehaviour
{
    ////參數設定//
    public static int PlayerHp = 10;      //玩家血量
    public static int PlayerMoney = 100;  //玩家金錢
    ////////////////
    float NowTime = 0f;
    //Image VictoryWindow;                //勝利視窗("勝利視窗")
    //Image GGWindow;                     //失敗視窗("失敗視窗")
    //public GameObject OptionWindow;     //放置("選項視窗大小")，讓遊戲場景的選項，可以被點到，放置一個透明的物件
    string NextName;                      //開頭的場景名稱
    float xx = 9.85f;                     //透明物件的X和Y
    float yy = 4.6f;
    public static int GameState = 1;     //遊戲狀態，0為暫停，1為開始

    // Start is called before the first frame update
    void Start()
    {
        //transform.Find 可以找到隱藏的物件
        transform.Find("勝利視窗").gameObject.SetActive(false);
        transform.Find("失敗視窗").gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

        NowTime += Time.deltaTime;
        GameObject.Find("時間").GetComponent<Text>().text = NowTime.ToString("F1");  //顯示下一關開始的時間
        GameObject.Find("Button").transform.position = GameObject.Find("Main Camera").transform.position + new Vector3(xx, yy, 1f); //透明物件，讓右上按鍵被點選後也不會觸發其他功能
        GameObject.Find("金錢TXT").GetComponent<Text>().text = PlayerMoney.ToString();  //顯示金錢
        GameObject.Find("生命TXT").GetComponent<Text>().text = PlayerHp.ToString();  //顯示玩家生命


        //失敗條件
        if (PlayerHp <= 0) GoodGame();


    }



    public void StartGame()//遊戲開始
    {
        Time.timeScale = 1;
        GameState = 1;
    }
    public void PauseGame()//遊戲暫停
    {
        Time.timeScale = 0;
        GameState = 0;
    }

    public void Victory()//遊戲勝利
    {
        Time.timeScale = 0;
        transform.Find("勝利視窗").gameObject.SetActive(true);
        transform.Find("失敗視窗").gameObject.SetActive(false);
    }
    public void GoodGame()//遊戲失敗
    {
        Time.timeScale = 0;
        transform.Find("勝利視窗").gameObject.SetActive(false);
        transform.Find("失敗視窗").gameObject.SetActive(true);
    }
    public void Window_Yes(string NextName)//再來一場，回到開始場景(按鍵的Function要輸入"開始場景")
    {
        SceneManager.LoadScene(NextName);
    }
    public void Window_NO()//離開遊戲，關閉遊戲
    {
        Application.Quit();
    }

    public void EasyMode()//簡單模式，怪物血量90%
    {

    }
    public void NormalMode()//正常模式，怪物血量100%
    {

    }
    public void HardMode()//困難模式，怪物血量110%
    {

    }


}
