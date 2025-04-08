using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    public Slider slider;
    public float volume;
    public Toggle toggle;
    public TMP_Dropdown dropdown;
    private Resolution[] resolutions;


    // Start is called before the first frame update
    void Start()
    {
        //Maneja slider de volumen al iniciar
        slider.value = PlayerPrefs.GetFloat("volumeAudio");
        AudioListener.volume = slider.value;

        //Maneja bot�n pantalla completa
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        initResolutions();

    }

    public void ChangeSlider(float value)
    {
        volume = value;
        PlayerPrefs.SetFloat("volumeAudio", volume);
        AudioListener.volume = slider.value;
    }

    public void ChangeToogle(bool value)
    {
        Screen.fullScreen = value;
    }

    public void initResolutions()
    {
        int currentResolution = 0;
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();
        List<string> opciones = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "hz";
            opciones.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
            {
                currentResolution = i;
            }
        }

        dropdown.AddOptions(opciones);
        dropdown.value = currentResolution;
        dropdown.RefreshShownValue();
        dropdown.value = PlayerPrefs.GetInt("playerResolution");
    }

    public void ChangeDropdown(int resolution)
    {
        Resolution newResolution = resolutions[resolution];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("playerResolution", dropdown.value);
    }

}