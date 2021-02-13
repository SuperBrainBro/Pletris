using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public AudioSource UI;
    public AudioSource portalSound;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
