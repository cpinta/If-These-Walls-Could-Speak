using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    AudioClip clip;
    AudioSource audioSource;
    float timeTilPlayingTime = 5;
    float timeTilPlayingTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
    }

    // Update is called once per frame
    void Update()
    {
        if(clip != null)
        {
            if(!audioSource.isPlaying)
            {
                if(timeTilPlayingTimer > 0)
                {
                    timeTilPlayingTimer -= Time.deltaTime;
                }
                else
                {
                    audioSource.clip = this.clip;
                    audioSource.Play();
                    timeTilPlayingTimer = timeTilPlayingTime;
                }
            }
        }
    }

    public void SetAnswer(AudioClip clip)
    {
        this.clip = clip;
    }
}
