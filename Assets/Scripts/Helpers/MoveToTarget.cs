using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] float _speed;

    private GameObject _target;
    private float _damage;

    public void SetTargetAndDamage(GameObject target, float damage)
    {
        _target = target;
        _damage = damage;
    }

    private void FixedUpdate()
    {
        if(_target != null)
        {
            Vector3 dir = _target.transform.position - transform.position;
            transform.position += (dir.normalized * Time.deltaTime * _speed);
            transform.up = dir.normalized;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other?.GetComponent<IDamagable>() != null)
        {
            other.GetComponent<IDamagable>().GetDamage(_damage);
            Destroy(gameObject);
        }
    }
}
