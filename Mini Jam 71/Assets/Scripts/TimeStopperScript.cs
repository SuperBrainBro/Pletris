using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopperScript : MonoBehaviour
{
    public float timeStopFloat;
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void Update()
    {
        timeStopFloat -= .1f * Time.unscaledDeltaTime;
        if (timeStopFloat <= 0)
        {
            Time.timeScale = 1f;
        }
    }
    public void StopTime()
    {
        Time.timeScale = 0f;
        timeStopFloat = 1000;
    }

    public void StartTime()
    {
        Time.timeScale = 1f;

    }

    public void MiniHitStop()
    {
        Time.timeScale = 0f;
        timeStopFloat = .005f;
    }

    public void SmallHitStop()
    {
        Time.timeScale = 0f;
        timeStopFloat = .01f;
    }

    public void MediumHitStop()
    {
        Time.timeScale = 0f;
        timeStopFloat = .02f;
    }

    public void LargeHitStop()
    {
        Time.timeScale = 0f;
        timeStopFloat = .03f;
    }
}
