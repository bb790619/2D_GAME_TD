using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//淡入淡出下個場景，切換場景使用
public class FadeInOut : MonoBehaviour
{
    public Image FadeIn;
    public Image FadeOUT;
    // Start is called before the first frame update
    void Start()
    {
        //關閉淡入，開啟淡出
        FadeIn.transform.gameObject.SetActive(false);
        FadeOUT.transform.gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().name=="開始場景")//開始場景不需要淡出
        {
            FadeOUT.transform.gameObject.SetActive(false);
        }

        Invoke("FadeOutClose", 1f);//1秒後關閉淡出
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeInNow() //執行淡入
    {      
        FadeIn.transform.gameObject.SetActive(true);
    }
    public void FadeOutClose() //關閉淡出
    {
        FadeOUT.transform.gameObject.SetActive(false);
    }


}
