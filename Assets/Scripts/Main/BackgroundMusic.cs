using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip = null;

    private AudioSource audioSource = null;
    
    void Start()
    {
        if(!audioSource) audioSource = GetComponent<AudioSource>();
    }

    public void StopBackgroundMusic(){
        audioSource.volume = 0;
    }
    public void StartBackgroundMusic(){
        audioSource.volume = 1;
    }
}
