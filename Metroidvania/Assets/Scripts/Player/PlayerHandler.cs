using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private JumpHandler jump;
    [SerializeField] private MovementHandler move;

    private bool grounded;
    bool inputsDisabled;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputsDisabled)
            return;

        grounded = ground.CheckGrounded();

        if (InputHandler.Instance.move.pressed)
            move.StartAcceleration(InputHandler.Instance.dir);
        else if (InputHandler.Instance.move.down) {
            move.UpdateMovement(InputHandler.Instance.dir);
            sprite.flipX = InputHandler.Instance.dir < 0;
        } else if (InputHandler.Instance.move.released)
            move.StartDeceleration();


        if (grounded && InputHandler.Instance.jump.pressed)
            jump.StartJump();
        
    }

    public void DisableInputs() {
        move.ResetMovement();
        inputsDisabled = true;
    }

    public void EnableInputs() {
        inputsDisabled = false;
        if (InputHandler.Instance.move.down && !InputHandler.Instance.move.pressed)
            move.StartAcceleration(InputHandler.Instance.dir);
    }
}
