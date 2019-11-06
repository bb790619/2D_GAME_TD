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
    float HpMax;  //怪物最大血量，且會依照難度和波數改變怪的血量
    int AddPrice; //怪物死亡會增加多少金錢

    float Hp;     //怪物目前血量

    GameObject MovePoints;   //讀取且放置移動點的陣列
    int Index = 0;           //移動點的編號

    GameObject HpObj;        //怪物的血條
    float[] TimeCount = new float[SpaceControl.PlayerNum];   //子彈的效果的倒數計時。[0]=>角色1，[1]=>角色2，[2]=>角色3，[3]=>角色4(目前只有[2][3]有使用)  
    float[] EffectConti = new float[SpaceControl.PlayerNum]; //子彈效果的持續時間
    GameObject Bomb;         //子彈的特效
    public AudioClip DeathMusic;  //怪物死亡的音效

    // Start is called before the first frame update
    void Start()
    {
        ////怪物參數設定，這裡修改怪物血量和金錢////
        HpMax = EnemyCreater.EnemyWave * 30 ;   //怪物血量和波數以及難度有關      
        AddPrice = 5 + EnemyCreater.EnemyWave + StandByScene.TalentPoint[5]*2;                      //怪物死掉，玩家會增加金錢，增加的金錢和波數有關
        //////////////////////////////
        Hp = HpMax;                               //開場時，怪物現有血量=最大值
        MovePoints = PointSetting.points[0];      //怪物移動的方向
        CreatHpUI();                              //產生怪物血條

        //子彈效果的持續時間和倒數計時器
        for (int i = 0; i < SpaceControl.PlayerNum; i++)
        {
            EffectConti[i] = 2;               //持續2秒
            TimeCount[i] = EffectConti[i] + 1;  //因為要持續2秒，就隨便設定2以上的數字
        }
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
            if (Index >= PointSetting.points.Length)//如果超過移動點的數量(碰到終點)，就消除
            {
                Destroy(this.gameObject);
                Destroy(HpObj);
                UIControl.PlayerHp -= 1;
                GameObject.Find("UI").GetComponent<UIControl>().EndControl(); //使用<UIControl>的EndControl，執行扣血動畫
                GameObject.Find("守門人").GetComponent<AudioSource>().Play(); //播放扣血音效
            }
            else MovePoints = PointSetting.points[Index];

        }

        //控制CD的時間條
        #region
        HpObj.transform.SetParent(GameObject.Find("UI").transform);                //沒加父物件就不會出現
        HpObj.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + new Vector3(0, 0.2f, 0));//怪物血量一直跟著怪物
        HpObj.transform.GetChild(0).GetComponent<Image>().fillAmount = Hp / HpMax; //減少血條
        #endregion

        //額外效果的倒數計時
        //如果被子彈3打到，TimeCount[2]=2，2秒後就恢復，數字變為3(2秒+1，這樣就不會觸發)，這樣子彈3和子彈4就獨立分開計算
        for (int j = 0; j < SpaceControl.PlayerNum; j++)
        {
            if (TimeCount[j] <= EffectConti[j] & TimeCount[j] > 0)
            {
                TimeCount[j] -= Time.deltaTime;
            }
            else if (TimeCount[j] <= 0)
            {
                if (j == 0) { }              //詛咒恢復
                else if (j == 3) Recovery(); //緩速恢復
                else if (j == 4) Recovery(); //暈擊恢復
                TimeCount[j] = EffectConti[j] + 1;
            }
        }
    }

    /// <summary>
    /// 怪物出現就升成血量
    /// </summary>
    public void CreatHpUI()
    {
        HpObj = Instantiate(GameObject.Find("怪物血量底部"));
    }

    /// <summary>
    /// 怪物被子彈打到的效果
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        //讀取子彈特效
        for (int i = 1; i < SpaceControl.PlayerNum + 1; i++) //子彈種類
        {
            for (int j = 0; j < BulletDamageControl.NumMax + 1; j++) //流水號
            {
                if (collision.gameObject.name == "子彈" + i + "_" + j)
                    Bomb = Resources.Load<GameObject>("Player/爆炸/爆炸" + i);
            }

        }

        //中毒
        for (int i = 0; i < BulletDamageControl.NumMax; i++)
        {
            if (collision.gameObject.name == "子彈1_" + i)
            {
                TimeCount[0] = EffectConti[0];
                ContinDamage();    //執行每秒扣血
                BulletToEemy(BulletDamageControl.Damage[i], collision.gameObject, i);
            }
            //火球
            else if (collision.gameObject.name == "子彈2_" + i) BulletToEemy(BulletDamageControl.Damage[i], collision.gameObject, i);//被子彈打到的傷害，打到怪物後該消除的子彈，流水號
            //遠程
            else if (collision.gameObject.name == "子彈3_" + i) BulletToEemy(BulletDamageControl.Damage[i], collision.gameObject, i);
            //緩速
            else if (collision.gameObject.name == "子彈4_" + i)
            {
                //被打到的效果
                if (BulletControl.Lv == 1) Speed = 0.9f;      //LV1速度減少10%，，LV2速度減少20%，，LV3速度減少30%。持續2秒
                else if (BulletControl.Lv == 2) Speed = 0.8f;
                else if (BulletControl.Lv == 3) Speed = 0.6f;
                Speed -= StandByScene.TechPoint[11] *0.03f; //增加緩速能力
                TimeCount[3] = EffectConti[3];     //緩速2秒，2秒後就恢復速度和顏色
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 120, 255, 255);//怪物被打到變藍色
                BulletToEemy(BulletDamageControl.Damage[i], collision.gameObject, i);
            }
            //暈擊
            else if (collision.gameObject.name == "子彈5_" + i)
            {
                if (BulletDamageControl.Effect[i] != null) Speed = 0f;
                else Speed = 1f;
                TimeCount[4] = EffectConti[4];
                BulletToEemy(BulletDamageControl.Damage[i], collision.gameObject, i);//被子彈打到的傷害，打到怪物後該消除的子彈
            }
            //爆擊
            else if (collision.gameObject.name == "子彈6_" + i) BulletToEemy(BulletDamageControl.Damage[i], collision.gameObject, i);//被子彈打到的傷害，打到怪物後該消除的子彈
        }
    }


    /// <summary>
    /// 怪物被打到會扣血，子彈會消失，怪物血歸0就消失，血條也消失(傷害值，子彈名稱，子彈流水號)
    /// </summary>
    /// <param name="Damage"></param>
    /// <param name="Colli"></param>
    /// <param name="Num"></param>
    public void BulletToEemy(int Damage, GameObject Colli, int Num)
    {
        Hp -= Damage;  //減少HP       
        Instantiate(Bomb, Colli.transform.position, Quaternion.identity).name = Bomb.name + "_" + Num; //升成攻擊特效，特效上有腳本會自己消除
        GameObject.Find(Bomb.name + "_" + Num).GetComponent<Animator>().speed = 0.5f; //控制特效時間
        Destroy(Colli);//打到怪物子彈就消失
        if (Hp <= 0)//怪物HP歸0，怪物消失，血條消失，玩家增加金錢，
        {
            UIControl.PlayerMoney += AddPrice;
            AudioSource.PlayClipAtPoint(DeathMusic,GameObject.Find("Main Camera").transform.position);//播放音效，用這個方法，怪物死亡音效也會撥完。聲音是以攝影機的位子收音，離太遠會很小聲。
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
        Invoke("TimeDamage", 1f);
        Invoke("TimeDamage", 2f);
    }
    public void TimeDamage() //LV1每秒扣最大血量5%，，LV2每秒扣最大血量10%，，LV3每秒扣最大血量15%
    {
        int ConDamage = 0;

        if (BulletControl.Lv == 1) ConDamage = 5;
        else if (BulletControl.Lv == 2) ConDamage = 10;
        else if (BulletControl.Lv == 3) ConDamage = 15;
        ConDamage += StandByScene.TechPoint[2];
       Hp -= HpMax * ConDamage / 100;
    }



}
