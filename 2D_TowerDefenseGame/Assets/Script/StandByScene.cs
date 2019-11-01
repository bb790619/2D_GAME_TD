﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//控制準備場景的UI，放在"UI"上
public class StandByScene : MonoBehaviour
{
    //放置視窗，按下個按鈕會跳出的視窗
    [Header("戰役")] public GameObject AdvtureWindow;
    [Header("天賦")] public GameObject TalentWindow;
    [Header("科技樹")] public GameObject TechnologyWindow;

    //可以控制上方面板的能力值
    [Header("等級Text")] public GameObject LevelText;
    [Header("等級條")] public GameObject LevelBar;
    [Header("體力Text")] public GameObject BodyStrengthText;
    [Header("體力條")] public GameObject BodyStrengthBar;
    [Header("能量Text")] public GameObject EnergyText;

    //上方面板的能力數值
    int LevelNow = 10;                               //現在等級
    float LevelEXP = 10, LevelEXPNow = 8;            //升等所需經驗，現在經驗
    float BodyStrngthMAX = 30, BodyStrngthNow = 25;  //最大體力值，現在體力值
    int EnergyNow = 2;                               //現在能量值

    //控制戰役視窗
    [Header("章節按鍵底部")] public GameObject Chapter;     //放置章節
    int ChapterMax;                                         //章節的最大數量(在START計算)
    [Header("目前開放的章節")] public int ChapterLimit = 2; //目前開放的章節，假如等於2，代表只會開放到第二章
    [Header("即將推出的文字")] public Text LimitTXT;
    [Header("關卡按鍵底部")] public GameObject[] ChapterLevel;  //放置關卡
    int[] ChapterLevelMax;                                 //各關卡的最大數量(在START計算)
    [Header("選擇關卡背景")] public GameObject ChapterLevelBG;
    int ChapterNow, ChapterLevelNow;                        //現在選到的章節，現在選到的關卡
    [Header("目前通過的章節")] public int ChapterPass;
    [Header("目前通過的關卡")] public int[] ChapterLevelPass;
    [Header("選擇模式的文字")] public Text ChooseModeText;
    [Header("過關的星星數")] public GameObject PassStars;
    public int XX;//測試用，目前過關的星星數，之後在刪除

    //控制天賦視窗
    [Header("天賦按鍵底部")] public GameObject TalentButton;
    [Header("詢問升級視窗")] public GameObject TalentLevelUpWindow;


    // Start is called before the first frame update
    void Start()
    {
        ChapterMax = Chapter.transform.childCount;  //章節的數量
        ChapterLevelMax = new int[ChapterMax];      //每個章節的最大關卡數量
        ChapterLevelPass = new int[ChapterMax];     //每個章節通過的關卡數量
        //int[] ChapterLevelPass = { 3, 6, 4, 5, 3 };
        for (int i = 0; i < ChapterMax; i++)
        {
            ChapterLevelMax[i] = ChapterLevel[i].transform.childCount; //各關卡的數量
            for (int j = 0; j < ChapterLevelMax[i]; j++)
            {
                ChapterLevel[i].transform.GetChild(j).GetChild(0).GetComponent<Text>().text = (i + 1) + "-" + (j + 1);
                ChapterLevelPass[i] = 0;
            }
        }
        /*
          初始畫面為通過的關卡，假設要顯示第4章節第6關卡(代表已通過第三章節，已通過第四章節中的第5關卡)
          1.ChapterPass為通過的章節，假設為3，要顯示第4章節，所以就是ChapterPass+1   => "4" => 顯示第四章節的關卡
            ChapterLevelPass[0]=>第一章節通過的關卡數，所以要顯示第四章節的第6關卡數就是ChapterLevelPass[3]+1 => 4 - 6 
          2.Chapter.transform.GetChild(ChapterPass)=>Chapter.transform.GetChild(3)=>第四章節
            ChapterLevel[ChapterPass].transform.GetChild(ChapterLevelPass[ChapterPass])=> ChapterLevel[3].transform.GetChild(ChapterLevelPass[3])
            => ChapterLevel[3].transform.GetChild(5) => "4-6"
        */
        ChooseButton((ChapterPass + 1) + "-" + (ChapterLevelPass[ChapterPass] + 1));  //顯示目前已通過的關卡
        //Chapter.transform.GetChild(ChapterPass).GetComponent<Image>().color = new Color32(255, 255, 0, 255); //章節變黃色，代表選取
        //ChapterLevel[ChapterPass].transform.GetChild(ChapterLevelPass[ChapterPass]).GetComponent<Image>().color = new Color32(255, 255, 0, 255);//關卡變黃色，代表選取

        LimitTXT.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        AbilityValue();  //UI上的數值
        LockChapterLevel();// 未通過的關卡會被鎖按鍵


        //關卡升級
        if (Input.GetKeyDown("up"))
        {
            PassChapterLevel(ChapterNow, ChapterLevelNow, XX);
        }

    }

    /// <summary>
    /// 過關之後，關卡數+1，顯示星星數(過關的星星數)
    /// </summary>
    public void PassChapterLevel(int Chap, int ChapLevel, int Grade)
    {

        if (ChapterPass < ChapterLimit) //小於最大關卡數，過關後關卡+ 1
        {
            //點選關卡後，過關會新增星星。星星會出現在關卡的父物件下。調整大小。
            if (GameObject.Find("星星" + Chap + "-" + ChapLevel) == null) //如果沒有產生過星星，才會產生星星，避免重複產生
                Instantiate(PassStars, ChapterLevel[Chap - 1].transform.GetChild(ChapLevel - 1).position, Quaternion.identity)
                        .name = "星星" + Chap + "-" + ChapLevel;
            GameObject.Find("星星" + Chap + "-" + ChapLevel)
                       .transform.SetParent(ChapterLevel[Chap - 1].transform.GetChild(ChapLevel - 1));
            GameObject.Find("星星" + Chap + "-" + ChapLevel)
                       .transform.localScale = new Vector3(1, 1, 1);
            /*
            Instantiate(PassStars,   ChapterLevel[ChapterPass].transform.GetChild( ChapterLevelPass[ChapterPass] ).position , Quaternion.identity)
                       .name ="星星"+ (ChapterPass+1)+"-"+ (ChapterLevelPass[ChapterPass] + 1);
            GameObject.Find("星星" + (ChapterPass + 1) + "-" + (ChapterLevelPass[ChapterPass] + 1))
                       .transform.SetParent( ChapterLevel[ChapterPass].transform.GetChild(ChapterLevelPass[ChapterPass]) );
            GameObject.Find("星星" + (ChapterPass + 1) + "-" + (ChapterLevelPass[ChapterPass] + 1))
                       .transform.localScale = new Vector3(1, 1, 1);
                       */
            //依照過關星星數改變顏色
            for (int i = 0; i < Grade; i++)
                GameObject.Find("星星" + Chap + "-" + ChapLevel).
                    transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            //如果關卡等於最新進度，關卡開放+1
            if (Chap == ChapterPass + 1 && ChapLevel == ChapterLevelPass[ChapterPass] + 1)
            {
                ChapterLevelPass[ChapterPass] += 1; //關卡就+1
                ChooseButton((ChapterPass + 1) + "-" + (ChapterLevelPass[ChapterPass] + 1));  //顯示目前已通過的關卡
            }
        }

        //過關之後關卡數+1，如果超過最大關卡數，章節就+1
        //如果破關到最大限制的章節，後面就顯示尚未推出，而且不能再點選
        if (ChapterLevelPass[ChapterPass] >= ChapterLevelMax[ChapterPass])
        {
            if (ChapterPass + 1 < ChapterLimit) //小於最大關卡數，章節才能進位
                ChapterPass += 1;
        }
        if (ChapterPass + 1 == ChapterLimit && ChapterLevelPass[ChapterPass] == ChapterLevelMax[ChapterPass])
        {
            LimitTXT.transform.gameObject.SetActive(true);
            LimitTXT.transform.position = Chapter.transform.GetChild(ChapterLimit).position;
        }


    }


    ////////戰役視窗的按鍵功能////////
    /// <summary>
    /// 戰役視窗的按鍵功能，紀錄選到的章節及改變顏色(在面板上輸入章節關卡)
    /// </summary>
    /// <param name="Name"></param>
    public void ChooseButton(string Name)
    {
        //第一章的按鍵為1-0，第二章為2-0...1-1顯示1-1，2-5顯示2-5..
        for (int i = 0; i < ChapterMax; i++)
        {

            for (int j = 0; j <= ChapterLevelMax[i]; j++)
            {
                if (Name == (i + 1) + "-" + j)
                {
                    ChapterNow = i + 1;      //現在選到的章節
                    ChapterLevelNow = j;     //現在選到的關卡
                }
            }
        }

        //按哪個章節，哪個關卡就出現，其他關卡隱藏
        for (int i = 0; i < ChapterMax; i++)
        {
            if (ChapterNow == (i + 1))
            {
                ChapterLevelBG.transform.GetChild(i).gameObject.SetActive(true);
                Chapter.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 0, 255); //選到的章節變黃色
            }
            else if (ChapterNow != (i + 1))
            {
                ChapterLevelBG.transform.GetChild(i).gameObject.SetActive(false);
                Chapter.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255); //沒選到的章節顏色恢復
            }
        }

        for (int j = 1; j <= ChapterLevelMax[ChapterNow - 1]; j++)//選到的關卡變黃色
        {
            if (ChapterLevelNow == j)
            {
                ChapterLevel[ChapterNow - 1].transform.GetChild(j - 1).GetComponent<Image>().color = new Color32(255, 255, 0, 255); //選到的章節變黃色
            }
            else if (ChapterLevelNow != j)
                ChapterLevel[ChapterNow - 1].transform.GetChild(j - 1).GetComponent<Image>().color = new Color32(255, 255, 255, 255); //沒選到的章節顏色恢復

        }

    }
    /// <summary>
    /// 戰役視窗，未通過的關卡會被鎖按鍵
    /// </summary>
    public void LockChapterLevel()
    {
        for (int i = 0; i < ChapterMax; i++)    //章節
        {
            for (int j = 0; j < ChapterLevelMax[i]; j++)  //關卡
            {
                if (i <= ChapterPass && j <= ChapterLevelPass[i]) //解鎖
                {
                    Chapter.transform.GetChild(i).GetComponent<Button>().interactable = true;        //章節解鎖
                    ChapterLevel[i].transform.GetChild(j).GetComponent<Button>().interactable = true;//關卡解鎖
                }
                else if (i > ChapterPass)  //上鎖
                    Chapter.transform.GetChild(i).GetComponent<Button>().interactable = false;    //章節上鎖
                else if (j > ChapterLevelPass[i])  //上鎖 
                    ChapterLevel[i].transform.GetChild(j).GetComponent<Button>().interactable = false; //關卡上鎖
            }
        }
    }
    /// <summary>
    /// 戰役視窗，選擇模式
    /// </summary>
    public void Mode()
    {
        string ModeTXT = ChooseModeText.GetComponent<Text>().text;
        if (ModeTXT == "普通模式")
        {
            ChooseModeText.GetComponent<Text>().text = "困難模式";
            GameObject.Find("戰役視窗").GetComponent<Image>().sprite = Resources.Load<Sprite>("背景3");
        }
        else if (ModeTXT == "困難模式")
        {
            ChooseModeText.GetComponent<Text>().text = "普通模式";
            GameObject.Find("戰役視窗").GetComponent<Image>().sprite = Resources.Load<Sprite>("背景2");
        }
    }
    /// <summary>
    /// 戰役視窗，戰鬥按鍵
    /// </summary>
    public void ChangeScene()
    {
        Invoke("RealChangeScene", 1f);
    }
    /// <summary>
    /// 移動至遊戲場景
    /// </summary>
    public void RealChangeScene()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    ////////天賦視窗的按鍵功能////////
    /// <summary>
    /// 天賦視窗的按鍵功能，按了會出現詢問升級視窗。取消功能。確定功能。(按鍵的編號)
    /// </summary>
    /// <param name="Name"></param>
    public void ChooseTalentButton(int Name)
    {
        //0是玩家體力，1是經驗，2是初始金錢，3是戰鬥CD，4是玩家生命，5是戰鬥金錢
        //6是取消。7是確定。
        int Count = TalentButton.transform.childCount;
        if (Name < Count)
        {
            TalentLevelUpWindow.SetActive(true);
            TalentLevelUpWindow.transform.position = TalentButton.transform.GetChild(Name).transform.position;
        }
        else if (Name == Count)  //取消
            TalentLevelUpWindow.SetActive(false);
        else if (Name == Count + 1) //確定
        {
            print("升級");
        }
    }
    public void CloseTalentLevelUp()
    {
        TalentLevelUpWindow.SetActive(false);
    }
    public void OpenTalentLevelUp()
    {
        print("升級");
    }


    ////////控制上方面板數值變動   ////////
    /// <summary>
    /// UI上的能力值
    /// </summary>
    public void AbilityValue()
    {
        LevelText.GetComponent<Text>().text = LevelNow.ToString();                         //現在等級(文字)
        LevelBar.GetComponent<Image>().fillAmount = LevelEXPNow / LevelEXP;                 //現在經驗值(圖)
        BodyStrengthText.GetComponent<Text>().text = BodyStrngthNow + "/" + BodyStrngthMAX; //現在體力(文字)
        BodyStrengthBar.GetComponent<Image>().fillAmount = BodyStrngthNow / BodyStrngthMAX; //現在體力(圖)
        EnergyText.GetComponent<Text>().text = EnergyNow.ToString();                        //現在能量(文字)

    }


    ////////控制視窗關閉或開啟////////
    /// <summary>
    /// 關閉視窗，返回最初介面
    /// </summary>
    public void WindowClose()
    {
        AdvtureWindow.SetActive(false);
        TalentWindow.SetActive(false);
        TechnologyWindow.SetActive(false);
    }
    /// <summary>
    /// 打開戰役視窗
    /// </summary>
    public void Window_Advture()
    {
        AdvtureWindow.SetActive(true);
        ChooseButton((ChapterPass + 1) + "-" + (ChapterLevelPass[ChapterPass] + 1));  //顯示目前已通過的關卡
    }
    /// <summary>
    /// 打開天賦視窗
    /// </summary>
    public void Window_Talent()
    {
        TalentWindow.SetActive(true);
    }
    /// <summary>
    /// 打開科技樹視窗
    /// </summary>
    public void Window_Technology()
    {
        TechnologyWindow.SetActive(true);
    }



}
