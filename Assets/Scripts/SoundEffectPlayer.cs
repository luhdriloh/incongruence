using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public static SoundEffectPlayer _sfxPlayer;

    private AudioSource _audiosource;

	private void Awake ()
    {
        if (_sfxPlayer == null)
        {
            _sfxPlayer = this;
            _audiosource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this);
        }
	}
	
	private void Update ()
    {
		
	}

    public void PlaySoundEffect(AudioClip sfx)
    {
        _audiosource.PlayOneShot(sfx, 1f);
    }
}
