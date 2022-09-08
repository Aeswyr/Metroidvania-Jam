using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnImpact : MonoBehaviour
{
    public int damage;
    private CombatEntityHandler entity;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.TryGetComponent(out entity))
        {
            entity.Damage(damage);
        }
    }
}