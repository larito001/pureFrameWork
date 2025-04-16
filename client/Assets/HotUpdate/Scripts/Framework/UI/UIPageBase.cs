using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// UI�ű���Ҫ�̳���
/// </summary>
/// 
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIPageBase : MonoBehaviour
{
    public bool isEnable = false;
    public CanvasGroup canvasGroup;
    public abstract void OnLoad();
    public abstract void OnShow();
    public abstract void OnHide();

    public void Enter()
    {
        GetComponent<RectTransform>().anchoredPosition=new Vector2(100f,     GetComponent<RectTransform>().anchoredPosition.y);
        GetComponent<RectTransform>().DOAnchorPosX(0, 0.5f)
            .SetEase(Ease.OutQuad);
    }
    public void EnterFromTop()
    {
        GetComponent<RectTransform>().anchoredPosition=new Vector2( GetComponent<RectTransform>().anchoredPosition.x,    100f);
        GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f)
            .SetEase(Ease.OutQuad);
    }
}
