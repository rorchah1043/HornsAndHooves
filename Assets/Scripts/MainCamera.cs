using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject lookTarget;
    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float sensitivity;

    private Quaternion _rotation;

    private void Start()
    {
        _rotation = lookTarget.transform.rotation;
    }

    private void Update()
    {
        transform.rotation = _rotation;
        transform.position = lookTarget.transform.position - _rotation * Vector3.forward * maxDistanceFromPlayer;
    }

    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();

        var pitch = _rotation.eulerAngles.x - value.y * sensitivity;
        if (pitch > 90 && _rotation.eulerAngles.x <= 90)
        {
            pitch = 90;
        }

        if (pitch < 270 && _rotation.eulerAngles.x >= 270)
        {
            pitch = 270;
        }

        _rotation = Quaternion.Euler(pitch, _rotation.eulerAngles.y + value.x * sensitivity, 0);
    }
}