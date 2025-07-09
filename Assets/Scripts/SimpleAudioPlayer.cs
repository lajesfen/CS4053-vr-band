using Unity.Netcode;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SimpleAudioPlayer : NetworkBehaviour
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

    // Call this locally or from other scripts
    public void PlaySound()
    {
        if (IsServer)
        {
            PlaySoundClientRpc();
        }
        else if (IsClient)
        {
            PlaySoundServerRpc();
        }
    }

    public void StopSound()
    {
        if (IsServer)
        {
            StopSoundClientRpc();
        }
        else if (IsClient)
        {
            StopSoundServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void PlaySoundServerRpc()
    {
        PlaySoundClientRpc();
    }

    [ClientRpc]
    void PlaySoundClientRpc()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            audioSource.volume = 1.0f;
        }

        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip, 1.0f);
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned to SimpleAudioPlayer.");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void StopSoundServerRpc()
    {
        StopSoundClientRpc();
    }

    [ClientRpc]
    void StopSoundClientRpc()
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
