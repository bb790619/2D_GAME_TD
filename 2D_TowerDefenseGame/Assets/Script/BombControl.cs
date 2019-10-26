using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//放在"特效"上，讓特效0.5秒後就消除
public class BombControl : MonoBehaviour
{
     public static GameObject EffectText; //"攻擊特效文字"

    // Start is called before the first frame update
    void Start()
    {
        EffectText = GameObject.Find("攻擊特效文字"); //抓取文字(不知為何不能用Public)
        EffectText.SetActive(false); EffectText.SetActive(true);
        EffectControl();   //執行文字和聲音效果
        EffectText.transform.position =Camera.main.WorldToScreenPoint( this.gameObject.transform.position+Vector3.up );
        Destroy(this.gameObject, 0.5f); //特效0.5秒後消失
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    /// <summary>
    /// 子彈文字和聲音效果
    /// </summary>
    public void EffectControl()
    {
        
        if (this.name == "爆炸_子彈1")//詛咒，扣血
        {
            //EffectText.GetComponent<Text>().text = "詛咒";
        }
        if (this.name == "爆炸_子彈2")//範圍，爆炸
        {
            //EffectText.GetComponent<Text>().text = "爆炸";
        }
        if (this.name == "爆炸_子彈3")//遠程，遠距離攻擊
        {
           // EffectText.GetComponent<Text>().text = "遠程";
        }
        if (this.name == "爆炸_子彈4")//緩速，減速
        {
           // EffectText.GetComponent<Text>().text = "緩速";
        }
        if (this.name == "爆炸_子彈5")//暈擊
        {
            EffectText.GetComponent<Text>().text = BulletControl.Effect5;
        }
        if (this.name == "爆炸_子彈6")//爆擊
        {
            EffectText.GetComponent<Text>().text = BulletControl.Effect6;
        }

    }
}
