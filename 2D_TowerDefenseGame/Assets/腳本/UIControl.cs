using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//控制遊戲場景的UI及勝利和失敗視窗，放在"UI"上
public class UIControl : MonoBehaviour
{
    float NowTime = 0f;
    public Image VictoryWindow; //放置勝利視窗
    public Image GGWindow;      //放置失敗視窗
    string NextName;            //開頭的場景名稱
    // Start is called before the first frame update
    void Start()
    {
        VictoryWindow.transform.gameObject.SetActive(false);
        GGWindow.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        NowTime += Time.deltaTime;
        GameObject.Find("時間").GetComponent<Text>().text = NowTime.ToString("F1");

        //if (Input.GetMouseButtonDown(0)) StartGame();
        //else if (Input.GetMouseButtonDown(1)) PauseGame();
        if (Input.GetMouseButtonDown(1)) Victory();
        if (Input.GetMouseButtonDown(2)) GoodGame();
    }

    public void StartGame()//遊戲開始
    {
        Time.timeScale = 1;
    }
    public void PauseGame()//遊戲暫停
    {
        Time.timeScale = 0;
    }

    public void Victory()//遊戲勝利
    {
        VictoryWindow.transform.gameObject.SetActive(true);
        GGWindow.transform.gameObject.SetActive(false);
    }
    public void GoodGame()//遊戲失敗
    {
        GGWindow.transform.gameObject.SetActive(true);
        VictoryWindow.transform.gameObject.SetActive(false);
    }
    public void Window_Yes(string NextName)//再來一場，回到開始場景(按鍵的Function要輸入"開始場景")
    {
        SceneManager.LoadScene(NextName);
    }
    public void Window_NO()//離開遊戲，關閉遊戲
    {
        Application.Quit();
    }

}
