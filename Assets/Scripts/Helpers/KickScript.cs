using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickScript : MonoBehaviour, IInteractable
{
    [SerializeField] private float _damage;
    private Outline _outline;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 0;
    }

    public void InteractableAction(Vector3 vector)
    {
        Vector3 dir = transform.position - vector;
        _rigidbody.AddForce(dir * 10, ForceMode.Impulse);
    }

    public void OnHover()
    {
        _outline.OutlineWidth = 2;
    }

    public void OnHoverExit()
    {
        _outline.OutlineWidth = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider?.GetComponent<IDamagable>() != null)
        {
            collision.collider.GetComponent<IDamagable>().GetDamage(_damage);
        }
    }
}
