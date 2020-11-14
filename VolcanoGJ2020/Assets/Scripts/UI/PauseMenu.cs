﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AudioManager;

public class PauseMenu : MonoBehaviour
{

    public static bool IsGamePaused = false;

    public GameObject pauseMenuUI;
    public Slider volumeSlider;
    public Slider volumeMusicSlider;
    public Slider volumeSoundSlider;

    public void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        volumeMusicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        volumeSoundSlider.value = PlayerPrefs.GetFloat("SoundVol", 0.75f);

        //Adds a listener to the main slider and invokes a method when the value changes.
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(volumeType.master); });
        volumeMusicSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(volumeType.music); });
        volumeSoundSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(volumeType.sound); });

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();

            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck(volumeType type)
    {
        switch (type)
        {
            case volumeType.master:
                AudioManager.instance.mainMixer.audioMixer.SetFloat("MasterVol", Mathf.Log10(volumeSlider.value) * 20);
                PlayerPrefs.SetFloat("MasterVol", volumeSlider.value);
                break;
            case volumeType.music:
                AudioManager.instance.mainMixer.audioMixer.SetFloat("MusicVol", Mathf.Log10(volumeMusicSlider.value) * 20);
                PlayerPrefs.SetFloat("MusicVol", volumeMusicSlider.value);
                break;
            case volumeType.sound:
                AudioManager.instance.mainMixer.audioMixer.SetFloat("SoundVol", Mathf.Log10(volumeSoundSlider.value) * 20);
                PlayerPrefs.SetFloat("SoundVol", volumeSoundSlider.value);
                break;
        }
    }
}