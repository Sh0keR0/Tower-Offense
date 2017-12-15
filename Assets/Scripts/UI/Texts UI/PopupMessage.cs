using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PopupMessage : MonoBehaviour
{


    public static PopupMessage current;


    [SerializeField]
    private GameObject PopupGameObject;

    [SerializeField]
    private TextMeshProUGUI titleText, messageText;

    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private Transform ButtonsUITransform;

    [SerializeField]
    private GameObject UIBlockPanel;


    private bool timeWasFrozen = false;

    public Action<string> ActionPopupMessageButtonPressed;


    public bool IsDisplaying
    {
        get { return PopupGameObject.activeInHierarchy; }
    }


    private void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Debug.LogError("PopupMessage - there is already an instance objected in the code of PopupMessage, this should be a singleton!");
            return;
        }

        Hide();


    }

    /// <summary>
    /// Display a pop up message, will return an error if there is already one
    /// </summary>
    /// <param name="title">The title that will be displayed at the top of the message.</param>
    /// <param name="message">Body text of the message.</param>
    /// <param name="options">A list that represent the options, for each string in the list a button will be created</param>
    /// <param name="blockUI">Will the popup message block other UI elements?</param>
    /// <param name="freezeTime">Will the popup message freeze the game time?</param>
    public void Show(string title, string message, List<string> options, Action<string> eventListener = null, bool blockUI = true, bool freezeTime = false)
    {
        if(IsDisplaying)
        {
            Debug.LogError("PopupMessage - trying to show a message when there is already one!");
            return;
        }



        titleText.text = title;
        messageText.text = message;

        // remove old buttons from previous messages
        int index = 0;
        while(index < ButtonsUITransform.childCount)
        {
            Destroy(ButtonsUITransform.GetChild(index).gameObject);
            index++;
        }

        foreach (string option in options)
        {

            // Creates a new button as a child of the ButtonsUI, which automatically arrange buttons sizes
            GameObject button = (GameObject)Instantiate(buttonPrefab, ButtonsUITransform);
            // Adds a on click event to the button
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonPressed(option); });
            button.GetComponent<Button>().onClick.AddListener(delegate { SoundManager.current.PlaySound("ui_Soft"); });
            button.GetComponentInChildren<Text>().text = option;

        }

        
            UIBlockPanel.SetActive(blockUI);

        if(freezeTime)
        {
            Time.timeScale = 0f;
           
        }
            timeWasFrozen = freezeTime;


        PopupGameObject.SetActive(true);

        if(eventListener != null)
        {
            // There can only be only be one listener since it doesn't make sense that two scripts will listen to this event at a time. This may change later.
            ActionPopupMessageButtonPressed = eventListener;
            
        }

    }

    public void Show(string title, string message, string option, Action<string> eventListener = null, bool blockUI = true, bool freezeTime = false)
    {
        List<string> options = new List<string>();
        options.Add(option);
        Show(title, message, options, eventListener, blockUI, freezeTime);
    }

    public void Hide()
    {

        if (IsDisplaying == false) return;


        if (timeWasFrozen)
            Time.timeScale = 1f;



        UIBlockPanel.SetActive(false);
        PopupGameObject.SetActive(false);

    }

    public void OnButtonPressed(string buttonName)
    {
        Hide();
        // Fires an event when a button is pressed

        if(ActionPopupMessageButtonPressed != null)
        {
            ActionPopupMessageButtonPressed(buttonName);
        }

       
    }

    /// <summary>
    /// Register a listener for the popup message event and remove all other listeners.
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterListener(Action<String> listener)
    {
        ActionPopupMessageButtonPressed = listener;
    }

    
    /// <summary>
    /// A class equipped with static void functions that will display common Popup messages
    /// </summary>
    public static class CommonPopups
    {
        

        /// <summary>
        /// Display a popup saying we don't have enough money
        /// </summary>
        public static void NotEnoughMoney()
        {
            PopupMessage.current.Show("NOT ENOUGH MONEY!", "You don't have enough money to purchase this currently.", "OKAY");
        }
    }



}
