using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //路徑要加入這個

//控制砲塔，放在"角色"上
public class WeaponControl : MonoBehaviour
{
    GameObject Bullet;                 //放置子彈的預置物
    float Range = 4f;                  //砲塔攻擊範圍
    public static Vector3 PlayerDir;   //紀錄砲塔的位子
    public static Vector3 TargetDir;   //記錄怪物的位子

    float fireCountdown = 2f;         //子彈發射頻率的計數器
    float fireRate = 0.1f;            //控制子彈發射的頻率

    // Start is called before the first frame update
    void Start()
    {
        //做法1，從Resources/Prefab/子彈，自動加入子彈的Prefab(要把要加入的Prefab放入Resources資料夾)
        //Bullet = Resources.Load<GameObject>("Prefab/子彈");   

        //做法2，放置任何位子都行，先找出Prefab的路徑，自動加入子彈的Prefab
        string path = "Assets/Prefab/子彈.prefab";
        Bullet = AssetDatabase.LoadAssetAtPath<GameObject>(path);     
    }

    // Update is called once per frame
    void Update()
    {
        //3秒開始執行，每一秒執行一次"SearchEnemy"
        InvokeRepeating("SearchEnemy", SpaceControl.CoolTime, 0.5f);
    }


    ////尋找敵人及攻擊////
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
                for (int j = 0; j < SpaceControl.SpacePoints.Length; j++) //加這2段才能固定子彈的名稱
                {
                    if (this.gameObject.name == "砲塔" + j)  
                    {
                        Instantiate(Bullet, this.gameObject.transform.position, Quaternion.identity).name = "子彈" + SpaceControl.PlayerKind[j]; //子彈命名，子彈1=角色1的子彈
                        BulletControl.Lv=SpaceControl.LvState[j];
                    }
                }
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }   

    ////繪製攻擊範圍線////
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
