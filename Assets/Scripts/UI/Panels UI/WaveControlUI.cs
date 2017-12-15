using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class WaveControlUI : MonoBehaviour
{

    [System.Serializable]
    public struct UnitInformation
    {
        public TextMeshProUGUI UnitName, UnitCost, UnitCount, UnitTotalCost;
        public Image UnitSprite;
        public Scrollbar UnitHealth, UnitSpeed, UnitDamage;
    }


    public TextMeshProUGUI TotalCost, TotalUnits;
    public UnitInformation unitInfo;
    public GameObject UnitInformationPanel;

    #region properties
    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }

        set
        {
            enemyCount = value;
            OnCountChanged();
        }
    }

    // Used for display purposes, to store the total number of each enemy type
    public Dictionary<EnemyData, int> CountForEachEnemyData
    {
        get
        {
            return countForEachEnemyData;
        }

    }

    public List<Wave> WavesData
    {
        get
        {
            return wavesData;
        }

        set
        {
            wavesData = value;
            OnWaveDataChanged();
        }
    }

    #endregion

    #region Events / callbacks
    public Action ActionOnUnitsCountChanged;
    #endregion




    private List<Wave> wavesData;
   // private Queue<EnemyData> waveQueue;
    private Dictionary<EnemyData, int> countForEachEnemyData;

    // the amount of enemies we want to add to the queue when we press the Add button
    // Use the property EnemyCount incase we want to change it
    private int enemyCount = 0;
  

    private EnemyData selectedUnit;

    private WaveControlWaveOrderUI currentWaveOrderUI;
    private WaveControlUnitViewUI currentUnitViewUI;


    private void Awake()
    {
        currentWaveOrderUI = transform.GetComponentInChildren<WaveControlWaveOrderUI>();
        currentUnitViewUI = transform.GetComponentInChildren<WaveControlUnitViewUI>();

    }

    public void DisplayWaveControlPanel()
    {

        gameObject.SetActive(true);
        UnitInformationPanel.SetActive(false);

        
        currentUnitViewUI.DisplayUnits();

        WavesData = new List<Wave>();
        currentWaveOrderUI.ResetDisplay();

        countForEachEnemyData = new Dictionary<EnemyData, int>();
        // Adds all the enemydata to the dict with the value of 0 (since there are 0 currently in the queue)
        foreach (EnemyData e in GameData.Enemies)
        {
            countForEachEnemyData.Add(e, 0);
        }
        OnUnitsCountChanged();
    }

    

    public void OnUnitClicked(EnemyData enemy)
    {
        // Called when the player select a unit they want to spawn.
        // Display the information of the unit that was selected

        selectedUnit = enemy;

        unitInfo.UnitName.text = enemy.Name;
        unitInfo.UnitCost.text = enemy.Cost.ToString() + "$";
        unitInfo.UnitSprite.sprite = GameSpritesLoader.Sprites[enemy.SpriteName];
        unitInfo.UnitHealth.size = enemy.Health / EnemyData.Healthcap;
        unitInfo.UnitSpeed.size = enemy.Speed / EnemyData.Speedcap;
        unitInfo.UnitDamage.size = enemy.LifeDamage / EnemyData.LifeDamagecap;
        EnemyCount = 0;

        UnitInformationPanel.SetActive(true);

    }

    public void OnIncreaseCountClicked()
    {
        EnemyCount++;
    }
    public void OnDecreaseCountClicked()
    {
        if (EnemyCount == 0) return;
        EnemyCount--;
    }

    public void OnAddUnitsToWaveClicked()
    {
        if(EnemyCount > 0)
        {
            int totalCost = 0;
            for (int i = 0; i < EnemyCount; i++)
            {
                totalCost += selectedUnit.Cost;
            }
            
            if (GameController.instance.Money < CalculateTotalCost() + totalCost)
            {
                PopupMessage.current.Show("NOT ENOUGH MONEY!", "You will not have enough money to spawn that many units. " + "\n" + "Reduce the amount of units or use the reset button.", "OKAY");
                return;
            }

            // Creates a new wave and add it to the list
            Wave newWave = new Wave();
            newWave.EnemyID = selectedUnit.ID;
            newWave.EnemyCount = EnemyCount;
            newWave.SpawnRate = 0.20f - (selectedUnit.Speed / 100);
            //newWave.CooldownTillNextWave = 1 + ((EnemyCount * (newWave.SpawnRate * 100)) / 100);
            newWave.CooldownTillNextWave = 1 + ((EnemyCount / newWave.SpawnRate) / 100);

            WavesData.Add(newWave);

            // Counts the total number of units
            countForEachEnemyData[selectedUnit] += EnemyCount;

            OnUnitsCountChanged();
            OnWaveDataChanged();

            // Add the unit sprite and count to the display queue
            currentWaveOrderUI.AddUnitToDisplay(selectedUnit, EnemyCount);
            
            EnemyCount = 0;
        }


    }


    public void OnResetButtonClicked()
    {
        DisplayWaveControlPanel();
    }




    public void OnSpawnWaveClicked()
    {
        if (WavesData.Count == 0) return;

        if(GameController.instance.Money < CalculateTotalCost())
        {
            PopupMessage.CommonPopups.NotEnoughMoney();
            return;
        }

        if (EnemySpawner.current.IsWorking)
        {
            Debug.LogError("WaveControlUI - trying to tell enemy spawner to spawn a wave when it's already working.");
            return;
        }

   

        EnemySpawner.current.StartSpawner(WavesData.ToArray());

        EnemyControlPanelUI.current.HidePanel();
        GameController.instance.GainMoney(-CalculateTotalCost());

    }




    void OnUnitsCountChanged()
    {
        // Call the event to notify subscribers that the count of a unit type has changed
        if (ActionOnUnitsCountChanged != null)
            ActionOnUnitsCountChanged();
    }

    void OnCountChanged()
    {
        unitInfo.UnitCount.text = EnemyCount.ToString();
        int totalCost = 0;
        for (int i = 0; i < EnemyCount; i++)
        {
            totalCost += selectedUnit.Cost;
        }
        if (GameController.instance.Money > totalCost + CalculateTotalCost())
            unitInfo.UnitTotalCost.color = new Color32(0, 236, 21,255);
        else
            unitInfo.UnitTotalCost.color = new Color32(203, 7, 36, 255);
        unitInfo.UnitTotalCost.text = totalCost.ToString() + "$";
    }

    void OnWaveDataChanged()
    {
        TotalCost.text = CalculateTotalCost().ToString() + "$";

        int totalUnits = 0;
        foreach(Wave w in WavesData)
        {
            totalUnits += w.EnemyCount;
        }
        TotalUnits.text = totalUnits.ToString();
    }


    int CalculateTotalCost()
    {
        int cost = 0;
        foreach(Wave e in WavesData)
        {
            cost += GameData.GetEnemyByID(e.EnemyID).Cost * e.EnemyCount;
        }

        return cost;
    }


}
