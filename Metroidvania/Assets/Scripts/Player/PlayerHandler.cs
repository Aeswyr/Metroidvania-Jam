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
    [SerializeField] private GameObject interactPrompt;

    private bool grounded;
    bool inputsDisabled;

    private List<UnityEvent> possibleInteractions = new List<UnityEvent>();


    // Start is called before the first frame update
    void Start()
    {
        interactPrompt.SetActive(false);
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

        if (InputHandler.Instance.interact.pressed && possibleInteractions.Count > 0) {
            possibleInteractions[0].Invoke();
            DeregisterInteraction(possibleInteractions[0]);
        }
            
        
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
}
