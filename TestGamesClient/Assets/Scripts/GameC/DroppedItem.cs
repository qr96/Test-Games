using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour
{
    public TriggerEvent2D trigger;
    public SpriteRenderer image;

    int itemId;

    private void Awake()
    {
        trigger.SetTriggerEvent(onEnter: OnEnterPlayer);
    }

    public void OnSpawnItem(int itemId, int itemCode)
    {
        this.itemId = itemId;
        trigger.enabled = false;
    }

    void OnEnterPlayer(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            LocalPacketSender.SendAcquireItem(itemId);
            gameObject.SetActive(false);
        }
    }
}
