using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagement : MonoBehaviour
{
    public GameObject transitionObject;
    private Animator transitionAnim;
    private TransitionScript transitionScript;
    private AudioManagement audioManager;

    private void Start()
    {
        transitionAnim = transitionObject.GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagement>();
        transitionScript = transitionObject.GetComponent<TransitionScript>();
    }
    public void Play()
    {
        audioManager.UI.Play();
        transitionScript.Close();
        Debug.Log("Transition Closed");
        StartCoroutine(TransitionScene("play"));
    }
    public void PlayScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void Exit()
    {
        audioManager.UI.Play();
        transitionScript.Close();
        Debug.Log("Transition Closed");
        StartCoroutine(TransitionScene("exit"));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public IEnumerator TransitionScene(string whatButton)
    {
        if (whatButton == "play")
        {
            yield return new WaitForSeconds(2);
            PlayScene();
        }
        if (whatButton == "exit")
        {
            yield return new WaitForSeconds(2);
            ExitGame();
        }
    }
}
