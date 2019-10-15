using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//說明場景使用
public class ExampleControl : MonoBehaviour
{
    public Image ExampleWindow;           //說明視窗("暫停視窗")
    // Start is called before the first frame update
    void Start()
    {
        ExampleWindow.transform.gameObject.SetActive(true);//開啟視窗，淡出1秒後執行，讓淡出隱藏
        Invoke("FadeOutClose", 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Example() //"說明視窗"，開始說明
    {
        Destroy(ExampleWindow);
    }
    public void ExampleSkip() //"說明視窗"，跳過說明
    {
        Invoke("ExampleSkipNow", 1f);//要有淡出效果，所以延遲
    }
    public void ExampleSkipNow() //跳至遊戲場景
    {
        SceneManager.LoadScene("遊戲場景");
    }

}


