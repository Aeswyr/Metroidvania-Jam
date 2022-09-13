using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChaseAndShootBehaviour : AIBehaviour
{
    [SerializeField] private float shootRange;
    [SerializeField] private float shootDelay;
    [SerializeField] private float windupTime;
    [SerializeField] private float attackRecovery = 0.34f;
    [SerializeField] private float maxYOffset;
    [SerializeField] private Vector2 rightFootOffset;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private bool meleeAttack;
    [SerializeField] private UnityEvent onWindup;
    [SerializeField] private UnityEvent onAttack;

    private float aheadDistance = 1f;
    private float floorDetectDistance = 1f;
    private PlayerHandler player;
    private float shootTimestamp = -100;
    private float windupTimestamp = -100;
    private bool windup = false;
    
    

    protected override void AIAwake()
    {
        player = GameHandler.Instance.GetPlayer();
    }

    protected override void AIUpdate()
    {
        if (windup)
        {
            if (Time.time - windupTimestamp >= windupTime)
            {
                windup = false;
                if (meleeAttack)
                    Melee();
                else
                    Shoot();
                onAttack.Invoke();
                shootTimestamp = Time.time;
            }
            return;
        }

        if (!player.gameObject.activeSelf)
            return;
        float dis = player.transform.position.x - transform.position.x;
        if (Mathf.Abs(dis) > shootRange && Time.time - shootTimestamp - shootDelay >= 0)
        {
            RaycastHit2D ground = Physics2D.Raycast(transform.position + new Vector3((rightFootOffset.x+aheadDistance)  * Mathf.Sign(dis), rightFootOffset.y, 0), Vector2.down, floorDetectDistance, floorMask.value);
            if (ground != null)
            {
                AIMove((int)Mathf.Sign(dis));
            }
        }
        else if (Mathf.Abs(player.transform.position.y - transform.position.y) <= maxYOffset)
        {
            if (Mathf.Abs(dis) <= shootRange)
            {
                if ((Mathf.Sign(dis) == 1 && sprite.flipX) || (Mathf.Sign(dis) == -1 && !sprite.flipX))
                {
                    AIMove((int)Mathf.Sign(dis));
                }
                else if (Time.time - shootTimestamp >= shootDelay)
                {
                    Windup();
                    //shootTimestamp = Time.time;
                }
            }
        }
    }

    private void Windup()
    {
        windup = true;
        windupTimestamp = Time.time;
        animator.SetTrigger("windup");
        onWindup.Invoke();
    }

    private void Shoot()
    {
        float dist = 3.5f;
        dist *= facing;
        /*
        if (sprite.flipX)
        {
            VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Muzzleflash_1, transform.position + new Vector3(-dist, 1f, 0), facing);
            CombatHandler.Instance.PlayOneShotProjectile(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(-dist, 1f, 0),
            facing, facing * 125 * Vector2.right, destroyOnImpact: true, damage: 1);
        }
        else
        {
            */
            VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Muzzleflash_1, transform.position + new Vector3(dist, 1f, 0), facing);
            CombatHandler.Instance.PlayOneShotProjectile(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 1f, 0),
            facing, facing * 125 * Vector2.right, destroyOnImpact: true, damage: 1);
        //}
        animator.SetTrigger("attack");
    }

    private void Melee()
    {
        float dist = 3f;
        dist *= facing;
        CombatHandler.Instance.PlayOneShotProjectile(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 0, 0)
        , facing, size: new Vector2(4, 4), parent: transform, duration: 0.125f, destroyOnImpact: true, shouldHitpause: true, damage: 1);
        animator.SetTrigger("attack");
    }
}