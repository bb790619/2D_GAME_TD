using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制怪物移動
public class EnemyControl : MonoBehaviour
{
    GameObject MovePoints;//讀取且放置移動點的陣列
     float Speed = 1f;//怪物移動速度
    int Index = 0;//移動點的編號
    // Start is called before the first frame update
    void Start()
    {
        MovePoints = PointSetting.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        ////控制怪的方向////
        Vector3 EnemyDir = MovePoints.transform.position - this.gameObject.transform.position;//此向量(方向)為移動點的位子-此物件的位子
        this.gameObject.transform.position += EnemyDir.normalized * Speed*Time.deltaTime;//讓此物件往這個方向移動
        if (Vector3.Distance(this.gameObject.transform.position , MovePoints.transform.position) <=0.1f)//到移動點，就再移動到下一個移動點
        {
            if (Index >= PointSetting.points.Length - 1)//如果超過移動點的數量(碰到終點)，就消除
            {
                Destroy(this.gameObject);
            }
            Index++;
            MovePoints= PointSetting.points[Index];
        }
    }

    //怪物碰撞控制
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")//碰到子彈時
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
