using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSttings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer; 
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            
            SetMusicVolume();
        }
    }

    public void SetMusicVolume()
    {
        float Volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(Volume)*20);
        PlayerPrefs.SetFloat("musicVolume", Volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        SetMusicVolume();
    }
}
