using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Linq;

public class OptionsMenu : MonoBehaviour
{
    public Toggle invertYToggle;
    private CameraController cameraController;
    public int previousSceneIndex;
    public Slider BGMSlider;
    public Slider SFXSlider;
    private AudioMixer masterMixer;
    private AudioMixerGroup[] bgmGroup;
    private AudioMixerGroup[] sfxGroups;


    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            // Load the saved invert Y-axis setting
            invertYToggle.isOn = cameraController.isInverted;
        }
        else
        {
            // Disable the invert Y-axis toggle if the CameraController is not found
            invertYToggle.interactable = false;
        }

        // Get a reference to the "MasterMixer" audio mixer
        masterMixer = Resources.Load<AudioMixer>("MasterMixer");

        // Find the BGM group from the audio mixer
        bgmGroup = masterMixer.FindMatchingGroups("BGM");

        // Find the SFX groups from the audio mixer
        sfxGroups = new AudioMixerGroup[0];
        sfxGroups = sfxGroups.Concat(masterMixer.FindMatchingGroups("Running")).ToArray();
        sfxGroups = sfxGroups.Concat(masterMixer.FindMatchingGroups("Landing")).ToArray();
        sfxGroups = sfxGroups.Concat(masterMixer.FindMatchingGroups("Ambience")).ToArray();

        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        // Load the saved BGM and SFX slider values
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Set the initial audio volume based on the slider values
        SetAudioVolumes();
    }

    public void Apply()
    {
        // Save the invert Y-axis setting
        if (cameraController != null)
        {
            cameraController.isInverted = invertYToggle.isOn;
        }

        // Save the BGM and SFX slider values
        if (bgmGroup != null && bgmGroup.Length > 0)
        {
            foreach (var group in bgmGroup)
            {
                if (group.audioMixer != null)
                {
                    group.audioMixer.SetFloat("BGMVolume", Mathf.Log10(BGMSlider.value) * 20);
                }
            }
            PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
        }

        if (sfxGroups != null && sfxGroups.Length > 0)
        {
            foreach (var group in sfxGroups)
            {
                if (group.audioMixer != null)
                {
                    group.audioMixer.SetFloat("SFXVolume1", Mathf.Log10(SFXSlider.value) * 20); // Ambience
                    group.audioMixer.SetFloat("SFXVolume2", Mathf.Log10(SFXSlider.value) * 20); // Landing
                    group.audioMixer.SetFloat("SFXVolume3", Mathf.Log10(SFXSlider.value) * 20); // Running
                }
            }
            PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        }

        // Return to the previous scene
        SceneManager.LoadScene(previousSceneIndex);
    }

    public void Back()
    {
        // Discard any changes and return to the previous scene
        SceneManager.LoadScene(previousSceneIndex);
    }

    private void SetAudioVolumes()
    {
        if (bgmGroup != null && bgmGroup.Length > 0)
        {
            // BGM Volume
            float bgmVolume = BGMSlider.value;
            masterMixer.SetFloat("BGMVolume", Mathf.Log10(bgmVolume) * 20);
        }
        else
        {
            BGMSlider.value = 1f;
            Debug.LogError("bgmGroups is null or empty");
        }

        if (sfxGroups != null && sfxGroups.Length > 0)
        {
            float sfxVolume = SFXSlider.value;
            masterMixer.SetFloat("SFXVolume1", Mathf.Log10(sfxVolume) * 20); // Ambience
            masterMixer.SetFloat("SFXVolume2", Mathf.Log10(sfxVolume) * 20); // Landing
            masterMixer.SetFloat("SFXVolume3", Mathf.Log10(sfxVolume) * 20); // Running
        }
        else
        {
            SFXSlider.value = 1f;
            Debug.Log("sfxGroups is null or empty");
        }
    }

    private AudioMixerGroup[] FindAudioMixerGroupsByTag(string tag)
    {
        var mixerGroups = FindObjectsOfType<AudioMixerGroup>();
        return System.Array.FindAll(mixerGroups, g => g.name.Contains(tag));
    }
}