using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorExtender : MonoBehaviour
{

    [SerializeField] private Animator animator;
    float endPause;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (endPause != 0 && Time.time > endPause) {
            animator.speed = 1;
            endPause = 0;
        }
    }

    public void StartPause(float amt) {
        if (endPause < Time.time + amt) {
            animator.speed = 0;
            endPause = Time.time + amt;
        }
    }
}
