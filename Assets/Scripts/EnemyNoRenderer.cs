using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNoRenderer : MonoBehaviour, IDamagable
{
    [SerializeField] private float preHitWait = 0.2f;
    private float _hp;
    private Animator _animator;
    private bool _isPlaying = false;


    private void Awake()
    {
        _hp = 100f;
        _animator = GetComponent<Animator>();
    }

    public void GetDamage(float damageValue)
    {
        if (!_isPlaying)
        {
            StartCoroutine(GetHit(damageValue));

        }
        if (_hp <= 0) {
            _animator.SetBool("IsDead", true);
            StartCoroutine(WaitForDeath());
        }

    }

    private IEnumerator GetHit(float damageValue)
    {
        yield return new WaitForSeconds(preHitWait);
        _isPlaying = true;
        _animator.SetTrigger("GotHit");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        _hp -= damageValue;
        _isPlaying = false;
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        Destroy(this);
    }
}
