using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//控制開始場景，放在開始場景的"StartScene"上
public class StartScene : MonoBehaviour
{
    public Image Setting;   //放置"問號視窗"
    public static bool Right = true; //true代表按鍵在右邊(YES)
    public Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        
        //讀取檔案
        #region
        string XX = PlayerPrefs.GetString("Start");
        print(XX);
        if (XX == "True") Right = true;
        else if (XX == "False") Right = false;
        #endregion
        
        Time.timeScale = 1; //讓遊戲不要處於暫停狀態

        //開場時問號視窗移至外面，讓問號視窗一直都在，用SetActive之後，NO的動畫不會執行
        Setting.transform.position = GameObject.Find("隱藏").transform.position;

        /* YES文字在左邊，NO文字在右邊
          Right=true代表按鍵在右邊，會顯示YES，也就是觀看說明。
          Ani.SetInteger("YesNo",0) => 代表不動
          Ani.SetInteger("YesNo",1) => 代表出現向右滑的動畫
          Ani.SetInteger("YesNo",2) => 代表出現向左滑的動畫
        */
        if (Right == true)  //初始狀態，如果按鍵應該在右邊(Right=true)，會偷偷執行向右滑動讓按鍵在右邊
        {
            Ani.SetInteger("YesNo", 1); 
            Ani.speed = 10f;             //撥放速度加快            
        }
        else if (Right == false)//相反
        {
            Ani.SetInteger("YesNo", 2);
            Ani.speed = 10f;                          
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            print("存檔");
            PlayerPrefs.SetString("Start", Right.ToString());//存檔
        }

        if (Input.GetKeyDown("up"))
        {
            print("清除");
            PlayerPrefs.DeleteKey("Start");//按"上"就清除存檔資料
        }
    }

    ////開始場景的按鍵功能
    public void NextScene() //開始場景的"開始"，會延遲1秒執行
    {
        Invoke("RealNextScene", 1f);
    }
    public void RealNextScene()
    {
        if (Right == true) SceneManager.LoadScene("說明場景"); //代表YES，要觀看說明
        else SceneManager.LoadScene("準備場景");               //代表NO，直接開始遊戲
    }
    public void Quit()//開始場景的"離開"
    {
        Application.Quit();
    }
    public void Settings() //開始場景的"問號"
    {
        Setting.transform.position = GameObject.Find("出現").transform.position; //點選問號時，讓視窗移至"出現"(正中間)
    }
    public void Return() //開始場景的問號的"返回"
    {
        Setting.transform.position = GameObject.Find("隱藏").transform.position; //點選返回時，讓視窗移置"穩藏"
    }

    public void SettingsYesNo() //"問號視窗"，選擇是否要跳過說明
    {
        /* YES文字在左邊，NO文字在右邊
           Right=true代表按鍵在右邊，會顯示YES，也就是觀看說明。
           按下按鍵後，會讓按鍵移動至另一邊，如果原本按鍵在右，就會移向左
          Ani.SetInteger("YesNo",0) => 代表不動
          Ani.SetInteger("YesNo",1) => 代表出現向右滑的動畫
          Ani.SetInteger("YesNo",2) => 代表出現向左滑的動畫
        */
        if (Right == true) //YES
        {
            Ani.SetInteger("YesNo", 2); //如果一開始按鍵在右邊，按下按鍵會往左滑動
            Ani.speed = 2f;             //撥放速度加快
            Right = false;              //按鍵變成在左邊
        }
        else  //相反
        {
            Ani.SetInteger("YesNo", 1);
            Ani.speed = 2f;
            Right = true;
        }
    }


}
