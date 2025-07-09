using Unity.Netcode;
using UnityEngine;

public class CajonManager : NetworkBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] cajonSnareClips;
    [SerializeField] private AudioClip[] cajonBassClips;

    [Header("Settings")]
    [SerializeField] private float snareZoneY = 0.3f;         // Y threshold for snare vs bass
    [SerializeField] private float minVolume = 0.3f;
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float maxForce = 5f;

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Stick"))
            return;

        float force = col.relativeVelocity.magnitude;
        Vector3 hitPoint = col.contacts[0].point;
        float height = transform.InverseTransformPoint(hitPoint).y;

        // Call server RPC to notify hit with height and force
        PlayHitServerRpc(height, force);
    }

    [ServerRpc(RequireOwnership = false)]
    void PlayHitServerRpc(float height, float force)
    {
        // Tell all clients to play the sound with parameters
        PlayHitClientRpc(height, force);
    }

    [ClientRpc]
    void PlayHitClientRpc(float height, float force)
    {
        if (height > snareZoneY)
        {
            PlaySnare(force);
        }
        else
        {
            PlayBass(force);
        }
    }

    void PlaySnare(float force)
    {
        if (cajonSnareClips.Length == 0) return;

        int randomIndex = Random.Range(0, cajonSnareClips.Length);
        AudioClip clip = cajonSnareClips[randomIndex];
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.Clamp01(force / maxForce));

        audioSource.pitch = Random.Range(0.95f, 1.05f); // Slight pitch variety
        audioSource.PlayOneShot(clip, volume);
    }

    void PlayBass(float force)
    {
        if (cajonBassClips.Length == 0) return;

        int randomIndex = Random.Range(0, cajonBassClips.Length);
        AudioClip clip = cajonBassClips[randomIndex];
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.Clamp01(force / maxForce));

        audioSource.pitch = Random.Range(0.95f, 1.05f); // Slight pitch variety
        audioSource.PlayOneShot(clip, volume);
    }
}
