using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreeNodeB : MonoBehaviour
{
    public TreeNodeBoardB board;
    public List<TreeMakerChoiceB> choiceList;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        board.SetEventTrigger(OnDrag, OnEndDrag);
    }

    public void SetNodeId(int id)
    {
        board.SetNodeId(id);
        foreach (var choice in choiceList)
            choice.SetId(id);
    }

    public void Connect(int choiceId, int targetNodeId)
    {
        choiceList[choiceId].Connect(targetNodeId);
    }

    public void Disconnect(int choiceId)
    {
        choiceList[choiceId].Disconnect();
    }

    void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.SetAsLastSibling();
    }

    void OnEndDrag(PointerEventData eventData)
    {
        var correctPos = rectTransform.anchoredPosition;
        correctPos.x = Mathf.RoundToInt(correctPos.x / 80) * 80;
        correctPos.y = Mathf.RoundToInt(correctPos.y / 80) * 80;
        rectTransform.anchoredPosition = correctPos;
    }
}
