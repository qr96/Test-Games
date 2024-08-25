using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public TriggerEvent2D trigger;
    int id;

    private void Awake()
    {
        trigger.SetTriggerEvent(onEnter: OnEnterPlayer);
    }

    public void OnSpawnItem(int id)
    {
        this.id = id;
        trigger.enabled = false;
    }

    void OnEnterPlayer(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            LocalPacketSender.SendAcquireItem(id);
            gameObject.SetActive(false);
        }
    }
}
