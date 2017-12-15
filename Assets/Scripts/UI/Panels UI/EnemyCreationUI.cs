using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCreationUI : MonoBehaviour
{


    public GameObject TutorialScreen, CreationScreen;
    private string[] EnemiesSpritesNames;


    // CONTROLS //
    public TMP_InputField CreationNameInputField,
        CreationHealthInputField,
        CreationSpeedInputField,
        CreationDamageInputField;

    public Scrollbar CreationHealthScrollbar,
        CreationSpeedScrollbar,
        CreationDamageScrollbar;

    public Image UnitApperanceImage;

    public TextMeshProUGUI ProductionCostText,
        DeploymentCostText;


    


    private bool showTutorial = true;

    private string creationName;
    private string creationSpriteName;
    private int creationHealth;
    private float creationSpeed;
    private int creationDamage;

    private int spriteIndex;




    private void Awake()
    {
        // loop through the loaded sprites and find all the sprites that have enemy_ in them
        List<string> EnemySpritesFinder = new List<string>();
        foreach(string s in GameSpritesLoader.Sprites.Keys)
        {
            if(s.Contains("enemy_"))
            {
                EnemySpritesFinder.Add(s);
            }

        }

        EnemiesSpritesNames = EnemySpritesFinder.ToArray();
    }

    public void NeverShowTutorialAgain()
    {
        showTutorial = false;
        DisplayCreationScreen(false);
    }



    public void OnCreateUnitPressed()
    {
        List<string> options = new List<string>();
        options.Add("Yes");
        options.Add("No");
        PopupMessage.current.Show("ARE YOU SURE?", "Are you sure you want to produce this unit for " + GetProductionCost() + "$ ?"
            + "  You can't not change your mind later!", options, CreateUnit);
    }


    private void CreateUnit(string accept)
    {
        if (accept != "Yes") return;

        if(GameController.instance.Money < GetProductionCost())
        {
            PopupMessage.CommonPopups.NotEnoughMoney();
            return;
        }

        if(GetHealthValue() == 0 || GetSpeedValue() == 0 || GetDamageValue() == 0)
        {
            PopupMessage.current.Show("INVALID STATS", "You can not produce a unit with a stat of zero!", "OKAY");
            return;
        }

        GameData.AddNewEnemyData(EnemiesSpritesNames[spriteIndex],
            CreationNameInputField.text,
            GetHealthValue(),
            GetSpeedValue(),
            GetDamageValue(),
            2000);


        GameController.instance.GainMoney(-GetProductionCost());

        // If we are in the tutorial, advance to the next stage
        if (TutorialScript.TutorialActive)
        {
            TutorialScript.current.stage++;
            TutorialScript.current.ExecuteStage();
        }
        else
        {
            PopupMessage.current.Show("UNIT PRODUCTION", "You sccuessfully added new unit to your collection", "Hell yeah!");
        }


        EnemyControlPanelUI.current.HidePanel();

    }

    public void DisplayCreationScreen(bool tutorial = true)
    {
        if (showTutorial && tutorial)
        {
            // Display the tutorial screen
            TutorialScreen.SetActive(true);
            CreationScreen.SetActive(false);
            return;
        } 
        
        if(TutorialScript.TutorialActive)
        {
            TutorialScript.current.stage++;
            TutorialScript.current.ExecuteStage();
        }
        
        TutorialScreen.SetActive(false);
        CreationScreen.SetActive(true);

        CreationHealthInputField.text = "10";
        CreationSpeedInputField.text = "1";
        CreationDamageInputField.text = "1";

        CreationNameInputField.text = "Default Name";

        spriteIndex = 0;
        UnitApperanceImage.sprite = GameSpritesLoader.Sprites[EnemiesSpritesNames[spriteIndex]];


    }

    public void OnStatValueChanged()
    {
        int healthValue = GetHealthValue();
        float speedValue = GetSpeedValue();
        int damageValue = GetDamageValue();


        if (healthValue > EnemyData.Healthcap) CreationHealthInputField.text = EnemyData.Healthcap.ToString();
        if (speedValue > EnemyData.Speedcap) CreationSpeedInputField.text = EnemyData.Speedcap.ToString();
        if (damageValue > EnemyData.LifeDamagecap) CreationDamageInputField.text = EnemyData.LifeDamagecap.ToString();


        if (healthValue <= 0) CreationHealthInputField.text = "1";
        if (speedValue <= 0) CreationSpeedInputField.text = "0.1";
        if (damageValue <= 0) CreationDamageInputField.text = "1";

        // Calculate and update the scrollbars

        CreationHealthScrollbar.size = healthValue / EnemyData.Healthcap;
        CreationSpeedScrollbar.size = speedValue / EnemyData.Speedcap;
        CreationDamageScrollbar.size = damageValue / EnemyData.LifeDamagecap;

        // Update the production cost display and deployment cost

        ProductionCostText.text = GetProductionCost().ToString() + "$";
        DeploymentCostText.text = GetDeploymentCost().ToString() + "$";

    }

    public void IncreaseStat(string statName)
    {
        float increase = 1;
        if (statName == "Speed") increase = 0.1f;
        ChangeStatAmount(statName, increase);
    }

    public void DecreaseStat(string statName)
    {
        float increase = -1;
        if (statName == "Speed") increase = -0.1f;
        ChangeStatAmount(statName, increase);
    }


    private void ChangeStatAmount(string statName, float increase)
    {
        if(statName == "Health")
        {
            int health = GetHealthValue();
            if (health + increase > EnemyData.Healthcap || health + increase <= 0) return;

            CreationHealthInputField.text = (GetHealthValue() + increase).ToString();
        }
        else if (statName == "Speed")
        {

            float speed = GetSpeedValue();
            if (speed + increase > (double)EnemyData.Speedcap + 0.001f || speed + increase <= 0) return;

            CreationSpeedInputField.text = (GetSpeedValue() + increase).ToString();
        }
        else if (statName == "Damage")
        {

            int damage = GetDamageValue();
            if (damage + increase > EnemyData.LifeDamagecap || damage + increase <= 0) return;

            CreationDamageInputField.text = (GetDamageValue() + increase).ToString();
        }
    }


    public void ChangeSpriteForUnitIndex(int increase)
    {

        // check if the sprite index will get out of bounds of the array, if so set it back to zero
        // Then check if we are trying to go below zero, and set it array length -1 if true
        // if we are not out of bounds and not trying to go below zero, simply increase the index

        if (spriteIndex + increase >= EnemiesSpritesNames.Length) spriteIndex = 0;
        else if (spriteIndex + increase < 0) spriteIndex = EnemiesSpritesNames.Length - 1;
        else spriteIndex += increase;

        // Get the sprite from the sprites dictionary by name
        UnitApperanceImage.sprite = GameSpritesLoader.Sprites[EnemiesSpritesNames[spriteIndex]];
    }

    private int GetProductionCost()
    {
        return (GetDeploymentCost() * 5) + 1000;
    }
    private int GetDeploymentCost()
    {
        return EnemyData.GetCostByStats(GetHealthValue(), GetSpeedValue(), GetDamageValue());
    }

    private int GetHealthValue()
    {
        if (CreationHealthInputField.text.Length == 0) return 0;

        return int.Parse(CreationHealthInputField.text);

    }
    private float GetSpeedValue()
    {
        if (CreationSpeedInputField.text.Length == 0) return 0;

        return float.Parse(CreationSpeedInputField.text);
    }
    private int GetDamageValue()
    {
        if (CreationDamageInputField.text.Length == 0) return 0;

        return int.Parse(CreationDamageInputField.text);
    }


}
