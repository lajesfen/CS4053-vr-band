using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SimpleAudioPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    public float fadeDuration = 1.0f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlaySound()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            audioSource.volume = 1.0f;
        }

        // if (audioClip != null)
        // {
        //     audioSource.clip = audioClip;
        //     audioSource.volume = 1.0f;
        //     audioSource.Play();
        // }
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip, 1.0f); // Volume = 1.0
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned to SimpleAudioPlayer.");
        }
    }

    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOut(audioSource, fadeDuration));
        }
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();
        source.volume = 1.0f;
    }
}
