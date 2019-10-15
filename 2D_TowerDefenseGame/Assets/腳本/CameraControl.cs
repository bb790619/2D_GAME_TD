using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制攝影機移動，可用手指移動螢幕，放在"Main Camera"上
public class CameraControl : MonoBehaviour
{
    Vector2 m_screenPos = new Vector2();
    float Camera_Xmax = 2.5f; //攝影機向左右移動的最大範圍，Size:8.5f
    float Camera_Ymax = 1.8f; //攝影機向上下移動的最大範圍

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //判斷平台
#if UNITY_ANDROID || UNITY_IOS
        MobileInput();
#endif
#if UNITY_EDITOR_WIN
        DeskopInput();
#endif

    }

    void MobileInput()
    {
        float Speed = 0.5f; //螢幕移動的速度

        //1個手指觸碰螢幕
        if (Input.touchCount == 1)
        {           
            if (Input.touches[0].phase == TouchPhase.Began)//開始觸碰
            {
                //紀錄觸碰位置
                m_screenPos = Input.touches[0].position;
            }//手指移動
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                //移動攝影機
                Camera.main.transform.position += new Vector3(-Input.touches[0].deltaPosition.x * Time.deltaTime * Speed, -Input.touches[0].deltaPosition.y * Time.deltaTime * Speed, 0);
            }


            //手指離開螢幕
            if (Input.touches[0].phase == TouchPhase.Ended && Input.touches[0].phase == TouchPhase.Canceled)
            {
                Vector2 pos = Input.touches[0].position;
                Speed = Speed * 100f; //滑動螢幕速度

                //手指水平移動
                if (Mathf.Abs(m_screenPos.x - pos.x) > Mathf.Abs(m_screenPos.y - pos.y))
                {
                    if (m_screenPos.x > pos.x)
                    {
                        //手指向左滑動
                        Camera.main.transform.position += new Vector3((m_screenPos.x - pos.x) * Time.deltaTime * Speed, 0, 0);
                    }
                    else
                    {
                        //手指向右滑動
                        Camera.main.transform.position += new Vector3(-(m_screenPos.x - pos.x) * Time.deltaTime * Speed, 0, 0);
                    }
                }
                else
                {
                    if (m_screenPos.y > pos.y)
                    {
                        //手指向下滑動
                        Camera.main.transform.position += new Vector3(0, (m_screenPos.y - pos.y) * Time.deltaTime * Speed, 0);

                    }
                    else
                    {
                        //手指向上滑動
                        Camera.main.transform.position += new Vector3(0, -(m_screenPos.y - pos.y) * Time.deltaTime * Speed, 0);
                    }
                }
            }
            //限制攝影機移動範圍
            Camera.main.transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Camera_Xmax, Camera_Xmax),
                                                         Mathf.Clamp(transform.position.y, -Camera_Ymax, Camera_Ymax), transform.position.z);
        }
        
    }

    void DeskopInput()
    {
        //紀錄滑鼠左鍵的移動距離
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        float speed = 6.0f;

        if (mx != 0 || my != 0)
        {
            //滑鼠左鍵
            if (Input.GetMouseButton(0))
            {
                //移動攝影機位置
                Camera.main.transform.position += new Vector3(-mx * Time.deltaTime * speed, -my * Time.deltaTime * speed, 0);

            }
            //限制攝影機移動範圍
            Camera.main.transform.position = new Vector3(Mathf.Clamp(transform.position.x, -Camera_Xmax, Camera_Xmax),
                                             Mathf.Clamp(transform.position.y, -Camera_Ymax, Camera_Ymax), transform.position.z);

        }

    }

}

