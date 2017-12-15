using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class AoeAttack : MonoBehaviour
{

   // Right now, we are now apply the animations and the attack code in this component, we may want a different behaivor later if we decide to reuse these attack types on other towers!
   // For example, our machine gun tower can reuse the NormalAttack script, but because we are also applying the animations on that script, it's impossible to use different animations on the machine gun tower
   // TLDR: Seperate the animations scripts from the attack types scripts


            
    private Tower tower;
    private float remainingFireRate;
    private float aoeRange = 1f;



    // Animation
    // This is a rocket prefab we create in Awake method
    private GameObject rocketGO;
    private bool hasRocket = false;
    private GameObject currentRocket = null;

    private void Awake()
    {
        tower = GetComponent<Tower>();

        // Creates a prefab game object that is needed for the animation!
        // this prefab will be created each time we want to shoot a rocket.
        Sprite rocketSprite = GameSpritesLoader.Sprites["bombTower_rocket"]; 
        rocketGO = new GameObject("rocket");
        SpriteRenderer r = rocketGO.AddComponent<SpriteRenderer>();
        r.sprite = rocketSprite;
        r.sortingLayerName = "Towers";
        r.sortingOrder = 2;
        rocketGO.AddComponent<RocketAnimation>();
        rocketGO.SetActive(false);
    

    }

    private void Start()
    {
        PrepareRocket();
    }

    private void Update()
    {

        if (remainingFireRate > 0)
        {
            remainingFireRate -= Time.deltaTime;
            return;
        }
        if(hasRocket == false)
            PrepareRocket();

        Enemy e = Tower.GetClosestEnemyToTower(tower);


        if (e != null)
        {
       
            ShootAtTarget(e);
        }
    }

    private void ShootAtTarget(Enemy e)
    {
    

        Tower.LookAtTarget(tower, e);


    
  
        if (hasRocket == false)
        {
            PrepareRocket();
        }


        PlayShootingAnimation(e);

        remainingFireRate = 1 / tower.FireRate;


    }

    // Called from RocketAnimation once the rocket has landed
    public void OnRocketReachedTheTarget(Vector2 target)
    {
        // Deal damage to all enemies in the radius

        // Raycast and find all the enemies hit in a radius from the enemy position
        // This should include the enemy that was selected as target
        Collider2D[] hit = Physics2D.OverlapCircleAll(target, aoeRange);
        foreach (Collider2D enemy in hit)
        {
            Enemy enemyHit = enemy.GetComponent<Enemy>();
            if (enemyHit != null)
            {
                enemyHit.TakeDamage(tower.Damage);
            }
        }
    }

    private void PlayShootingAnimation(Enemy e)
    {
        hasRocket = false;
        currentRocket.GetComponent<RocketAnimation>().ShootAtTarget(e.transform.position, this);
            
    }

    void PrepareRocket()
    {
        if(hasRocket)
        {
            Debug.LogError("Trying to create a rocket when there is already one!");
            return;
        }

        currentRocket = (GameObject)Instantiate(rocketGO);
        currentRocket.transform.position = tower.transform.position;
        currentRocket.transform.rotation = tower.gunGO.transform.rotation;
        currentRocket.transform.SetParent(tower.gunGO.transform, true);

        currentRocket.SetActive(true);
        hasRocket = true;
    }


}
