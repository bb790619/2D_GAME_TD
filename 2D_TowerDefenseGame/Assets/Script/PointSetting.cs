using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//設定怪物的移動點，放在"移動點"上
//依照移動點的子物件的數量而自動增加長度
public class PointSetting : MonoBehaviour
{
    public static GameObject[] points;//移動點的陣列
    // Start is called before the first frame update
    void Awake()//Awake比Start先執行
    {
        //開場時，先設定怪物的移動點陣列
        points = new GameObject[transform.childCount];//移動點陣列為此物體的子物件
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
