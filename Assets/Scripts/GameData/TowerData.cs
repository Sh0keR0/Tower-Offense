using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The Type of the tower will determine which script will be attached to the tower
/// </summary>
public enum TowerType { normalTower, bombTower, freezingTower};

public class TowerData
{



    // TowerData hold the data for different type of towers in the game
    // These data will be based to game object with a tower script attached.
    // This can be used in the future if we want to display all the available towers in the game 

    public int ID;
    public string Name;
    public int Damage;
    public float Range;
    public float FireRate;
    public TowerType type;
    public List<Downgrade> DowngradesData;
    public int DestroyCost;

    public TowerData(int id,string name, int damage, float range, float fireRate, TowerType type, List<Downgrade> downgradesData, int destroyCost)
    {
        ID = id;
        Name = name;
        Damage = damage;
        Range = range;
        FireRate = fireRate;
        this.type = type;
        DowngradesData = downgradesData;
        DestroyCost = destroyCost;
    }

}
