using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

    public class Downgrade
    {
        public int Cost;
        public int DamageSubtraction;
        public float FireRateSubtraction, RangeSubtraction;

    public Downgrade(int cost, int damageSubtraction, float fireRateSubtraction, float rangeSubtraction)
    {
        Cost = cost;
        DamageSubtraction = damageSubtraction;
        FireRateSubtraction = fireRateSubtraction;
        RangeSubtraction = rangeSubtraction;
    }
}

[RequireComponent(typeof(Tower))]
public class TowerDowngradeScript : MonoBehaviour
{

    /* TowerDowngradeScript is attached to a tower game object
     * A tower basically starts at the highest level
     * There are X levels of downgrades, downgrades are stored in a list
     * Each downgrade level contain: Damage subtraction, Range subtraction, Fire Rate subtraction, and cost
     * After doing all the downgrades, the destory button is unlocked
     * This class will hold a cost for the destory cost and will notify the GameUI class to make the destory button interactable
     * 
     * */




     // The list of downgrade, when we apply a downgrade we take the first item from the list and apply the downgrade stats before removing it.
     // Therefore the first downgrade added to the list will also be the first downgrade the player will purchase
    private List<Downgrade> towerDowngrades;
    // Which level of downgrade we are currently at, starts at 0
    // Later on we may want to expose this as a property in case we want to display the level of the tower
    // NOTE: this variable increased each downgrade, it is around if we want to know the level of the tower (maximumLevel - downgradeLevel)
    private int downgradeLevel = 0;
    // The cost for destorying the tower, will only unlock after all the tower is completely downgraded
    private int destroyCost;

    // The maximum level of the tower, and the level it start the game with
    // we set this value to towerDowngrades.Count when we initialize.
    // We can display the current level by substracting maximumLevel by downgradeLevel
    private int maximumLevel;

    // The tower script that it is attached to the same game object as this script. NOTE: This script REQUIRE a tower component to be attached
    private Tower tower;


    // some getters we need in order to give information to other classes, more specifically to the UI, in TowerInformationUI class
    public List<Downgrade> TowerDowngrades
    {
        get { return towerDowngrades; }
    }

    // When displaying Downgrade level, it's good idea to keep in mind that this game basically start the tower at max level.
    // Therefore, if we want to display the tower's current level we should probably keep a variable of the maxium level
    // The maxium level is Count of Downgrade list  
    public int DowngradeLevel
    {
        get { return downgradeLevel; }
    }

    public int DestroyCost
    {
        get { return destroyCost; }
    }
    
    public int MaximumLevel
    {
        get { return maximumLevel; }
    }

    public Downgrade NextDowngrade
    {
        get
        {
            if (hasDowngradesAvailable == false) return null;

            return towerDowngrades[0];
        }
    }

    /// <summary>
    ///  Returns true if the tower has any downgrades that are still available (the player didn't purchase them yet)
    /// </summary>
    public bool hasDowngradesAvailable
    {
        get
        {
            if (TowerDowngrades.Count == 0)
                return false;

            return true;
        }
    }

    private void Awake()
    {
        tower = GetComponent<Tower>();
        if (tower == null)
            Debug.LogError("TowerDowngradeScript couldn't find a Tower component");
    }

    private bool initialized = false;

    public void InitializeDowngradeData(List<Downgrade> downgrades, int destroycost)
    {
        // This method supposed to be called only once, and it is used as a constructor
        if (initialized)
        {
            Debug.LogError("InitializeDowngradeData is being called when it was already initialized!");
            return;
        }


        towerDowngrades = downgrades.ConvertAll(downgrade => new Downgrade(downgrade.Cost, 
            downgrade.DamageSubtraction,
            downgrade.FireRateSubtraction,
            downgrade.RangeSubtraction));

        destroyCost = destroycost;
        // the maxium level is basically  the number of downgrades available + 1
        // Since the tower start at level 1, each downgrade decrease it's level by one, a tower should't be at level 0
        maximumLevel = TowerDowngrades.Count + 1;
        initialized = true;
    }

    // Take the first Downgrade from TowerDowngrades, apply the downgrade and then remove it from the list.
    // returns true if performed successfully
    public bool PerformDowngrade()
    {
        if(initialized == false)
        {
            Debug.LogError("TowerDowngradeScript is not yet initialized!");
            return false;
        }

        // Check if the list is empty, if it is empy, we already applied all the downgrades that are possible.
        if(towerDowngrades.Count == 0)
        {
            Debug.LogError("PerformDowngrade was called but there aren't any downgrades in the list");
            return false;
        }


        // Save the data for the downgrade
        Downgrade dg = towerDowngrades[0];


        // before continuing check if the player has enough money for the downgrade!
        if(GameController.instance.Money < dg.Cost)
        {
            return false;
        }


        // Remove the downgrade from the list
        towerDowngrades.RemoveAt(0);

        // Increase the downgradeLevel which is only used to keep track of how many downgrades we performed and what our current level is.
        downgradeLevel++;


        // Reduce the towers stats
        tower.Damage -= dg.DamageSubtraction;
        tower.Range -= dg.RangeSubtraction;
        tower.FireRate -= dg.FireRateSubtraction;

        Vector2 offset = new Vector2(0, 1f);
        GameController.instance.GainMoney(-dg.Cost, (Vector2)tower.gameObject.transform.position + offset);    
        return true;


    }

    public bool DeconstructTower()
    {
        if(initialized == false)
        {
            Debug.LogError("DeconstructTower was called but the class was not initialized!");
            return false;
        }

        if(GameController.instance.Money < DestroyCost)
        {
            // We don't have enough money for that!
            return false;
        }

        // if we have enough money, take it and destroy the tower!
        Vector2 offset = new Vector2(0, 1f);
        GameController.instance.GainMoney(-DestroyCost, (Vector2)tower.gameObject.transform.position + offset);
        Destroy(gameObject);
        return true;
    }




}
