using UnityEngine;

public class StartMusicAudioSource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = GameSettingsController.Instance.GetMusicVolume();
    }

}
