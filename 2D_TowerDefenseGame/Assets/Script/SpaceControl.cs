using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//建造角色，觸控的狀態，放在"SapceControl"上
//畫面狀態分為 => 0為無空格，1為空格出現，2為出現選角視窗或進階視窗
//空格狀態分為 => 0無空格，1為空格出現，2為已建造角色
public class SpaceControl : MonoBehaviour
{
    ////參數設定////
    public static int PlayerNum = 4;   //角色數量，讓其他腳本使用
    public static int LvMax = 3;       //角色最大等級
    public static float CoolTime = 3f; //建造砲塔的冷卻時間
    ////////////////
    int PictureState = 0;             //畫面狀態
    public static GameObject[] SpacePoints;         //所有空格的陣列名稱
    int[] SpaceState;                 //空格狀態
    public static int[] LvState;      //砲塔等級
    int[] PlayerCount;                //建造時的冷卻狀態，0為無，1為正在建造
    public static int[] PlayerKind;   //砲塔建造種類，角色1、角色2...

    public GameObject ChoosePlayer;    //選角視窗
    public GameObject ChoosePlayerPlus;//進階視窗(升級or販賣)
    public GameObject CDObj;           //放置"CD底部"
    public Text NoMoney;               //放置沒錢的TXT
    public Text DontBuildTxt;          //顯示不能建造的TXT
    public Text DontLvUpTxt;           //顯示不能升級的TXT
    public static int Choose_i = -1;   //紀錄碰到的空格編號，-1代表沒有儲存
    public static int Choose_j = -1;   //紀錄碰到的砲塔編號，-1代表沒有儲存
    public string PlayerName;          //紀錄碰到的砲塔名稱
    public string PlayerTag;           //紀錄碰到的砲塔Tag

    Vector2 Mouse_pos;                //紀錄觸碰的座標
    RaycastHit2D hit;

    public GameObject Player1, Player2, Player3, Player4, Player5, Player6;   //放置角色的預置物
    float TXTCountDown = 0f;           //"不能建造"文字的倒數計數器

    /*觸控間隔，時間內才算點擊，超過就算移動。無使用
    float begainTime = 0f;              //觸控螢幕開始的時間
    float intervals;                    //觸控螢幕和放開的間隔
    float DelayTime = 0.5f;             //幾秒內放開才算點擊，超過就算移動
    */
    // Start is called before the first frame update
    void Start()
    {
        //開場時，先設定空格位置
        SpacePoints = new GameObject[transform.childCount];  //空格位子為此物體的子物件
        SpaceState = new int[SpacePoints.Length];            //空格狀態的數量=空格數量
        LvState = new int[SpacePoints.Length];               //砲塔等級
        PlayerCount = new int[SpacePoints.Length];           //建造時的冷卻狀態，0為無，1為正在建造
        PlayerKind = new int[SpacePoints.Length];            //砲塔建造種類，角色1、角色2...
        for (int i = 0; i < SpacePoints.Length; i++)
        {
            SpacePoints[i] = transform.GetChild(i).gameObject;  //紀錄所有空格
            SpacePoints[i].SetActive(false);                    //讓所有空格先關閉
            SpaceState[i] = 0;                                  //記錄所有空格的狀態為0   
            LvState[i] = 0;
            PlayerCount[i] = 0;
            PlayerKind[i] = 0;
        }

        //開場時先關閉選角視窗和進階視窗，視窗底部，文字
        ChoosePlayer.gameObject.SetActive(false);
        ChoosePlayerPlus.gameObject.SetActive(false);
        DontBuildTxt.gameObject.SetActive(false);
        DontLvUpTxt.gameObject.SetActive(false);
        NoMoney.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        //觸控間隔，時間內才算點擊，超過就算移動。原本要加在Input.GetMouseButtonUp(0)後面" && intervals < DelayTime)"，但不使用了
        if (Input.GetMouseButtonDown(0))
        {
            begainTime = Time.realtimeSinceStartup;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            intervals = Time.realtimeSinceStartup - begainTime;
        }
        */

        if (GameObject.FindWithTag("Window") == null)//新增，視窗消失，觸碰其他地方才有反應
        {

            //初始的畫面狀態為0，無空位
            Vector2 Mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //紀錄滑鼠觸碰的2D座標比例
            RaycastHit2D hit = Physics2D.Raycast(Mouse_pos, Vector2.zero);           //2D使用的指令 

            ////畫面狀態0且觸碰螢幕時 => 進入畫面狀態1(空格出現)////
            if (Input.GetMouseButtonDown(0) && PictureState == 0 && Time.timeScale == 1)//新增暫停按鍵，暫停時會出現視窗，且不能觸碰螢幕。遊戲開始時才能觸碰螢幕
            {
                if (GameObject.FindWithTag("Window") == null)
                {
                    if (hit.collider == null) State_1();       //執行畫面狀態1
                    else if (hit.collider.tag == "Button") { }
                    else if (hit.collider != null) AdvancedWindow(hit.collider.name, hit.collider.tag);                   //若有建造砲塔且觸碰砲塔時    
                }
            }

            ////畫面狀態1且觸碰螢幕時 => 觸碰空格進入畫面狀態2 or 無觸碰到空格進入畫面狀態0////
            else if (Input.GetMouseButtonDown(0) && PictureState == 1 && Time.timeScale == 1)
            {
                if (hit.collider == null) State_0();                                      //執行畫面狀態0
                else if (hit.collider.tag == "Weapon Space") State_2(hit.collider.name);  //執行畫面狀態2 
                else AdvancedWindow(hit.collider.name, hit.collider.tag);                 //若有建造砲塔且觸碰砲塔時}       
            }
            ////畫面狀態2且觸碰螢幕時 => 觸碰角色視窗建造角色，空格狀態變為2 or 觸控到其他空格，繼續執行畫面狀態2 or 無觸碰到空格進入畫面狀態0////
            else if (Input.GetMouseButtonDown(0) && PictureState == 2 && Time.timeScale == 1)
            {
                if (hit.collider == null) State_0();
                else if (hit.collider.name == "角色1按鍵") BuildPlayer(Player1, hit.collider.name);  //建造角色
                else if (hit.collider.name == "角色2按鍵") BuildPlayer(Player2, hit.collider.name);
                else if (hit.collider.name == "角色3按鍵") BuildPlayer(Player3, hit.collider.name);
                else if (hit.collider.name == "角色4按鍵") BuildPlayer(Player4, hit.collider.name);
                else if (hit.collider.name == "角色5按鍵") BuildPlayer(Player5, hit.collider.name);
                else if (hit.collider.name == "角色6按鍵") BuildPlayer(Player6, hit.collider.name);
                else if (hit.collider.name == "升級") ChangePlayer();
                else if (hit.collider.name == "販賣") SellPlayer();
                else if (hit.collider.tag == "Weapon Space") State_2(hit.collider.name);     //執行畫面狀態2  
                else AdvancedWindow(hit.collider.name, hit.collider.tag);                    //進階視窗   
            }
        }
        //控制TXT文字
        TXTCountDown -= Time.deltaTime;
        if (TXTCountDown <= 0f)
        {
            DontBuildTxt.gameObject.SetActive(false);
            DontLvUpTxt.gameObject.SetActive(false);
            NoMoney.gameObject.SetActive(false);
        }

        //砲塔使用的倒數計數器，時間到就消失        
        for (int i = 0; i < PlayerCount.Length; i++) //讓所有CD不會互相衝突
        {
            if (GameObject.Find("CD" + i) != null)//如果出現CD
            {
                GameObject.Find("CD" + i).transform.SetParent(GameObject.Find("UI").transform);                                //沒加父物件就不會出現，而且此物件(CD底部)也不會出現
                GameObject.Find("CD" + i).transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("砲塔" + i).transform.position +Vector3.up*-1);
                if (PlayerCount[i] == 1)   //建造時的冷卻狀態，0為無，1為正在建造
                {
                    GameObject.Find("CD" + i).transform.GetChild(0).GetComponent<Image>().fillAmount -= Time.deltaTime / CoolTime; //此物件(CD底部)的子物件(CD)，隨著時間讓圖片改變 
                    if (GameObject.Find("CD" + i).transform.GetChild(0).GetComponent<Image>().fillAmount / CoolTime <= 0f)
                    {
                        PlayerCount[i] = 0;
                        Destroy(GameObject.Find("CD" + i));
                    }
                }
            }
        }
    }

    ////觸控的狀態////
    //畫面狀態分為 => 0為無空格，1為空格出現，2為出現選角視窗
    //空格狀態分為 => 0無空格，1為空格出現，2為已建造角色
    /// <summary>
    /// 畫面狀態0(空格消失)
    /// </summary>
    public void State_0()
    {
        PictureState = 0;     //畫面狀態變為0，空格狀態此時為1就變為0                             
        for (int i = 0; i < SpacePoints.Length; i++)//當畫面狀態從1變為0時(空格消失)，空格狀態若為1則變為0(空格消失)
        {
            ChangePic(SpacePoints[i].name, "空格"); //讓所有空格恢復空格圖案

            if (SpaceState[i] < 2)
            {
                SpaceState[i] = 0;
                SpacePoints[i].SetActive(false);         //空格消失
            }
        }
        ChoosePlayer.gameObject.SetActive(false);//選角視窗消失
        ChoosePlayerPlus.gameObject.SetActive(false);//進階視窗消失
        DontBuildTxt.gameObject.SetActive(false);    //TXT文字消失
        DontLvUpTxt.gameObject.SetActive(false);
        NoMoney.gameObject.SetActive(false);
    }
    /// <summary>
    /// 畫面狀態1(空格出現)
    /// </summary>
    public void State_1()
    {
        PictureState = 1;     //畫面狀態變為1                              
        for (int i = 0; i < SpacePoints.Length; i++)//當畫面狀態從0變為1時(空格出現)，空格狀態若為0則變為1(空格出現)
        {
            if (SpaceState[i] == 0)
            {
                SpacePoints[i].SetActive(true);
                SpaceState[i] = 1;
            }
        }
    }
    /// <summary>
    /// 畫面狀態2(出現選角視窗)
    /// </summary>
    /// <param name="Name"></param>
    public void State_2(string Name)
    {
        for (int i = 0; i < SpacePoints.Length; i++)
        {
            if (SpaceState[i] == 1 && Name == SpacePoints[i].name)//必須要點到空格狀態為1的塔，才進入狀態2(比如點建造後的，就不會有反應)
            {
                PictureState = 2;     //畫面狀態變為2，出現選角視窗     
                ChoosePlayer.gameObject.SetActive(true);//開啟選角視窗且關閉進階視窗
                ChoosePlayerPlus.gameObject.SetActive(false);
                ChoosePlayer.transform.position = GameObject.Find(SpacePoints[i].name).transform.position ;//選角視窗位子會在空格中間
                Choose_i = i;         //紀錄被點選空格的位子，給BuildPlayer1()使用

                ChangePic(SpacePoints[i].name, "選取空格"); //被點選的格子會換成選取空格的圖案
            }
            else ChangePic(SpacePoints[i].name, "空格");    //點選第二個空格時，讓第一個被選取的空格恢復空格圖案
        }
    }

    ////選角視窗的功能，畫面狀態2時，觸碰角色視窗的按鈕建造角色////
    /// <summary>
    /// 建造角色(要建造的角色，觸碰的按鍵名字)
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="ButtonName"></param>
    public void BuildPlayer(GameObject Name, string ButtonName)
    {
        for (int i = 1; i < PlayerNum + 1; i++)
            if (ButtonName == "角色" + i + "按鍵") PlayerKind[Choose_i] = i;  //觸碰的按鍵名字=角色種類，角色1、2...
        int Seat = LvMax * (PlayerKind[Choose_i] - 1); //金錢，讀取 UIControl.Player_Price[0]、[3]、[6]、[9]
        if (UIControl.PlayerMoney >= UIControl.Player_Price[Seat]) //金錢夠才能建造
        {
            State_0();                  //選完角色之後，空格消失
            SpaceState[Choose_i] = 2;  //空格狀態為2
            LvState[Choose_i] = 1;     //砲塔等級為1
            Instantiate(Name, SpacePoints[Choose_i].transform.position, Quaternion.identity).name = "砲塔" + Choose_i;  //產生的砲塔，命名為砲塔i
            //GameObject.Find("砲塔" + Choose_i).transform.localScale = new Vector2(0.6f, 0.6f);                              //改變角色大小
            CreatCDUI(Choose_i);      //建造砲塔的冷卻時間的UI
            UIControl.PlayerMoney -= UIControl.Player_Price[Seat];//建造砲塔後扣錢
        }
        else //金錢不夠就不能建造且顯示沒錢的文字
        {
            TXTCountDown = 1.1f;                 //播放時間
            NoMoney.gameObject.SetActive(false); //因為有做動畫，所以必須關閉再開啟，就會再撥放 
            NoMoney.gameObject.SetActive(true);
            GameObject.Find("沒錢TXT").transform.position = Camera.main.WorldToScreenPoint(ChoosePlayer.transform.position + Vector3.up * 1);
        }

    }

    ////出現進階視窗，畫面狀態2時，可升級或販賣////
    /// <summary>
    /// 出現進階視窗(觸碰的角色名字，觸碰的角色Tag)
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="tag"></param>
    public void AdvancedWindow(string Name, string tag)
    {
        PictureState = 2;
        PlayerName = Name;  //紀錄名字給其他使用
        PlayerTag = tag;
        for (int i = 1; i < PlayerNum + 1; i++)  //判斷是"Player" + i
        {
            for (int j = 0; j < SpacePoints.Length; j++)
            {
                ChangePic(SpacePoints[j].name, "空格");    //空格恢復空格圖案
                if (tag == "Player" + i && Name == "砲塔" + j)
                {
                    Choose_j = j;                                     //點選砲塔的編號
                    ChoosePlayerPlus.gameObject.SetActive(true);      //點選砲塔時就開起進階視窗，關閉選角視窗
                    ChoosePlayer.gameObject.SetActive(false);
                    ChoosePlayerPlus.transform.position = GameObject.Find(Name).transform.position ;//進階視窗位子會在砲塔中間  
                    int LvNext = LvState[Choose_j] + 1;               //原本Lv1，要升級Lv2
                    ChangePic("升級", "Player/頭像/角色-0" + i );     //進階視窗的圖片更換(UI的Image)，原本Lv1，要顯示Lv2的圖片
                    //ChangePricePic("升級", i, LvNext);  //金錢
                    if (LvNext > LvMax) ChangePic("升級", "Price/不能建造");         //超過最高等級，就會顯示不能建造的圖片
                }
            }
        }
    }
    /// <summary>
    /// //更換視窗的圖片(要更換的物件名稱，更換後的圖片)
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Pic"></param>
    public void ChangePic(string Name, string Pic)
    {
        if (GameObject.Find(Name) != null)
            GameObject.Find(Name).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Pic); //更換圖片
    }

    public void ChangePricePic(string Name, int Player, int Lv)
    {
        /*  <UIControl>的Player_Price，各角色金錢
            [0][1][2] 、[3][4][5]、[6][7][8]、[9][10][11]
            角色1等級1或2或3...以此類推，但這裡只會改變等級2和3
        */
        int PriceTemp = 0; //金錢
        for (int i = 2; i <= LvMax; i++) //升級就只有升2或3
            if (Lv == i) PriceTemp = UIControl.Player_Price[LvMax * (Player - 1) + i - 1];

        int PriceLength = (PriceTemp.ToString()).Length;                       //金錢的長度
        int PriceCount = GameObject.Find(Name).transform.childCount;           //圖的長度

        //先讓圖變空圖案
        for (int i = 0; i < PriceCount; i++) GameObject.Find(Name).transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("");


        //換金錢的圖
        for (int i = 0; i < PriceLength; i++)
        {
            if (GameObject.Find(Name) != null && PriceLength == PriceCount)         //金錢和圖的長度一樣，就直接換 
            {
                GameObject.Find(Name).transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Price/Price_" + (PriceTemp.ToString())[i]);
            }
            else if (GameObject.Find(Name) != null && PriceLength != PriceCount)   //金錢和圖的長度不一樣
            {
                /*計算圖的長度-金錢的長度，看缺多少就少算一些數字。
                    圖[0][1][2]
                  金錢   [0][1]
                  所以圖的[0]為空，[1]為金錢的0，[2]為金錢的1
                */
                int Unnes = PriceCount - PriceLength;
                GameObject.Find(Name).transform.GetChild(i + Unnes).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Price/Price_" + (PriceTemp.ToString())[i]);
            }

        }
        //如果超過最高等級，就讓他變為空
        for (int i = 0; i < PriceCount; i++) if (Lv > LvMax) GameObject.Find(Name).transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("");

    }


    /// <summary>
    /// 進階視窗的功能，升級
    /// </summary>
    public void ChangePlayer()
    {
        if (GameObject.Find("CD" + Choose_j) == null)//CD消失前，按了不會有反應
        {
            /*要讀取 UIControl.Player_Price[1][2]、[4][5]、[7][8]、[10][11]
              [1]=3*0+1，[4]=3*1+1，[7]=3*2+1，[10]=3*3+1 => 角色1的等級1升2、角色2的等級1升2... 
              [2]=3*0+2，[5]=3*1+2，[78=3*2+2，[11]=3*3+2 => 角色1的等級2升3、角色2的等級2升3...
              最大等級 * 角色種類 + 現有等級
            */
            int Seat = LvMax * (PlayerKind[Choose_j] - 1) + LvState[Choose_j];

            if (LvState[Choose_j] < LvMax)//等級內才可以升級
            {
                if (UIControl.PlayerMoney >= UIControl.Player_Price[Seat]) //新增，金錢夠才能升級
                {
                    Destroy(GameObject.Find(PlayerName).GetComponent<WeaponControl>()); //因為WeaponControl啟用時，會間隔時間才攻擊，先刪除腳本，再加入腳本(WeaponControl也會自動加入子彈Prefab)
                    GameObject.Find(PlayerName).AddComponent<WeaponControl>();
                    State_0();
                    for (int i = 1; i < PlayerNum + 1; i++)  //判斷是"Player" + i
                    {
                        if (PlayerTag == "Player" + i)
                        {
                            LvState[Choose_j] += 1;       //升等
                            GameObject.Find(PlayerName).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/角色" + i + "_LV" + LvState[Choose_j]);
                            UIControl.PlayerMoney -= UIControl.Player_Price[Seat];//升級砲塔後扣錢
                            CreatCDUI(Choose_j);
                        }
                    }
                }
                else//錢不夠就不能升級
                {
                    TXTCountDown = 1.1f;                 //播放時間
                    NoMoney.gameObject.SetActive(false); //因為有做動畫，所以必須關閉再開啟，就會再撥放 
                    NoMoney.gameObject.SetActive(true);
                    GameObject.Find("沒錢TXT").transform.position = Camera.main.WorldToScreenPoint(GameObject.Find(PlayerName).transform.position + Vector3.up * 1);
                }
            }
            else  //如果超過等級就顯示不能建造
            {
                TXTCountDown = 1.1f;                      //播放時間
                DontBuildTxt.gameObject.SetActive(false); //因為有做動畫，所以必須關閉再開啟，就會再撥放 
                DontBuildTxt.gameObject.SetActive(true);
                GameObject.Find("不能建造TXT").transform.position = Camera.main.WorldToScreenPoint(GameObject.Find(PlayerName).transform.position + Vector3.up * 1);
            }
        }
        else //如果冷卻中就顯示不能升級
        {
            TXTCountDown = 1.1f;                     //播放時間
            DontLvUpTxt.gameObject.SetActive(false); //因為有做動畫，所以必須關閉再開啟，就會再撥放 
            DontLvUpTxt.gameObject.SetActive(true);
            GameObject.Find("不能升級TXT").transform.position = Camera.main.WorldToScreenPoint(GameObject.Find(PlayerName).transform.position + Vector3.up * 1);
        }

    }

    /// <summary>
    /// 進階視窗的功能，販賣
    /// </summary>
    public void SellPlayer()
    {
        //將目前升級的錢加總起來，賣掉會得到0.8倍的金錢
        /*         
         [0][1][2] 、[3][4][5]、[6][7][8]、[9][10][11]
         角色1等級1賣掉，會加上[0]。角色1等級2賣掉，會加上[0][1]。角色1等級3賣掉，會加上[0][1][2]...以此類推
         最大等級 * 角色種類 + 現有等級
         */
        int Sum = 0;
        for (int i = 0; i < LvState[Choose_j]; i++) //等級
        {
            for (int j = 1; j < PlayerNum + 1; j++)  //種類
            {
                if (PlayerTag == "Player" + j)
                {
                    Sum += UIControl.Player_Price[LvMax * (j - 1) + i];
                }
            }
        }
        State_0();
        SpaceState[Choose_j] = 0;
        Destroy(GameObject.Find(PlayerName));
        UIControl.PlayerMoney += Sum * 8 / 10;
        if (GameObject.Find("CD" + Choose_j) != null) Destroy(GameObject.Find("CD" + Choose_j)); //CD消失前，按了會賣掉，也會消除CD
    }

    /// <summary>
    /// 產生建造和升級的冷卻CD
    /// </summary>
    /// <param name="n"></param>
    public void CreatCDUI(int n)
    {
        Instantiate(CDObj).name = "CD" + n;  //CD命名，讓各CD不會衝突到
        PlayerCount[n] = 1;                  //建造時的冷卻狀態，0為無，1為正在建造
    }

}
