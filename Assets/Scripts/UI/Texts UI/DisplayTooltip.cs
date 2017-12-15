using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Attach this script to any game Object to display a tooltip when the mouse is over it
  
    public bool IsUI = true; 
    public string TextToDisplay;
    public Vector2 Offset;
    public bool UsePointerPosition = true;
    
  
    private bool overUIGameObject = false;



    private RectTransform UITransform;

    public void OnPointerEnter(PointerEventData eventData)
    {
        overUIGameObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overUIGameObject = false;
    }

    private void Awake()
    {
      
        if(IsUI)
        {
            
            UITransform = GetComponent<RectTransform>();
            if(UITransform == null)
            {
                Debug.LogError("DisplayTooltip - gameObject does not contain RectTransform, are you trying to use this script on a non-UI gameObject?");
                Destroy(gameObject);
                return;
            }
        }else if (IsUI == false)
        {
            if(GetComponent<Collider2D>() == null)
            {
                Debug.LogError("DisplayTooltip - gameObject does not contain a Collider2D, it needs a collider to be detected by the mouse!");
                Destroy(gameObject);
                return;
            }
        }
    }

    private void Update()
    {

        if (IsUI == false)
        {

            if (MouseController.current.CurrentGameObject != gameObject) return;          

        }
        else
        {
            if (overUIGameObject == false) return;
        }


        Vector2 screenPosition = new Vector2();

        Vector2 currentOffset = new Vector2(Offset.x, Offset.y);

         if (UsePointerPosition)
         {
            // if we are using the pointer position we must set a default offset!
            screenPosition = (Vector2)Input.mousePosition;
            // A pointer must have a default offset
            currentOffset += new Vector2(0, 35);

        }
        else if (IsUI)
        {

                // here we are using the transform position
                screenPosition = (Vector2)UITransform.position;
           
        }
        else
        {
            // if it's not a UI, we are getting the gameObject world position
            screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        }

        Tooltip.current.ShowTooltip(TextToDisplay, screenPosition + currentOffset);

    }


}
