using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class XylophoneKey : NetworkBehaviour
{
    public AudioClip keySound;
    public float bounceAmount = 0.005f;
    public float bounceSpeed = 10f;

    private AudioSource audioSource;
    private Vector3 originalPosition;
    private bool isBouncing = false;
    private Rigidbody rb;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.localPosition;
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Stick"))
            return;

        if (collision.collider is not CapsuleCollider)
            return;

        if (IsServer || IsClient)
        {
            PlaySoundServerRpc();
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
        PlaySound(1f);
        if (!isBouncing)
            StartCoroutine(BounceEffect());
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
