using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour {
    [SerializeField] UnityEvent OnEnterEvent = new UnityEvent();
    [SerializeField] UnityEvent OnExitEvent = new UnityEvent();
    [SerializeField] UnityEvent OnStayEvent = new UnityEvent();
    [SerializeField] bool isOnce;
    private Collider triggerCollider;


    // Use this for initialization
    void Awake ()
    {
        triggerCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEnterEvent.Invoke();
            triggerCollider.enabled = !isOnce;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnExitEvent.Invoke();
            triggerCollider.enabled = !isOnce;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnStayEvent.Invoke();
            triggerCollider.enabled = !isOnce;
        }
    }
}
