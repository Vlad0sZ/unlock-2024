using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static readonly List<MouseController> ThisQueue = new();
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var selectable = gameObject.GetComponent<Selectable>();
        if (selectable != null && !selectable.interactable)
        {
            return;
        }
        
        if(!ThisQueue.Contains(this))
        {
            ThisQueue.Add(this);
        }
        
        CursorController.ChangeCursor(CursorType.Hand);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RemoveThis();
    }

    private void RemoveThis()
    {
        if(ThisQueue.Contains(this))
        {
            ThisQueue.Remove(this);
        }
        
        if (ThisQueue.Count == 0)
        {
            CursorController.ChangeCursor(CursorType.Arrow);
        }
    }
    
    private void OnDisable()
    {
        RemoveThis();
    }

    private void OnDestroy()
    {
        RemoveThis();
    }
}
