using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour, ICanAttack
{
    [Header("Параметры Атаки")]
    [SerializeField] private float radiusOfAreaAttack;
    [SerializeField] private float distance;
    [SerializeField] private float damage;
    [SerializeField] private float coldownTime;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject _newBull;
    private bool _isInCooldown = false;
    private SimpleAttack _simpleAttack;
    private Animator _animator;

    private void Awake()
    {
        _simpleAttack = GetComponent<SimpleAttack>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void Attack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Melee:
                _simpleAttack.Attack(AttackType.Melee);
                break;
            case AttackType.Range:
                DistantAttack();
                break;
        }
        
        
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        _isInCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position + transform.forward * distance, radiusOfAreaAttack);
    }

    private void DistantAttack()
    {
        if (!_isInCooldown)
        {
            RaycastHit hit;
            Physics.SphereCast(transform.position, radiusOfAreaAttack, transform.forward, out hit, distance, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
            if (hit.collider?.GetComponent<IDamagable>() != null)
            {
                _animator.SetTrigger("Range");
                _newBull = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);
                _newBull.GetComponent<MoveToTarget>().SetTargetAndDamage(hit.collider.gameObject, damage);
                _isInCooldown = true;
                StartCoroutine(Cooldown(coldownTime));
            }
        }
    }
}
