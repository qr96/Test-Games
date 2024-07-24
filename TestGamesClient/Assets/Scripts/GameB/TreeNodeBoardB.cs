using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreeNodeBoardB : MonoBehaviour, IDragHandler, IEndDragHandler
{
    int nodeId;

    Action<PointerEventData> onDrag;
    Action<PointerEventData> onEndDrag;

    public GameObject selected;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke(eventData);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            var choice = collision.GetComponent<TreeMakerChoiceB>();
            if (!choice.CompareId(nodeId))
            {
                selected.SetActive(true);
                choice.Connect(nodeId);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            var choice = collision.GetComponent<TreeMakerChoiceB>();
            if (!choice.CompareId(nodeId))
            {
                selected.SetActive(false);
                choice.Disconnect();
            }
        }
    }

    public void SetNodeId(int id)
    {
        nodeId = id;
    }

    public void SetEventTrigger(Action<PointerEventData> onDrag, Action<PointerEventData> onEndDrag)
    {
        this.onDrag = onDrag;
        this.onEndDrag = onEndDrag;
    }
}
