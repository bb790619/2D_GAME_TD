using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//建造角色
//觸控的狀態
//畫面狀態分為 => 0為無空格，1為空格出現，2為出現選角視窗或進階視窗
//空格狀態分為 => 0無空格，1為空格出現，2為已建造角色

public class SpaceControl : MonoBehaviour
{

    int PictureState = 0;             //畫面狀態
    GameObject[] SpacePoints;         //所有空格的陣列名稱
    int[] SpaceState;                 //空格狀態
    int[] LvState;

    float Window_Length;           //當作選角視窗的長和寬(可調整SIZE-選角視窗大小)
    float Window_Width;
    float Advanced_Length;         //當作進階視窗的長和寬(可調整SIZE-進階視窗大小)
    float Advanced_Width;
    float Player_Length;           //當作角色大小的長和寬(可調整SIZE-角色大小)
    float Player_Width;
    float Pos_X;                   //可點選範圍的長和寬
    float Pos_Y;

    public Image ChoosePlayer;         //選角視窗
    public Image ChoosePlayerPlus;     //進階視窗(升級or販賣)
    int Choose_i = -1;                 //紀錄碰到的空格編號，-1代表沒有儲存
    int Choose_j = -1;                 //紀錄碰到的砲塔編號，-1代表沒有儲存
    public string PlayerName;          //紀錄碰到的砲塔名稱
    public string PlayerTag;           //紀錄碰到的砲塔Tag

    Vector2 Mouse_pos;                 //紀錄觸碰的座標
    RaycastHit2D hit;

    public static int PlayerNum = 2;    //角色數量，讓其他腳本使用
    public GameObject Player1;          //放置角色1的預置物
    public GameObject Player2;          //放置角色2的預置物


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


        //開場時先關閉選角視窗和進階視窗，和視窗底部
        ChoosePlayer.gameObject.SetActive(false);
        ChoosePlayerPlus.gameObject.SetActive(false);

        //設定選角視窗大小、進階視窗大小和角色大小
        Window_Length = GameObject.Find("選角視窗大小").GetComponent<SpriteRenderer>().bounds.size.x;
        Window_Width = GameObject.Find("選角視窗大小").GetComponent<SpriteRenderer>().bounds.size.y;
        Advanced_Length = GameObject.Find("進階視窗大小").GetComponent<SpriteRenderer>().bounds.size.x;
        Advanced_Width = GameObject.Find("進階視窗大小").GetComponent<SpriteRenderer>().bounds.size.y;
        Player_Length = GameObject.Find("角色大小").GetComponent<SpriteRenderer>().bounds.size.x;
        Player_Width = GameObject.Find("角色大小").GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        //初始的畫面狀態為0，無空位
        Vector2 Mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //紀錄滑鼠觸碰的2D座標比例
        RaycastHit2D hit = Physics2D.Raycast(Mouse_pos, Vector2.zero);           //2D使用的指令 

        ////畫面狀態0且觸碰螢幕時 => 進入畫面狀態1(空格出現)////
        if (Input.GetMouseButtonDown(0) && PictureState == 0)
        {
            if (hit.collider == null) State_1();       //執行畫面狀態1
            else if (hit.collider != null) AdvancedWindow(hit.collider.name, hit.collider.tag);                   //若有建造砲塔且觸碰砲塔時        
        }

        ////畫面狀態1且觸碰螢幕時 => 觸碰空格進入畫面狀態2 or 無觸碰到空格進入畫面狀態0////
        else if (Input.GetMouseButtonDown(0) && PictureState == 1)
        {
            if (hit.collider == null) State_0();                                      //執行畫面狀態0
            else if (hit.collider.tag == "Weapon Space") State_2(hit.collider.name);  //執行畫面狀態2 
            else AdvancedWindow(hit.collider.name, hit.collider.tag);                 //若有建造砲塔且觸碰砲塔時}       
        }
        ////畫面狀態2且觸碰螢幕時 => 觸碰角色視窗建造角色，空格狀態變為2 or 觸控到其他空格，繼續執行畫面狀態2 or 無觸碰到空格進入畫面狀態0////
        else if (Input.GetMouseButtonDown(0) && PictureState == 2)
        {
            if (hit.collider == null && ChoosePlayer.gameObject.activeSelf == true)
            {
                Pos_X = Mathf.Abs(SpacePoints[Choose_i].transform.position.x - Mouse_pos.x);       //可點選範圍 = 選角視窗的長(X) - 觸控位子(X)
                Pos_Y = Mathf.Abs(SpacePoints[Choose_i].transform.position.y + 1f - Mouse_pos.y);
                if (Pos_X >= Window_Length / 2f && Pos_Y >= Window_Width / 2f) State_0();  //超出範圍，執行畫面狀態0。否則不執行(可以按按鍵)
            }
            else if (hit.collider == null &&  ChoosePlayerPlus.gameObject.activeSelf == true)
            {
                Pos_X = Mathf.Abs(GameObject.Find(PlayerName).transform.position.x - Mouse_pos.x);        //可點選範圍 = 進階視窗的長(X) - 觸控位子(X)
                Pos_Y = Mathf.Abs(GameObject.Find(PlayerName).transform.position.y + 1f - Mouse_pos.y);
                if (Pos_X >= Advanced_Length / 2f && Pos_Y >= Advanced_Width / 2f) State_0();  //超出範圍，執行畫面狀態0。否則不執行(可以按按鍵)}
            }
            else if (hit.collider.tag == "Weapon Space") State_2(hit.collider.name);            //執行畫面狀態2                                            
            else AdvancedWindow(hit.collider.name, hit.collider.tag);                           //進階視窗   }
        }
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
                ChoosePlayer.transform.position = Camera.main.WorldToScreenPoint(SpacePoints[i].transform.position + Vector3.up * 1);//選角視窗位子會在空格上方
                Choose_i = i;         //紀錄被點選空格的位子，給BuildPlayer1()使用
            }
        }
    }

    ////進階視窗，可升級或販賣////
    public void AdvancedWindow(string Name, string tag) //進階視窗，限定點選範圍
    {
        PictureState = 2;
        PlayerName = Name;
        PlayerTag = tag;
        for (int i = 1; i < PlayerNum + 1; i++)  //判斷是"Player" + i
        {
            for (int j = 0; j < SpacePoints.Length; j++)
            {
                if (tag == "Player" + i && Name == "砲塔" + j)
                {
                    ChoosePlayerPlus.gameObject.SetActive(true);      //點選砲塔時就開起進階視窗，關閉選角視窗
                    ChoosePlayer.gameObject.SetActive(false);
                    ChoosePlayerPlus.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find(Name).transform.position + Vector3.up * 1);//進階視窗位子會在砲塔上方  
                    ChangeUIPic("角色" + i + "_LV2(範例)");           //進階視窗的圖片更換(UI的Image)
                    Choose_j = j;
                }
            }
        }
    }
    public void ChangePic(string Pic) //更換圖片(未完成)
    {
        GameObject.Find("升級圖").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Pic);
    }
    public void ChangeUIPic(string UIPic) //更換UI的Image圖片
    {
        GameObject.Find("升級圖").GetComponent<Image>().sprite = Resources.Load<Sprite>(UIPic); //UI的Image更換圖片
    }


    ////進階視窗，UI的BUTTON////
    public void ChangePlayer()
    {
        State_0();
        print("升級" + PlayerName);
        SpaceState[Choose_j] += 1;                //升等，state:2=.Lv1，state:3=.Lv2，以此類推
        int Lv = SpaceState[Choose_j] - 1;
        for (int i = 1; i < PlayerNum + 1; i++)  //判斷是"Player" + i
        {
            if (PlayerTag == "Player" + i)
            {
                GameObject.Find(PlayerName).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("角色" + i + "_LV" + Lv + "(範例)");
            }
        }
    }

    public void SellPlayer()
    {
        print("販賣" + PlayerName);
        State_0();
        SpaceState[Choose_j] = 0;
        Destroy(GameObject.Find(PlayerName));
    }

    ////選角視窗，UI的Button////
    public void BuildPlayer1()    //畫面狀態2時，觸碰角色視窗的按鈕1建造角色1
    {
        State_0();//選完角色之後，空格消失
        SpaceState[Choose_i] = 2;  //空格狀態為2  
        Instantiate(Player1, SpacePoints[Choose_i].transform.position, Quaternion.identity).name = "砲塔" + Choose_i;//產生的砲塔，命名為砲塔i
    }
    public void BuildPlayer2()     //畫面狀態2時，觸碰角色視窗的按鈕2建造角色2
    {
        State_0();//選完角色之後，空格消失
        SpaceState[Choose_i] = 2;  //空格狀態為2
        Instantiate(Player2, SpacePoints[Choose_i].transform.position, Quaternion.identity).name = "砲塔" + Choose_i;//產生的砲塔，命名為砲塔i
    }


}
/*
Sprite sp;
float s_width;
float s_height;
void Start()
{
//紀錄原圖的SIZE
s_width = GetComponent<SpriteRenderer>().bounds.size.x;
s_height = GetComponent<SpriteRenderer>().bounds.size.y;
}
void OnMouseDown()
{
//改變新圖，Resources的圖
sp = Resources.Load<Sprite>("錢");
//讓新圖的大小和原圖一樣，假設原圖100，新圖120，120(新圖)*( 100(原圖)/120(新圖) )=100
GetComponent<SpriteRenderer>().sprite = sp;
GetComponent<SpriteRenderer>().transform.localScale = new Vector2(s_width / GetComponent<SpriteRenderer>().bounds.size.x, s_height / GetComponent<SpriteRenderer>().bounds.size.y);
}*/
