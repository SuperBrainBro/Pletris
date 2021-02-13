using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().SetBool("close", false);
        GetComponent<Animator>().SetBool("open", true);
    }
    public void Open()
    {
        GetComponent<Animator>().SetBool("close", false);
        GetComponent<Animator>().SetBool("open", true);
    }
    public void Close()
    {
        GetComponent<Animator>().SetBool("open", false);
        GetComponent<Animator>().SetBool("close", true);
    }
}
