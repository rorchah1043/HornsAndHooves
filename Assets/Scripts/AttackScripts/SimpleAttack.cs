using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttack : MonoBehaviour, ICanAttack
{
    [Header("Параметры ближней Атаки")]
    [SerializeField] float radiusOfAreaAttack;
    [SerializeField] float damage;
    [SerializeField] private float coldownTime;

    private bool _isInCooldown = false;

    public void Attack(AttackType type)
    {
        switch(type)
        {
            case AttackType.Milli:
                MilliAttack();
                break;
            case AttackType.Range:
                break;
        }  
    }

    private void MilliAttack()
    {
        if (!_isInCooldown)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radiusOfAreaAttack, transform.forward, radiusOfAreaAttack, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal
                        );
            foreach (var hit in hits)
            {
                if (hit.collider?.GetComponent<IDamagable>() != null)
                {
                    hit.collider.GetComponent<IDamagable>().GetDamage(damage);
                }
            }
            _isInCooldown = true;
            StartCoroutine(Cooldown(coldownTime));
        }
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        _isInCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position + transform.forward * radiusOfAreaAttack, radiusOfAreaAttack);
    }

}
