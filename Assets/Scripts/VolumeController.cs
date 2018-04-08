using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeController : MonoBehaviour
{

    public const float DefaultVolumeLevel = 1f;

    Slider volumeSlider;



    void Start()
    {
        GameObject temp = GameObject.Find("Volume Slider");
        if (temp != null)
        {
            temp.SetActive(true);
            volumeSlider = temp.GetComponent<Slider>();

            if (volumeSlider != null)
            {
                volumeSlider.normalizedValue = PlayerPrefs.HasKey("VolumeLevel") ? PlayerPrefs.GetFloat("VolumeLevel") : DefaultVolumeLevel;
            }
            else
            {
                Debug.LogError("[" + temp.name + "] - Does not contain a Slider Component!");
            }

        }
        else
        {
            Debug.LogError("Could not find an active GameObject named Volume Slider!");
        }

    }
    public void OnApply()
    {
        PlayerPrefs.SetFloat("VolumeLevel", volumeSlider.normalizedValue);
        AudioListener.volume = volumeSlider.normalizedValue;
        Debug.Log("Volume Changed");
    }
}