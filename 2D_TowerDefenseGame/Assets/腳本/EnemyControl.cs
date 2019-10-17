using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//控制怪物移動，放在"怪物"上
public class EnemyControl : MonoBehaviour
{
    ////怪物參數設定////
    float Speed = 1f;    //怪物移動速度
    public float Hp;     //怪物目前血量
    public float HpMax;  //怪物最大血量，且會依照難度和波數改變怪的血量
    public int Price;    //怪物死亡會增加多少金錢
    ////////////////
    GameObject MovePoints;   //讀取且放置移動點的陣列
    int Index = 0;           //移動點的編號
    GameObject HpObj;        //怪物的血條

    // Start is called before the first frame update
    void Start()
    {
        //開場產生怪物時，這裡使用變數才會記錄目前波數，否則會一直改變。
        HpMax = (100f + EnemyCreater.EnemyWave * 10) * UIControl.Mode;  //怪物血量和波數以及難度有關      
        Price = 5 + EnemyCreater.EnemyWave;       //怪物死掉，玩家會增加金錢，增加的金錢和波數有關

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
            if (Index >= PointSetting.points.Length - 1)//如果超過移動點的數量(碰到終點)，就消除
            {
                Destroy(this.gameObject);
                Destroy(HpObj);
                UIControl.PlayerHp -= 1;
            }
            Index++;
            MovePoints = PointSetting.points[Index];
        }

        //控制CD的UI
        HpObj.transform.SetParent(GameObject.Find("UI").transform); //沒加父物件就不會出現
        HpObj.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + new Vector3(0, 0.2f, 0));//怪物血量一直跟著怪物


    }

    //怪物出現就升成血量
    void CreatHpUI()
    {
        HpObj = Instantiate(GameObject.Find("怪物血量底部"));               
    }

    //怪物碰撞控制
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")//碰到子彈時
        {
            //扣血
            Hp -= BulletControl.Damage;
            HpObj.transform.GetChild(0).GetComponent<Image>().fillAmount = Hp / HpMax;
            print(this.name + "受到" + BulletControl.Damage + "的傷害，現有血量" + Hp + "，最大血量" + HpMax);
            //額外效果
            if (BulletControl.effect == "火球") { print("被火球打"); }
            else if (BulletControl.effect == "巨石") { print("被巨石打"); }
            else if (BulletControl.effect == "緩速")
            {
                print("緩速");
                Speed = 0.7f;  //速度減慢30%
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 120, 255, 255);//被打到變藍色
                Invoke("Recovery", 2f);
            }
            else if (BulletControl.effect == "毒")
            {
                print("毒");
                Hp -= 0.1f * HpMax;
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(145, 205, 51, 255);//被打到變綠色
                Invoke("Recovery", 2f);
            }

            //打到怪物子彈就消失，怪的血歸0也消失
            Destroy(collision.gameObject);

            //怪物HP歸0，怪物死掉，玩家增加金錢，
            if (Hp <= 0)
            {
                UIControl.PlayerMoney += Price;
                Destroy(this.gameObject);
                Destroy(HpObj);
            }
        }
    }

    public void Recovery()
    {
        Speed = 1f;  //速度恢復
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);//顏色恢復(變白色)
    }

}
