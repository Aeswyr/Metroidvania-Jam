using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool requiresConfirm;
    [SerializeField] private UnityEvent action;
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("bruh");
        if (!requiresConfirm)
            action.Invoke();
        else
            GameHandler.Instance.GetPlayer().RegisterInteraction(action);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (requiresConfirm)
            GameHandler.Instance.GetPlayer().DeregisterInteraction(action);
    }
}
