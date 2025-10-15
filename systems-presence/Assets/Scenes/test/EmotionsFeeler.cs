using System.Collections;
using System.Threading;
using UnityEngine;

public class EmotionsFeeler : MonoBehaviour
{
    public enum Emotion { Happy, Sad, Angry, Aloof };

    private Emotion currentEmotion;
    private float timer;
    private float timeNeededToWait;
    private bool hasPrintedToScreen;

    private void Start()
    {
        timer = 0;
        timeNeededToWait = 2.0f;
        currentEmotion = Emotion.Happy;
        hasPrintedToScreen = false;
        Debug.Log("hello");
    }


    private void Update()
    {
        timer += Time.deltaTime;
        switch (currentEmotion)
        {
            case Emotion.Happy:
                OnHappy();
                break;
            case Emotion.Sad:
                OnSad();
                break;
            case Emotion.Angry:
                OnAngry();
                break;
            case Emotion.Aloof:
                OnAloof();
                break;
        }
    }


    private void OnHappy()
    {
        if (!hasPrintedToScreen)
        {
            Debug.Log(":))");
            hasPrintedToScreen = true;
        }
        
        if (timer >= timeNeededToWait)
        {
            timer = 0.0f;
            hasPrintedToScreen = false;
            if (Random.Range(0, 2) == 0)
            {
                currentEmotion = Emotion.Sad;
            }
            else
            {
                currentEmotion = Emotion.Aloof;
            }
            timeNeededToWait = Random.Range(2.0f, 4.0f);
        }
    }

    private void OnSad()
    {
        if (!hasPrintedToScreen)
        {
            Debug.Log("wahhhh");
            hasPrintedToScreen = true;
        }
        
        if (timer >= timeNeededToWait)
        {
            hasPrintedToScreen = false;
            timer = 0.0f;
            currentEmotion = Emotion.Aloof;
            timeNeededToWait = Random.Range(2.0f, 4.0f);
        }
    }

    private void OnAngry()
    {
        if (!hasPrintedToScreen)
        {
            Debug.Log(">:[");
            hasPrintedToScreen = true;
        }
        
        if (timer >= timeNeededToWait)
        {
            hasPrintedToScreen = false;
            timer = 0.0f;
            currentEmotion = Emotion.Sad;
            timeNeededToWait = Random.Range(2.0f, 4.0f);
        }
    }
    
    private void OnAloof()
    {
        if (!hasPrintedToScreen)
        {
            Debug.Log("...");
            hasPrintedToScreen = true;
        }

        if (timer >= timeNeededToWait)
        {
            hasPrintedToScreen = false;
            timer = 0.0f;
            int r = Random.Range(0, 3);
            if (r == 0)
            {
                currentEmotion = Emotion.Happy;
                timeNeededToWait = Random.Range(1.0f, 3.0f);
            }
            else if (r == 1)
            {
                currentEmotion = Emotion.Sad;
                timeNeededToWait = Random.Range(2.0f, 4.0f);
            }
            else
            {
                currentEmotion = Emotion.Angry;
                timeNeededToWait = Random.Range(0.5f, 2.0f);
            }
        }
    }
}