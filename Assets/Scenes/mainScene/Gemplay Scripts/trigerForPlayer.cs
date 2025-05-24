using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class trigerForPlayer : MonoBehaviour
{
    public LayerMask targetLayer;

    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    public bool destroyAfterTrigger = false;

    public float destroyDelay = 0.5f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && (targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            onPlayerEnter.Invoke();

            if (destroyAfterTrigger)
                Destroy(gameObject, destroyDelay);
            else
                hasTriggered = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            onPlayerExit.Invoke();
            hasTriggered = false;
        }

    }

}

