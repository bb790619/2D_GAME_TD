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
        EffectText.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + Vector3.up);
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
        for (int i = 0; i < BulletDamageControl.NumMax; i++)
        {
            if (this.name == "爆炸1_"+i)//詛咒，扣血
            {
                EffectText.GetComponent<Text>().text ="1";
            }
            if (this.name == "爆炸2_" + i)//範圍，爆炸
            {
                EffectText.GetComponent<Text>().text = null;
            }
            if (this.name == "爆炸3_" + i)//遠程，遠距離攻擊
            {
                EffectText.GetComponent<Text>().text = null;
            }
            if (this.name == "爆炸4_" + i)//緩速，減速
            {
                EffectText.GetComponent<Text>().text = null;
            }
            if (this.name == "爆炸5_" + i)//暈擊
            {
                 EffectText.GetComponent<Text>().text = BulletDamageControl.Effect[i];
            }
            if (this.name == "爆炸6_" + i)//爆擊
            {
                 EffectText.GetComponent<Text>().text =BulletDamageControl.Effect[i];
            }
        }
    }
}
