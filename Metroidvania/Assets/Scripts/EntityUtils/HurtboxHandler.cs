using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtboxHandler : MonoBehaviour
{
    [SerializeField] private bool hasBlood;
    [SerializeField] private UnityEvent action;
    private void OnTriggerEnter2D(Collider2D other) {

        if (hasBlood) {
            VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Bloodspark0, transform.position, (int)(-1 * other.transform.lossyScale.x));
        }
        action.Invoke();
    }
}

