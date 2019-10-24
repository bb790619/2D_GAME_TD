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
    public static int Damage1;      //子彈種類的傷害數值
    public static int Damage2;
    public static int Damage3;
    public static int Damage4;
    public static float Range;     //子彈攻擊半徑
    public static int Lv;          //砲塔等級，讀取<WeaponControl>的值

    public int CRI;               //爆擊率，角色2的參數

    // Start is called before the first frame update
    void Start()
    {
        CRI = Random.Range(1, 101); //當作爆擊率，1-100的亂數
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
        if (this.gameObject.name == "子彈1")//角色2，範圍攻擊，火球
        {
            //LV1傷害1倍，LV2傷害2倍，LV3傷害4倍
            if (Lv==1) Damage1 = Random.Range(25, 30) ;             
            else if (Lv == 2) Damage1 = Random.Range(55, 60) ;
            else if (Lv == 3) Damage1 = Random.Range(110, 120) ;

            GetComponent<CircleCollider2D>().radius*=3f;  //攻擊範圍增加                          
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);//換照片
  
        }
        else if (this.gameObject.name == "子彈2")//角色2，單體攻擊(攻擊範圍不變)，爆擊
        {
            //LV1爆擊率10%有1.5的傷害，LV2爆擊率20%有1.5的傷害，LV3爆擊率40%有1.5的傷害   
            if (Lv == 1 && CRI > 10) Damage2 = Random.Range(20, 25);
            else if (Lv == 1 && CRI <= 10) Damage2 = Random.Range(20, 25) *3/2 ;
            else if(Lv == 2 && CRI > 20) Damage2 = Random.Range(45, 50);
            else if (Lv == 2 && CRI <= 20) Damage2 = Random.Range(45, 50) * 3 / 2;
            else if(Lv == 3 && CRI > 40) Damage2 = Random.Range(90, 100);
            else if (Lv == 3 && CRI <= 40) Damage2 = Random.Range(90, 100) * 3 / 2;
            
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
        }
        else if (this.gameObject.name == "子彈3")//角色3，範圍攻擊，緩速
        {
            //LV1傷害1倍速度減少10%，，LV2傷害2倍速度減少20%，，LV3傷害3倍速度減少30%。持續2秒，身體變藍色
            if (Lv == 1) Damage3 = Random.Range(15, 20);
            else if (Lv == 2) Damage3 = Random.Range(35, 40);
            else if (Lv == 3) Damage3 = Random.Range(55, 60);

            GetComponent<CircleCollider2D>().radius *= 3f;  //攻擊範圍增加                              
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 255, 255);
        }
        else if (this.gameObject.name == "子彈4")//角色4，單體攻擊(攻擊範圍不變)，中毒
        {
            //LV1每秒扣最大血量。持續2秒，身體變綠色
            if (Lv == 1) Damage4 = Random.Range(18, 22);
            else if (Lv == 2) Damage4 = Random.Range(40, 45);
            else if (Lv == 3) Damage4 = Random.Range(65, 70);

            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
        }
    }


}
