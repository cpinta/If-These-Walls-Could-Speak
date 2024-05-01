using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TV : CameraSpot
{
    VHSTape tape;
    Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Transform tapeLocation;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip outroVideo;
    [SerializeField] VideoClip staticVideo;

    bool playingOutro = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        videoPlayer.loopPointReached += VideoOver;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Entity entity)
    {
        if (tape == null)                            //if there is no plate on the table
        {
            if (entity.currentCollectable != null)
            {
                if (entity.currentCollectable is VHSTape)     //if the player is currently holding a plate
                {
                    VHSTape playersTape = entity.currentCollectable as VHSTape;
                    tape = (VHSTape)entity.TakeCurrentItem();

                    tape.GiveDestination(tapeLocation);
                    animator.SetBool("Open", true);
                }
                else
                {
                    return;
                }
            }
        }
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {
        if (isHovering)
        {
            if (entity.HasCollectableType(typeof(VHSTape)))
            {
                interactText = "Insert Tape";
            }
            else
            {
                interactText = "";
            }
        }
    }

    public override void ResetGame()
    {
        animator.SetBool("Open", false);
        playingOutro = false;
        if (tape != null)
        {
            Destroy(tape);
            tape = null;
        }
    }

    void VideoOver(VideoPlayer source)
    {
        if (playingOutro)
        {
            GM.I.OutroTapeDone();
            audioSource.volume = 0.1f;
            videoPlayer.Stop();
            videoPlayer.clip = staticVideo;
            videoPlayer.Play();
        }
    }

    public void PlayTape()
    {
        playingOutro = true;
        GM.I.OutroTapePlaying(this);
        audioSource.volume = 1;
        videoPlayer.Stop();
        videoPlayer.clip = outroVideo;
        videoPlayer.Play();
    }
}
