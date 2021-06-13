using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

// Inspired by Brackey's audio manager.
public class AudioManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private Sound[] sounds;
#pragma warning restore

    public static AudioManager instance;

    private List<Sound> currentlyLooping;

    public static float FADE_TIME = 1.0f;
    public static float BGM_VOLUME = 0.5f;
    public static float CROWD_NOISE_MAX_VOLUME = 0.5f;
    private const int FADE_STEPS_PER_SEC = 10;

    private void OnEnable() {
        SceneManager.sceneLoaded += PlayNewSceneSounds;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= PlayNewSceneSounds;
    }

    void Awake() {
        // Handle audio manager instancing between scene loads.
        // If there is no instance, let this be the new instance, otherwise, destroy this object.
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        // If this object was set as the instance, make sure it is not destroyed on scene loads.
        DontDestroyOnLoad(gameObject);

        currentlyLooping = new List<Sound>();

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;

            if (sound.name == "LowBGM") {
                BGM_VOLUME = sound.volume;
			} else if (sound.name == "CrowdNoise") {
                CROWD_NOISE_MAX_VOLUME = sound.volume;
			}
        }
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, e => e.name == name);

        if (sound != null) {
            // If sound is not a looping sound, or is a looping sound and currently not playing, play the sound.
            if (!sound.loop || (sound.loop && !currentlyLooping.Contains(sound))) {
                sound.source.Play();

                // Add the looping sound to currently playing looping sounds.
                if (sound.loop) {
                    currentlyLooping.Add(sound);
                }
            }
        } else {
            Debug.LogWarning("Audio clip " + name + " not found. No audio played.");
        }
    }

    public void Stop(string name) {
        Sound sound = Array.Find(sounds, e => e.name == name);

        if (sound != null) {
            sound.source.Stop();

            // If the sound was a loop, remove the sound from the currently playing looping sounds.
            if (sound.loop) {
                currentlyLooping.Remove(sound);
            }
        } else {
            Debug.LogWarning("Audio clip " + name + " not found. No audio stopped.");
        }
    }

    public void ChangeSoundVolume(string name, float volume) {
        volume = Mathf.Clamp01(volume);
        Sound sound = Array.Find(sounds, e => e.name == name);

        if (sound != null) {
            sound.source.volume = volume;
        } else {
            Debug.LogWarning("Audio clip " + name + " not found. No volume changed.");
        }
    }

    public void FadeSoundVolume(string name, float targetVolume, float fadeTime) {
        targetVolume = Mathf.Clamp01(targetVolume);
        Sound sound = Array.Find(sounds, e => e.name == name);

        if (sound != null) {
            StartCoroutine(FadeOverTime(sound, targetVolume, fadeTime));
        } else {
            Debug.LogWarning("Audio clip " + name + " not found. No volume changed.");
        }
    }

    private IEnumerator FadeOverTime(Sound sound, float targetVolume, float fadeTime) {
        float startVolume = sound.source.volume;
        int fadeSteps = Mathf.FloorToInt(fadeTime * FADE_STEPS_PER_SEC);
        float timePerStep = (float)fadeTime / fadeSteps;
        for (int i = 0; i <= fadeSteps; ++i) {
            float t = (float)i / fadeSteps;
            sound.source.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return new WaitForSecondsRealtime(timePerStep);
		}
    }

    private void PlayNewSceneSounds(Scene scene, LoadSceneMode mode) {
        switch (scene.name) {
            case "GameScene":
                Stop("MenuMusic");
                Play("LowBGM");
                Play("BaseBGM");
                Play("MidBGM");
                Play("TopBGM");
                FadeSoundVolume("LowBGM", 0.5f, FADE_TIME);
                FadeSoundVolume("BaseBGM", 0.0f, FADE_TIME);
                FadeSoundVolume("MidBGM", 0.0f, FADE_TIME);
                FadeSoundVolume("TopBGM", 0.0f, FADE_TIME);

                Play("CrowdNoise");
                ChangeSoundVolume("CrowdNoise", 0.0f);
                break;
            case "MainMenuScene":
                Stop("LowBGM");
                Stop("BaseBGM");
                Stop("MidBGM");
                Stop("TopBGM");
                Stop("CrowdNoise");
                Play("MenuMusic");
                break;
        }
    }
}
