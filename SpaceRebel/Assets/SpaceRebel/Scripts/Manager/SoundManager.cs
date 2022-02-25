using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region SERIALIZE FIELDS

    [SerializeField] AudioClip buttonAudio;

    [SerializeField] AudioClip playerShootAudio;

    [SerializeField] AudioClip explosionAudio;

    #endregion

    #region PRIVATE FIELDS
    AudioSource source;
    #endregion
    // Plays common audios
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayAudio(AUDIOTYPE type, bool isAudioOff,bool isLoop = false,float volume=1)
    {
        source.mute = isAudioOff;
        switch (type)
        {
            case AUDIOTYPE.BUTTON:
                source.clip = buttonAudio;
                break;

            case AUDIOTYPE.PLAYERSHOT:
                source.clip = playerShootAudio;
                break;

            case AUDIOTYPE.EXPLOSION:
                source.clip = explosionAudio;
                break;
        }

        source.volume = volume;
        source.loop = isLoop;
        source.Play();
    }
}
