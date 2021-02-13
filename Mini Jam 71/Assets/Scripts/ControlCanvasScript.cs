using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCanvasScript : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        Time.timeScale = 1;
    }
}
