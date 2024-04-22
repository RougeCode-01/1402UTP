using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-- Audio Sources --")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [Header("--Audio Clips--")]
    [SerializeField] public AudioClip bgm; public AudioClip die; public AudioClip land; public AudioClip dash; public AudioClip checkpoint; public AudioClip portal; public AudioClip shoot; public AudioClip coin; public AudioClip mainmenu;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        bgmSource.clip = bgm;
        bgmSource.Play();

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }    
}
