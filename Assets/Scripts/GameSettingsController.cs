using UnityEngine;
using UnityEngine.UI;

public class GameSettingsController : MonoBehaviour
{
    public static GameSettingsController Instance;

    public bool useScreenShake = true;

    public bool isMute = false;

    public AudioSource MainMenuBGMSource;
    public AudioSource SFXSource;

    private float musicVolume;
    private float sfxVolume;

    public Slider sfxSlider;
    public Slider musicSlider;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMute()
    {
        isMute = !isMute;
        SetVolumes();
    }

    public void ChangeScreenShake()
    {
        useScreenShake = !useScreenShake;

        if (useScreenShake) StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(.3f, .1f));
    }

    public void SetVolumes()
    {
        musicVolume = MainMenuBGMSource.volume = isMute ? 0 : musicSlider.value;
        sfxVolume = SFXSource.volume = isMute ? 0 : sfxSlider.value;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
}
