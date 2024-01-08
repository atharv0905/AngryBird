using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AngryBirdHandler : MonoBehaviour
{
    [SerializeField] private GameObject angryBirdPrefab;
    [SerializeField] private Rigidbody2D pivot;

    [SerializeField] private float DetachDelay;
    [SerializeField] private float RespawnDelay;

    private Rigidbody2D currentAngryBirdRigidbody;
    private SpringJoint2D currentAngryBirdSpringJoint;
    private bool isDraging;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        RespawnAngryBird();
        Debug.Log("Game started");
    }


    void Update()
    {
        if(currentAngryBirdRigidbody == null)
        {
            return;
        }
        if (!Touchscreen.current.press.isPressed)
        {
            if(isDraging)
            {
                LaunchAngryBird();
            }
            isDraging = false;
            return;
        }

        isDraging = true;
        currentAngryBirdRigidbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentAngryBirdRigidbody.position = worldPosition;
    }

    private void LaunchAngryBird()
    {
        currentAngryBirdRigidbody.isKinematic = false;
        currentAngryBirdRigidbody = null;

        Invoke(nameof(DetachAngryBird), DetachDelay);
    }

    private void DetachAngryBird()
    {
        currentAngryBirdSpringJoint.enabled = false;
        currentAngryBirdSpringJoint = null;

        Invoke(nameof(RespawnAngryBird), RespawnDelay);
    }

    private void RespawnAngryBird()
    {
        GameObject angryBirdInstance = Instantiate(angryBirdPrefab, pivot.position, Quaternion.identity);

        currentAngryBirdRigidbody = angryBirdInstance.GetComponent<Rigidbody2D>();
        currentAngryBirdSpringJoint = angryBirdInstance.GetComponent<SpringJoint2D>();

        currentAngryBirdSpringJoint.connectedBody = pivot;
    }
}
