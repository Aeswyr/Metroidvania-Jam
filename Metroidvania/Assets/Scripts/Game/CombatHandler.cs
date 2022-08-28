using UnityEngine;

public class CombatHandler : Singleton<CombatHandler> {
    [SerializeField] private AnimationClip[] oneShotProjectiles;
    [SerializeField] private GameObject genericOneShotPrefab;

        public void PlayOneShotParticle(ProjectileType type, Vector3 position, int facing = 1, Vector2 velocity = default, Collider2D owner = null) {
            GameObject particle = Instantiate(genericOneShotPrefab, position, Quaternion.identity);
            particle.transform.localScale = new Vector3(facing, 1, 1);
            Animator animator = particle.GetComponent<Animator>();
            AnimatorOverrideController animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverride;
            animatorOverride["one_shot"] = oneShotProjectiles[(int)type];

            particle.GetComponent<Rigidbody2D>().velocity = velocity;
        }

        public enum ProjectileType {
            Bullet
        }
}