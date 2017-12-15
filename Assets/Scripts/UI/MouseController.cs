using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseController : MonoBehaviour {


    public static MouseController current;

    public LayerMask LayerMasksToRaycast;

    public GameObject CurrentGameObject
    {
        get { return currentGameObject; }
    }

    private GameObject currentGameObject;


    void Awake()
    {
        current = this;
    }

    void Update()
    {

        RaycastToGameObject();

        if (Input.GetMouseButtonDown(0))
        {

            // Check if you are clicking on a UI element.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Later on we may want to add something here, for now we just return so our UI won't disappear when we click on it!
                return;
            }


            if (currentGameObject != null)
            {
                // if the player click on a tower, display the tower UI
                if (currentGameObject.tag == "TowerClickable")
                {
                    Tower t = currentGameObject.transform.parent.GetComponent<Tower>();
                    if (t != null)
                    {
                        GameUI.currentUI.OnTowerPressed(t);
                        SoundManager.current.PlaySound("ui_Soft");
                    }

                    return;
                }

            }


            // If the player didn't click on a UI element or a tower, hide the tower information UI
            GameUI.currentUI.towerInformationUI.HideDisplayInformation();
            GameUI.currentUI.HideHighlightedTowerRange();

        }
		
	}


    void RaycastToGameObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
        Vector2.zero, 1f,
        LayerMasksToRaycast);

        if(hit.collider != null)
             currentGameObject = hit.collider.gameObject;

    }



}
