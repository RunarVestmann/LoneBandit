using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    const string VOLUME = "mainVolume";
    
    void Awake()
    {
        if (!PlayerPrefs.HasKey(VOLUME))
            PlayerPrefs.SetFloat(VOLUME, 1f);
        Load();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    void Save() => PlayerPrefs.SetFloat(VOLUME, volumeSlider.value);

    void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(VOLUME);
        ChangeVolume();
    }
}
