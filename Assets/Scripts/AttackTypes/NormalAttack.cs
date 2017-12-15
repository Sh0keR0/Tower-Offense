using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class NormalAttack : MonoBehaviour
{

    /*
        Normal attack, will simply attack enemies with no special effect 
        Display a "Flashing" fire scripted "animation".
        It's not a real animation, just a gameObject getting enabled / disabled over and over
        

    */

     // The tower that this component is attached to
     
    private Tower tower;
    private float remainingFireRate;
    private GameObject gunGO, gunFireGO;

    private void Awake()
    {
        tower = GetComponent<Tower>();
        gunGO = transform.FindChild("Gun").gameObject;
        gunFireGO = gunGO.gameObject.transform.FindChild("GunFire").gameObject;
    }

    private void Update()
    {
        Enemy target = Tower.GetClosestEnemyToTower(tower);

        if (target != null)
        {
            ShootAtTarget(target);
        }


    }

    private void ShootAtTarget(Enemy e)
    {

        if (remainingFireRate > 0)
        {
            remainingFireRate -= Time.deltaTime;
            return;
        }

        Tower.LookAtTarget(tower, e);



        // Small visual effect, TEMPORARY
        gunFireGO.SetActive(true);
        Invoke("FireAnimation", 0.5f);

        // Apply the damage to the enemy and set the timer back
        e.TakeDamage(tower.Damage);
        remainingFireRate = 1 / tower.FireRate;


    }

    void FireAnimation()
    {
        gunFireGO.SetActive(false);
    }

}
