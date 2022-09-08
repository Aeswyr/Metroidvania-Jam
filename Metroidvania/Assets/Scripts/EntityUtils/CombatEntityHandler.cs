using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntityHandler : MonoBehaviour
{
    [SerializeField] private int MaxHealth;
    private int health;

    void Start()
    {
        health = MaxHealth;
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
}
