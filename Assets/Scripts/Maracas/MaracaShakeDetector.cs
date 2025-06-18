using UnityEngine;

public class MaracaShakeDetector : MonoBehaviour
{
    [SerializeField] private float sensitivity = 10f;  // Adjust for responsiveness
    [SerializeField] private float maxShake = 100f;    // Cap value for scaling

    public float ShakeValue { get; private set; } // Final output (0–100)

    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    
    void Start()
    {
        lastVelocity = Vector3.zero;
    }

    void Update()
    {
        // Approximate velocity via position difference
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;

        // Approximate acceleration = delta of velocity
        Vector3 deltaV = velocity - lastVelocity;
        float intensity = deltaV.magnitude * sensitivity;

        // Scale and clamp
        ShakeValue = Mathf.Clamp(intensity, 0f, maxShake);

        // Save for next frame
        lastVelocity = velocity;
        lastPosition = transform.position;
    }

}
