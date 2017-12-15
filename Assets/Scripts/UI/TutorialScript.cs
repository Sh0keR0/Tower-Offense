using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{


    public static bool TutorialActive = false;

    public static TutorialScript current;

    // This black panel will be used to block other UI elements to make sure the player is following the tutorial
    [SerializeField]
    private GameObject highlightPanel;

    // The original parent of the UI element we are highlighting.
    private GameObject uiOriginalParent;
    private GameObject currentHiglightUI;





    // ----------- UI CONTROLS -----------
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private Button buttonControlPanel;
    [SerializeField]
    private Button buttonWaveControl;
    [SerializeField]
    private Button buttonExitPanel;
    [SerializeField]
    private Button buttonUnitCreation;


    // -------------------------------------

    private UnityAction msg;


    public int stage = 0;

    void Start()
    {
        if (PopupMessage.current == null)
        {
            Debug.LogError("PopupMessage.current is null, TutorialScript is depended on it");
            Destroy(gameObject);
            return;
        }
        current = this;
        TutorialActive = true;
        stage = 0;

        ExecuteStage();



    }


    public void ExecuteStage()
    {
        List<string> popupOptions;

        switch (stage)
        {

            case 0:
                {
                    popupOptions = new List<string>();
                    popupOptions.Add("Yes");
                    popupOptions.Add("No");
                    ShowTutorialPopup("Welcome to Tower Offense! \n" +
                        "Would you like to go through our short tutorial?", popupOptions);
                    break;
                }

            case 1:
                {

                    ShowTutorialPopup("In Tower Offense we swap roles\nInstead of controling the towers, you control the enemies", "NEXT");
                    break;
                }
            case 2:
                {
                    ShowTutorialPopup("Let's start by entering our very own control panel\nClick on the the button on the upper left corner of the screen", "NEXT");
                    break;
                }
            case 3:
                {
                    HighlightUIControl(buttonControlPanel.gameObject);
                    break;
                }
            case 4:
                {
                    RemoveHighlight();
                    ShowTutorialPopup("Press on the right panel to enter the ''Attack Plan control''", "NEXT");
                    break;
                }
            case 5:
                {
                    HighlightUIControl(buttonWaveControl.gameObject);
                    break;
                }
            case 6:
                {
                    RemoveHighlight();
                    ShowTutorialPopup("In this panel we can spawn waves of units to invade the enemy field.\nBut before we do that we should develop more types of units", "NEXT");
                    break;
                }
            case 7:
                {
                    ShowTutorialPopup("Close the panel and press on the control panel button again", "NEXT");
                    break;
                }
            case 8:
                {
                    HighlightUIControl(buttonExitPanel.gameObject);
                    break;
                }
            case 9:
                {
                    RemoveHighlight();
                    HighlightUIControl(buttonControlPanel.gameObject);
                    break;
                }
            case 10:
                {
                    RemoveHighlight();
                    HighlightUIControl(buttonUnitCreation.gameObject);
                    break;
                }
            case 11:
                {
                    RemoveHighlight();
                    ShowTutorialPopup("There is a short explaination here, go ahead and read it if you'd like.", "OKAY");
                    buttonExitPanel.interactable = false;
                    break;
                }
            case 12:
                {
                    ShowTutorialPopup("In this panel we can create our own type of units", "NEXT");
                    break;
                }
            case 13:
                {
                    ShowTutorialPopup("Creating new unit has a one time production cost.\nProducing a unit will allow you to deploy it in the Wave Attack Plan panel.", "NEXT");
                    break;
                }
            case 14:
                {
                    ShowTutorialPopup("The Deployment cost is the cost per unit for spawning it to the field.", "NEXT");
                    break;
                }
            case 15:
                {
                    ShowTutorialPopup("The higher the stats, the higher the <color=\"red\">production</color> cost and deployment cost will be.", "NEXT");
                    break;
                }
            case 16:
                {
                    ShowTutorialPopup("The deployment cost is the cost per unit to deploy the unit to the field.\nSelect the name, stats and apperance that you want.\nOnce you are done, click the \"Produce\" button", "OKAY");
                    break;
                }
            case 17:
                {
                    ShowTutorialPopup("Now that you have created your own unit, you can deploy it to the field!.", "NEXT");
                    break;
                }
            case 18:
                {
                    ShowTutorialPopup("Enter the Attack Wave Panel again.", "NEXT");
                    break;
                }
            case 19:
                {
                    HighlightUIControl(buttonControlPanel.gameObject);
                    break;
                }
            case 20:
                {
                    RemoveHighlight();
                    HighlightUIControl(buttonWaveControl.gameObject);
                    break;
                }
            case 21:
                {
                    RemoveHighlight();
                    ShowTutorialPopup("Now it's time to spawn some units.\nYou can now select the unit you have created and spawn many copies of it as long as your budget allows it!", "NEXT");
                    break;
                }
            case 22:
                {
                    ShowTutorialPopup("You have limited budget, before you spawn think how much you want to spend.\nRemember, your goal is to reduce the opponent life points to zero!", "NEXT");
                    break;
                }
            case 23:
                {
                    buttonExitPanel.interactable = true;
                    ShowTutorialPopup("From here you are on your own.\nYou are free to do whatever you like, good luck!", "FINISH");
                    break;
                }




        }

    }

    private void ShowTutorialPopup(string message, string option)
    {
        List<string> opt = new List<string>();
        opt.Add(option);
        ShowTutorialPopup(message, opt);
    }

    private void ShowTutorialPopup(string message, List<string> options)
    {
        PopupMessage.current.Show("TUTORIAL", message,
             options, OnMessageReceived, true, true);
    }

    private void OnMessageReceived()
    {
        OnMessageReceived("NEXT");
    }

    private void OnMessageReceived(string message)
    {


        if(message == "FINISH")
        {
            TutorialActive = false;
            current = null;
            Destroy(gameObject);
        }

        if (message == "NEXT") { stage++; ExecuteStage(); }


        switch (stage)
        {
            case 0:
                {
                    if(message == "Yes")
                    {
                        stage = 1;
                        ExecuteStage();
                    }else { Destroy(gameObject); }
                    break;
                }
        }
    }

    /// <summary>
    /// Highlight a button control then register a callback
    /// </summary>
    /// <param name="ui"></param>
    private void HighlightUIControl(GameObject ui)
    {
        if(currentHiglightUI != null)
        {
            Debug.LogError("TutorialScript - Trying to highlight a UI button but there is already one active, did you forget to call RemoveHighlight()?");
            return;
        }

        highlightPanel.SetActive(true);
        msg = OnMessageReceived;
        ui.GetComponent<Button>().onClick.AddListener(msg);

        currentHiglightUI = ui;
        uiOriginalParent = ui.transform.parent.gameObject;

        ui.transform.SetParent(highlightPanel.transform, true);

    }

    /// <summary>
    /// Hide the highlight and unregister the callback
    /// </summary>
    private void RemoveHighlight()
    {
        highlightPanel.SetActive(false);
        if (currentHiglightUI == null) return;

        currentHiglightUI.transform.SetParent(uiOriginalParent.transform, true);
        currentHiglightUI.GetComponent<Button>().onClick.RemoveListener(msg);

        currentHiglightUI = null;

    }




}
