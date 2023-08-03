using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableByTrunk : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<KickScript>())
        {
            _animator.SetTrigger("Death");
            collision.gameObject.SetActive(false);
        }
    }
}
