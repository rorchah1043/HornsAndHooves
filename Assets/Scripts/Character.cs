using System;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private Vector3 _boxColiderSize;

    private Rigidbody _rigidbody;
    private NavMeshAgent _navMeshAgent;
    private BoxCollider _interactZone;

    private Vector3 _force;
    private Quaternion _targetRotation;

    private void Awake()
    {
        _interactZone = GetComponent<BoxCollider>();
        _boxColiderSize = _interactZone.size;
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_rigidbody == null)
        {
            Debug.LogError($"{name} должен содержать Rigidbody");
        }

        if (_navMeshAgent == null)
        {
            Debug.LogError($"{name} должен содержать NavMeshAgent");
        }

        _rigidbody.maxLinearVelocity = _navMeshAgent.speed;
    }

    private void Update()
    {
        if (Math.Abs(_rigidbody.velocity.x) > 0.05f || Math.Abs(_rigidbody.velocity.z) > 0.05f)
        {
            var lookVector = _rigidbody.velocity;
            lookVector.y = 0;
            _targetRotation = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                _targetRotation, _navMeshAgent.angularSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_force);
    }

    public void SetPlayerControlled(bool isPlayerControlled)
    {
        enabled = isPlayerControlled;
        _interactZone.enabled = isPlayerControlled;
        _rigidbody.isKinematic = !isPlayerControlled;
        _navMeshAgent.enabled = !isPlayerControlled;

        if (isPlayerControlled)
        {
            _targetRotation = transform.rotation;
        }
    }

    public void SetPlayerMoveDirection(Vector3 direction)
    {
        _force = direction * _navMeshAgent.acceleration;
    }

    public void SetAIDestination(Vector3 destination)
    {
        _navMeshAgent.destination = destination;
    }

    public Quaternion GetTargetRotation()
    {
        return _targetRotation;
    }

    private void CheckColiderStatus(bool value)
    {
        if(value)
        {
            _interactZone.size = _boxColiderSize;
        }
        else
        {
            _boxColiderSize = _interactZone.size;
            _interactZone.size = Vector3.zero;
        }
    }
}