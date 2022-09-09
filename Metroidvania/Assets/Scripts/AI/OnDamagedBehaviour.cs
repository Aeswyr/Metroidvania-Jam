using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CombatEntityHandler))]
public class OnDamagedBehaviour : AIBehaviour
{
    [SerializeField] private UnityEvent onDamaged;
    private CombatEntityHandler entity;
    private int oldHealth;

    protected override void AIAwake()
    {
        entity = GetComponent<CombatEntityHandler>();
        oldHealth = entity.GetHealth();
    }

    void OnEnable()
    {
        oldHealth = entity.GetHealth();
    }

    protected override void AIUpdate()
    {
        int health = entity.GetHealth();
        if (health != oldHealth)
        {
            enabled = false;
            onDamaged.Invoke();
            return;
        }
        oldHealth = health;
    }
}