using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KickScript : MonoBehaviour, IInteractable
{
    [SerializeField] private float damage;
    private Outline _outline;
    private Rigidbody _rigidbody;

    private bool _isWaitingForKick = false;

    [SerializeField] private Animator animator;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 0;
    }

    public void InteractableAction(GameObject gameObject)
    {
        if(!_isWaitingForKick)StartCoroutine(WaitForKick(gameObject));
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
            collision.collider.GetComponent<IDamagable>().GetDamage(damage);
        }
    }

    private IEnumerator WaitForKick(GameObject gameObject)
    {
        _isWaitingForKick = true;
        Animator animator = gameObject?.GetComponentInChildren<Animator>();
        //int number = Random.Range(0, 2);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
        {
            animator.SetInteger("KickNumber", 1);
            animator.SetTrigger("Kick");
        }

        yield return new WaitForSeconds(0.11f);
        _isWaitingForKick = false;
        //Vector3 dir = transform.position - gameObject.transform.position;
        Vector3 dir = -transform.right;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(dir * 50, ForceMode.Impulse);
    }
}
