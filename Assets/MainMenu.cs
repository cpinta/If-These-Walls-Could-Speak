using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI.Table;

public class MainMenu : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Canvas canvasUI;
    [SerializeField] Canvas canvasUIBackground;
    [SerializeField] Canvas canvasSkipUI;
    [SerializeField] Scrollbar scrollbarSkip;
    [SerializeField] Scene sceneGame;

    bool skipping;
    float SkipTime = 3;
    float SkipTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += LoadGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (skipping)
        {
            canvasSkipUI.enabled = true;
            if (SkipTimer > 0)
            {
                SkipTimer -= Time.deltaTime;
                scrollbarSkip.size = (SkipTime - SkipTimer) /SkipTime;
            }
            else
            {
                LoadGame(null);
                skipping = false;
            }
        }
        else
        {
            scrollbarSkip.size = 0;
            canvasSkipUI.enabled = false;
            SkipTimer = SkipTime;
        }
    }

    public void PlayIntroVideo()
    {
        canvasUI.enabled = false;
        canvasUIBackground.enabled = false;
        videoPlayer.Play();
    }

    public void LoadGame(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Skip(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            skipping = true;
        }
        else if(context.canceled)
        {
            skipping = false;
        }
    }
}
