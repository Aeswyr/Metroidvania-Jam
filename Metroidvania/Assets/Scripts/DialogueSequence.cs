using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSequence : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueResponse
    {
        public string text;
        public UnityEvent action;
    }

    [System.Serializable]
    public struct DialogueEvent
    {
        public string text;
        public DialogueResponse[] responses;
    }

    [SerializeField] private GameObject content;
    [SerializeField] private SpriteRenderer bubble;
    [SerializeField] private TextMeshPro text;

    [SerializeField] private Vector2 bubbleOffset;
    [SerializeField] private DialogueEvent[] dialogueEvents;
    private int index;

    void Awake()
    {
        index = -1;
    }

    public void NextEvent()
    {
        if (!gameObject.activeSelf)
            return;
        if (!content.activeSelf)
            content.SetActive(true);
        index++;
        if (index >= dialogueEvents.Length)
        {
            index = -1;
            content.SetActive(false);
        }
        
    }
}