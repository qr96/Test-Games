using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreeMakerChoiceB : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform connection;
    
    RectTransform rectTransform;
    Image image;
    Collider2D collider2d;

    int nodeId;

    Vector2 connectionOriginSize;
    int connectedId = -1;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Start()
    {
        collider2d.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        connectionOriginSize = connection.sizeDelta;

        image.raycastTarget = false;
        collider2d.enabled = true;
        transform.parent.parent.parent.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        Vector2 connectionSize;
        connectionSize.x = connectionOriginSize.x + GetComponent<RectTransform>().anchoredPosition.x;
        connectionSize.y = connectionOriginSize.y - GetComponent<RectTransform>().anchoredPosition.y;
        connection.sizeDelta = connectionSize;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (connectedId >= 0)
        {
            gameObject.SetActive(false);
            connection.gameObject.SetActive(false);
        }
        
        connection.sizeDelta = connectionOriginSize;
        rectTransform.anchoredPosition = Vector2.zero;
        image.raycastTarget = true;
        collider2d.enabled = false;
    }

    public void SetId(int id)
    {
        nodeId = id;
    }

    public bool CompareId(int id)
    {
        return nodeId == id;
    }

    public void Connect(int id)
    {
        connectedId = id;
    }

    public void Disconnect()
    {
        connectedId = -1;
    }
}
