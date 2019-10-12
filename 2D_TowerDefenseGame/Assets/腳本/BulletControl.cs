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
    public static int Damage;      //子彈種類的傷害數值
    public static float Range;     //子彈攻擊半徑
    public static string effect;   //子彈效果
    public static int Lv;          //砲塔等級 

    // Start is called before the first frame update
    void Start()
    {
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

    //依照選定的角色，給<BulltCntrol>使用不同的攻擊參數
    //攻擊傷害(亂數)，攻擊半徑(單體或範圍)，攻擊效果(緩速...)
    public void PlayerValue()//子彈參數，傷害、範圍、效果
    {
        //角色(子彈)種類=> 砲塔名稱:(砲塔)6，等級:1，種類:(角色)1
        if (this.gameObject.name == "子彈1")//角色2，傷害中，範圍攻擊，火球
        {         
            Damage = Random.Range(20, 30)*Lv;             //升等就是傷害加倍
            GetComponent<CircleCollider2D>().radius*=3f;  //攻擊範圍增加
            effect = "火球";                              //無效果
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);//換照片
  
        }
        else if (this.gameObject.name == "子彈2")//角色2，傷害高，單體攻擊(攻擊範圍不變)，巨石
        {
            Damage = Random.Range(30, 40) * Lv;
            effect = "巨石";   //無效果
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
        }
        else if (this.gameObject.name == "子彈3")//角色3，傷害低，範圍攻擊，緩速
        {
            Damage = Random.Range(10, 20) * Lv;
            GetComponent<CircleCollider2D>().radius *= 3f;  //攻擊範圍增加
            effect = "緩速";                                //速度減少30%，持續2秒，身體變藍色
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 255, 255);
        }
        else if (this.gameObject.name == "子彈4")//角色4，傷害中，單體攻擊(攻擊範圍不變)，毒
        {
            Damage = Random.Range(20, 30) * Lv;
            effect = "毒";                                 //持續扣血，持續2秒，身體變綠色
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
        }
    }


}
