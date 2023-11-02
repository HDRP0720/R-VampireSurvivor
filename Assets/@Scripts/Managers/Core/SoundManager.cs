using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SoundManager
{
  private AudioSource[] _audioSources = new AudioSource[(int)Define.ESound.Max];
  private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
  
  private GameObject _soundRoot = null;

  public void Init()
  {
    if (_soundRoot == null)
    {
      _soundRoot = GameObject.Find("SoundRoot");
      if (_soundRoot == null)
      {
        _soundRoot = new GameObject{ name = "SoundRoot" };
        Object.DontDestroyOnLoad(_soundRoot);
        
        string[] soundTypeNames = Enum.GetNames(typeof(Define.ESound));
        for (int count = 0; count < soundTypeNames.Length; count++)
        {
          GameObject go = new GameObject { name = soundTypeNames[count] };
          _audioSources[count] = go.AddComponent<AudioSource>();
          go.transform.parent = _soundRoot.transform;
        }
        
        _audioSources[(int)Define.ESound.BGM].loop = true;
      }
    }
  }

  public void Play(Define.ESound type, string key, float pitch = 1.0f)
  {
    AudioSource audioSource = _audioSources[(int)type];

    if (type == Define.ESound.BGM)
    {
      LoadAudioClip(key, (audioClip) =>
      {
        if(audioSource.isPlaying)
          audioSource.Stop();
        
        audioSource.clip = audioClip;
        if(Managers.Game.BGMOn)
          audioSource.Play();
      });
    }
    else if (type == Define.ESound.Effect)
    {
      LoadAudioClip(key, (audioClip) =>
      {
        audioSource.pitch = pitch;
        if (Managers.Game.EffectSoundOn)
          audioSource.PlayOneShot(audioClip);
      });
    }
  }

  public void Stop(Define.ESound type)
  {
    AudioSource audioSource = _audioSources[(int)type];
    audioSource.Stop();
  }
  
  public void Clear()
  {
    foreach (AudioSource audioSource in _audioSources)
      audioSource.Stop();
    _audioClips.Clear();
  }

  public void PlayButtonClick()
  {
    Play(Define.ESound.Effect, "Click_CommonButton");
  }

  public void PlayPopupClose()
  {
    Play(Define.ESound.Effect, "PopupClose_Common");
  }

  private void LoadAudioClip(string key, Action<AudioClip> callback)
  {
    AudioClip audioClip = null;
    if (_audioClips.TryGetValue(key, out audioClip))
    {
      callback?.Invoke(audioClip);
      return;
    }
    
    audioClip = Managers.Resource.Load<AudioClip>(key);

    if (!_audioClips.ContainsKey(key))
      _audioClips.Add(key, audioClip);
    
    callback?.Invoke(audioClip);
  }
}
