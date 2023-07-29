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

    private InteractScript _interactObject;
    private SimpleAttack _simpleAttack;
    private RangeAttack _rangeAttack;

    private void Start()
    {
        Cursor.visible = false;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxLinearVelocity = maxSpeed;
        CheckReference();
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

    private void CheckReference()
    {
        if (GetComponent<InteractScript>())
        {
            _interactObject = GetComponent<InteractScript>();
        }
        else
        {
            _interactObject = null;
            Debug.LogError("Проебался InteractScript");
        }

        if (GetComponent<SimpleAttack>())
        {
            _simpleAttack = GetComponent<SimpleAttack>();
        }
        else
        {
            _simpleAttack = null;
            Debug.LogError("Проебался SimpleAttack");
        }

        if (GetComponent<RangeAttack>())
        {
            _rangeAttack = GetComponent<RangeAttack>();
        }
        else
        {
            _rangeAttack = null;
            Debug.LogError("Проебался RangeAttack");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _force = new Vector3(value.x, 0, value.y) * force;
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && _interactObject.GetInteractableObject() != null)
        {
            _interactObject.GetInteractableObject().InteractableAction();
            Debug.Log("Использован");
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        _simpleAttack.Attack();
    }

    public void OnRangeFire(InputAction.CallbackContext context)
    {
        _rangeAttack.Attack();
    }
}