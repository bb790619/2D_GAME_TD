using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//放在"MapControl"上，控制目前開啟地圖和怪物血量
public class MapControl : MonoBehaviour
{
    [Header("背景1(普通模式)")] public GameObject BG1;
    [Header("背景2(普通模式)")] public GameObject BG2;
    [Header("背景3(普通模式)")] public GameObject BG3;
    [Header("背景1(困難模式)")] public GameObject BGHard1;
    [Header("背景2(困難模式)")] public GameObject BGHard2;
    [Header("背景3(困難模式)")] public GameObject BGHard3;

    int Chap; //目前地圖的章節
    int Level;//目前地圖的關卡 
    public static int Hp; //怪物血量，給<EnemyControl>使用

    private void Awake()
    {
        Chap = StandByScene.ChapterNow;
        Level = StandByScene.ChapterLevelNow;
        BG1.SetActive(false); BG2.SetActive(false); BG3.SetActive(false);
        BGHard1.SetActive(false); BGHard2.SetActive(false); BGHard3.SetActive(false);
        //看難度開啟背景，關卡/的餘數來決定開啟的地圖
        if (StandByScene.HardMode == false)
        {
            if (Level % 3 == 1) BG1.SetActive(true);
            else if (Level % 3 == 2) BG2.SetActive(true);
            else if (Level % 3 == 0) BG3.SetActive(true);

            /*怪物血量
              預設血量: 1-1~8:55 60 65 70 75 80 85 90
                        2-1~8:80 85 90 95 100 105 110 115
                        3-1~8 105 110 115 120 125 130 135 140
                       血量=(章節+1)*25+(關卡)
                       因為測試時覺得血量太少，所以直接*2
             */
            Hp =(   ( (Chap+1) *25 ) + ( Level *5 )   ) ;
        }
        else if (StandByScene.HardMode == true)
        {
            if (Level % 3 == 1) BGHard1.SetActive(true);
            else if (Level % 3 == 2) BGHard2.SetActive(true);
            else if (Level % 3 == 0) BGHard3.SetActive(true);

            //怪物血量
            Hp =( ( (Chap + 1) * 25) + (Level  * 5) )* 4;
        }

    }

}
