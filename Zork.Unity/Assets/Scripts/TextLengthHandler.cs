using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextLengthHandler : MonoBehaviour
{
    private TMP_Text text;
    private RectTransform parentRect;

    protected void Start()
    {
        parentRect = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();

        if(CheckTextWidth())
        {
            //Double textline height
            parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, parentRect.sizeDelta.y + 30);
        }
    }

    //Check if text is overflowing
    protected bool CheckTextWidth()
    {
        float textWidth = LayoutUtility.GetPreferredWidth(text.rectTransform);
        float parentWidth = parentRect.rect.width;
        return (textWidth > parentWidth);
    }
}
