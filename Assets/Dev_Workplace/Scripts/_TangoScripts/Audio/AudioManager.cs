using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
        return;
    }

    public enum MusicType
    {
        Menu, PreCombat, Combat, Credit,
    }
    [Serializable]
    public struct MusicPair
    {
        public MusicType _MusicType;
        public AudioClip _Audio;
    }
    //!===============================

    public enum BuildingSFX
    {
        Selected, Build
    }
    [Serializable]
    public struct BuildingSFXPair
    {
        public BuildingSFX _MusicType;
        public AudioClip _Audio;
    }
    //!===============================

    public enum BoomSFX
    {
        Cannon, Car
    }
    [Serializable]
    public struct BoomSFXPair
    {
        public BoomSFX _MusicType;
        public AudioClip _Audio;
    }
    //!===============================

    public enum DeadSFX
    {
        Dead1, Dead2, Dead3, Dead4, Dead5, Dead6, Dead7, Dead8
    }
    [Serializable]
    public struct DeadSFXPair
    {
        public DeadSFX _MusicType;
        public AudioClip _Audio;
    }
    //!===============================


    public enum OtherSFX
    {
        PageTurning, Select, Click
    }
    [Serializable]
    public struct OtherSFXPair
    {
        public OtherSFX _MusicType;
        public AudioClip _Audio;
    }
    //!===============================

    public float _MusicVolume;
    public float _SFXVolume;

    [Header("REFERENCE")]
    [SerializeField] AudioSource _musicAudioSource;
    [SerializeField] AudioSource _buildingSFXaudioSource;
    [SerializeField] AudioSource _boomAudioSource;
    [SerializeField] AudioSource _deadAudioSource;
    [SerializeField] AudioSource _otherAudioSource;

    [Space(15)]
    [SerializeField] List<MusicPair> _music;
    [SerializeField] List<BuildingSFXPair> _buildingSFX;
    [SerializeField] List<BoomSFXPair> _boomSFX;
    [SerializeField] List<DeadSFXPair> _deadSFX;
    [SerializeField] List<OtherSFXPair> _otherSFX;



    private void Start()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                PlayMusic(MusicType.Menu, _MusicVolume);
                break;
            case 1:
                PlayMusic(MusicType.PreCombat, _MusicVolume);
                break;
        }
    }

    private void Update()
    {
        _musicAudioSource.volume = _MusicVolume;
    }




    public void PlayMusic(MusicType t, float volume)
    {
        _musicAudioSource.loop = true;
        _musicAudioSource.PlayOneShot(_music[(int)t]._Audio, volume);
    }
    public void PlayBuildingSFX(BuildingSFX t, float volume)
    {
        _buildingSFXaudioSource.PlayOneShot(_buildingSFX[(int)t]._Audio, volume);
    }
    public void PlayBoomSFX(BoomSFX t, float volume)
    {
        _boomAudioSource.PlayOneShot(_boomSFX[(int)t]._Audio, volume);
    }
    public void PlayDeadSFX(DeadSFX t, float volume)
    {
        _deadAudioSource.PlayOneShot(_deadSFX[(int)t]._Audio, volume);
    }
    public void PlayOtherSFX(OtherSFX t, float volume)
    {
        _otherAudioSource.PlayOneShot(_otherSFX[(int)t]._Audio, volume);
    }






}
