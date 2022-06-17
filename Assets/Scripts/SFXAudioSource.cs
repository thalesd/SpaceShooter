using UnityEngine;

public class SFXAudioSource : MonoBehaviour
{
    public static SFXAudioSource Instance;

    public AudioClip[] effectAudioClips;
    private AudioSource _audioSource;

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

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = GameSettingsController.Instance.GetSFXVolume();
    }

    // Update is called once per frame
    public void PlayGameplayEffectOneShot(int indexOfClip)
    {
        _audioSource.PlayOneShot(effectAudioClips[indexOfClip]);
    }
}
