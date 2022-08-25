using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
This file is for creating a sequenced dialog with voice lines.

- To start the sequence playing call PlayNext
- To stop any currently playing items call Stop()
- To play the next clip in the sequence call PlayNext()

- onClipFinished is called when the audio clip finishes
- onDialogFinished is called when the last clip has finished
- onDisplayDialog is called when a clip is started and passes the related text

Only thing I see missing is a clean way to associate _this_ object to the UI/Input processor to call Next
That will have to be handled currently by the system that calls StartSequence on this object
*/

[RequireComponent(typeof(AudioSource))]
public class DialogSequence: MonoBehaviour {
  [SerializeField] List<DialogClip> dialogClips;
  
  public UnityEvent onClipFinished = new UnityEvent();
  public UnityEvent onDialogFinished = new UnityEvent();
  public UnityEvent onDisplayDialog = new MyStringEvent();
  
  private AudioSource audioSource;
  private int currentClip = 0;
  
  private void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }
  
  public void StartSequence()
  {
    currentClip = -1;
    PlayNext();
  }

  public void PlayNext()
  {
    currentClip++;
    if(currentClip >= dialogClips.length) return;
    
    onDisplayDialog.Invoke(dialogClips[currentClip].textLine);
    StartCoroutine(WaitForSound(dialogClips[currentClip].audioClip));
  }
  
  private IEnumerator WaitForSound(AudioClip audioClip) 
  {
    audioSource.Play(audioClip);
    yield return new WaitUntil(() => audiosource.isPlaying == false);
    onClipFinished.Invoke();
    if(currentClip >= dialogClips.length-1) onDialogFinished.Invoke();
  }

  public void Stop()
  {
    audioSource.Stop();
    StopAllCoroutines();
  }
}

[System.Serializable]
public class DialogClip {
  public AudioClip audioClip;
  public String textLine;
}

[System.Serializable]
public class MyStringEvent : UnityEvent<String>();
