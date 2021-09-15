using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewAssistant : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentPanel;

    public void VerticalSnapTo(RectTransform target)
    {
        _scrollRect.verticalNormalizedPosition = target.localPosition.y / _contentPanel.sizeDelta.y;
    }
}
