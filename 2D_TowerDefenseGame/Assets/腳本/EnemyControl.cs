using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//控制怪物移動
public class EnemyControl : MonoBehaviour
{
    ////怪物參數設定////
    float Speed = 1f;//怪物移動速度
    float Hp ;
    float HpMax=5f; //怪物的目前血量和最大血量

    GameObject HpObj;

    GameObject MovePoints;//讀取且放置移動點的陣列
    int Index = 0;//移動點的編號

    // Start is called before the first frame update
    void Start()
    {
        Hp = HpMax;
        MovePoints = PointSetting.points[0];

        CreatHpUI();
    }

    // Update is called once per frame
    void Update()
    {
        ////控制怪的方向////
        Vector3 EnemyDir = MovePoints.transform.position - this.gameObject.transform.position;//此向量(方向)=移動點的位子-此物件的位子
        this.gameObject.transform.position += EnemyDir.normalized * Speed*Time.deltaTime;//讓此物件往這個方向移動
        if (Vector3.Distance(this.gameObject.transform.position , MovePoints.transform.position) <=0.1f)//到移動點，就再移動到下一個移動點
        {
            if (Index >= PointSetting.points.Length - 1)//如果超過移動點的數量(碰到終點)，就消除
            {
                Destroy(this.gameObject);
                Destroy(HpObj);
            }
            Index++;
            MovePoints= PointSetting.points[Index];
        }

        HpObj.transform.SetParent(GameObject.Find("UI").transform); //沒加父物件就不會出現
        HpObj.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + new Vector3(0,0.2f,0) );//怪物血量一直跟著怪物
    }

    void CreatHpUI()
    {
        HpObj=Instantiate(GameObject.Find("怪物血量底部"));               //怪物出現就升成血量
    }

    //怪物碰撞控制
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")//碰到子彈時
        {
            Hp -= 1;
            HpObj.transform.GetChild(0).GetComponent<Image>().fillAmount=Hp/HpMax; 


            Destroy(collision.gameObject);
            if (Hp<=0)
            { 
                Destroy(this.gameObject);
                Destroy(HpObj);
            }
        }
    }
}
