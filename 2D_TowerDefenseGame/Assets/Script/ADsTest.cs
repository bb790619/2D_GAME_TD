using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements; //引用ADS API

public class ADsTest : MonoBehaviour
{
    public static ADsTest Inst; //建一個靜態實體，方便呼叫

    public delegate void OnAdRewardallback();//Call back
    OnAdRewardallback onAdRewardallback;

    //private string GameID_IOS = "3393438"; //後台的ID_IOS
    private string GameID_Android = "3393439"; //後台的ID_IOS

    public bool IsAdReady = false;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this; //建一個靜態實體，方便呼叫

        //初始化
#if UNITY_ANDROID
        Advertisement.Initialize(GameID_Android, false);
//#else
 //        Advertisement.Initialize(GameID_IOS, false);
#endif

    }
    /// <summary>
    /// 檢查廣告有無加載到廣告
    /// </summary>
    public void CheckRewardIsReady()
    {
        if (IsAdReady) return;

        if (Advertisement.IsReady("rewardedVideo"))//這邊要Key後台記的廣告ID
        {
            IsAdReady = true;
        }
    }

    /// <summary>
    /// 開始撥放廣告
    /// </summary>
    /// <param name="callback"></param>
    [System.Obsolete]
    public void ShowRewardAD(OnAdRewardallback callback)
    {
        if (IsAdReady)
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options); //這邊要Key後台記的廣告ID
            IsAdReady = false;
            this.onAdRewardallback = callback;
        }


    }
    /// <summary>
    /// 廣告播放結果
    /// </summary>
    /// <param name="result"></param>
    public void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //撥放成功，呼叫call back
                if (this.onAdRewardallback != null)
                {
                    this.onAdRewardallback();
                    this.onAdRewardallback = null;
                }
                break;

            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end");
                //略過影片會沒有獎勵
                break;

            case ShowResult.Failed:
                 Debug.Log("The ad was failed to be shown");
                CheckRewardIsReady();
                break;
        }
    }
}
