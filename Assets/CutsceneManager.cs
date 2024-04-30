using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    public UnityEvent<PlayableAsset> cutsceneDone;
    bool eventCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        director.stopped += OnDirectorEnded;
    }

    // Update is called once per frame
    void Update()
    {
        if(director.time > director.duration - 0.01f)
        {
            if(!eventCalled)
            {
                cutsceneDone.Invoke(director.playableAsset);
                eventCalled = true;
            }
        }
        else if(director.state == PlayState.Playing)
        {
            eventCalled = false;
        }
    }

    public void PlayCutscene(TimelineAsset timelineAsset)
    {
        if(director.state == PlayState.Playing)
        {
            director.Stop();
        }
        director.playableAsset = timelineAsset;
        director.Play();
    }

    public void OnDirectorEnded(PlayableDirector playableDirector)
    {
        cutsceneDone.Invoke(director.playableAsset);
    }

    public void StopCutscene()
    {
        director.Stop();
        director.playableAsset = null;
    }
}
