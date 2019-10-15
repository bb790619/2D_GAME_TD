using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//控制開始場景，放在開始場景的"StartScene"上
public class StartScene : MonoBehaviour
{
    public string NextName;

    public Image Setting;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //讓遊戲不要處於暫停狀態
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextScene() //開始場景的"開始"
    {
        Invoke("RealNextScene", 1f);
    }
    public void Quit()//開始場景的"離開"
    {
        Application.Quit();
    }

    public void RealNextScene()
    {
        SceneManager.LoadScene(NextName);//到"遊戲場景"
    }

}
