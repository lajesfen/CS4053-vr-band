using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumstickSound : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] drumstickClips;
    [SerializeField] private float volume;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Stick")) return;
        PlaySound();
    }


    void PlaySound()
    {
        if (drumstickClips.Length == 0) return;

        int randomIndex = Random.Range(0, drumstickClips.Length);
        AudioClip clip = drumstickClips[randomIndex];
        audioSource.PlayOneShot(clip, volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
