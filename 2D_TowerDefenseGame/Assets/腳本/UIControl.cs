using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//控制遊戲場景的UI，放在"UI"上
public class UIControl : MonoBehaviour
{
    float NowTime=0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        NowTime += Time.deltaTime;
        GameObject.Find("時間").GetComponent<Text>().text = NowTime.ToString("F1");

        if (Input.GetMouseButtonDown(0)) StartGame();
        else if (Input.GetMouseButtonDown(1)) PauseGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
}
