using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制子彈
public class BulletControl : MonoBehaviour
{
    float Speed = 5f;//子彈速度
    float SurviveTime = 1f;//砲彈存活時間
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //砲彈從"此位子"至"怪物位子"，用一定速度移動
        this.gameObject.transform.position = Vector3.Lerp(transform.position , WeaponControl.TargetDir, Speed * Time.deltaTime);
        //Vector3 Dir = WeaponControl.TargetDir - WeaponControl.PlayerDir  ;
        //transform.position += Dir.normalized * Speed * Time.deltaTime;

        //砲彈超過3秒就會消失
        if (SurviveTime <= 0f )
        {
            SurviveTime = 3f;
            Destroy(this.gameObject);
        }
        SurviveTime -= Time.deltaTime;

    }



}
