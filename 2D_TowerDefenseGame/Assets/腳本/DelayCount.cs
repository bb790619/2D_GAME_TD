using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//延遲計數器
public class DelayCount : MonoBehaviour
{
    public static float CoolCount = 0f;    //建造角色的冷卻時間的倒數計時器，給SpaceControl使用，冷卻時間內不能再點此砲塔；給WeaponControl使用，冷卻時間內不能再點此砲塔
    public static float CoolTime = 3f;     //建造角色的冷卻時間，給WeaponControl使用，冷卻時間結束才能攻擊
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

        if (CoolCount <= 0f) Destroy(CDObj);
    }

    //砲塔出現就升成CD冷卻時間
    void CreatCDUI()
    {
        CoolCount = CoolTime;
        CDObj = Instantiate(GameObject.Find("CD底部"));
    }
}
