using UnityEngine;

public class SetVolumeOnAwake : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.HasKey("mainVolume"))
            AudioListener.volume = PlayerPrefs.GetFloat("mainVolume");
    }
}
