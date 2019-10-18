using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//控制怪物移動，放在"怪物"上
public class EnemyControl : MonoBehaviour
{
    ////怪物參數設定////
    float Speed = 1f;    //怪物移動速度
   
    //開場產生怪物時，這裡使用變數才會記錄目前波數，否則會一直改變
    public float HpMax;  //怪物最大血量，且會依照難度和波數改變怪的血量
    public int Price;    //怪物死亡會增加多少金錢

    public float Hp;     //怪物目前血量

    GameObject MovePoints;   //讀取且放置移動點的陣列
    int Index = 0;           //移動點的編號
    GameObject HpObj;        //怪物的血條
    float TimeCount;         //子彈的效果的倒數計時

    // Start is called before the first frame update
    void Start()
    {
        ////怪物參數設定，這裡修改怪物血量和金錢////
        HpMax = EnemyCreater.EnemyWave  * 30 * UIControl.Mode;   //怪物血量和波數以及難度有關      
        Price = 5 + EnemyCreater.EnemyWave;                      //怪物死掉，玩家會增加金錢，增加的金錢和波數有關
        //////////////////////////////
        Hp = HpMax;                               //開場時，怪物現有血量=最大值
        MovePoints = PointSetting.points[0];      //怪物移動的方向
        CreatHpUI();                              //產生怪物血條
    }

    // Update is called once per frame
    void Update()
    {
        ////控制怪的方向////
        Vector3 EnemyDir = MovePoints.transform.position - this.gameObject.transform.position;//此向量(方向)=移動點的位子-此物件的位子
        this.gameObject.transform.position += EnemyDir.normalized * Speed * Time.deltaTime;//讓此物件往這個方向移動
        if (Vector3.Distance(this.gameObject.transform.position, MovePoints.transform.position) <= 0.1f)//到移動點，就再移動到下一個移動點
        {
            Index++;
            if (Index >= PointSetting.points.Length )//如果超過移動點的數量(碰到終點)，就消除
            {
                Destroy(this.gameObject);
                Destroy(HpObj);
                UIControl.PlayerHp -= 1;
            }
            else MovePoints = PointSetting.points[Index];
            
        }

        //控制CD的UI
        HpObj.transform.SetParent(GameObject.Find("UI").transform);                //沒加父物件就不會出現
        HpObj.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + new Vector3(0, 0.2f, 0));//怪物血量一直跟著怪物
        HpObj.transform.GetChild(0).GetComponent<Image>().fillAmount = Hp / HpMax; //減少血條

        //額外效果的倒數計時
        TimeCount -= Time.deltaTime;
        if (TimeCount <= 0f) Recovery(); //子彈3，2秒後恢復
    }
    //怪物出現就升成血量
    void CreatHpUI()
    {
        HpObj = Instantiate(GameObject.Find("怪物血量底部"));
    }

    //怪物碰撞控制
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "子彈1") BulletToEemy(BulletControl.Damage1, collision.gameObject);//被子彈打到的傷害，打到怪物後該消除的子彈
        else if (collision.gameObject.name == "子彈2") BulletToEemy(BulletControl.Damage2, collision.gameObject);//被子彈打到的傷害，打到怪物後該消除的子彈
        else if (collision.gameObject.name == "子彈3")//碰到子彈時
        {
            //被打到的效果
            if (BulletControl.Lv == 1) Speed = 0.9f;      //LV1速度減少10%，，LV2速度減少20%，，LV3速度減少30%。持續2秒
            else if (BulletControl.Lv == 2) Speed = 0.8f;
            else if (BulletControl.Lv == 3) Speed = 0.7f;
            TimeCount = 2;                                                                    //緩速2秒，2秒後就恢復速度和顏色
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 120, 255, 255);//怪物被打到變藍色

            BulletToEemy(BulletControl.Damage3, collision.gameObject);  //被子彈打到的傷害，打到怪物後該消除的子彈

        }
        else if (collision.gameObject.name == "子彈4")//碰到子彈時
        {
            TimeCount = 2;                                                                         
            ContinDamage();    //執行每秒扣血
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 120, 0, 255);   //怪物被打到變綠色
            BulletToEemy(BulletControl.Damage4, collision.gameObject);//被子彈打到的傷害，打到怪物後該消除的子彈
        }
    }

    //怪物被打到會扣血，子彈會消失，怪物血歸0就消失，血條也消失
    public void BulletToEemy(int Damage, GameObject Colli)
    {
        Hp -= Damage;  //減少HP       
        Destroy(Colli);//打到怪物子彈就消失
        if (Hp <= 0)//怪物HP歸0，怪物消失，血條消失，玩家增加金錢，
        {
            UIControl.PlayerMoney += Price;
            Destroy(this.gameObject);
            Destroy(HpObj);
        }
    }

    //恢復速度和顏色
    public void Recovery()
    {
        Speed = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255); //顏色恢復(變白色)
    }

    //每秒扣血，維持2秒
    public void ContinDamage() //每秒扣血
    {
        Invoke("TimeDamage",1f); 
        Invoke("TimeDamage", 2f);       
    }
    public void TimeDamage() //LV1每秒扣最大血量5%，，LV2每秒扣最大血量10%，，LV3每秒扣最大血量25%
    {
        int ConDamage = 0;

        if (BulletControl.Lv == 1) ConDamage = 5;
        else if (BulletControl.Lv == 2) ConDamage = 10;
        else if (BulletControl.Lv == 3) ConDamage = 25;
        Hp -= HpMax * ConDamage / 100;
        print("扣" + HpMax * ConDamage / 100);
    }

}
