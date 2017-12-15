using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class HoldDetection : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {


    // Detect hold and call OnClick for a button

    public float delayBetweenClicks = 0.2f;
    public float increasedSpeedClick = 0.1f;

    private float cooldown;

    private float totalHoldTime = 0;

    private bool isHolding = false;

    private Button theButton;

    private bool overGameObject = false;

    private void Awake()
    {
        theButton = GetComponent<Button>();
        if(theButton == null)
        {
            Debug.LogError("HoldDetection - no button script attached to the object " + gameObject.name);

            Destroy(gameObject);
            return;
        }
    }
	void Start () {
        cooldown = delayBetweenClicks;

    }
	
	void Update () {

        if (overGameObject == false) return;

		if(isHolding)
        {
            totalHoldTime += Time.deltaTime;
            if(cooldown <= 0)
            {
                theButton.onClick.Invoke();
                if (totalHoldTime > 1f) cooldown = increasedSpeedClick;
                else
                    cooldown = delayBetweenClicks;
            }
            else
            {
                cooldown -= Time.deltaTime;
            }
            
        }
	}


    void PointerUP()
    {
        isHolding = false;
        cooldown = delayBetweenClicks;
        totalHoldTime = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       // throw new NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUP();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        overGameObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overGameObject = false;
        PointerUP();
    }
}
