using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    Resolution[] resolutions;

    private TMPro.TMP_Dropdown resolutionDropdown;
    private Toggle fullScreenToggle;

    public AudioMixer audioMixer;

    private void Awake()
    {
        fullScreenToggle = gameObject.GetComponentInChildren<Toggle>();
        resolutionDropdown = gameObject.GetComponentInChildren<TMPro.TMP_Dropdown>();
    }

    private void Start()
    {
        fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen") == 1;

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        resolutionDropdown.RefreshShownValue();

    }

    private void OnDestroy()
    {
        //EventCenter.GetInstance().RemoveEventListener("Options", UpdateNumOfSeeds);
    }


    public void SetResolution()
    {
        Resolution resolution = resolutions[resolutionDropdown.value];

        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float Volume)
    {
        audioMixer.SetFloat("volume", Volume);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
       
    }

    public void SetFullscreen()
    {
       

        PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        
        Screen.fullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
    }

}
