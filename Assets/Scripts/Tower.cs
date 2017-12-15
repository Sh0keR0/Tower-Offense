using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Tower : MonoBehaviour
{

    // We must attach an attack type script component on the tower game object if we want it to be able to attack!

    // Fire rate is the number of bullets the tower can shoot per second
    private string towerName;
    private float fireRate;
    private int damage;
    private float range;


    public Action ActionTowerInformationChanged;

    public GameObject gunGO;


    public List<Enemy> GetEnemiesInRange
    {
        get { return enemiesInRange; }
    }

    public TowerDowngradeScript TowerDowngrade
    {
        get
        {
            return towerDowngrade;
        }
        set
        {
            towerDowngrade = value;
        }

    }

    public string TowerName
    {
        get
        {
            return towerName;
        }

        set
        {
            towerName = value;
            OnTowerInformationChanged();
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }

        set
        {
            fireRate = value;
            OnTowerInformationChanged();
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
            OnTowerInformationChanged();
        }
    }

    public float Range
    {
        get
        {
            return range;
        }

        set
        {
            range = value;
            OnTowerInformationChanged();
        }
    }

    private List<Enemy> enemiesInRange;
    private CircleCollider2D rangeCollider;
    private TowerDowngradeScript towerDowngrade;

    private void Awake()
    {
        enemiesInRange = new List<Enemy>();


        // Add A circle Collider that will indicate as range
        rangeCollider = GetComponent<CircleCollider2D>();
        if (rangeCollider == null)
           rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;



    }


    void OnTowerInformationChanged()
    {
        if (ActionTowerInformationChanged != null)
            ActionTowerInformationChanged();
    }

    private void Update()
    {


        // Always keep the radius of the collider updated to the range of the tower.
        // The range may be updated during gameplay, most likely through downgrades
        if (rangeCollider.radius != Range)
            rangeCollider.radius = Range;

        // we are checking to see if any of the stats of the tower are below zero and if one of them is below 0
        // we will throw a debug error message and set them back to zero
        // this can happen due to a downgrade
        if(Damage < 0)
        {
            Damage = 0;
            Debug.LogError("Damage stat is below ZERO in Tower.cs");
        }
        if (FireRate < 0)
        {
            FireRate = 0.001f;
            Debug.LogError("FireRate stat is below ZERO in Tower.cs - FireRate can not be set to zero either!");
        }
        if (Range < 0)
        {
            Range = 0;
            Debug.LogError("Range stat is below ZERO in Tower.cs");
        }
    }


    // When an enemy enters the range of a tower, add it to the list of available targets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if(e != null)
        {
            enemiesInRange.Add(e);
        }
    }

    // When an enemy exits the range of the towers, remove it from the list
    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e != null && enemiesInRange.Contains(e))
        {
            enemiesInRange.Remove(e);
        }
    }

    /// <summary>
    /// Returns the closet enemy in the tower's range
    /// </summary>
    /// <param name="tower"></param>
    /// <returns></returns>
    public static Enemy GetClosestEnemyToTower(Tower tower)
    {
        float dist = 999;
        Enemy target = null;

        // Check for the closest enemy to the tower that is in the tower range
        var listBuffer = tower.GetEnemiesInRange.ToArray();
        foreach (Enemy e in listBuffer)
        {

            if (e == null)
            {
                // This unit has been destroyed but we didn't update the list yet
                tower.GetEnemiesInRange.Remove(e);
            }
            else
            {
                float distBetweenTargets = Vector2.Distance(new Vector2(tower.gameObject.transform.position.x, tower.gameObject.transform.position.y),
                    new Vector2(e.transform.position.x, e.transform.position.y));

                if (distBetweenTargets < dist)
                {
                    dist = distBetweenTargets;
                    target = e;
                }
            }
        }

        return target;
    }

    /// <summary>
    /// Rotates the gun of the tower to look at the target
    /// </summary>
    /// <param name="tower"></param>
    /// <param name="e"></param>
    public static void LookAtTarget(Tower tower, Enemy e)
    {
        var dir = e.transform.position - tower.gameObject.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tower.gunGO.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

}
