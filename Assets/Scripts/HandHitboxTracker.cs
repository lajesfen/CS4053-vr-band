using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHitboxTracker : MonoBehaviour
{
    [Header("Hand GameObjects")]
    [SerializeField] private GameObject virtualHand;
    [SerializeField] private GameObject controller;

    [SerializeField] private Rigidbody cubeRigidbody;
    BoxCollider cubeCollider;

    void Start()
    {
        cubeCollider = cubeRigidbody.gameObject.GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        Transform handTransform = controller.activeSelf ? controller.transform : virtualHand.transform;
        if (handTransform == null)
        {
            cubeCollider.enabled = false;
            return;
        }
        cubeCollider.enabled = true;

        // handle controller transform
        if (controller.activeSelf)
        {
            Vector3 positioned = handTransform.position + handTransform.up * 0.1f + Vector3.down * 0.1f + Vector3.up * 0.03f;
            Quaternion rotated = handTransform.rotation * Quaternion.Euler(0, 90, -45);
            cubeRigidbody.MovePosition(positioned);
            cubeRigidbody.MoveRotation(rotated);
        }
        // handle virtual hand transform
        else
        {
            Vector3 positioned = handTransform.position + handTransform.forward * 0.1f;
            Quaternion rotated = handTransform.rotation * Quaternion.Euler(0, 90, 0);
            cubeRigidbody.MovePosition(positioned);
            cubeRigidbody.MoveRotation(rotated);
        }

    }
}
