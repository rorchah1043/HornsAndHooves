using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private float lookTargetVerticalOffset;
    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float sensitivity;

    [SerializeField] private float maxCameraSpeed;

    private GameObject _lookTarget;
    private Quaternion _rotation;

    private void Start()
    {
        _rotation = _lookTarget.transform.rotation;
    }

    private void Update()
    {
        transform.rotation = _rotation;
        transform.position = Vector3.MoveTowards(transform.position,
            _lookTarget.transform.position + Vector3.up * lookTargetVerticalOffset -
            _rotation * Vector3.forward * maxDistanceFromPlayer, maxCameraSpeed * Time.deltaTime);
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

    public void SetLookTarget(GameObject lookTarget)
    {
        _lookTarget = lookTarget;
    }
}