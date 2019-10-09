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
        SceneManager.LoadScene(NextName);//到"遊戲場景"
    }
    public void Option()//開始場景的"選項"
    {
        if (Setting.transform.gameObject.activeSelf == true) Setting.transform.gameObject.SetActive(false);
        else if (Setting.transform.gameObject.activeSelf == false) Setting.transform.gameObject.SetActive(true);
    }
    public void Quit()//開始場景的"離開"
    {
        Application.Quit();
    }

}
