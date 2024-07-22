using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditorInternal.ReorderableList;

public class TreeNodeB : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
{
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var correctPos = rectTransform.anchoredPosition;
        correctPos.x = Mathf.RoundToInt(correctPos.x / 40) * 40;
        correctPos.y = Mathf.RoundToInt(correctPos.y / 40) * 40;
        rectTransform.anchoredPosition = correctPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.position = transform.position;
        }
    }
}
