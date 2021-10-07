using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private AdSettings _adSettings;

    private void Awake()
    {
        _adSettings = Singleton<AdSettings>.Instance;
        _adSettings.LoadRewardedAd();
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot(@"D:\scr.png");
        }
    }
}
