using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //路徑要加入這個

//控制砲塔，放在"角色"上
public class WeaponControl : MonoBehaviour
{
    float[] Range = { 4f, 4f, 6f, 4f, 4f, 4f };  //砲塔攻擊範圍
    GameObject Bullet;                 //放置子彈的預置物
    public Animator PlayerAni;                    //放置角色動畫

    int PlayerKind;                    //紀錄砲塔的種類(用在攻擊距離上)
    int PlayerName;                     //紀錄砲塔的名稱(也等於砲塔位子)
    public static Vector3 PlayerDir;   //紀錄砲塔的位子
    public static Vector3 TargetDir;   //記錄怪物的位子

    GameObject Target;      //要攻擊的怪物目標
    float[] fireCountdown;  //子彈發射頻率的計數器
    float fireRate = 1f;    //控制子彈發射的頻率

    // Start is called before the first frame update
    void Start()
    {
        //做法1，從Resources/Prefab/子彈，自動加入子彈的Prefab(要把要加入的Prefab放入Resources資料夾)
        Bullet = Resources.Load<GameObject>("子彈");

        /*
        //做法2，放置任何位子都行，先找出Prefab的路徑，自動加入子彈的Prefab
        //這個做法不知為何不能寫進APK
        string path = "Assets/Prefab/子彈.prefab";
        Bullet = AssetDatabase.LoadAssetAtPath<GameObject>(path);  
        */
        fireCountdown = new float[SpaceControl.SpacePoints.Length];
        for (int i = 0; i < SpaceControl.SpacePoints.Length; i++) fireCountdown[i] = 0f;                                    //給各砲塔倒數計時器
        for (int i = 1; i < SpaceControl.PlayerNum + 1; i++) if (this.tag == "Player" + i) PlayerKind = i;                  //找出角色的種類，給予不同的攻擊範圍
        for (int j = 0; j < SpaceControl.SpacePoints.Length; j++) if (this.gameObject.name == "砲塔" + j) PlayerName = j;   //找出角色的名稱

        if (GameObject.Find("砲塔" + PlayerName) != null) PlayerAni = GameObject.Find("砲塔" + PlayerName).GetComponent<Animator>(); //抓取人物動畫

        //CD結束後開始開始執行，每0.5秒執行一次"SearchEnemy"
        InvokeRepeating("SearchEnemy", SpaceControl.CoolTime, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null) return;

        PlayerDir = this.gameObject.transform.position;//紀錄砲塔的位子
        TargetDir = Target.transform.position;         //紀錄怪物的位子

        //子彈發射的速度
        if (fireCountdown[PlayerName] <= 0f)
        {
            Shoot();
            fireCountdown[PlayerName] = 1f / fireRate; //攻擊速率
        }
        fireCountdown[PlayerName] -= Time.deltaTime;
    }

    /// <summary>
    /// 尋找敵人位置
    /// </summary>
    public void SearchEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");//找到敵人
        GameObject NearestEnemy = null;//先預設找到的敵人為"空"
        float MinDist = Mathf.Infinity;//砲塔和怪物的距離，預設為無限
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
        //如果怪物出現，且進入攻擊範圍內，就紀錄
        if (NearestEnemy != null && MinDist <= Range[PlayerKind - 1]) Target = NearestEnemy;
        else Target = null;
    }

    /// <summary>
    /// 射擊子彈
    /// </summary>
    public void Shoot()
    {
        //砲塔、子彈、子彈位子會改變方向
        if (PlayerDir.x > TargetDir.x) //向左
        {
            GetComponent<SpriteRenderer>().flipX = true;
            //子彈命名，子彈1 + X = 角色1的子彈 + 流水號
            Instantiate(Bullet, this.gameObject.transform.position + Vector3.left, Quaternion.identity).name = "子彈" + PlayerKind + "_" + BulletDamageControl.BulletSerialNum;
            GameObject.Find("子彈" + PlayerKind + "_" + BulletDamageControl.BulletSerialNum).GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (PlayerDir.x < TargetDir.x) //向右
        {
            GetComponent<SpriteRenderer>().flipX = false;
            Instantiate(Bullet, this.gameObject.transform.position + Vector3.right, Quaternion.identity).name = "子彈" + PlayerKind + "_" + BulletDamageControl.BulletSerialNum;
            GameObject.Find("子彈" + PlayerKind + "_" + BulletDamageControl.BulletSerialNum).GetComponent<SpriteRenderer>().flipX = false;
        }

        BulletControl.Lv = SpaceControl.LvState[PlayerName]; //這個Lv X 的砲塔的子彈，給<BulletControl>使用
        PlayerAni.SetTrigger("攻擊");                        //控制人物動畫-攻擊
        BulletDamageControl.BulletSerialNum += 1;            //子彈流水號+1
    }

    /// <summary>
    /// 繪製攻擊範圍線
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (PlayerKind - 1 >= 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Range[PlayerKind - 1]);
        }
        else { }
    }
}
