using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//放在"TimeRecord"上，記錄時間，恢復體力
public class TimeRecord : MonoBehaviour
{
    //欄位變數
    #region
    float TimeNow;   //現在的秒數
    float TimeEnd;   //關機至開機經過的秒數(切換場景也會計算)
    int TimeReset = 60;    //恢復體力的秒數(暫定1分鐘=60秒恢復一次)
    //要存檔的時間
    public float Time_Year;
    public float Time_Month;
    public float Time_Day;
    public float Time_Hour;
    public float Time_Min;
    public float Time_Second;

    public static int Repeat = 0;
    public static bool SaveNow = true;
    #endregion
    //存檔和讀檔
    #region
    [SerializeField]
    TimeData Data;
    [System.Serializable]
    public class TimeData
    {
        public float Time_Year;
        public float Time_Month;
        public float Time_Day;
        public float Time_Hour;
        public float Time_Min;
        public float Time_Second;
        public float TimeNow;
    }
    public void Save()
    {
        Data.Time_Year = Time_Year;
        Data.Time_Month = Time_Month;
        Data.Time_Day = Time_Day;
        Data.Time_Hour = Time_Hour;
        Data.Time_Min = Time_Min;
        Data.Time_Second = Time_Second;
        Data.TimeNow = TimeNow;
    }
    /// <summary>
    /// 讀取的檔案資料
    /// </summary>
    public void Load()
    {
        Time_Year = Data.Time_Year;
        Time_Month = Data.Time_Month;
        Time_Day = Data.Time_Day;
        Time_Hour = Data.Time_Hour;
        Time_Min = Data.Time_Min;
        Time_Second = Data.Time_Second;
        TimeNow = Data.TimeNow;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Repeat = PlayerPrefs.GetInt("RepeatTime"); //讀取檔案
        //共用同一個存檔，只有第一次執行會存檔，避免數值一直被更新歸0
        if (Repeat == 0)
        {
            Repeat = 1;
            RecordNomTime();
            Save();//沒存檔先讀檔會有問題，所以先存一次
            PlayerPrefs.SetString("TimeData", JsonUtility.ToJson(Data));
        }

        Data = JsonUtility.FromJson<TimeData>(PlayerPrefs.GetString("TimeData"));
        Load();
        TimeMath();
        PlayerPrefs.SetInt("RepeatTime", Repeat);//存檔
    }

    /// <summary>
    /// 如果Android背景執行時，會記錄時間
    /// </summary>
    /// <param name="paused"></param>
    public void OnApplicationPause(bool paused)
    {
        if (paused) //如果背景執行，遊戲暫停，記錄時間
        {
            RecordNomTime();
            Save();
            PlayerPrefs.SetString("TimeData", JsonUtility.ToJson(Data));
            // Game is paused, start service to get notifications
        }
        else //遊戲未暫停，讀取時間
        {
            Data = JsonUtility.FromJson<TimeData>(PlayerPrefs.GetString("TimeData"));
            Load();
            TimeMath();
            // Game is unpaused, stop service notifications. 
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (SaveNow == true)
        {
            RecordNomTime();
            Save();
            PlayerPrefs.SetString("TimeData", JsonUtility.ToJson(Data));
        }
        if (Input.GetKeyDown("space"))//為了清除數據，false就先不存檔
        {
            SaveNow = false;
            print("不存了");
        }
        /*如果現在體力沒有滿才執行
          體力加上 (現在秒數+經過秒數)/300的商，之後現在秒數再加上 (現在秒數+經過秒數)/300的餘數，繼續計算
          比如說，現在體力10，經過700秒，體力+2，現在秒數+100再繼續算
         */
        if (StandByScene.BodyStrngthNow < StandByScene.BodyStrngthMAX)
        {
            if (TimeNow >= TimeReset)
            {
                StandByScene.BodyStrngthNow += (int)TimeNow / TimeReset;
                TimeNow = TimeNow % TimeReset;
                if (StandByScene.BodyStrngthNow >= StandByScene.BodyStrngthMAX) StandByScene.BodyStrngthNow = StandByScene.BodyStrngthMAX;
            }
            TimeNow += Time.deltaTime;
            //顯示恢復倒數時間
            GameObject.Find("體力恢復倒數").GetComponent<Text>().text = ((int)(TimeReset-TimeNow) / 60).ToString("D2") + ":" + ((int)(TimeReset - TimeNow) % 60).ToString("D2");
        }
        else
        {
            GameObject.Find("體力恢復倒數").GetComponent<Text>().text = "";
            TimeNow = 0;
        }


    }
    /// <summary>
    /// 紀錄現在的時間(存檔用)
    /// </summary>
    public void RecordNomTime()
    {
        Time_Year = DateTime.Now.Year;
        Time_Month = DateTime.Now.Month;
        Time_Day = DateTime.Now.Day;
        Time_Hour = DateTime.Now.Hour;
        Time_Min = DateTime.Now.Minute;
        Time_Second = DateTime.Now.Second;
    }
    /// <summary>
    /// 計算存檔至讀檔期間，經過多少秒(不只關機，切換場景也換計算)
    /// </summary>
    public void TimeMath()
    {
        float Year = 0, Month = 0, Day = 0, Hour = 0, Min = 0, Second = 0;

        Year = DateTime.Now.Year - Time_Year;       // print("紀錄的年" + Year);
        Month = DateTime.Now.Month - Time_Month;    // print("紀錄的月" + Month);
        Day = DateTime.Now.Day - Time_Day;          // print("紀錄的日" + Day);
        Hour = DateTime.Now.Hour - Time_Hour;       // print("紀錄的時" + Hour);
        Min = DateTime.Now.Minute - Time_Min;       // print("紀錄的分" + Min);
        Second = DateTime.Now.Second - Time_Second; // print("紀錄的秒" + Second);

        //經過多少秒
        TimeEnd = Second + 60 * Min + 60 * 60 * Hour + 60 * 60 * 24 * Day + 60 * 60 * 24 * 30 * Month + 60 * 60 * 24 * 30 * 12 * Year;
        TimeNow += TimeEnd;
    }

}
