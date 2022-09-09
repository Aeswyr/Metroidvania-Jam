using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFXOnImpact : MonoBehaviour
{
    [SerializeField] private VFXHandler.ParticleType[] particles;
    [SerializeField] private Collider2D col;
   
    private void OnTriggerEnter2D(Collider2D other) {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        filter.useTriggers = true;
        RaycastHit2D[] cols = new RaycastHit2D[10];
        col.Cast(Vector2.right, filter,  cols, 0, true);

        Vector2 point = cols[0].point;
        if (point == Vector2.zero)
            point = other.transform.position;

        foreach (var particle in particles) {
            VFXHandler.Instance.PlayOneShotParticle(particle, point, (int)(-1 * transform.localScale.x));
        }
    }
}
