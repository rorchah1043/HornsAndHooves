using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [Header("Параметры Атаки")]
    [SerializeField] private float radiusOfAreaAttack;
    [SerializeField] private float distance;
    [SerializeField] private float damage;
    [SerializeField] private float coldownTime;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject _newBull;
    private bool _isInCooldown = false;

    public void Attack()
    {
        if(!_isInCooldown)
        {
            RaycastHit hit;
            Physics.SphereCast(transform.position, radiusOfAreaAttack, transform.forward, out hit, distance, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
            if (hit.collider?.GetComponent<IDamagable>() != null)
            {
                _newBull = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);
                _newBull.GetComponent<MoveToTarget>().SetTargetAndDamage(hit.collider.gameObject, damage);
                _isInCooldown = true;
                StartCoroutine(Cooldown(coldownTime));
            }
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
}
