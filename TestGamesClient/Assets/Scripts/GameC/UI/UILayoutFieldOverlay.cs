using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILayoutFieldOverlay : UILayout
{
    public TMP_Text damagePrefab;

    Stack<TMP_Text> damagePool = new Stack<TMP_Text>();

    public void ShowDamage(long damage, Vector3 worldPosition)
    {
        TMP_Text damageIns;
        var startPos = worldPosition;

        if (damagePool.Count > 0)
            damageIns = damagePool.Pop();
        else
            damageIns = Instantiate(damagePrefab, transform);

        damageIns.text = damage.ToString();
        damageIns.color = Color.white;
        damageIns.transform.localPosition = worldPosition;
        damageIns.transform.SetAsLastSibling();
        damageIns.transform.DOLocalMoveY(startPos.y + 1f, 1f);
        damageIns.DOFade(0f, 1f)
            .SetEase(Ease.InCirc)
            .OnComplete(() => damagePool.Push(damageIns));
    }

    public void ShowDamage(List<long> damages, Vector3 worldPosition)
    {
        for (int i = 0; i < damages.Count; i++)
        {
            var newWolrdPos = worldPosition.ChangeY(worldPosition.y + i * 0.5f);
            ShowDamage(damages[i], newWolrdPos);
        }
    }
}
