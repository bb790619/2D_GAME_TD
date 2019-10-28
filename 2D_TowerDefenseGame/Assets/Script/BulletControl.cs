using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制子彈，放在"子彈"上
public class BulletControl : MonoBehaviour
{
    ////參數設定////
    float Speed = 15f;              //子彈速度
    float SurviveTime = 1f;         //砲彈存活時間
    ////角色參數////
    //public static int Damage1, Damage2, Damage3, Damage4, Damage5, Damage6; //子彈種類的傷害數值

    public static int Lv;           //砲塔等級，讀取<WeaponControl>的值
    int PlayerKind;                 //子彈種類
    int PlayerNum;                  //子彈流水號
    int CRIPro;                     //技能發動機率

    // Start is called before the first frame update
    void Start()
    {
        CRIPro = Random.Range(1, 101); //當作爆擊率，1-100的亂數

        FindName();     //找出子彈的種類和流水號
        PlayerValue();  //依照被選的角色，給不同的攻擊參數
    }

    // Update is called once per frame
    void Update()
    {
        //砲彈從"此位子"至"怪物位子"，用一定速度移動
        this.gameObject.transform.position = Vector3.Lerp(transform.position, BulletDamageControl.Target[PlayerNum], Speed * Time.deltaTime);


        //砲彈超過2秒就會消失
        if (SurviveTime <= 0f)
        {
            SurviveTime = 2f;
            Destroy(this.gameObject);
        }
        SurviveTime -= Time.deltaTime;
    }

    /// <summary>
    /// 找出子彈的種類和流水號
    /// </summary>
    public void FindName()
    {
        for (int i = 1; i < SpaceControl.PlayerNum + 1; i++) //子彈種類
        {
            for (int j = 0; j < BulletDamageControl.NumMax + 1; j++) //流水號
            {
                if (this.gameObject.name == "子彈" + i + "_" + j)
                {
                    PlayerKind = i;
                    PlayerNum = j;
                }
            }

        }
    }
    /// <summary>
    /// 子彈參數，傷害、範圍、效果
    /// </summary>
    public void PlayerValue()
    {

        //依照選定的角色，給<BulltCntrol>使用不同的攻擊參數
        //攻擊傷害(亂數)，攻擊半徑(單體或範圍)，攻擊效果(緩速...)
        //角色(子彈)種類=> 砲塔名稱:(砲塔)6，等級:1，種類:(角色)1

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/子彈/子彈" + PlayerKind);//換子彈

        //子彈的傷害和額外效果，要修改數值從這邊改
        for (int i = 0; i < BulletDamageControl.NumMax; i++)
        {
            if (this.gameObject.name == "子彈1_" + i)//角色1，單體攻擊(攻擊範圍不變)，中毒
            {
                //LV1每秒扣最大血量。持續2秒
                if (Lv == 1) BulletDamageControl.Damage[i] = Random.Range(18, 22);
                else if (Lv == 2) BulletDamageControl.Damage[i] = Random.Range(40, 45);
                else if (Lv == 3) BulletDamageControl.Damage[i] = Random.Range(65, 70);
            }
            //角色2，範圍攻擊，火球
            else if (this.gameObject.name == "子彈2_" + i)
            {
                //LV1傷害1倍，LV2傷害2倍，LV3傷害4倍
                if (Lv == 1) BulletDamageControl.Damage[i] = Random.Range(25, 30);
                else if (Lv == 2) BulletDamageControl.Damage[i] = Random.Range(55, 60);
                else if (Lv == 3) BulletDamageControl.Damage[i] = Random.Range(110, 120);
                GetComponent<CircleCollider2D>().radius = 2f;  //攻擊範圍增加                          
            }
            //角色3，單體攻擊(攻擊距離遠)，遠程
            else if (this.gameObject.name == "子彈3_" + i)
            {
                //LV1每秒扣最大血量。持續2秒
                if (Lv == 1) BulletDamageControl.Damage[i] = Random.Range(18, 22);
                else if (Lv == 2) BulletDamageControl.Damage[i] = Random.Range(40, 45);
                else if (Lv == 3) BulletDamageControl.Damage[i] = Random.Range(65, 70);
                transform.localScale = new Vector3(1f, 1f, 0);
            }
            //角色4，範圍攻擊，緩速
            else if (this.gameObject.name == "子彈4_" + i)
            {
                //LV1傷害1倍速度減少10%，，LV2傷害2倍速度減少20%，，LV3傷害3倍速度減少40%。持續2秒，身體變藍色
                if (Lv == 1) BulletDamageControl.Damage[i] = Random.Range(15, 20);
                else if (Lv == 2) BulletDamageControl.Damage[i] = Random.Range(35, 40);
                else if (Lv == 3) BulletDamageControl.Damage[i] = Random.Range(55, 60);
                GetComponent<CircleCollider2D>().radius = 2f;  //攻擊範圍增加                              
            }
            //角色5，單體攻擊(攻擊範圍不變)，暈擊
            else if (this.gameObject.name == "子彈5_" + i)
            {
                //LV1有15%暈2秒並10傷害，LV2有30%暈2秒並20傷害，LV3有50%暈2秒並40傷害 
                if (Lv == 1 && CRIPro > 15)
                {
                    BulletDamageControl.Effect[i] = null;
                    BulletDamageControl.Damage[i] = Random.Range(16, 23);
                }
                else if (Lv == 1 && CRIPro <= 15)
                {
                    BulletDamageControl.Effect[i] = "暈擊";
                    BulletDamageControl.Damage[i] = Random.Range(20, 25) + 10;
                }
                else if (Lv == 2 && CRIPro > 30)
                {
                    BulletDamageControl.Effect[i] = null;
                    BulletDamageControl.Damage[i] = Random.Range(25, 48);
                }
                else if (Lv == 2 && CRIPro <= 30)
                {
                    BulletDamageControl.Effect[i] = "暈擊";
                    BulletDamageControl.Damage[i] = Random.Range(45, 50) + 20;
                }
                else if (Lv == 3 && CRIPro > 50)
                {
                    BulletDamageControl.Effect[i] = null;
                    BulletDamageControl.Damage[i] = Random.Range(80, 90);
                }
                else if (Lv == 3 && CRIPro <= 50)
                {
                    BulletDamageControl.Effect[i] = "暈擊";
                    BulletDamageControl.Damage[i] = Random.Range(90, 100) + 40;
                }
            }
            //角色6，單體攻擊(攻擊範圍不變)，爆擊
            else if (this.gameObject.name == "子彈6_" + i)
            {
                //LV1爆擊率10%有1.5的傷害，LV2爆擊率20%有1.5的傷害，LV3爆擊率40%有1.5的傷害   
                if (Lv == 1 && CRIPro > 10)
                {
                    BulletDamageControl.Effect[i] = null;
                    BulletDamageControl.Damage[i] = Random.Range(20, 25);
                }

                else if (Lv == 1 && CRIPro <= 10)
                {
                    BulletDamageControl.Effect[i] = "爆擊";
                    BulletDamageControl.Damage[i] = Random.Range(20, 25) * 3 / 2;
                }
                else if (Lv == 2 && CRIPro > 20)
                {
                    BulletDamageControl.Effect[i] = null;
                    BulletDamageControl.Damage[i] = Random.Range(45, 50);
                }
                else if (Lv == 2 && CRIPro <= 20)
                {
                    BulletDamageControl.Effect[i] = "爆擊";
                    BulletDamageControl.Damage[i] = Random.Range(45, 50) * 3 / 2;
                }
                else if (Lv == 3 && CRIPro > 40)
                {
                    BulletDamageControl.Effect[i] = null;
                    BulletDamageControl.Damage[i] = Random.Range(90, 100);
                }
                else if (Lv == 3 && CRIPro <= 40)
                {
                    BulletDamageControl.Effect[i] = "爆擊";
                    BulletDamageControl.Damage[i] = Random.Range(90, 100) * 3 / 2;
                }
            }
        }

    }


}
