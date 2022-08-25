using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/* 
  method to play audio based on a string ID
  This works for reasonable amounts of audio clips
  but if you have to many to not want to load them all into 
  memory at the same time, you would need a different method
  
  for optimiation for setting this up, moving the `dialogClips` into a scriptable object
  would be an improvment, and you could create an editor script to automatically populate it
  based on the audio clips in a directory
*/

[RequireComponent(typeof(AudioSource))]
public class DialogDictionary: MonoBehaviour {
  [SerializeField] List<DialogClipID> dialogClips;

  private Dictionary<String, AudioClip> dialogDictionary = new Dictionary<String, AudioClip>();
  private AudioSource audioSource;
  
  private void Start()
  {
    audioSource = GetComponent<AudioSource>();
    foreach(DialogClipID clip in dialogClips)
    {
      dialogDictionary.Add(clip.id, clip.audioClip);
    }
    
  }

  public void PlayClipID(String id) 
  {
    AudioClip clip;
    if(dialogDictionary.TryGet(id, out clip)
    {
      audioSource.Play(clip);
    }
  }
}

[System.Serializable]
public class DialogClipID 
{
  public AudioClip audioClip;
  public String id;
}
