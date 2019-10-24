using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//放在<MouseSetting>，控制滑鼠特效
public class MouseSetting : MonoBehaviour 
{
    public GameObject MouseEffect;  //觸碰特效
    float MouseEffectTimeCount;                //觸碰特效的計時倒數
    // Start is called before the first frame update
    void Start()
    {
        MouseEffect.SetActive(false);
        MouseEffectTimeCount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        MouseEffectTimeCount -= Time.deltaTime;
        MouseEffectApear(); 

    }



    public void MouseEffectApear()
    {
        Vector2 Mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //紀錄滑鼠觸碰的2D座標比例
        //RaycastHit2D hit = Physics2D.Raycast(Mouse_pos, Vector2.zero);
        if (Input.GetMouseButtonDown(0))
        {
            MouseEffectTimeCount = 0.4f;
            MouseEffect.SetActive(true);
            MouseEffect.transform.position = Mouse_pos;
        }
        if (MouseEffectTimeCount <= 0f) MouseEffect.SetActive(false);

    }



}
