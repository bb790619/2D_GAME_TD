using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//說明場景使用
public class ExampleScene : MonoBehaviour
{
    [Header("重新說明視窗")] public Image ExampleWindow;
    [Header("跳過視窗")] public Image SkipWindow;
    [Header("對話視窗")] public Image TXTWindow;
    [Header("變暗背景")] public GameObject BlackBG;
    [Header("提示")] public Image[] Cricle;
    [Header("開始遊戲的提示")] public GameObject StartGame;
    [Header("空格")] public GameObject Space;
    float NowTime = 0f;                    //時間，幾秒後出現以下的對話



    // Start is called before the first frame update
    void Start()
    {
        //ExampleWindow.transform.gameObject.SetActive(true);   //開啟視窗
    }

    // Update is called once per frame
    void Update()
    {
        //觸控螢幕可選擇是否跳過說明
        if (Input.GetMouseButtonDown(0) && ExampleWindow.gameObject.activeSelf == false)
        {
            SkipWindow.transform.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        NowTime += Time.deltaTime;

        //TXT文字
        string[] TXT = { "歡迎來到TD!!","這裡是與世隔絕的世外桃源!!",
                      "不過我們遇到麻煩...","外島上有可怕的怪物要侵略我們!!",
                      "怪物操控森林的動物要搶走我們的寶石!","希望冒險者可以保護我們!!",
                     "讓我來說明目前的狀況吧!"," ",
                     "左上方顯示的是時間、金錢、玩家生命!!"," ",
                     "可怕的怪物會從提示圖案的位置出現。","記得要點選圖示圖案怪物才會出現",
                     "如果怪物穿過森林抵達這裡，玩家就會扣血。", "小心，如果生命歸0就失敗了!!",
                     "不過不用擔心!!", "冒險者可以召喚夥伴來幫你!!",
                     "觸碰螢幕就會出現空位!!","可以選擇建造、升級或販賣!!",
                     "當然，必須消滅怪物才有金錢可以建造砲塔!!"," ",
                     "所以消滅怪物守護森林，還可以升級成更強的夥伴","真是一石二鳥阿!!",
                     "最後，如果有狀況可以點這邊的暫停或加速!!"," ",
                     "好了，讓我們開始遊戲吧!!", };

        //開啟對話視窗，變暗場景消失
        if (NowTime >= 1.5f) TXTWindow.transform.gameObject.SetActive(true);
        if (NowTime >= 2.5f) BlackBG.transform.gameObject.SetActive(false);

        //按照TXT的字串，開始自動對話
        #region
        ChangTXT(4f, "對話文字1", TXT[0]); ChangTXT(5.5f, "對話文字2", TXT[1]);
        ChangTXT(8f, "對話文字1", TXT[2]); ChangTXT(8f, "對話文字2", ""); ChangTXT(9.5f, "對話文字2", TXT[3]);
        ChangTXT(12f, "對話文字1", TXT[4]); ChangTXT(12f, "對話文字2", ""); ChangTXT(13.5f, "對話文字2", TXT[5]);
        ChangTXT(16f, "對話文字1", TXT[6]); ChangTXT(16f, "對話文字2", ""); ChangTXT(17.5f, "對話文字2", TXT[7]);

        if (NowTime >= 20f) Cricle[0].transform.gameObject.SetActive(true); if (NowTime >= 24f) Cricle[0].transform.gameObject.SetActive(false);
        ChangTXT(20f, "對話文字1", TXT[8]); ChangTXT(20f, "對話文字2", ""); ChangTXT(22f, "對話文字2", TXT[9]);

        if (NowTime >= 24f)
        {
            Cricle[1].transform.gameObject.SetActive(true);   StartGame.transform.gameObject.SetActive(true);
        }
        if (NowTime >= 29f)
        {
            Cricle[1].transform.gameObject.SetActive(false);   StartGame.transform.gameObject.SetActive(false);
        }
        ChangTXT(24f, "對話文字1", TXT[10]); ChangTXT(24f, "對話文字2", ""); ChangTXT(26f, "對話文字2", TXT[11]);

        if (NowTime >= 29f) Cricle[2].transform.gameObject.SetActive(true); if (NowTime >= 34f) Cricle[2].transform.gameObject.SetActive(false);
        ChangTXT(29f, "對話文字1", TXT[12]); ChangTXT(29f, "對話文字2", ""); ChangTXT(31f, "對話文字2", TXT[13]);

        ChangTXT(34f, "對話文字1", TXT[14]); ChangTXT(34f, "對話文字2", ""); ChangTXT(36f, "對話文字2", TXT[15]);
        if (NowTime >= 38f) Space.transform.gameObject.SetActive(true); if (NowTime >= 43f) Space.transform.gameObject.SetActive(false);
        ChangTXT(38f, "對話文字1", TXT[16]); ChangTXT(38f, "對話文字2",""); ChangTXT(40f, "對話文字2", TXT[17]);

        ChangTXT(43f, "對話文字1", TXT[18]); ChangTXT(43f, "對話文字2", ""); ChangTXT(45f, "對話文字2", TXT[19]);
        ChangTXT(47f, "對話文字1", TXT[20]); ChangTXT(47f, "對話文字2", ""); ChangTXT(49f, "對話文字2", TXT[21]);
        if (NowTime >= 51f) Cricle[3].transform.gameObject.SetActive(true); if (NowTime >= 56f) Cricle[3].transform.gameObject.SetActive(false);
        ChangTXT(51f, "對話文字1", TXT[22]); ChangTXT(51f, "對話文字2", ""); ChangTXT(52f, "對話文字2", TXT[23]);
        ChangTXT(56f, "對話文字1", TXT[24]); ChangTXT(56f, "對話文字2", ""); 
        //自動出現，是否要再看一次的視窗
        if (NowTime >= 58f && ExampleWindow.gameObject.activeSelf == false)
        {
            Time.timeScale = 0;
            ExampleWindow.transform.gameObject.SetActive(true);
        }
        #endregion
    }
    /// <summary>
    /// 讓文字隨著時間撥放[時間，顯示在文字框1or2，顯示的文字]
    /// </summary>
    /// <param name="Time"></param>
    /// <param name="TEXT_0"></param>
    /// <param name="TEXT_1"></param>
    public void ChangTXT(float Time, string TEXT_0, string TEXT_1)
    {
        if (NowTime >= Time) GameObject.Find(TEXT_0).GetComponent<Text>().text = TEXT_1;
    }

    /// <summary>
    /// "跳過說明視窗"，NO
    /// </summary>
    public void SkipNo()
    {
        Time.timeScale = 1;
        SkipWindow.transform.gameObject.SetActive(false);
        BlackBG.transform.gameObject.SetActive(false);
    }
    /// <summary>
    /// 重新說明視窗的按鍵
    /// </summary>
    public void ReturnSkip() //"重新說明視窗"，重新說明
    {
        Time.timeScale = 1;
        Invoke("Return", 1f);//要有淡出效果，所以延遲
    }
    /// <summary>
    /// 跳至說明場景，重新說明一次
    /// </summary>
    public void Return()
    {
        SceneManager.LoadScene("說明場景");
    }
    /// <summary>
    /// "重新說明視窗"，跳過說明
    /// </summary>
    public void ExampleSkip()
    {
        Time.timeScale = 1;
        Invoke("ExampleSkipNow", 1f);//要有淡出效果，所以延遲
    }
    /// <summary>
    /// 跳至準備場景
    /// </summary>
    public void ExampleSkipNow()
    {
        SceneManager.LoadScene("準備場景");
    }

}


