using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//控制開始場景，放在"StartScene"上
public class StartScene : MonoBehaviour
{
    public string NextName;
    public Image Setting;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextScene()
    {
        SceneManager.LoadScene(NextName);
    }

    public void Option()
    {
        if (Setting.transform.gameObject.activeSelf == true) Setting.transform.gameObject.SetActive(false);
        else if (Setting.transform.gameObject.activeSelf == false) Setting.transform.gameObject.SetActive(true);

    }
    public void Quit()
    {
        Application.Quit();
    }

}
