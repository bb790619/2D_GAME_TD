using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControl : MonoBehaviour
{
    public GameObject Bullet;       //放置子彈的預置物
    float Range = 4f;               //砲塔攻擊範圍
    public static Vector3 PlayerDir;//紀錄砲塔的位子
    public static Vector3 TargetDir;//記錄怪物的位子

    float fireCountdown = 2f;      //子彈發射頻率的計數器
    float fireRate=0.1f;           //控制子彈發射的頻率

    float CoolCount = 0f;          //建造角色的冷卻時間的倒數計時器
    float CoolTime = 3f;           //建造角色的冷卻時間
    GameObject CDObj;
    // Start is called before the first frame update
    void Start()
    {
        CreatCDUI();
    }

    // Update is called once per frame
    void Update()
    {
        CoolCount -= Time.deltaTime;
        //砲塔生成CD
        CDObj.transform.SetParent(GameObject.Find("UI").transform);                          //沒加父物件就不會出現，而且此物件(CD底部)也不會出現
        CDObj.transform.position = Camera.main.WorldToScreenPoint(transform.position);       //此砲塔的位子
        CDObj.transform.GetChild(0).GetComponent<Image>().fillAmount = CoolCount / CoolTime; //此物件(CD底部)的子物件(CD)，隨著時間讓圖片改變

        if (CoolCount <= 0f)   Destroy(CDObj);

        //3秒開始執行，每一秒執行一次"SearchEnemy"
        InvokeRepeating("SearchEnemy", CoolTime, 0.5f);
    }


    ////尋找敵人////
    void SearchEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");//找到敵人
        float MinDist = Mathf.Infinity;//砲塔和怪物的距離，預設為無限
        GameObject NearestEnemy = null;//先預設找到的敵人為"空"

        //如果砲塔和怪物的距離 小於  無限值 => 出現在畫面中
        //就記錄怪物的座標位子
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, Enemies[i].transform.position) < MinDist)//
            {
                MinDist = Vector3.Distance(transform.position, Enemies[i].transform.position);
                NearestEnemy = Enemies[i];
            }
        }
        //如果怪物出現，且進入攻擊範圍內，就發射子彈
        if (NearestEnemy != null && MinDist <= Range)
        {
            PlayerDir = this.gameObject.transform.position;//紀錄砲塔的位子
            TargetDir = NearestEnemy.transform.position;//紀錄怪物的位子

            //砲塔會改變方向
            if (PlayerDir.x > TargetDir.x) GetComponent<SpriteRenderer>().flipX = false;
            else if (PlayerDir.x < TargetDir.x) GetComponent<SpriteRenderer>().flipX = true;

            //子彈發射的速度
            if (fireCountdown <= 0f)
            {
                Instantiate(Bullet, this.gameObject.transform.position, Quaternion.identity);
                fireCountdown = 1f/fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    //砲塔出現就升成CD冷卻時間
    void CreatCDUI()
    {
        CoolCount = CoolTime;
        CDObj = Instantiate(GameObject.Find("CD底部"));                         
    }

    ////繪製攻擊範圍線////
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
