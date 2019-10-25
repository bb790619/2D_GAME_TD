using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制子彈，放在"子彈"上
public class BulletControl : MonoBehaviour
{
    ////參數設定////
    float Speed = 5f;               //子彈速度
    float SurviveTime = 1f;         //砲彈存活時間
    ////角色參數////
    public static int Damage1, Damage2, Damage3, Damage4, Damage5, Damage6; //子彈種類的傷害數值

    public static int Lv;             //砲塔等級，讀取<WeaponControl>的值
    public static string Effect5, Effect6;    //技能的額外效果

    int CRIPro_5, CRIPro_6;                  //技能發動機率，角色5和6的參數

    // Start is called before the first frame update
    void Start()
    {
        CRIPro_5 = Random.Range(1, 101); //當作爆擊率，1-100的亂數
        CRIPro_6 = Random.Range(1, 101);
        PlayerValue();  //依照被選的角色，給不同的攻擊參數
    }

    // Update is called once per frame
    void Update()
    {
        //砲彈從"此位子"至"怪物位子"，用一定速度移動
        this.gameObject.transform.position = Vector3.Lerp(transform.position, WeaponControl.TargetDir, Speed * Time.deltaTime);
        //Vector3 Dir = WeaponControl.TargetDir - WeaponControl.PlayerDir  ;
        //transform.position += Dir.normalized * Speed * Time.deltaTime;

        //砲彈超過3秒就會消失
        if (SurviveTime <= 0f)
        {
            SurviveTime = 3f;
            Destroy(this.gameObject);
        }
        SurviveTime -= Time.deltaTime;
    }

    /// <summary>
    /// 子彈參數，傷害、範圍、效果
    /// </summary>
    public void PlayerValue()
    {
        //依照選定的角色，給<BulltCntrol>使用不同的攻擊參數
        //攻擊傷害(亂數)，攻擊半徑(單體或範圍)，攻擊效果(緩速...)
        //角色(子彈)種類=> 砲塔名稱:(砲塔)6，等級:1，種類:(角色)1
        if (this.gameObject.name == "子彈1")//角色1，單體攻擊(攻擊範圍不變)，中毒
        {
            //LV1每秒扣最大血量。持續2秒
            if (Lv == 1) Damage1 = Random.Range(18, 22);
            else if (Lv == 2) Damage1 = Random.Range(40, 45);
            else if (Lv == 3) Damage1 = Random.Range(65, 70);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/子彈/" + this.gameObject.name);//換子彈
        }
        //角色2，範圍攻擊，火球
        else if (this.gameObject.name == "子彈2")
        {
            //LV1傷害1倍，LV2傷害2倍，LV3傷害4倍
            if (Lv == 1) Damage2 = Random.Range(25, 30);
            else if (Lv == 2) Damage2 = Random.Range(55, 60);
            else if (Lv == 3) Damage2 = Random.Range(110, 120);
            GetComponent<CircleCollider2D>().radius *= 3f;  //攻擊範圍增加                          
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/子彈/"+ this.gameObject.name);//換子彈
        }
        //角色3，單體攻擊(攻擊距離遠)，遠程
        else if (this.gameObject.name == "子彈3")
        {
            //LV1每秒扣最大血量。持續2秒
            if (Lv == 1) Damage3 = Random.Range(18, 22);
            else if (Lv == 2) Damage3 = Random.Range(40, 45);
            else if (Lv == 3) Damage3 = Random.Range(65, 70);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/子彈/" + this.gameObject.name);//換子彈
        }
        //角色4，範圍攻擊，緩速
        else if (this.gameObject.name == "子彈4")
        {
            //LV1傷害1倍速度減少10%，，LV2傷害2倍速度減少20%，，LV3傷害3倍速度減少40%。持續2秒，身體變藍色
            if (Lv == 1) Damage4= Random.Range(15, 20);
            else if (Lv == 2) Damage4 = Random.Range(35, 40);
            else if (Lv == 3) Damage4 = Random.Range(55, 60);
            GetComponent<CircleCollider2D>().radius *= 3f;  //攻擊範圍增加                              
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/子彈/" + this.gameObject.name);//換子彈
        }
        //角色5，單體攻擊(攻擊範圍不變)，暈擊
        else if (this.gameObject.name == "子彈5")
        {
            //LV1有15%暈2秒並50傷害，LV2有30%暈2秒並100傷害，LV3有50%暈2秒並200傷害 
            if (Lv == 1 && CRIPro_5 > 15) Damage5 = Random.Range(16, 23);
            else if (Lv == 1 && CRIPro_5 <= 15)
            {
                Effect5 = "暈擊";
                Damage5 = Random.Range(20, 25) + 50;
            }
            else if (Lv == 2 && CRIPro_5 > 30) Damage5 = Random.Range(25, 48);
            else if (Lv == 2 && CRIPro_5 <= 30)
            {
                Effect5 = "暈擊";
                Damage5 = Random.Range(45, 50) + 100;
            }
            else if (Lv == 3 && CRIPro_5 > 50) Damage5 = Random.Range(80, 90);
            else if (Lv == 3 && CRIPro_5 <= 50)
            {
                Effect5 = "暈擊";
                Damage5 = Random.Range(90, 100) + 200;
            }
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/子彈/" + this.gameObject.name);//換子彈
        }
        //角色6，單體攻擊(攻擊範圍不變)，爆擊
        else if (this.gameObject.name == "子彈6")
        {
            //LV1爆擊率10%有1.5的傷害，LV2爆擊率20%有1.5的傷害，LV3爆擊率40%有1.5的傷害   
            if (Lv == 1 && CRIPro_6 > 10) Damage6 = Random.Range(20, 25);
            else if (Lv == 1 && CRIPro_6 <= 10)
            {
                Effect6 = "爆擊";
                Damage6 = Random.Range(20, 25) * 3 / 2;
            }
            else if (Lv == 2 && CRIPro_6 > 20) Damage6 = Random.Range(45, 50);
            else if (Lv == 2 && CRIPro_6 <= 20)
            {
                Effect6 = "爆擊";
                Damage6 = Random.Range(45, 50) * 3 / 2;
            }
            else if (Lv == 3 && CRIPro_6 > 40) Damage6 = Random.Range(90, 100);
            else if (Lv == 3 && CRIPro_6 <= 40)
            {
                Effect6 = "爆擊";
                Damage6 = Random.Range(90, 100) * 3 / 2;
            }
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/子彈/" + this.gameObject.name);//換子彈
        }

    }


}
