using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    long frameCount;
    public float dir {
        get;
        private set;
    }

    private ButtonState m_jump;
    public ButtonState jump {
        get{return m_jump;}
    }

    private ButtonState m_primary;
    public ButtonState primary {
        get {return m_primary;}
    }

    private ButtonState m_skill1;
    public ButtonState skill1 {
        get {return m_skill1;}
    }

    private ButtonState m_skill2;
    public ButtonState skill2 {
        get {return m_skill2;}
    }

    private ButtonState m_skill3;
    public ButtonState skill3 {
        get {return m_skill3;}
    }

    private ButtonState m_skill4;
    public ButtonState skill4 {
        get {return m_skill4;}
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
        this.m_jump.Reset(frameCount);
        this.m_primary.Reset(frameCount);
        this.m_skill1.Reset(frameCount);
        this.m_skill2.Reset(frameCount);
        this.m_skill3.Reset(frameCount);
        this.m_skill4.Reset(frameCount);
        this.m_interact.Reset(frameCount);
        this.m_menu.Reset(frameCount);
        frameCount++;
    }

    public void Move(InputAction.CallbackContext ctx) {
        this.dir = ctx.ReadValue<float>();
    }

    public void Jump(InputAction.CallbackContext ctx) {
        this.m_jump.Set(ctx, frameCount);
    }

    public void Primary(InputAction.CallbackContext ctx) {
        this.m_primary.Set(ctx, frameCount);
    }

    public void Skill1(InputAction.CallbackContext ctx) {
        this.m_skill1.Set(ctx, frameCount);
    }

    public void Skill2(InputAction.CallbackContext ctx) {
        this.m_skill2.Set(ctx, frameCount);
    }

    public void Skill3(InputAction.CallbackContext ctx) {
        this.m_skill3.Set(ctx, frameCount);
    }

    public void Skill4(InputAction.CallbackContext ctx) {
        this.m_skill4.Set(ctx, frameCount);
    }

    public void Interact(InputAction.CallbackContext ctx) {
        this.m_interact.Set(ctx, frameCount);
    }

    public void Menu(InputAction.CallbackContext ctx) {
        this.m_menu.Set(ctx, frameCount);
    }

    public struct ButtonState {
        private long frame;
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

        public void Set(InputAction.CallbackContext ctx, long frame) {
            this.frame = frame;
            down = !ctx.canceled;             
            firstFrame = true;
        }
        public void Reset(long frame) {
            if (frame != this.frame)
                firstFrame = false;
        }
    }
}
