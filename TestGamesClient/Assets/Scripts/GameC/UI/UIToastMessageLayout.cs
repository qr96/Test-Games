using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIToastMessageLayout : UILayout
{
    public CanvasGroup messageCanvas;
    public TMP_Text messageTmp;

    Sequence sequence;

    private void Start()
    {
        messageCanvas.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        messageTmp.text = message;
        messageCanvas.gameObject.SetActive(true);
        messageCanvas.transform.localScale = Vector3.one * 0.8f;
        messageCanvas.alpha = 1f;

        if (sequence == null)
        {
            sequence = DOTween.Sequence();
            sequence.SetAutoKill(false);
            sequence.Append(messageCanvas.transform.DOScale(Vector3.one, 0.2f));
            sequence.Append(messageCanvas.DOFade(0f, 0.3f));
            sequence.OnComplete(() => messageCanvas.gameObject.SetActive(false));
        }
        else
        {
            sequence.Restart();
        }
    }
}
