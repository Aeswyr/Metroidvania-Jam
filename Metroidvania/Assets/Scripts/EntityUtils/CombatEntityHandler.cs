using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntityHandler : MonoBehaviour
{
    [SerializeField] private int MaxHealth;
    private int health;

    public void Damage(int amt) {
        health -= amt;
    }
}
