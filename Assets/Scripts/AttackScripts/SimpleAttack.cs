using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttack : MonoBehaviour
{
    [Header("Параметры Атаки")]
    [SerializeField] float _radiusOfAreaAttack;
    [SerializeField] float _damage;
    [SerializeField] private float _coldownTime;

    private bool _isInCooldown = false;

    public void Attack()
    {
        if(!_isInCooldown)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radiusOfAreaAttack, transform.forward, _radiusOfAreaAttack, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal
                        );
            foreach (var hit in hits)
            {
                if (hit.collider?.GetComponent<IDamagable>() != null)
                {
                    hit.collider.GetComponent<IDamagable>().GetDamage(_damage);
                }
            }
            _isInCooldown = true;
            StartCoroutine(Cooldown(_coldownTime));
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

        Gizmos.DrawWireSphere(transform.position + transform.forward * _radiusOfAreaAttack, _radiusOfAreaAttack);
    }

}
