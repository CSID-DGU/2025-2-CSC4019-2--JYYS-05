using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip[] clips;


    public void DetectedPerson()
    {
        audioSource.clip = clips[0];
        audioSource.Play();
    }

    public void SelectButtonSound()
    {
        audioSource.clip = clips[1];
        audioSource.Play();
    }

    public void PayCompleteSound()
    {
        audioSource.clip = clips[2];
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
