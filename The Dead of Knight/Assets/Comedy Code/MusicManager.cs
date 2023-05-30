using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager mManger;

    [SerializeField]
    private AudioClip[] Track;

    private int world;

    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        world = Int32.Parse(SceneManager.GetActiveScene().name.Substring(1,1));

        _audio.clip = Track[world];
        _audio.Play();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        print(scene.name.Substring(1,1));
        try
        {
            if (Int32.Parse(scene.name.Substring(1,1)) != world)
            {
                world = Int32.Parse(scene.name.Substring(1,1));
                _audio.clip = Track[world];
                _audio.Play();
            }
        } catch (Exception e) {print(e);}
    }
}
