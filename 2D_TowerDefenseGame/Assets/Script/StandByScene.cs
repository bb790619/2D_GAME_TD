using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//控制"準備場景"的UI，放在"UI"上
public class StandByScene : MonoBehaviour
{
    //放置視窗，按下個按鈕會跳出的視窗
    [Header("戰役")] public GameObject AdvtureWindow;
    [Header("天賦")] public GameObject TalentWindow;
    [Header("科技")] public GameObject TechnologyWindow;

    //可以控制上方面板的能力值
    [Header("等級Text")] public GameObject LevelText;
    [Header("等級條")] public GameObject LevelBar;
    [Header("體力Text")] public GameObject BodyStrengthText;
    [Header("體力條")] public GameObject BodyStrengthBar;
    [Header("能量Text")] public GameObject EnergyText;

    //上方面板的能力數值
    public static int LevelNow = 1;          //現在等級
    public static float LevelEXP = 5;       //升等所需經驗
    public static float LevelEXPMAX = 8;    //經驗的最大值(超過就升等)
    public static float LevelEXPNow = 0;     //現在經驗
    public static float BodyStrngthMAX = 30;//最大體力值
    public static float BodyStrngthNow = 30;//現在體力值
    public static int EnergyNow = 1;       //現在能量值
    int BodyNormal = 5;  //普通模式扣5體力
    int BodyHard = 10;  //困難模式扣10體力

    //控制戰役視窗
    [Header("章節按鍵底部")] public GameObject Chapter;     //放置章節
    public static int ChapterMax;                           //章節的最大數量(在START計算)
    [Header("目前開放的章節")] public int ChapterLimit = 2; //目前開放的章節，假如等於2，代表只會開放到第二章
    [Header("即將推出的文字")] public Text LimitTXT;
    [Header("關卡按鍵底部")] public GameObject[] ChapterLevel;  //放置關卡
    public static int[] ChapterLevelMax;                        //各關卡的最大數量(在START計算)
    [Header("選擇關卡背景")] public GameObject ChapterLevelBG;
    public static int ChapterNow, ChapterLevelNow;        //現在選到的章節，現在選到的關卡
    [Header("目前通過的章節")] public static int ChapterPass;
    [Header("目前通過的關卡")] public static int[] ChapterLevelPass;
    [Header("選擇模式的文字")] public Text ChooseModeText;
    [Header("過關的星星數的圖片")] public GameObject PassStars;
    [Header("體力不足的文字")] public Text BodyStrngthTXT;
    public static int[,] StarsNum = new int[5, 8];//目前過關的星星數，存檔用
    public static bool Repeat = true; //初始化數字


    //控制天賦視窗
    [Header("天賦按鍵底部")] public GameObject TalentButton;
    [Header("詢問天賦升級視窗")] public GameObject TalentLevelUpWindow;
    int TalentSpace;                    //天賦視窗，被點選到的能力
    public static int TalenCount = 6;   //天賦視窗，被選到的能力視窗名稱
    public static int[] TalentPoint = new int[TalenCount]; //天賦視窗，要提升的能力
    int[] TalentPointMax = { 3, 3, 3, 3, 3, 3 }; //天賦視窗，要提升的能力的最大值

    //控制科技視窗
    [Header("科技按鍵底部")] public GameObject TechnologyButton;
    [Header("詢問科技升級視窗")] public GameObject TechnologyLevelUpWindow;
    int TechPos;      //科技視窗，被點選到的能力                 [1][2][3]、[5][6][7]...
    int TechPosPlus;  //科技視窗，被點選到的能力，對應的陣列位子 [0][1][2]、[3][4][5]...
    public static int TechMAX = 3;    //科技視窗，各角色的可升級能力數量
    public static int TechCount = TechMAX * SpaceControl.PlayerNum; //所有可升級能力的總數量
    public static int[] TechPoint = new int[TechCount]; //天賦視窗，要提升的能力(只有要升級的能力，沒有圖片)
    int[] TechPointMax = { 5, 1, 5, 5, 1, 5, 5, 1, 5, 5, 1, 5, 5, 1, 5, 5, 1, 5 };//天賦視窗，要提升的能力的最大值(只有要升級的能力，沒有圖片)

    [Header("點數不足的文字")] public Text EnergyTXT;

    [SerializeField]
    PlayerData data;
    [System.Serializable]
    public class PlayerData
    {
        public int LevelNow;          //現在等級
        public float LevelEXPMAX;    //經驗的最大值(超過就升等)
        public float LevelEXPNow;    //現在經驗
        public float BodyStrngthMAX;//最大體力值
        public float BodyStrngthNow;//現在體力值
        public int EnergyNow;       //現在能量值

        public int ChapterPass;       //目前通過的章節
        public int[] ChapterLevelPass;//目前通過的關卡
        public int[,] StarsNum = new int[5, 8];//目前過關的星星數，存檔用
        public bool Repeat; //初始化數字

        public int[] TalentPoint; //天賦視窗，要提升的能力
        public int[] TechPoint; //天賦視窗，要提升的能力(只有要升級的能力，沒有圖片)
    }

    public void Save()//存檔
    {
        data.LevelNow = LevelNow; data.LevelEXPMAX = LevelEXPMAX; data.LevelEXPNow = LevelEXPNow;
        data.BodyStrngthMAX = BodyStrngthMAX; data.BodyStrngthNow = BodyStrngthNow; data.EnergyNow = EnergyNow;
        data.ChapterPass = ChapterPass; data.ChapterLevelPass = ChapterLevelPass;
        data.StarsNum = StarsNum; data.Repeat = Repeat;
        data.TalentPoint = TalentPoint; data.TechPoint = TechPoint;
        print("儲存" + JsonUtility.ToJson(data));
    }
    public void Load()
    {
        LevelNow = data.LevelNow; LevelEXPMAX = data.LevelEXPMAX; LevelEXPNow = data.LevelEXPNow;
        BodyStrngthMAX = data.BodyStrngthMAX; BodyStrngthNow = data.BodyStrngthNow; EnergyNow = data.EnergyNow;
        ChapterPass = data.ChapterPass; ChapterLevelPass = data.ChapterLevelPass;
        StarsNum = data.StarsNum; Repeat = data.Repeat;
        TalentPoint = data.TalentPoint; TechPoint = data.TechPoint;
        print("讀檔" + data);
    }

    // Start is called before the first frame update
    void Start()
    {
        //戰役視窗，初始化
        #region
        //因為重新開啟時，數字都會歸0，使用Bool值讓初始化數值只執行一次
        if (Repeat == true)
        {
            ChapterMax = Chapter.transform.childCount;  //章節的數量
            ChapterLevelMax = new int[ChapterMax];      //每個章節的最大關卡數量
            ChapterLevelPass = new int[ChapterMax];     //每個章節通過的關卡數量

            for (int i = 0; i < ChapterMax; i++)
            {
                ChapterLevelMax[i] = ChapterLevel[i].transform.childCount; //各關卡的數量
                for (int j = 0; j < ChapterLevelMax[i]; j++)
                {
                    ChapterLevelPass[i] = 0;
                }
            }
            Repeat = false;
            Save();//沒存檔先讀檔會有問題，所以先存一次
        }
        Load();
        for (int i = 0; i < ChapterMax; i++)
        {
            for (int j = 0; j < ChapterLevelMax[i]; j++)
            {
                ChapterLevel[i].transform.GetChild(j).GetChild(0).GetComponent<Text>().text = (i + 1) + "-" + (j + 1);
            }
        }
        #endregion

        //如果過關了
        if (UIControl.Chap != 0 && UIControl.Level != 0)
            PassChapterLevel(UIControl.Chap, UIControl.Level, StarsNum[UIControl.Chap - 1, UIControl.Level - 1]);

        LimitTXT.transform.gameObject.SetActive(false);
        /*
          初始畫面為通過的關卡，假設要顯示第4章節第6關卡(代表已通過第三章節，已通過第四章節中的第5關卡)
          ChapterPass為通過的章節，假設為3，要顯示第4章節，所以就是ChapterPass+1   => "4" => 顯示第四章節的關卡
            ChapterLevelPass[0]=>第一章節通過的關卡數，所以要顯示第四章節的第6關卡數就是ChapterLevelPass[3]+1 => 4 - 6 
        */
        // ChooseButton((ChapterPass + 1) + "-" + (ChapterLevelPass[ChapterPass] + 1));  //顯示目前已通過的關卡

    }

    // Update is called once per frame
    void Update()
    {
        Save();
        AbilityValue();  //UI上的數值
        LockChapterLevel();// 未通過的關卡會被鎖按鍵

        //過關，測試用。按"上"就是過關，星星數隨機產生
        if (Input.GetKeyDown("up"))
        {
            StarsNum[ChapterNow - 1, ChapterLevelNow - 1] = Random.Range(1, 4);
            print("過關" + ChapterNow + "-" + ChapterLevelNow + "的星星" + StarsNum[ChapterNow - 1, ChapterLevelNow - 1]);
            PassChapterLevel(ChapterNow, ChapterLevelNow, StarsNum[ChapterNow - 1, ChapterLevelNow - 1]);
            //StarsNum[UIControl.Chap - 1, UIControl.Level - 1]
        }
        //存檔，測試用
        if (Input.GetKeyDown("right"))
        {
            Save();

            PlayerPrefs.SetString("JsonData", JsonUtility.ToJson(data));
            //print("儲存" + ChapterPass);
            //PlayerPrefs.SetInt("Chapter", ChapterPass);
        }
        //讀檔，測試用
        if (Input.GetKeyDown("left"))
        {
            data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("JsonData"));

            Load();
            //print("讀檔" + ChapterPass);
            //ChapterPass = PlayerPrefs.GetInt("Chapter");
            print(StarsNum);
        }
        //清除，測試用
        if (Input.GetKeyDown("down"))
        {
            print("清除");
            PlayerPrefs.DeleteKey("JsonData");
        }
    }



    /// <summary>
    /// 過關之後，關卡數+1，顯示星星數(過關的星星數)
    /// </summary>
    public void PassChapterLevel(int Chap, int ChapLevel, int Grade)
    {
        AdvtureWindow.SetActive(true);//先開啟視窗，紀錄星星數再關閉

        if (ChapterPass < ChapterLimit) //小於最大關卡數，過關後關卡+ 1
        {
            //點選關卡後，過關會新增星星。星星會出現在關卡的父物件下。調整大小。
            if (GameObject.Find("星星" + Chap + "-" + ChapLevel) == null) //如果沒有產生過星星，才會產生星星，避免重複產生
            {
                Instantiate(PassStars, ChapterLevel[Chap - 1].transform.GetChild(ChapLevel - 1).position, Quaternion.identity)
                      .name = "星星" + Chap + "-" + ChapLevel;
            }
            GameObject.Find("星星" + Chap + "-" + ChapLevel)
                       .transform.SetParent(ChapterLevel[Chap - 1].transform.GetChild(ChapLevel - 1));
            GameObject.Find("星星" + Chap + "-" + ChapLevel)
                       .transform.localScale = new Vector3(1, 1, 1);
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

        //過關之後，獲得經驗。若經驗值超過最大值則升等，並且提升經驗值的最大值
        //升1等+1能量
        LevelEXPNow += LevelEXP;
        if (LevelEXPNow >= LevelEXPMAX)
        {
            LevelNow += 1;
            LevelEXPNow = LevelEXPNow - LevelEXPMAX;
            LevelEXPMAX *= 1.5f;
            EnergyNow += 1;
        }
        AdvtureWindow.SetActive(false);
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
                ChapterLevel[ChapterNow - 1].transform.GetChild(j - 1).GetComponent<Image>().color = new Color32(255, 255, 0, 255); //選到的關卡變黃色
            }
            else if (ChapterLevelNow != j)
                ChapterLevel[ChapterNow - 1].transform.GetChild(j - 1).GetComponent<Image>().color = new Color32(255, 255, 255, 255); //沒選到的關卡顏色恢復

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
            GameObject.Find("消耗能量").transform.GetChild(0).GetComponent<Text>().text = "X" + BodyHard;
        }
        else if (ModeTXT == "困難模式")
        {
            ChooseModeText.GetComponent<Text>().text = "普通模式";
            GameObject.Find("戰役視窗").GetComponent<Image>().sprite = Resources.Load<Sprite>("背景2");
            GameObject.Find("消耗能量").transform.GetChild(0).GetComponent<Text>().text = "X" + BodyNormal;
        }
    }
    /// <summary>
    /// 戰役視窗，戰鬥按鍵
    /// </summary>
    public void ChangeScene()
    {
        //普通模式，體力扣5
        if (ChooseModeText.GetComponent<Text>().text == "普通模式")
        {
            if (BodyStrngthNow >= BodyNormal) //體力足夠就轉移遊戲場景
            {
                Invoke("RealChangeScene", 1f);
                BodyStrngthNow -= BodyNormal;
                GameObject.Find("UI").GetComponent<FadeInOut>().FadeInNow(); //淡入淡出效果
            }
            else if (BodyStrngthNow < BodyNormal) //體力不夠久顯示體力不足
            {
                BodyStrngthTXT.transform.gameObject.SetActive(false);
                BodyStrngthTXT.transform.gameObject.SetActive(true);
                BodyStrngthTXT.transform.position = ChapterLevel[ChapterNow - 1].transform.GetChild(ChapterLevelNow - 1).position;
            }
        }
        //困難模式，體力扣10
        else if (ChooseModeText.GetComponent<Text>().text == "困難模式")
        {
            if (BodyStrngthNow >= BodyHard) //體力足夠就轉移遊戲場景
            {
                Invoke("RealChangeScene", 1f);
                BodyStrngthNow -= BodyHard;
                GameObject.Find("UI").GetComponent<FadeInOut>().FadeInNow(); //淡入淡出效果
            }
            else if (BodyStrngthNow < BodyHard) //體力不夠久顯示體力不足
            {
                BodyStrngthTXT.transform.gameObject.SetActive(false);
                BodyStrngthTXT.transform.gameObject.SetActive(true);
                BodyStrngthTXT.transform.position = ChapterLevel[ChapterNow - 1].transform.GetChild(ChapterLevelNow - 1).position;
            }
        }
    }
    /// <summary>
    /// 移動至遊戲場景
    /// </summary>
    public void RealChangeScene()
    {
        SceneManager.LoadScene("遊戲場景");
    }



    ////////天賦視窗的按鍵功能////////
    #region
    /// <summary>
    /// 天賦視窗的按鍵功能，按了會出現詢問升級視窗。取消功能。確定功能。(按鍵的編號)
    /// </summary>
    /// <param name="Name"></param>
    public void ChooseTalentButton(int Name)
    {
        TalentSpace = Name;
        //0是玩家體力，1是經驗，2是初始金錢，3是戰鬥CD，4是玩家生命，5是戰鬥金錢

        if (Name < TalenCount)
        {
            TalentLevelUpWindow.SetActive(true);
            TalentLevelUpWindow.transform.position = TalentButton.transform.GetChild(TalentSpace).transform.position;
        }
    }
    /// <summary>
    /// 天賦視窗，詢問升級的取消功能
    /// </summary>
    public void CloseTalentLevelUp()
    {
        TalentLevelUpWindow.SetActive(false);
    }
    /// <summary>
    /// 天賦視窗，詢問升級的確定功能
    /// </summary>
    public void OpenTalentLevelUp()
    {
        //0是玩家體力，1是經驗，2是初始金錢，3是戰鬥CD，4是玩家生命，5是戰鬥金錢
        //如果未達最高等級且能量足夠時，按升級就+1等。若升到最高等級則鎖住按鍵
        if (TalentPoint[TalentSpace] < TalentPointMax[TalentSpace] && EnergyNow >= 1)
        {
            EnergyNow -= 1; //能量-1
            TalentPoint[TalentSpace] += 1;//等級+1

            //升級提升能力
            //其實有些可以不用寫，但這樣比較好修正，所以還是寫出來。
            if (TalentSpace == 0) //每升一級，體力+5
            {
                BodyStrngthNow += 5; BodyStrngthMAX += 5;
            }
            else if (TalentSpace == 1) LevelEXP += 1; //每升一級，過關經驗+1
            else if (TalentSpace == 2) { }                         //每升一級，初始金錢+50。<UIControl>會增加
            else if (TalentSpace == 3) SpaceControl.CoolTime -= 1; //每升一級，冷卻時間-1秒
            else if (TalentSpace == 4) { }                         //每升一級，玩家生命+2。<UIControl>會增加
            else if (TalentSpace == 5) { }                         //每升一級，怪物死亡金錢+2。<EnemControl>會增加

        }
        else if (EnergyNow <= 0)
        {
            //因為有動畫，所以先開啟再關閉
            EnergyTXT.transform.gameObject.SetActive(false);
            EnergyTXT.transform.gameObject.SetActive(true);
            EnergyTXT.transform.position = TalentButton.transform.GetChild(TalentSpace).transform.position;
        }
        TalentLevelUpWindow.SetActive(false); //關閉詢問視窗
    }
    #endregion


    ////////科技視窗的按鍵功能////////
    #region
    /// <summary>
    /// 科技視窗的按鍵功能，按了會出現詢問升級視窗。取消功能。確定功能。(按鍵的編號)
    /// </summary>
    /// <param name="Name"></param>
    public void ChooseTechnologyButton(string Name)
    {
        /*
         Name => 1-1是角色1的+攻擊力[1]，1-2是角色1的+等級上限[2]，1-3是角色1的額外能力[3]。[0]為角色1圖案。
                 2-1是角色2的+攻擊力[5]，2-2是角色2的+等級上限[6]，2-3是角色2的額外能力[7]。[4]為角色2圖案。
         TechnologyPos是數字對應的按鍵位子。 
         TechMAX是各角色可提升的能力數量 。   
        */
        for (int i = 1; i <= SpaceControl.PlayerNum; i++) //角色數量
        {
            for (int j = 1; j <= TechMAX; j++)            //能力數量
            {
                if (Name == i + "-" + j)
                {
                    TechPos = (i - 1) * (TechMAX + 1) + j;
                    TechPosPlus = (i - 1) * (TechMAX) + (j - 1);
                    TechnologyLevelUpWindow.SetActive(true);
                    TechnologyLevelUpWindow.transform.position = TechnologyButton.transform.GetChild(TechPos).transform.position;
                }
            }
        }
    }
    /// <summary>
    /// 科技視窗，詢問升級的取消功能以及視窗底部的取消功能
    /// </summary>
    public void CloseTechnologyLevelUp()
    {
        //詢問升級和視窗底部都有關閉詢問視窗，如果滑動視窗會按到視窗底部，也會關閉視窗
        TechnologyLevelUpWindow.SetActive(false);
    }
    /// <summary>
    /// 科技視窗，詢問升級的確定功能
    /// </summary>
    public void OpenTechnologyLevelUp()
    {
        //如果未達最高等級且能量足夠時，按升級就+1等。若升到最高等級則鎖住按鍵
        if (TechPoint[TechPosPlus] < TechPointMax[TechPosPlus] && EnergyNow >= 1)
        {
            EnergyNow -= 1; //能量-1
            TechPoint[TechPosPlus] += 1;//等級+1

            /*  
            升級提升能力。其實有些可以不用寫，但這樣比較好修正，所以還是寫出來。
            TechPosPlus=0、3、6、9、12、15，<BulletControl>增加角色攻擊力，每升一級，角色1攻擊力+5 。 TechPosPlus=0=>TechPoint[0]+1
            */
            //增加各角色等級上限，原始等級上限2級(Lv0)，最高可提高到3級(Lv1)TXT
            if (TechPosPlus == 1) SpaceControl.LvMax[0] += 1;
            else if (TechPosPlus == 4) SpaceControl.LvMax[1] += 1;
            else if (TechPosPlus == 7) SpaceControl.LvMax[2] += 1;
            else if (TechPosPlus == 10) SpaceControl.LvMax[3] += 1;
            else if (TechPosPlus == 13) SpaceControl.LvMax[4] += 1;
            else if (TechPosPlus == 16) SpaceControl.LvMax[5] += 1;
            else if (TechPosPlus == 5) for (int i = 3; i <= 5; i++) UIControl.Player_Price[i] -= 3;//角色2額外能力，減少成本
            /*
              TechPosPlus=2、5、8、11、14、17，增加角色額外能力   
              2  => <EnemyControl>增加詛咒扣血量，5  => <UIControl>減少建造成本
              8  => <Weaponcontrol>增加攻擊射程 ，11 => <EnemyControl>增加緩速能力
              14 => <BulletControl>增加暈擊機率  ，17 => <<BulletControl>>增加爆擊機率
             */
        }
        else if (EnergyNow <= 0)
        {
            //因為有動畫，所以先開啟再關閉
            EnergyTXT.transform.gameObject.SetActive(false);
            EnergyTXT.transform.gameObject.SetActive(true);
            EnergyTXT.transform.position = TechnologyButton.transform.GetChild(TechPos).transform.position;
        }
        TechnologyLevelUpWindow.SetActive(false); //關閉詢問視窗

    }
    #endregion

    ////////控制上方面板數值變動   ////////
    /// <summary>
    /// UI上的能力值
    /// </summary>
    public void AbilityValue()
    {
        LevelText.GetComponent<Text>().text = LevelNow.ToString();                         //現在等級(文字)
        LevelBar.GetComponent<Image>().fillAmount = LevelEXPNow / LevelEXPMAX;                 //現在經驗值(圖)
        BodyStrengthText.GetComponent<Text>().text = BodyStrngthNow + "/" + BodyStrngthMAX; //現在體力(文字)
        BodyStrengthBar.GetComponent<Image>().fillAmount = BodyStrngthNow / BodyStrngthMAX; //現在體力(圖)
        EnergyText.GetComponent<Text>().text = EnergyNow.ToString();                        //現在能量(文字)

        //天賦視窗的文字
        for (int i = 0; i < TalenCount; i++)
        {
            TalentButton.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Lv" + TalentPoint[i];
            if (TalentPoint[i] >= TalentPointMax[i])
            {
                TalentButton.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Lv" + TalentPoint[i] + "(MAX)";
                TalentButton.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        //科技視窗的文字
        for (int i = 1; i <= SpaceControl.PlayerNum; i++) //角色數量
        {
            for (int j = 1; j <= TechMAX; j++)            //能力數量
            {
                int Pos = (i - 1) * (TechMAX + 1) + j;     //可升級的能力格，[1][2][3]、[5][6][7]...
                int PosPlus = (i - 1) * (TechMAX) + (j - 1);//對應的陣列，    [0][1][2]、[3][4][5]...
                TechnologyButton.transform.GetChild(Pos).GetChild(1).GetComponent<Text>().text = "Lv" + TechPoint[PosPlus];
                if (TechPoint[PosPlus] >= TechPointMax[PosPlus])
                {
                    TechnologyButton.transform.GetChild(Pos).GetChild(1).GetComponent<Text>().text = "Lv" + TechPoint[PosPlus] + "(MAX)";
                    TechnologyButton.transform.GetChild(Pos).GetComponent<Button>().interactable = false;
                }
            }
        }

    }


    ////////控制視窗關閉或開啟////////
    #region
    /// <summary>
    /// 關閉視窗，返回最初介面
    /// </summary>
    public void WindowClose()
    {
        AdvtureWindow.SetActive(false);
        TalentWindow.SetActive(false);
        TechnologyWindow.SetActive(false);
        TalentLevelUpWindow.SetActive(false);
        TechnologyLevelUpWindow.SetActive(false);
    }
    /// <summary>
    /// 打開戰役視窗
    /// </summary>
    public void Window_Advture()
    {
        AdvtureWindow.SetActive(true);
        //因為星星不會儲存，所以每次開啟場景會消失，所以改成開啟視窗就重新產生星星
        for (int i = 0; i < ChapterMax; i++)//先開啟關卡視窗，不然會找不到是否有星星
            ChapterLevelBG.transform.GetChild(i).gameObject.SetActive(true);
        for (int i = 0; i < ChapterPass + 1; i++)
        {
            for (int j = 0; j < ChapterLevelPass[i]; j++)
            {
                if (GameObject.Find("星星" + (i + 1) + "-" + (j + 1)) == null) //如果沒有產生過星星，才會產生星星，避免重複產生
                {
                    Instantiate(PassStars, ChapterLevel[i].transform.GetChild(j).position, Quaternion.identity).name = "星星" + (i + 1) + "-" + (j + 1);
                }
                GameObject.Find("星星" + (i + 1) + "-" + (j + 1)).transform.SetParent(ChapterLevel[i].transform.GetChild(j));
                GameObject.Find("星星" + (i + 1) + "-" + (j + 1)).transform.localScale = new Vector3(1, 1, 1);
                //依照過關星星數改變顏色
                for (int l = 0; l < StarsNum[i, j]; l++)
                    GameObject.Find("星星" + (i + 1) + "-" + (j + 1)).transform.GetChild(l).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
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
    #endregion


}
