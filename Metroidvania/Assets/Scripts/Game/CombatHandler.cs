using UnityEngine;

public class CombatHandler : Singleton<CombatHandler> {
    [SerializeField] private AnimationClip[] oneShotProjectiles;
    [SerializeField] private GameObject genericOneShotPrefab;

        public void PlayOneShotProjectile(ProjectileType type, Vector3 position, int facing = 1, Vector2 velocity = default,
        Vector2 size = default, Transform parent = null, float duration = default, bool isPlayerOwned = false, bool destroyOnImpact = false,
        bool shouldHitpause = false, int damage = 0) {
            GameObject particle = Instantiate(genericOneShotPrefab, position, Quaternion.identity);
            particle.transform.localScale = new Vector3(facing, 1, 1);

            Animator animator = particle.GetComponent<Animator>();
            AnimatorOverrideController animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverride;
            animatorOverride["one_shot"] = oneShotProjectiles[(int)type];

            if (isPlayerOwned)
                particle.layer = LayerMask.NameToLayer("PlayerHitbox");

            if (parent != null) {
                particle.transform.SetParent(parent);
                if (shouldHitpause)
                    particle.AddComponent<HitpauseOnImpact>().Init(parent.GetComponent<AnimatorExtender>(), parent.GetComponent<MovementHandler>(), parent.GetComponent<JumpHandler>());

            }

            if (size != default)
                particle.GetComponent<BoxCollider2D>().size = size;

            if (velocity != default)
                particle.AddComponent<Rigidbody2D>().velocity = velocity;

            if (duration != default)
                particle.AddComponent<DestroyAfterDelay>().lifetime = duration;

            if (destroyOnImpact)
                particle.AddComponent<DestroyOnImpact>();
            
            if (damage != 0) 
                particle.AddComponent<DamageOnImpact>().damage = damage;;

        }

        public enum ProjectileType {
            Bullet
        }
}