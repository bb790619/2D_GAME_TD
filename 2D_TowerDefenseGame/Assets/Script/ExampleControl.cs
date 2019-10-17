using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//說明場景使用
public class ExampleControl : MonoBehaviour
{
    public Image ExampleWindow;           //重新說明視窗
    public Image SkipWindow;             //跳過視窗
    public Image TXTWindow;               //對話視窗
    float NowTime=0f;                    //時間，幾秒後出現以下的對話

    string[] TXT = { "歡迎來到TD!!!",
                      "我們遇到麻煩...","需要拜託冒險者保護我們不被怪物入侵!!",
                     "讓我來說明目前的狀況吧!!",
                     "左上方顯示的是時間、金錢、玩家生命!!",
                     "可怕的怪物會從左下方出現。",
                     "如果怪物穿過森林抵達右上方，玩家就會扣血。", "小心，如果生命歸0就失敗了!!",
                     "不過不用擔心!!", "冒險者可以召喚夥伴來幫你!!",
                     "觸碰螢幕就會出現空位!!","可以選擇建造、升級或販賣!!",
                     "當然，必須消滅怪物才有金錢可以建造砲塔!!",
                     "所以消滅怪物守護森林，還可以升級成更強的夥伴","真是一石二鳥阿!!",
                     "最後，如果有狀況可以點這邊的暫停!!",
                     "好了，讓我們開始遊戲吧!!", };

    public Image Cricle_1; public Image Cricle_2; public Image Cricle_3; public Image Cricle_4; public GameObject Space;

    // Start is called before the first frame update
    void Start()
    {
        //ExampleWindow.transform.gameObject.SetActive(true);   //開啟視窗
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ExampleWindow.gameObject.activeSelf==false)
        {
            Time.timeScale = 0;
            SkipWindow.transform.gameObject.SetActive(true);
        }

        NowTime += Time.deltaTime;

        if (NowTime >= 2f) TXTWindow.transform.gameObject.SetActive(true);

        //開始對話
        ChangTXT(4f, "對話文字1", TXT[0]);
        ChangTXT(6f, "對話文字1", TXT[1]); ChangTXT(8f, "對話文字2", TXT[2]);
        ChangTXT(10f, "對話文字1", TXT[3]); ChangTXT(10f, "對話文字2", null);
        ChangTXT(13f, "對話文字1", TXT[4]); ChangTXT(13f, "對話文字2", null); if (NowTime >= 13f) Cricle_1.transform.gameObject.SetActive(true); if (NowTime >= 16f) Cricle_1.transform.gameObject.SetActive(false);
        ChangTXT(16f, "對話文字1", TXT[5]); ChangTXT(16f, "對話文字2", null); if (NowTime >= 16f) Cricle_2.transform.gameObject.SetActive(true); if (NowTime >= 19f) Cricle_2.transform.gameObject.SetActive(false);
        ChangTXT(19f, "對話文字1", TXT[6]); ChangTXT(19f, "對話文字2", null); ChangTXT(21f, "對話文字2", TXT[7]); if (NowTime >= 19f) Cricle_3.transform.gameObject.SetActive(true); if (NowTime >= 23f) Cricle_3.transform.gameObject.SetActive(false);
        ChangTXT(23f, "對話文字1", TXT[8]); ChangTXT(23f, "對話文字2", null); ChangTXT(25f, "對話文字2", TXT[9]);
        ChangTXT(28f, "對話文字1", TXT[10]); ChangTXT(28f, "對話文字2", null); ChangTXT(30f, "對話文字2", TXT[11]); if (NowTime >= 28f) Space.transform.gameObject.SetActive(true); if (NowTime >= 33f) Space.transform.gameObject.SetActive(false);
        ChangTXT(33f, "對話文字1", TXT[12]); ChangTXT(33f, "對話文字2", null);
        ChangTXT(35f, "對話文字1", TXT[13]); ChangTXT(35f, "對話文字2", null); ChangTXT(37f, "對話文字2", TXT[14]);
        ChangTXT(39f, "對話文字1", TXT[15]); ChangTXT(39f, "對話文字2", null); if (NowTime >= 39f) Cricle_4.transform.gameObject.SetActive(true); if (NowTime >= 43f) Cricle_4.transform.gameObject.SetActive(false);
        ChangTXT(43f, "對話文字1", TXT[16]); ChangTXT(43f, "對話文字2", null);

        if (NowTime >= 46f)
        {
            Time.timeScale = 0;
            ExampleWindow.transform.gameObject.SetActive(true);
        } 
    }

    public void ChangTXT(float Time , string TEXT_0 , string TEXT_1) //讓文字隨著時間撥放
    {
        if (NowTime >= Time)  GameObject.Find(TEXT_0).GetComponent<Text>().text = TEXT_1;
            
    }


    public void SkipNo() //"跳過說明視窗"，NO
    {
        Time.timeScale = 1;
        SkipWindow.transform.gameObject.SetActive(false);
    }

    ////重新說明視窗的按鍵////
    public void ReturnSkip() //"重新說明視窗"，重新說明
    {
        Time.timeScale = 1;
        Invoke("Return", 1f);//要有淡出效果，所以延遲
    }
    public void Return() //跳至說明場景
    {
        SceneManager.LoadScene("說明場景");
    }
    public void ExampleSkip() //"重新說明視窗"，跳過說明
    {
        Time.timeScale = 1;
        Invoke("ExampleSkipNow", 1f);//要有淡出效果，所以延遲
    }
    public void ExampleSkipNow() //跳至遊戲場景
    {
        SceneManager.LoadScene("遊戲場景");
    }

}


