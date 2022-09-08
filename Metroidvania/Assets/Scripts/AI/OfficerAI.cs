using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerAI : AI
{
    [SerializeField] private float fireRange;
    [SerializeField] private float fireRate;
    [SerializeField] private float paceDistance;
    [SerializeField] private float pacePauseTime;

    private bool aggro = false;
    private float fireTimestamp;
    private float paceX;
    private float pauseTimestamp;
    private bool paused = false;
    private int paceDir = 1;

    void Start()
    {
        paceX = transform.position.x;
    }

    protected override void AIUpdate()
    {
        if (!aggro)
        {
            if (paused)
            {
                if (Time.time - pauseTimestamp >= pacePauseTime)
                {
                    paused = false;
                    paceX = transform.position.x;
                    paceDir *= -1;
                }
            }
            else
            {
                if (Mathf.Abs(paceX - transform.position.x) < paceDistance)
                {
                    AIMove(paceDir);
                }
                else
                {
                    pauseTimestamp = Time.time;
                    paused = true;
                }
            }
        }
        else
        {

        }
    }

    private void OfficerAttack() {
        float dist = 4f;
        dist *= facing;
        VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Muzzleflash_1, transform.position + new Vector3(dist, 1f, 0), facing);
        
        CombatHandler.Instance.PlayOneShotProjectile(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 1f, 0),
        facing, facing * 125 * Vector2.right, isPlayerOwned: true, destroyOnImpact: true, damage: 1);
    }
}