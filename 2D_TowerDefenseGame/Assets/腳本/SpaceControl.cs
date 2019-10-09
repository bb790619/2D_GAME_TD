using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//建造角色，觸控的狀態
//畫面狀態分為 => 0為無空格，1為空格出現，2為出現選角視窗或進階視窗
//空格狀態分為 => 0無空格，1為空格出現，2為已建造角色
public class SpaceControl : MonoBehaviour
{
    //可調整的參數
    public static int PlayerNum = 4;   //角色數量，讓其他腳本使用
    int LvMax = 3;                     //角色最大等級

    int PictureState = 0;             //畫面狀態
    GameObject[] SpacePoints;         //所有空格的陣列名稱
    int[] SpaceState;                 //空格狀態
    int[] LvState;                    //砲塔等級

    public GameObject ChoosePlayer;    //選角視窗
    public GameObject ChoosePlayerPlus;//進階視窗(升級or販賣)
    public Text DontBuildTxt;         //顯示不能建造的TXT
    int Choose_i = -1;                 //紀錄碰到的空格編號，-1代表沒有儲存
    int Choose_j = -1;                 //紀錄碰到的砲塔編號，-1代表沒有儲存
    public string PlayerName;          //紀錄碰到的砲塔名稱
    public string PlayerTag;           //紀錄碰到的砲塔Tag

    public static Vector2 Mouse_pos;    //紀錄觸碰的座標
    public static RaycastHit2D hit;     //static將值給CameraControl腳本使用

    public GameObject Player1;          //放置角色1的預置物
    public GameObject Player2;          //放置角色2的預置物
    public GameObject Player3;          //放置角色3的預置物
    public GameObject Player4;          //放置角色3的預置物
    int Lv = 1;                        //角色等級
    float TXTCountDown = 0f;           //不能建造文字的倒數計數器

    float begainTime = 0f;              //觸控螢幕開始的時間
    float intervals;                    //觸控螢幕和放開的間隔
    float DelayTime = 0.2f;               //幾秒內放開才算點擊，超過就算移動

    // Start is called before the first frame update
    void Start()
    {
        //開場時，先設定空格位置
        SpacePoints = new GameObject[transform.childCount];  //空格位子為此物體的子物件
        SpaceState = new int[SpacePoints.Length];   //空格狀態的數量=空格數量
        LvState = new int[SpacePoints.Length];
        for (int i = 0; i < SpacePoints.Length; i++)
        {
            SpacePoints[i] = transform.GetChild(i).gameObject;  //紀錄所有空格
            SpacePoints[i].SetActive(false);                    //讓所有空格先關閉
            SpaceState[i] = 0;                               //記錄所有空格的狀態為0   
            LvState[i] = 0;
        }


        //開場時先關閉選角視窗和進階視窗，視窗底部，文字
        ChoosePlayer.gameObject.SetActive(false);
        ChoosePlayerPlus.gameObject.SetActive(false);
        DontBuildTxt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //初始的畫面狀態為0，無空位
        Vector2 Mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //紀錄滑鼠觸碰的2D座標比例
        RaycastHit2D hit = Physics2D.Raycast(Mouse_pos, Vector2.zero);           //2D使用的指令 

        ////新增觸控間隔，時間內才算點擊，超過就算移動////
        if (Input.GetMouseButtonDown(0))
        {
            begainTime = Time.realtimeSinceStartup;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            intervals = Time.realtimeSinceStartup - begainTime;
        }

        ////畫面狀態0且觸碰螢幕時 => 進入畫面狀態1(空格出現)////
        if (Input.GetMouseButtonUp(0) && PictureState == 0 && intervals < DelayTime)
        {
            if (hit.collider == null) State_1();       //執行畫面狀態1
            else if (hit.collider != null) AdvancedWindow(hit.collider.name, hit.collider.tag);                   //若有建造砲塔且觸碰砲塔時        
        }

        ////畫面狀態1且觸碰螢幕時 => 觸碰空格進入畫面狀態2 or 無觸碰到空格進入畫面狀態0////
        else if (Input.GetMouseButtonUp(0) && PictureState == 1 && intervals < DelayTime)
        {
            if (hit.collider == null) State_0();                                      //執行畫面狀態0
            else if (hit.collider.tag == "Weapon Space") State_2(hit.collider.name);  //執行畫面狀態2 
            else AdvancedWindow(hit.collider.name, hit.collider.tag);                 //若有建造砲塔且觸碰砲塔時}       
        }
        ////畫面狀態2且觸碰螢幕時 => 觸碰角色視窗建造角色，空格狀態變為2 or 觸控到其他空格，繼續執行畫面狀態2 or 無觸碰到空格進入畫面狀態0////
        else if (Input.GetMouseButtonUp(0) && PictureState == 2 && intervals < DelayTime)
        {
            if (hit.collider == null) State_0();
            else if (hit.collider.name == "角色1按鍵") BuildPlayer1();  //建造角色
            else if (hit.collider.name == "角色2按鍵") BuildPlayer2();
            else if (hit.collider.name == "角色3按鍵") BuildPlayer3();
            else if (hit.collider.name == "角色4按鍵") BuildPlayer4();
            else if (hit.collider.name == "升級") ChangePlayer();
            else if (hit.collider.name == "販賣") SellPlayer();
            else if (hit.collider.tag == "Weapon Space") State_2(hit.collider.name);     //執行畫面狀態2                                            
            else AdvancedWindow(hit.collider.name, hit.collider.tag);                     //進階視窗   
        }

        //控制TXT文字
        TXTCountDown -= Time.deltaTime;
        if (TXTCountDown <= 0f) DontBuildTxt.gameObject.SetActive(false);
    }

    ////觸控的狀態////
    //畫面狀態分為 => 0為無空格，1為空格出現，2為出現選角視窗
    //空格狀態分為 => 0無空格，1為空格出現，2為已建造角色
    void State_0()//畫面狀態0(空格消失)
    {
        PictureState = 0;     //畫面狀態變為0，空格狀態此時為1就變為0                             
        for (int i = 0; i < SpacePoints.Length; i++)//當畫面狀態從1變為0時(空格消失)，空格狀態若為1則變為0(空格消失)
        {
            if (SpaceState[i] < 2)
            {
                SpaceState[i] = 0;
                SpacePoints[i].SetActive(false);         //空格消失
            }
        }
        ChoosePlayer.gameObject.SetActive(false);//選角視窗消失
        ChoosePlayerPlus.gameObject.SetActive(false);//進階視窗消失
        DontBuildTxt.gameObject.SetActive(false);
    }
    void State_1()//畫面狀態1(空格出現)
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
    void State_2(string Name)//畫面狀態2(出現選角視窗)
    {
        for (int i = 0; i < SpacePoints.Length; i++)
        {
            if (SpaceState[i] == 1 && Name == SpacePoints[i].name)//必須要點到空格狀態為1的塔，才進入狀態2(比如點建造後的，就不會有反應)
            {
                PictureState = 2;     //畫面狀態變為2，出現選角視窗     
                ChoosePlayer.gameObject.SetActive(true);//開啟選角視窗且關閉進階視窗
                ChoosePlayerPlus.gameObject.SetActive(false);
                ChoosePlayer.transform.position = GameObject.Find(SpacePoints[i].name).transform.position + Vector3.up * 0.5f;//選角視窗位子會在空格上方
                Choose_i = i;         //紀錄被點選空格的位子，給BuildPlayer1()使用
            }
        }
    }

    ////進階視窗，可升級或販賣////
    public void AdvancedWindow(string Name, string tag) //進階視窗
    {
        if (DelayCount.CoolCount <= 0)        //新增，冷卻時間內不能點此砲塔(點了也沒作用)
        {
            PictureState = 2;
            PlayerName = Name;  //紀錄名字給其他使用
            PlayerTag = tag;
            for (int i = 1; i < PlayerNum + 1; i++)  //判斷是"Player" + i
            {
                for (int j = 0; j < SpacePoints.Length; j++)
                {
                    if (tag == "Player" + i && Name == "砲塔" + j)
                    {
                        Choose_j = j;                                     //點選砲塔的編號
                        ChoosePlayerPlus.gameObject.SetActive(true);      //點選砲塔時就開起進階視窗，關閉選角視窗
                        ChoosePlayer.gameObject.SetActive(false);
                        ChoosePlayerPlus.transform.position = GameObject.Find(Name).transform.position + Vector3.up * 0.5f;//進階視窗位子會在砲塔上方  
                        float LvNext = LvState[Choose_j] + 1;
                        ChangePic("Player/角色" + i + "_LV" + LvNext);     //進階視窗的圖片更換(UI的Image)，原本Lv1，要顯示Lv2的圖片
                        if (LvNext > LvMax) ChangePic("不能建造");
                    }
                }
            }
        }
    }
    public void ChangePic(string Pic) //更換視窗的圖片
    {
        GameObject.Find("升級").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Pic); //更換圖片
    }
    //進階視窗的功能，升級//
    public void ChangePlayer() 
    {
        if (LvState[Choose_j] < LvMax) 
        {
            State_0();
            for (int i = 1; i < PlayerNum + 1; i++)  //判斷是"Player" + i
            {
                if (PlayerTag == "Player" + i)
                {
                    LvState[Choose_j] += 1;       //升等

                    //加入延遲計數器，時間結束才能攻擊
                    GameObject.Find(PlayerName).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/角色" + i + "_LV" + LvState[Choose_j]);
                    GameObject.Find(PlayerName).AddComponent<DelayCount>().transform.position= GameObject.Find(PlayerName).transform.position;
                }
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
    //進階視窗的功能，販賣
    public void SellPlayer() 
    {
        State_0();
        SpaceState[Choose_j] = 0;
        Destroy(GameObject.Find(PlayerName));
    }

    ////視窗的功能////
    public void BuildPlayer1()    //畫面狀態2時，觸碰角色視窗的按鈕1建造角色1
    {
        State_0();//選完角色之後，空格消失
        SpaceState[Choose_i] = 2;  //空格狀態為2  
        LvState[Choose_i] = 1;     //砲塔等級為1
        Instantiate(Player1, SpacePoints[Choose_i].transform.position, Quaternion.identity).name = "砲塔" + Choose_i;//產生的砲塔，命名為砲塔i
        GameObject.Find("砲塔" + Choose_i).transform.localScale = new Vector2(2f, 2f);                             //改變大小
    }
    public void BuildPlayer2()     //畫面狀態2時，觸碰角色視窗的按鈕2建造角色2
    {
        State_0();//選完角色之後，空格消失
        SpaceState[Choose_i] = 2;  //空格狀態為2
        LvState[Choose_i] = 1;     //砲塔等級為1
        Instantiate(Player2, SpacePoints[Choose_i].transform.position, Quaternion.identity).name = "砲塔" + Choose_i;//產生的砲塔，命名為砲塔i
        GameObject.Find("砲塔" + Choose_i).transform.localScale = new Vector2(2f, 2f);                             //改變大小
    }
    public void BuildPlayer3()     //畫面狀態2時，觸碰角色視窗的按鈕2建造角色3
    {
        State_0();//選完角色之後，空格消失
        SpaceState[Choose_i] = 2;  //空格狀態為2
        LvState[Choose_i] = 1;     //砲塔等級為1
        Instantiate(Player3, SpacePoints[Choose_i].transform.position, Quaternion.identity).name = "砲塔" + Choose_i;//產生的砲塔，命名為砲塔i
        GameObject.Find("砲塔" + Choose_i).transform.localScale = new Vector2(2f, 2f);                             //改變大小
    }
    public void BuildPlayer4()     //畫面狀態2時，觸碰角色視窗的按鈕2建造角色4
    {
        State_0();//選完角色之後，空格消失
        SpaceState[Choose_i] = 2;  //空格狀態為2
        LvState[Choose_i] = 1;     //砲塔等級為1
        Instantiate(Player4, SpacePoints[Choose_i].transform.position, Quaternion.identity).name = "砲塔" + Choose_i;//產生的砲塔，命名為砲塔i
        GameObject.Find("砲塔" + Choose_i).transform.localScale = new Vector2(2f, 2f);                             //改變大小
    }


}
