using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntityHandler : MonoBehaviour
{
    [SerializeField] private int MaxHealth;
    private int health;

    void Awake()
    {
        health = MaxHealth;
    }

    void Start()
    {
        
    }

    public void Damage(int amt) {
        health -= amt;
        if (health <= 0)
        {
            Kill();
        }
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
}
