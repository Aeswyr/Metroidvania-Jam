using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

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
        public UnityEvent onContinue;
        public bool dontFade;
    }

    [SerializeField] private GameObject content;
    [SerializeField] private SpriteRenderer bubble1;
    [SerializeField] private SpriteRenderer bubble2;
    [SerializeField] private TextMeshPro text1;
    [SerializeField] private TextMeshPro text2;
    [SerializeField] private SpriteRenderer indicator;
    [SerializeField] private Vector2 indicatorOffset;
    [SerializeField] private Vector2 responseSpacing;
    [SerializeField] private Vector2 fadeDirection;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private float fadeTime;
    [SerializeField] private SpriteRenderer[] responseBubbles;
    [SerializeField] private TextMeshPro[] responseTexts;
    [SerializeField] private SpriteRenderer fadeResponseBubble;
    [SerializeField] private TextMeshPro fadeResponseText;

    [SerializeField] private Vector2 bubbleOffset;
    [SerializeField] private DialogueEvent[] dialogueEvents;
    private int index;
    private int responseIndex;
    private float floatTimeStamp = -100;
    private float floatTimeStampRes = -100;
    private bool hideAfterFade;
    bool fading;
    bool fadingRes;

    void Awake()
    {
        index = -1;
        fading = false;
        fadingRes = false;
    }

    void Update()
    {
        if (Time.time - floatTimeStamp <= fadeTime)
        {
            bubble2.transform.localPosition = bubble1.transform.localPosition + (Vector3)fadeDirection * moveCurve.Evaluate((Time.time - floatTimeStamp) / fadeTime);
            /*
            float a = 1 - fadeCurve.Evaluate((Time.time - floatTimeStamp) / fadeTime);
            Color c = bubble2.color;
            c.a = a;
            bubble2.color = c;
            c = text2.color;
            c.a = a;
            text2.color = c;
            */
        } else if (fading)
        {
            bubble2.transform.localPosition = bubble1.transform.localPosition + (Vector3)fadeDirection;
            /*
            Color c = bubble2.color;
            c.a = 0;
            bubble2.color = c;
            c = text2.color;
            c.a = 0;
            text2.color = c;
            */
            if (hideAfterFade)
                content.SetActive(false);
        }

        if (Time.time - floatTimeStampRes <= fadeTime)
        {
            fadeResponseBubble.transform.localPosition = responseBubbles[responseIndex].transform.localPosition + (Vector3)fadeDirection * moveCurve.Evaluate((Time.time - floatTimeStampRes) / fadeTime);
            /*
            float a = 1 - fadeCurve.Evaluate((Time.time - floatTimeStampRes) / fadeTime);
            Color c = responseBubbles[responseIndex].color;
            c.a = a;
            fadeResponseBubble.color = c;
            c = fadeResponseText.color;
            c.a = a;
            fadeResponseText.color = c;
            */
        } else if (fading)
        {
            fadeResponseBubble.transform.localPosition = responseBubbles[responseIndex].transform.localPosition + (Vector3)fadeDirection;
            /*
            Color c = responseBubbles[responseIndex].color;
            c.a = 0;
            fadeResponseBubble.color = c;
            c = fadeResponseText.color;
            c.a = 0;
            fadeResponseText.color = c;
            */
        }
    }

    void StartFade()
    {
        /*
        Color c = bubble2.color;
        c.a = 1;
        bubble2.color = c;
        c = text2.color;
        c.a = 1;
        text2.color = c;
        */
        bubble2.gameObject.SetActive(true);
        bubble2.transform.localPosition = bubble1.transform.localPosition;
        floatTimeStamp = Time.time;
        text2.text = text1.text;
        fading = true;
    }

    void StartFadeResponse()
    {
        /*
        Color c = responseBubbles[responseIndex].color;
        c.a = 1;
        fadeResponseBubble.color = c;
        c = responseTexts[responseIndex].color;
        c.a = 1;
        fadeResponseText.color = c;
        */
        fadeResponseBubble.transform.localPosition = responseBubbles[responseIndex].transform.localPosition;
        floatTimeStampRes = Time.time;
        fadeResponseText.text = responseTexts[responseIndex].text;
        fadingRes = true;
    }

    public void NextEvent()
    {
        if (!gameObject.activeSelf)
            return;
        if (!content.activeSelf)
        {
            content.SetActive(true);
            bubble1.gameObject.SetActive(true);
            bubble2.gameObject.SetActive(false);
        }
        if (index >= 0)
        {
            if (dialogueEvents[index].responses.Length > 0)
            {
                dialogueEvents[index].responses[responseIndex].action.Invoke();
                indicator.gameObject.SetActive(false);
                for (int i = 0; i < responseBubbles.Length; i++)
                {
                    if (!responseBubbles[i].gameObject.activeSelf)
                        break;
                    responseBubbles[i].gameObject.SetActive(false);
                }
                StartFadeResponse();
            }
            dialogueEvents[index].onContinue.Invoke();
        }
        index++;
        if (index >= dialogueEvents.Length)
        {
            if (dialogueEvents[index-1].dontFade)
                content.SetActive(false);
            else
            {
                hideAfterFade = true;
                StartFade();
                bubble1.gameObject.SetActive(false);
            }
            index = -1;
            return;
        }
        else
        {
            if (index > 0)
            {
                if (!dialogueEvents[index-1].dontFade)
                    StartFade();
            }
            text1.text = dialogueEvents[index].text;
        }

        if (dialogueEvents[index].responses.Length == 0)
        {
            for(int i = 0; i < responseBubbles.Length; i++)
            {
                if (i >= dialogueEvents[index].responses.Length)
                {
                    responseBubbles[i].gameObject.SetActive(false);
                    continue;
                }
                responseTexts[i].text = dialogueEvents[index].responses[i].text;
                responseBubbles[i].gameObject.SetActive(true);
            }
            if(responseBubbles.Length > 0)
                indicator.transform.localPosition = responseBubbles[0].transform.localPosition + (Vector3)indicatorOffset;
            responseIndex = 0; 
        }
        
    }

    public void NextResponse()
    {
        if (dialogueEvents[index].responses.Length == 0)
            return;
        responseIndex++;
        if (responseIndex >= dialogueEvents[index].responses.Length)
        {
            responseIndex = 0;
        }
        indicator.transform.localPosition += responseBubbles[responseIndex].transform.localPosition + (Vector3)indicatorOffset;
        
    }

    public void PrevResponse()
    {
        if (dialogueEvents[index].responses.Length == 0)
            return;
        responseIndex--;
        if (responseIndex < 0)
            responseIndex = dialogueEvents[index].responses.Length-1;
        indicator.transform.localPosition += responseBubbles[responseIndex].transform.localPosition + (Vector3)indicatorOffset;
        
    }
}