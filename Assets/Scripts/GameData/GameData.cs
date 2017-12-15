using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    /* This class holds all the game's data, including:
    Towers data,
    Enemies data,
    Levels???

    */


    public static List<TowerData> Towers;
    public static List<EnemyData> Enemies;

    private static bool initialized = false;
    // We are now making game data via code but later we may want to read it from a file

    public static void InitalizeGameData()
    {
        if (initialized == true)
            Debug.LogError("InitializeGameData is being called when the GameData is already initialized");

        GenerateTowerData();
        GenerateEnemiesData();
    }


    public static void AddNewEnemyData(string spriteName, string name, int health, float speed, int lifeDamage, int moneyValue)
    {
        Enemies.Add(new EnemyData(Enemies.Count, spriteName, name, health, speed, lifeDamage, moneyValue));
    }

    private static void GenerateEnemiesData()
    {
        Enemies = new List<EnemyData>();

        // ID, SPRITENAME,NAME,  HEALTH, SPEED, LIFE DAMAGE, MONEY VALUE
        Enemies.Add(new EnemyData(Enemies.Count, "enemy_Default","Soldier", 100, 2.2f, 5, 1000));
        Enemies.Add(new EnemyData(Enemies.Count, "enemy_Robot", "Bulletproof Robot", 350, 1.2f, 5, 2000));
        Enemies.Add(new EnemyData(Enemies.Count, "enemy_Ninja", "Ninja", 65, 5.3f, 5, 2000));


    }


    public static EnemyData GetEnemyByID(int id)
    {
        foreach(EnemyData e in Enemies)
        {
            if (id == e.ID)
                return e;
        }

        Debug.LogError("Couldn't find an enemy with ID " + id);
        return null;
    }


    private static void GenerateTowerData()
    {

        List<Downgrade> downgrades = new List<Downgrade>();

        // Cost, Damage, fire rate, range
        downgrades.Add(new Downgrade(500, 10, 0, 0.3f));
        downgrades.Add(new Downgrade(300, 5, 0, 0));
        downgrades.Add(new Downgrade(200, 5, 0, 0));

        Towers = new List<TowerData>();
        // We may want different towers we the same tower type, for example a tower with freezing effect but with
        // ID, NAME, DAMAGE, RANGE, FIRE RATE, TOWER TYPE
        Towers.Add(new TowerData(0,"Defense turret", 50, 2f, 1.5f, TowerType.normalTower, downgrades, 3000));

        downgrades = new List<Downgrade>();
        downgrades.Add(new Downgrade(700, 5, 0, 0f));
        downgrades.Add(new Downgrade(500, 5, 0, 0));
        downgrades.Add(new Downgrade(300, 2, 0, 0.5f));

        Towers.Add(new TowerData(1, "Bomb Turret", 20, 2.5f, 0.5f, TowerType.bombTower, downgrades, 3500));



        ///////////////// UNUSED TOWERS!!! ///////////////////////////
        // Freezing tower will deal no damage but will have high range and will slow two units per second
        // The damage value of this tower will be the slow percentage 
        Towers.Add(new TowerData(2, "Freezing turret", 0, 5, 2, TowerType.freezingTower, downgrades, 600));
        Towers.Add(new TowerData(3, "Machinegun turret", 5, 2, 5, TowerType.normalTower, downgrades, 600));
    }

    public static TowerData GetTowerDataByID (int id)
    {
        foreach(TowerData data in Towers)
        {
            if (data.ID == id)
                return data;
        }

        Debug.LogError("GetTowerDataByID - couldn't find tower with ID of " + id);
        return null;
    }

    public static TowerData GetTowerDataByName(string name)
    {
        foreach (TowerData data in Towers)
        {
            if (data.Name == name)
                return data;
        }

        Debug.LogError("GetTowerDataByID - couldn't find tower with ID of " + name);
        return null;
    }

}
