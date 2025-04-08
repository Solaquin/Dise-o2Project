using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour
{
    public static SoundsController Instance;

    private AudioSource audioSource;

    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip hoverClick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();

    }


    public void EjecutarSonido(AudioClip Sonido)
    {
        audioSource.PlayOneShot(Sonido);
    }

}