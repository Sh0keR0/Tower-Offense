using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tooltip : MonoBehaviour

{

    public static Tooltip current;

    [SerializeField]
    private GameObject TooltipPanel;

    private RectTransform panel;
    private TextMeshProUGUI panelText;

    private string textDispaly;
    private Vector2 screenPosition;
    private bool displayTooltip = false;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Debug.LogError("Tooltip - there is already an instance of tooltip in the game");
            return;
        }

        panel = TooltipPanel.GetComponent<RectTransform>();
        panelText = TooltipPanel.GetComponentInChildren<TextMeshProUGUI>();

        TooltipPanel.SetActive(false);
        
    }

    // NOTE: Try Update and LateUpate, LateUpdate may be better since it will run the last, allowing other scripts to call Showtooltip for this frame
    private void LateUpdate()
    {
        // Each frame, if displayTooltip is true, display the tooltip then set displayTooltip to false, other scripts needs to call Showtooltip every frame

        if(displayTooltip)
        {
            panel.position = screenPosition;
            panelText.text = textDispaly;
            if (TooltipPanel.activeInHierarchy == false) TooltipPanel.SetActive(true);

        }
        else
        {
             if (TooltipPanel.activeInHierarchy == true) TooltipPanel.SetActive(false);
        }

        displayTooltip = false;
    }

    /// <summary>
    /// Display a tooltip for one frame, must be called every frame for continuous display, this should be called only ONCE per frame!
    /// </summary>
    /// <param name="text"></param>
    /// <param name="screenPoint">Use Camera.main.WorldToScreenPoint if necessary</param>
    public void ShowTooltip(string text, Vector2 screenPoint)
    {
        
        if(displayTooltip)
        {
            // Showtooltip was called more than once this frame, returns an error.
            Debug.LogError("Tooltip - ShowTooltip was called more than once this frame!");
            return;
        }

        textDispaly = text;
        screenPosition = screenPoint;
        displayTooltip = true;
    }



}
