using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private JumpHandler jump;
    [SerializeField] private MovementHandler move;
    [SerializeField] private WallSlideHandler wallSlide;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController[] characterOverrides;
    int characterIndex = 0;

    private bool grounded, acting, wallSliding;
    bool inputsDisabled;
    private int facing = 1;

    private List<UnityEvent> possibleInteractions = new List<UnityEvent>();


    // Start is called before the first frame update
    void Start()
    {
        animator.runtimeAnimatorController = characterOverrides[characterIndex];
        interactPrompt.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputsDisabled)
            return;

        bool groundedprev = grounded;
        grounded = ground.CheckGrounded();
        bool wallSlidingPrev = wallSliding;
        wallSliding = wallSlide.IsWallSliding() && !grounded && wallSlide.enabled;
        if (acting && ((grounded && !groundedprev) || (wallSliding && !wallSlidingPrev)))
            EndAction();
        animator.SetBool("grounded", grounded);
        animator.SetBool("hanging", wallSliding);

        if (InputHandler.Instance.move.pressed && !acting)
            move.StartAcceleration(InputHandler.Instance.dir);
        else if (InputHandler.Instance.move.down && !acting) {
            move.UpdateMovement(InputHandler.Instance.dir);
            sprite.flipX = InputHandler.Instance.dir < 0;
            facing = sprite.flipX ? -1 : 1;
            animator.SetBool("moving", true);
        } else if (InputHandler.Instance.move.released && !acting) {
            move.StartDeceleration();
            animator.SetBool("moving", false);
        }

        //Character Swap
        if (InputHandler.Instance.swap.pressed && !acting) {
            int prev = characterIndex;
            characterIndex = (characterIndex + 1) % characterOverrides.Length;
            animator.runtimeAnimatorController = characterOverrides[characterIndex];
            VFXHandler.Instance.PlayOneShotParticle((VFXHandler.ParticleType)prev, transform.position, facing);
        }

        //Jumping
        if (InputHandler.Instance.jump.pressed && !acting && grounded) {
            jump.StartJump();
            animator.SetBool("grounded", false);
            animator.SetTrigger("jump");
        }

        //Special
        if (InputHandler.Instance.special.pressed && !acting) {
            animator.SetTrigger("special");
            StartAction();
        }

        //Attacking
        if (InputHandler.Instance.attack.pressed && !acting) {
            animator.SetTrigger("attack");
            StartAction();
        }

        //Reloading
        if (InputHandler.Instance.reload.pressed && !acting && grounded) {
            animator.SetTrigger("reload");
        }

        //Interacting
        if (InputHandler.Instance.interact.pressed && possibleInteractions.Count > 0) {
            possibleInteractions[0].Invoke();
            DeregisterInteraction(possibleInteractions[0]);
        }
            
        
    }

    public void DisableInputs() {
        move.ResetMovement();
        inputsDisabled = true;
        animator.SetBool("moving", false);
    }
    public void EnableInputs() {
        inputsDisabled = false;
        if (InputHandler.Instance.move.down && !InputHandler.Instance.move.pressed)
            move.StartAcceleration(InputHandler.Instance.dir);
    }
    private void StartAction() {
        if (grounded)
            move.StartDeceleration();
        animator.SetBool("moving", false);
        acting = true;
    }

    private void EndAction() {
        acting = false;
    }

    private void FireAttack() {
        switch(characterIndex)
        {
            case 0:
                DetectiveAttack();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    private void FireSpecial() {
        switch(characterIndex)
        {
            case 0:
                DetectiveSpecial();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    private void FireReload() {
        switch(characterIndex)
        {
            case 0:
                DetectiveReload();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    public void RegisterInteraction(UnityEvent action) {
        possibleInteractions.Add(action);
        interactPrompt.SetActive(possibleInteractions.Count > 0);
    }

    public void DeregisterInteraction(UnityEvent action) {
        if (possibleInteractions.Contains(action)) {
            possibleInteractions.Remove(action);
            interactPrompt.SetActive(possibleInteractions.Count > 0);
        }
    }


    // Character specific actions
    private void DetectiveAttack() {
        float dist = 4f;
        if (grounded)
            dist = 3.5f;
        dist *= facing;
        VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Muzzleflash_1, transform.position + new Vector3(dist, 1f, 0), facing);
        
        CombatHandler.Instance.PlayOneShotParticle(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 1f, 0), facing, facing * 125 * Vector2.right);
    }

    private void DetectiveSpecial() {
        float dist = 4f;
        if (grounded)
            dist = 3.5f;
        dist *= facing;
        VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Muzzleflash_1, transform.position + new Vector3(dist, 1f, 0), facing);
        VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Detective_Special, transform.position + new Vector3(facing, 1f, 0), facing);

        CombatHandler.Instance.PlayOneShotParticle(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 1f, 0), facing, facing * 125 * Vector2.right);
    }

    private void DetectiveReload() {
        
    }
}
