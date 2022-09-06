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

    [Header("Data")]
    [SerializeField] private float wallJumpDelay;
    [SerializeField] private int jumpGraceFrames;
    int characterIndex = 0;
    private int jumpGraceCounter;
    private bool wallGrace, jumpGrace;
    private bool grounded, acting, wallSliding;
    bool inputsDisabled;
    private int facing = 1;
    private int wallFacing = 0;
    private float wallJumpTimer;
    private bool forceMoveRelease;

    private List<UnityEvent> possibleInteractions = new List<UnityEvent>();


    // Start is called before the first frame update
    void Start()
    {
        animator.runtimeAnimatorController = characterOverrides[characterIndex];
        interactPrompt.SetActive(false);
        wallJumpTimer = 0f;
        forceMoveRelease = false;
    }

    void Update()
    {
        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
            if(wallJumpTimer <= 0)
            {
                wallJumpTimer = 0;
            }
        }
        else if (wallJumpTimer < 0)
        {
            wallJumpTimer += Time.deltaTime;
            if(wallJumpTimer >= 0)
            {
                wallJumpTimer = 0;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (jumpGraceCounter > 0)
            jumpGraceCounter--;
        if (inputsDisabled)
            return;

        bool groundedprev = grounded;
        grounded = ground.CheckGrounded();
        bool wallSlidingPrev = wallSliding;
        wallSliding = wallSlide.IsWallSliding() && !grounded && wallSlide.enabled && wallJumpTimer == 0;

        if (grounded || wallSliding)
            jumpGraceCounter = jumpGraceFrames;

        if (grounded) {
            jumpGrace = true;
            wallGrace = false;
        }

        if (wallSliding) {
            wallGrace = true;
            jumpGrace = false;
            wallFacing = facing * -1;
        }

        if (wallSliding&&!wallSlidingPrev)
            jump.ForceLanding();
        if (acting && ((grounded && !groundedprev) || (wallSliding && !wallSlidingPrev)))
            EndAction();
        animator.SetBool("grounded", grounded);
        animator.SetBool("hanging", wallSliding);

        if (wallJumpTimer != 0)
        {
            if(grounded)
            {
                wallJumpTimer = 0;
            }
            else
            {
                if(forceMoveRelease)
                {
                    move.UpdateMovement(Mathf.Sign(wallJumpTimer));
                }
                else
                {
                    move.StartAcceleration(Mathf.Sign(wallJumpTimer));
                    forceMoveRelease = true;
                }
            }
        }
        else
        {
            if (InputHandler.Instance.move.pressed && !acting)
                move.StartAcceleration(InputHandler.Instance.dir);
            else if (InputHandler.Instance.move.down && !acting) {
                move.UpdateMovement(InputHandler.Instance.dir);
                if (wallJumpTimer == 0) {
                    sprite.flipX = InputHandler.Instance.dir < 0;
                    facing = sprite.flipX ? -1 : 1;
                }
                animator.SetBool("moving", true);
            } else if ((InputHandler.Instance.move.released | (!InputHandler.Instance.move.released && forceMoveRelease)) && !acting) {
                forceMoveRelease = false;
                move.StartDeceleration();
                animator.SetBool("moving", false);
            }
        }

        //Character Swap
        if (InputHandler.Instance.swap.pressed && !acting) {
            int prev = characterIndex;
            characterIndex = (characterIndex + 1) % characterOverrides.Length;
            animator.runtimeAnimatorController = characterOverrides[characterIndex];
            VFXHandler.Instance.PlayOneShotParticle((VFXHandler.ParticleType)prev, transform.position, facing);
        }

        //Jumping
        if (InputHandler.Instance.jump.pressed && !acting) {
            if (grounded || (jumpGrace && jumpGraceCounter > 0))
            {
                jumpGrace = false;
                jump.StartJump();
                animator.SetBool("grounded", false);
                animator.SetTrigger("jump");
            }
            else if ((wallSliding || (wallGrace && jumpGraceCounter > 0)) && wallJumpTimer == 0)
            {
                wallGrace = false;
                facing = wallFacing;
                wallJumpTimer = wallJumpDelay * facing;
                sprite.flipX = facing < 0;
                animator.SetBool("hanging", false);
                jump.StartJump();
                animator.SetTrigger("jump");
            }
            
        }

        //Special
        if (InputHandler.Instance.special.pressed && !acting && !wallSliding) {
            StartAction();
            animator.SetTrigger("special");
        }

        //Attacking
        if (InputHandler.Instance.attack.pressed && !acting && !wallSliding) {
            StartAction();
            animator.SetTrigger("attack");
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
        if (InputHandler.Instance.dir != 0) {
            sprite.flipX = InputHandler.Instance.dir < 0;
            facing = sprite.flipX ? -1 : 1;
        }
    }

    private void EndAction() {
        acting = false;
        if (!InputHandler.Instance.move.down)
            move.StartDeceleration();
    }

    private void FireAttack() {
        switch(characterIndex)
        {
            case 0:
                DetectiveAttack();
                break;
            case 1:
                AnarchistAttack();
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
        
        CombatHandler.Instance.PlayOneShotProjectile(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 1f, 0),
        facing, facing * 125 * Vector2.right, isPlayerOwned: true, destroyOnImpact: true);
    }

    private void DetectiveSpecial() {
        float dist = 4f;
        if (grounded)
            dist = 3.5f;
        dist *= facing;
        VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Muzzleflash_1, transform.position + new Vector3(dist, 1f, 0), facing);
        VFXHandler.Instance.PlayOneShotParticle(VFXHandler.ParticleType.Detective_Special, transform.position + new Vector3(facing, 1f, 0), facing);

        CombatHandler.Instance.PlayOneShotProjectile(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 1f, 0),
        facing, facing * 125 * Vector2.right, isPlayerOwned: true, destroyOnImpact: true);
    }

    private void DetectiveReload() {
        
    }

    private void AnarchistAttack() {
        float dist = 3f;
        dist *= facing;
        CombatHandler.Instance.PlayOneShotProjectile(CombatHandler.ProjectileType.Bullet, transform.position + new Vector3(dist, 0, 0)
        , facing, size: new Vector2(4, 4), parent: transform, duration: 0.125f, isPlayerOwned: true, destroyOnImpact: true, shouldHitpause: true);
    }
}
