using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManbouBodyCTRL : MonoBehaviour
{
    public AudioSource audioSource;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void SoundRun()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
