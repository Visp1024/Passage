using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Висит на гранях
public class Options : MonoBehaviour
{
    public Toggle parties;
    public Toggle fullscreen;
    public Slider volume;
    public Slider volumeMusic;
    public GameObject part;
    public ButtonAnimation manual;
    public GameObject manual_Max_button;
    public GameObject manual_Min_button;
    public Animation manual_animation;

    public GameObject[] entourage;

    Resolution[] rsl;
    List<string> resolutions;
    public Dropdown dropdown;
    public Dropdown dropdownQuality;

    public void ManualChange(bool Min_max)
    {
       if (Min_max)
        {
            PlayerPrefs.SetInt("Manual scale", 1); //пишем в реестр
        }
        else
            PlayerPrefs.SetInt("Manual scale", 0);
    }

    public void FullScrenOnOff()
    {
        if (fullscreen.isOn)
        {
            PlayerPrefs.SetInt("FullScreen", 1); //пишем в реестр
            Screen.fullScreen = true;
        }
        else
        {
            PlayerPrefs.SetInt("FullScreen", 0);
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }

    public void QualitySet()
    {
        QualitySettings.SetQualityLevel(dropdownQuality.value);
        PlayerPrefs.SetInt("Quality", dropdownQuality.value);
    }

    public void TargetOnOff()
    {
        if (part != null)
            part.SetActive(parties.isOn);
        if (parties.isOn)
            PlayerPrefs.SetInt("Parties enabled", 1); //пишем в реестр
        else
            PlayerPrefs.SetInt("Parties enabled", 0);
    }

    public void VolumeChange()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
    }

    public void VolumeChangeMusic()
    {
        PlayerPrefs.SetFloat("VolumeMusic", volumeMusic.value);
    }

    public void SetRes()
    {
        int r = dropdown.value;
        PlayerPrefs.SetInt("Resolution", r);
        Screen.SetResolution(rsl[r].width, rsl[r].height, fullscreen.isOn);
    }




    void Start()
    {

        resolutions = new List<string>();
        rsl = Screen.resolutions;
        foreach (var i in rsl)
        {
            resolutions.Add(i.width + "x" + i.height);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutions);
        dropdown.value= PlayerPrefs.GetInt("Resolution");

        int y = 0;

        if (entourage.Length > 0)
        {
            y = PlayerPrefs.GetInt("enturage");
            for (int i = 0; i < entourage.Length; i++)
            {
                if (i == y)
                    entourage[i].SetActive(true);
                else
                    entourage[i].SetActive(false);
            }
        }

        if (manual!=null)
            if (PlayerPrefs.GetInt("Manual scale") == 1) //читает с рееcтра
            {
                manual_animation.Blend("Chat_max");
                manual_Max_button.SetActive(false);
                manual_Min_button.SetActive(true);
            }

        if (PlayerPrefs.GetInt("Parties enabled") == 1) //читает с рееcтра
        {
            parties.isOn = true;
            if (part)
               part.SetActive(true);
        }
        else
        {
            parties.isOn = false;
            if (part)
               part.SetActive(false);
        }

        if (PlayerPrefs.GetInt("FullScreen") == 1) //читает с рееcтра
        {
            fullscreen.isOn = true;
        }
        else
        {
            fullscreen.isOn = false;
        }

        dropdownQuality.value = PlayerPrefs.GetInt("Quality");

        volume.value= PlayerPrefs.GetFloat("Volume");
        volumeMusic.value= PlayerPrefs.GetFloat("VolumeMusic");
    }
}
