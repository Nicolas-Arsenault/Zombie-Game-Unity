using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepZombie : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Pas sources")]
    public AudioClip[] sonsPas;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip getPasAleatoire()
    {
        return sonsPas[UnityEngine.Random.Range(0, sonsPas.Length)];
    }


    private void StepZombie()
    {
        AudioClip clip = getPasAleatoire();
        audioSource.PlayOneShot(clip);
    }
}
