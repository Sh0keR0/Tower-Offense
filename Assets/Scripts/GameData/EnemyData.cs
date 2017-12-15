using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{

    // NOTE: This is currently used only to calculate the cost, soon we will use it to limit the stats for the units the player can create

    public static float Healthcap = 350;
    public static float Speedcap = 5f;
    public static float LifeDamagecap = 20;


    // max health stat will add that much cost 
    static float maxHealthCost = 250;
    // max speed stat will add that much cost
    static float maxSpeedCost = 150f;
    // max LifeDamageStat will add that much cost
    static float maxLifeDamaggeCost = 50f;


    public int ID;
    public string SpriteName;
    public string Name;
    public int Health;
    public float Speed;
    public int LifeDamage;
    public int MoneyValue;

    public int Cost
    {
        get
        {
            return GetCostByStats(Health, Speed, LifeDamage);
        }
    }


    public static int GetCostByStats (int Health, float Speed, int LifeDamage)
    {
        // The Max Stat cost * the percentage of the current stat of the stat cap
        //
        // e.g if we have 300 health, the cost will be : 450 * (300 / 500) = 270 - (Just for the health)
        //
        float healthCost = maxHealthCost * (Health / Healthcap);
        float speedCost = maxSpeedCost * (Speed / Speedcap);
        float lifeDamageCost = maxLifeDamaggeCost * (LifeDamage / LifeDamagecap);


        return Mathf.RoundToInt(healthCost + speedCost + lifeDamageCost);
    }

    public EnemyData(int iD, string spriteName, string name, int health, float speed, int lifeDamage, int moneyValue)
    {
        ID = iD;
        SpriteName = spriteName;
        Name = name;
        Health = health;
        Speed = speed;
        LifeDamage = lifeDamage;
        MoneyValue = moneyValue;
    }
}
