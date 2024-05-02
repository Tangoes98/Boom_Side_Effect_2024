using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateMouseCursor.Instance.UpdateCursor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateMouseCursor.Instance.UpdateCursor(false);
    }

}
