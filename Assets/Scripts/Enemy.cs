using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    private float _hp;
    private Renderer _color;
    private float _red;
    private float _green;

    private void Awake()
    {
        _hp = 100f;
        _green = _hp;
        _red = 0;
        _color = GetComponent<Renderer>();
        _color.material.color = new Color(_red,_green,0);
    }

    public void GetDamage(float damageValue)
    {
        _hp -= damageValue;
        _red += damageValue;
        _green -= _hp;
        if(_hp <= 0) { Destroy(gameObject); }
        
        _color.material.color = new Color(_red * 0.01f, _green * 0.01f, 0);
    }
}
