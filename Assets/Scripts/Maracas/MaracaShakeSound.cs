using UnityEngine;
using System.Collections;

public class MaracaShakeSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] maracaClips;
    [SerializeField] private float shakeThreshold = 25f;
    [SerializeField] private float stopThreshold = 1f;       // Below this, we stop sound
    [SerializeField] private float cooldown = 0.05f;
    [SerializeField] private float fadeOutDuration = 0.3f; // Time to fade out
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    [SerializeField] private Vector2 volumeRange = new Vector2(0.8f, 1f);

    private float lastShakeValue;
    private float lastPlayTime;
    private float originalVolume;
    private Coroutine fadeOutCoroutine;

    private MaracaShakeDetector shakeDetector;

    void Start()
    {
        shakeDetector = GetComponent<MaracaShakeDetector>();
        lastPlayTime = -cooldown;
        originalVolume = audioSource.volume;
    }

    void Update()
    {
        float currentShake = shakeDetector.ShakeValue;

        // Shake spike: play sound
        if (currentShake > shakeThreshold && Time.time - lastPlayTime >= cooldown)
        {
            PlayShakeSound();
            lastPlayTime = Time.time;
        }

        // Shake drop: fade out
        if (lastShakeValue > stopThreshold && currentShake < stopThreshold)
        {
            if (audioSource.isPlaying && fadeOutCoroutine == null)
            {
                fadeOutCoroutine = StartCoroutine(FadeOutAndStop());
            }
        }

        lastShakeValue = currentShake;
    }

    void PlayShakeSound()
    {
        if (maracaClips.Length == 0) return;

        // Randomize clip
        int randomIndex = Random.Range(0, maracaClips.Length);
        AudioClip clip = maracaClips[randomIndex];

        // Randomize pitch and volume
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.volume = Random.Range(volumeRange.x, volumeRange.y);

        audioSource.PlayOneShot(clip);
    }

    IEnumerator FadeOutAndStop()
    {
        float startVol = audioSource.volume;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0f, t / fadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = originalVolume; // Reset for next time
        fadeOutCoroutine = null;
    }

}
