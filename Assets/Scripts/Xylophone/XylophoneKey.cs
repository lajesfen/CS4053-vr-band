using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class XylophoneKey : MonoBehaviour
{
    public AudioClip keySound;
    public float bounceAmount = 0.005f;
    public float bounceSpeed = 10f;

    [Header("Dynamic Volume")]
    public float minForce = 0.2f;   // below this = no sound
    public float maxForce = 3f;     // anything above this = full volume
    public float maxVolume = 1f;

    private AudioSource audioSource;
    private Vector3 originalPosition;
    private bool isBouncing = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.localPosition;
        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Stick")) return;

        Collider col = collision.collider;
        if (col is not CapsuleCollider) return; // quick hack

        Debug.Log($"Hit by: {collision.gameObject.name}");

        if (!isBouncing)
        {
            PlaySound(1f);
            StartCoroutine(BounceEffect());
        }
    }

    void PlaySound(float volume)
    {
        if (keySound != null)
        {
            audioSource.PlayOneShot(keySound, volume);
        }
    }

    System.Collections.IEnumerator BounceEffect()
    {
        isBouncing = true;
        Vector3 downPos = originalPosition - transform.up * bounceAmount;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * bounceSpeed;
            transform.localPosition = Vector3.Lerp(originalPosition, downPos, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * bounceSpeed;
            transform.localPosition = Vector3.Lerp(downPos, originalPosition, t);
            yield return null;
        }

        transform.localPosition = originalPosition;
        isBouncing = false;
    }
}
