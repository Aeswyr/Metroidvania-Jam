using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{
    public float dir {
        get;
        private set;
    }


    private ButtonState m_move;
    public ButtonState move {
        get{return m_move;}
    }

    private ButtonState m_jump;
    public ButtonState jump {
        get{return m_jump;}
    }

    private ButtonState m_swap;
    public ButtonState swap {
        get{return m_swap;}
    }

    public float swapDir {
        get;
        private set;
    }

    private ButtonState m_reload;
    public ButtonState reload {
        get{return m_reload;}
    }

    private ButtonState m_attack;
    public ButtonState attack {
        get {return m_attack;}
    }

    private ButtonState m_special;
    public ButtonState special {
        get {return m_special;}
    }

    private ButtonState m_interact;
    public ButtonState interact {
        get {return m_interact;}
    }

    private ButtonState m_menu;
    public ButtonState menu {
        get {return m_menu;}
    }

    private void FixedUpdate() {
        this.m_move.Reset();
        this.m_jump.Reset();
        this.m_attack.Reset();
        this.m_special.Reset();
        this.m_interact.Reset();
        this.m_menu.Reset();
        this.m_reload.Reset();
        this.m_swap.Reset();
    }

    public void Move(InputAction.CallbackContext ctx) {
        this.dir = ctx.ReadValue<float>();
        this.m_move.Set(ctx);
    }

    public void Jump(InputAction.CallbackContext ctx) {
        this.m_jump.Set(ctx);
    }

    public void Attack(InputAction.CallbackContext ctx) {
        this.m_attack.Set(ctx);
    }

    public void Special(InputAction.CallbackContext ctx) {
        this.m_special.Set(ctx);
    }

    public void Interact(InputAction.CallbackContext ctx) {
        this.m_interact.Set(ctx);
    }

    public void Menu(InputAction.CallbackContext ctx) {
        this.m_menu.Set(ctx);
    }

    public void Swap(InputAction.CallbackContext ctx) {
        this.m_swap.Set(ctx);
        swapDir = ctx.ReadValue<float>();
    }

    public void Reload(InputAction.CallbackContext ctx) {
        this.m_reload.Set(ctx);
    }

    public struct ButtonState {
        private bool firstFrame;

        public bool down {
            get;
            private set;
        }

        public bool pressed {
            get {
                return down && firstFrame;
            }
        }

        public bool released {
            get {
                return !down && firstFrame;
            }
        }

        public void Set(InputAction.CallbackContext ctx) {
            down = !ctx.canceled;             
            firstFrame = true;
        }

        public void Reset() {
            firstFrame = false;
        }
    }
}
