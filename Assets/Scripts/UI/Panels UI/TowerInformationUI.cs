using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TowerInformationUI : MonoBehaviour
{
    // TODO: Display a tooltip when hovering the downgrade button if we don't have enough money

    // This class handle the lower panel in the game
    

    // Display the information of tower in the game, and allow you to downgrade it for money or destory it completety if the tower is downgraded all the way down.

    [System.Serializable]
    public struct UIControls
    {
        public Text TowerName;
        public Scrollbar scrollBarRange, scrollBarFireRate, scrollBarDamage;
        public Button buttonDowngrade, buttonDestroyTower;
         
    }

    public UIControls controls;
    // The scrollbars Size will change depending on the tower stat value / HighValue
    public float HighDamageValue, HighFireRateValue, HighRangeValue;

    private Tower currentTower;

    private Text buttonDowngradeText;

    private void Awake()
    {
        buttonDowngradeText = controls.buttonDowngrade.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        // Hide it as soon as the game starts, it needs to be enable during the first frame for other scripts to access it. (more specifically GameUI for now)
        HideDisplayInformation();

      
    }

    /// <summary>
    /// Display the tower's information in the bottom UI panel
    /// </summary>
    /// <param name="tower"></param>
    public void DisplayInformation(Tower tower)
    {
        gameObject.SetActive(true);

        if(currentTower != null)
        {
            currentTower.ActionTowerInformationChanged -= OnTowerInformationChanged;
        }

        currentTower = tower;
        currentTower.ActionTowerInformationChanged += OnTowerInformationChanged;
        UpdateUI();

    }

    public void HideDisplayInformation()
    {
        if(currentTower != null)
             currentTower.ActionTowerInformationChanged -= OnTowerInformationChanged;
        currentTower = null;
        gameObject.SetActive(false);
    }



    public void OnDowngradeButtonPressed()
    {
        // If we couldn't perform a downgrade
        if(currentTower.TowerDowngrade.PerformDowngrade() == false)
        {
            PopupMessage.CommonPopups.NotEnoughMoney();

        }
    }

    public void OnDestroyTowerButtonPressed()
    {
        if(currentTower.TowerDowngrade.DeconstructTower() == false)
        {
            PopupMessage.CommonPopups.NotEnoughMoney();
        }
    }


    void OnTowerInformationChanged()
    {
        if (currentTower == null)
        {
            Debug.LogError("OnTowerInformationChanged was called but the currenTower is null");
            return;
        }
          
        UpdateUI();
    }


    private void Update()
    {
        if (currentTower == null)
        {
            // if it is null, it's probably because we don't have a tower selected OR
            // A tower that was selected was destroy
            // We make sure to call HideDisplayInformation incase a tower that was selected was destory (usually occur when the player purchase a destroy)
            HideDisplayInformation();
            return;
        }    

    }

    private void UpdateUI()
    {

        // If the player purchased all the downgrades and the tower don't have downgrades available, 
        // enable the destroy tower button, which allow the player to destroy the tower

        controls.scrollBarDamage.size = currentTower.Damage / HighDamageValue;
        controls.scrollBarFireRate.size = currentTower.FireRate / HighFireRateValue;
        controls.scrollBarRange.size = currentTower.Range / HighRangeValue;

        controls.TowerName.text = currentTower.TowerName;

        // if hasDowngradesAvailable is true then it sets the downgrade button to interactable
        controls.buttonDowngrade.interactable = currentTower.TowerDowngrade.hasDowngradesAvailable;
        if (currentTower.TowerDowngrade.hasDowngradesAvailable)
        {
            buttonDowngradeText.text = "Downgrade - $" + currentTower.TowerDowngrade.NextDowngrade.Cost;
        }
        else
        {
            buttonDowngradeText.text = "Fully downgraded";
        }
        // if hasDowngradesAvailable is true then it sets the destroy button to not interactable because the player must finish to downgrade the tower first
        controls.buttonDestroyTower.interactable = !currentTower.TowerDowngrade.hasDowngradesAvailable;
        controls.buttonDestroyTower.GetComponentInChildren<Text>().text = "Destory Tower (" + currentTower.TowerDowngrade.DestroyCost + "$)";
    }

}
