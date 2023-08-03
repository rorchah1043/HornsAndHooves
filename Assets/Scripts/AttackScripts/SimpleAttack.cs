using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SimpleAttack : MonoBehaviour, ICanAttack
{
    [Header("Параметры Атаки")]
    [SerializeField] float radiusOfAreaAttack;
    [SerializeField] float damage;
    [SerializeField] private float coldownTime;

    private bool _isInCooldown = false;

    [SerializeField] private Animator animator;
    [SerializeField] private Character character;

    public void Attack()
    {
        if(!_isInCooldown)
        {
            if(character.enabled) 
            {
                //int number = Random.Range(0, 4);
                animator.SetInteger("MeleeNumber", 0);
                animator.SetTrigger("Melee");                
            }
            
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radiusOfAreaAttack, transform.forward, radiusOfAreaAttack, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
            foreach (var hit in hits)
            {
                if (hit.collider?.GetComponent<IDamagable>() != null)
                {
                    StartCoroutine(WaitForSlashAttack(hit));
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
        animator.ResetTrigger("Melee");
    }

    private IEnumerator WaitForSlashAttack(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.4f);
        hit.collider.GetComponent<IDamagable>().GetDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * radiusOfAreaAttack, radiusOfAreaAttack);
    }

}
