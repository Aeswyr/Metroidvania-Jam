using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : Singleton<VFXHandler>
{

        [SerializeField] private GameObject screenWipePrefab;
        private GameObject screenWipe;
        [SerializeField] private AnimationClip[] oneShotParticles;
        [SerializeField] private GameObject genericOneShotPrefab;

        public void StartScreenWipe() {
            screenWipe = Instantiate(screenWipePrefab, Camera.main.transform);
            screenWipe.transform.position += new Vector3(0, 0, 10);
            screenWipe.GetComponent<Animator>().SetTrigger("StartWipe");
        }

        public void EndScreenWipe() {
            screenWipe.GetComponent<Animator>().SetTrigger("EndWipe");
            StartCoroutine(DelayDestroyWipe());
        }

        private IEnumerator DelayDestroyWipe() {
            yield return new WaitForSeconds(0.5f);
            Destroy(screenWipe);
            screenWipe = null;
        }

        public void PlayOneShotParticle(ParticleType type, Vector3 position, int facing = 1) {
            GameObject particle = Instantiate(genericOneShotPrefab, position, Quaternion.identity);
            particle.transform.localScale = new Vector3(facing, 1, 1);
            Animator animator = particle.GetComponent<Animator>();
            AnimatorOverrideController animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverride;
            animatorOverride["one_shot"] = oneShotParticles[(int)type];

            particle.GetComponent<DestroyAfterDelay>().lifetime = oneShotParticles[(int)type].length;
        }

        public enum ParticleType {
            Detective_Leave, Anarchist_Leave, Boxer_Leave, Mobster_Leave, Antiquarian_Leave, Muzzleflash_1, Detective_Special
        }
}
