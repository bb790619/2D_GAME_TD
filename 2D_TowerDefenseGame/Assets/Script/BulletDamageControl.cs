using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//放在"BulletDamageControl"上，存放子彈傷害、效果
public class BulletDamageControl : MonoBehaviour
{
    public static int BulletSerialNum = 0;       //子彈的流水號，給<WeaponControl>使用
    public static int NumMax=50;                 //陣列儲存的最大數量，超過就重0開始
    public static int[] Damage;                  //子彈的傷害
    public static string[] Effect;               //額外效果名稱
    public static Vector3[] Target;              //子彈的目標座標

    float SurviveTime=4;
    /*
       子彈傷害的計算方式，因為先前方法都會被覆蓋，所以分開計算
       先將子彈命名為，子彈X_Y ( X為子彈種類，Y為流水號)(流水號:讓每個數字分開，超過就重算)
       所以Damage[0]就是子彈X_0，Damage[1]就是子彈X_1...以此類推，這樣用矩陣分開，寫法也不會太複雜
       至於Damage在<BulletControl>計算，所以之後產生的傷害值就不會被覆蓋
       最後在<EnemyControl>使用這個傷害值
    */

    // Start is called before the first frame update
    void Start()
    {
        Damage = new int[NumMax];
        Effect = new string[NumMax];
        Target = new Vector3[NumMax];
    }

    // Update is called once per frame
    void Update()
    {
        if (BulletSerialNum >= NumMax) BulletSerialNum = 0;

    }
}
