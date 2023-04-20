using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]

public class TimelineActivator : MonoBehaviour
{
    public PlayableDirector playabledirector;
    public string playerTAG;
    public Transform interactionLocation;
    public bool autoActivate = false;

    public bool interact { get; set; }

    [Header("Activation Zone Events")]
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    [Header("Timeline Events")]
    public UnityEvent OnTimeLineStart;
    public UnityEvent OnTimeLineEnd;

    private bool isPlaying;
    private bool playerInside;
    private Transform playerTransform;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTAG))
        {
            playerInside = true;
            playerTransform = other.transform;
            onPlayerEnter.Invoke();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTAG))
        {
            playerInside = false;
            playerTransform = null;
            onPlayerEnter.Invoke();
        }
    }

    private void PlayTimeline()
    {
        if (playerTransform && interactionLocation)
            playerTransform.SetPositionAndRotation(interactionLocation.position, interactionLocation.rotation);

        if (autoActivate)
            playerInside = false;

        if (playabledirector)
            playabledirector.Play();

        isPlaying = true;
        interact = false;

        StartCoroutine(waitforTimelineToEnd());

    }

    private IEnumerator waitforTimelineToEnd()
    {
        OnTimeLineStart.Invoke();

        float timeLineDuration = (float)playabledirector.duration;

        while (timeLineDuration > 0)
        {
            timeLineDuration -= Time.deltaTime;
            yield return null;
        }

        isPlaying = false;

        OnTimeLineEnd.Invoke();
    }

    private void Update()
    {
        if (playerInside && !isPlaying)
        {
            if (interact || autoActivate)
            {
                PlayTimeline();
            }
        }
    }

}


