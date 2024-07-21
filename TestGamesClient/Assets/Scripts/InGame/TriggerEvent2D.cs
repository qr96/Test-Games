using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent2D : MonoBehaviour
{
    Action<Collider2D> onEnter;
    Action<Collider2D> onExit;
    Action<Collider2D> onStay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onEnter?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onExit?.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        onStay?.Invoke(collision);
    }

    public void SetTriggerEvent(Action<Collider2D> onEnter = null, Action<Collider2D> onExit = null, Action<Collider2D> onStay = null)
    {
        this.onEnter = onEnter;
        this.onExit = onExit;
        this.onStay = onStay;
    }
}
