using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public static List<Enemy> EnemiesInGame;


    public string Name;
    public float Health;
    public float Speed;
    public int LifeDamage;
    /// <summary>
    /// The value the player will get when the enemy will reach the end of the map
    /// </summary>
    public int MoneyValue;

    private static GameObject PathNodes;
    private Transform currentPathNode;
    private int walkPathIndex = 0;

    private bool init = false;


    public void InitializeEnemy(EnemyData e)
    {

        Health = e.Health;
        Speed = e.Speed;
        LifeDamage = e.LifeDamage;
        MoneyValue = e.MoneyValue;

        GetComponentInChildren<SpriteRenderer>().sprite = GameSpritesLoader.Sprites[e.SpriteName];

        init = true;
    }


    private void Awake()
    {

        if (EnemiesInGame == null)
            EnemiesInGame = new List<Enemy>();

        EnemiesInGame.Add(this);

        if(PathNodes == null)
             PathNodes = GameObject.FindGameObjectWithTag("PathNodes");

        walkPathIndex = 0;

        GetNextPathNode();

    }

    
    private void OnDestroy()
    {
        EnemiesInGame.Remove(this);
    }


    private void GetNextPathNode()
    {
        if (walkPathIndex > PathNodes.transform.childCount - 1)
        {
            currentPathNode = null;
            return;
        }
        currentPathNode = PathNodes.transform.GetChild(walkPathIndex);
        walkPathIndex++;
    }

    private void Update()
    {

        if (GameController.instance.GamePaused || init == false) return;

        if(currentPathNode == null)
        {
            OnEnemyReachedEnd();
            return;
        }

        // walk through the path
        gameObject.transform.position = new Vector2(Mathf.MoveTowards(gameObject.transform.position.x, currentPathNode.position.x, Speed/100),
            Mathf.MoveTowards(gameObject.transform.position.y, currentPathNode.position.y, Speed/100));

        // rotation code
        var dir = currentPathNode.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PathPoint")
        {
            GetNextPathNode();
        }
    }

    private void OnEnemyReachedEnd()
    {

        GameController.instance.LifePoints -= LifeDamage;
        GameController.instance.GainMoney(MoneyValue, gameObject.transform.position);
        Destroy(gameObject);
    }


    /// <summary>
    /// Deal damage to the enemy unit
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {

        if(init == false)
        {
            Debug.LogError("Enemy - trying to take damage before the enemy was initalized!");
            return;
        }

        Health -= amount;
        if(Health <= 0)
        {
            Destroy(gameObject);
        }

    }

}
