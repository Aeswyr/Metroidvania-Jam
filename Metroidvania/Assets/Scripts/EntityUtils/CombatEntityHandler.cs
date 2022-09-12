using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntityHandler : MonoBehaviour
{
    [SerializeField] private int MaxHealth;
    private int health;
    private bool immune = false;
    private bool damaged = false;

    private bool oldDamaged;

    private int damageDelayed = 0;


    void Awake()
    {
        health = MaxHealth;
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (oldDamaged && damaged)
        {
            damaged = false;
            if (!immune)
            {
                health -= damageDelayed;
                if (health <= 0)
                {
                    Kill();
                }
            }
        }
        oldDamaged = damaged;
    }

    public void Damage(int amt) {
        damaged = true;
        damageDelayed = amt;
    }

    public void Kill()
    {
        // TODO: death animations
        gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return health;
    }

    public bool CheckDamaged()
    {
        return damaged;
    }

    public void SetImmune(bool immune)
    {
        this.immune = immune;
    }
}
