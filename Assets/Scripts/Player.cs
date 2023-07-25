using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    
    [SerializeField] private float maxSpeed;
    [SerializeField] private float force;
    [SerializeField] private float maxTurnSpeed;

    private Vector3 _speed;
    private Vector3 _force;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxLinearVelocity = maxSpeed;
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0) * _force);
    }

    private void Update()
    {
        if (Math.Abs(_rigidbody.velocity.x) > 0.05f || Math.Abs(_rigidbody.velocity.z) > 0.05f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(_rigidbody.velocity), maxTurnSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _force = new Vector3(value.x, 0,value.y) * force;
    }
}