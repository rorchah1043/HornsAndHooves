using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [Header("Параметры Атаки")]
    [SerializeField] private float _radiusOfAreaAttack;
    [SerializeField] private float _distance;
    [SerializeField] private float _damage;
    [SerializeField] private float _coldownTime;
    [SerializeField] private GameObject _bulletPrefab;

    private GameObject _newBull;
    private bool _isInCooldown = false;

    public void Attack()
    {
        if(!_isInCooldown)
        {
            RaycastHit hit;
            Physics.SphereCast(transform.position, _radiusOfAreaAttack, transform.forward, out hit, _distance, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
            if (hit.collider?.GetComponent<IDamagable>() != null)
            {
                _newBull = Instantiate(_bulletPrefab, transform.position + transform.forward, Quaternion.identity);
                _newBull.GetComponent<MoveToTarget>().SetTargetAndDamage(hit.collider.gameObject, _damage);
                _isInCooldown = true;
                StartCoroutine(Cooldown(_coldownTime));
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

        Gizmos.DrawWireSphere(transform.position + transform.forward * _distance, _radiusOfAreaAttack);
    }
}
