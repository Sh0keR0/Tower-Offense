using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{

    // TODOS:
    /*
     * 
     *  Make a playable level and allow the player to lose if they didn't ran out of life before all of the waves

     *  finishing touches:
     *  
     *  More fancy animations?
     *  
     */

    [System.Serializable]
    public struct TowerSpawnInformation
    {
        public int DataID;
        public Vector2 Position;
    }

    public static GameController instance;

    public GameObject BasicTowerPrefab;

    public TowerSpawnInformation[] TowersToSpawn;

    public int StartMoney, StartLifePoints;
    
    public int Money
    {
        get
        {
            return money;
        }

        set
        {
            money = value;
            OnMoneyValueChanged();
        }
    }

    public int LifePoints
    {
        get
        {
            return lifePoints;
        }

        set
        {

            lifePoints = value;
            OnLifePointsChanged();
        }
    }



    public bool GamePaused
    {
        get; protected set;
    }

    private int lifePoints;
    private int money;

    private void Awake()
    {
        Enemy.EnemiesInGame = new List<Enemy>();

        instance = this;

        Time.timeScale = 1f;

        // Initialize game data
        GameData.InitalizeGameData();
    }

    private void Start()
    {
       
        // Set variables to default
        LifePoints = StartLifePoints;
        Money = StartMoney;


        SpawnAllTowers(TowersToSpawn);
    }


    private void SpawnAllTowers(TowerSpawnInformation[] towers )
    {
        foreach (TowerSpawnInformation data in towers)
        {
            SpawnTower(GameData.GetTowerDataByID(data.DataID), data.Position);
        }
    }

    // Spawn a tower and apply the data and attach any scripts needed
    void SpawnTower(TowerData data, Vector2 position)
    {
        // Creates a default tower
        GameObject newTowerGO = Instantiate(BasicTowerPrefab);

        // Set the values of the tower to the data values
        Tower t = newTowerGO.GetComponent<Tower>();
        t.Damage = data.Damage;
        t.Range = data.Range;
        t.FireRate = data.FireRate;
        t.TowerName = data.Name;

        // Check which type the tower is and apply the correct script

        switch(data.type)
        {
            case TowerType.normalTower:
                {
                    newTowerGO.AddComponent<NormalAttack>();
                    break;
                }
            case TowerType.bombTower:
                {
                    newTowerGO.transform.FindChild("Gun").GetComponent<SpriteRenderer>().sprite = GameSpritesLoader.Sprites["bombTower_"];
                    newTowerGO.AddComponent<AoeAttack>();
                    break;
                }
            case TowerType.freezingTower:
                {
                    Debug.LogError("GameController - FreezingTower TowerType not yet implemented");
                    break;
                }
        }


        // Add the downgrade script to the tower
        // Initialize the script, which basically set the basic values and allow you to perform downgrades

        t.TowerDowngrade = newTowerGO.AddComponent<TowerDowngradeScript>();

        t.TowerDowngrade.InitializeDowngradeData(data.DowngradesData, data.DestroyCost);

        

        // Lastly, sets the position of the tower

        newTowerGO.transform.position = position;

    }

    private void Update()
    {
        if (GamePaused) return;
        if (LifePoints <= 0)
        {
            GameOver(true);
            return;
        }
        if(Money <= 300 && Enemy.EnemiesInGame.Count == 0 && EnemySpawner.current.IsWorking == false)
        {
            GameOver(false);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }


    }

    private void OnPopupMessage(string message)
    {
        message = message.ToLower();
        if (message == "continue")
        {
            UnpauseGame();
        } else if (message == "retry" || message == "play again!")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 

        } else if(message == "exit to main menu")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }


    public void PauseGame()
    {
        GamePaused = true;
        List<string> options = new List<string>();
        options.Add("Continue");
        options.Add("Retry");
        PopupMessage.current.Show("PAUSED", "Game is paused", options, OnPopupMessage, true, true);

    }

    public void UnpauseGame()
    {
        if (PopupMessage.current.IsDisplaying) PopupMessage.current.Hide();
        GamePaused = false;
    }


    void GameOver(bool victory)
    {
        GamePaused = true;
        List<string> options = new List<string>();
        if (victory)
        {
            options.Add("Play Again!");
            options.Add("Exit to Main Menu");
            PopupMessage.current.Show("VICTORY", "You have won the game by reducing the life points to zero before you ran out of money! ", options, OnPopupMessage, true, true);
            return;
        }
        else
        {

            options.Add("Retry");
            options.Add("Exit to Main Menu");
            PopupMessage.current.Show("DEFEAT", "You ran out of money! ", options, OnPopupMessage, true, true);
        }
    }

    // Called when the int Money is changed
    void OnMoneyValueChanged()
    {
        GameUI.currentUI.MoneyText.text = Money.ToString() + "$";
    }

    void OnLifePointsChanged()
    {
        GameUI.currentUI.LifePointsText.text = LifePoints.ToString();
    }


    /// <summary>
    /// Gain money and display a floating text
    /// </summary>
    /// <param name="amount">The amount of money to increase</param>
    /// <param name="location">World position for the floating text to appear</param>
    /// <param name="displayFloatingText">If true, display the floating text</param>
    public void GainMoney (int amount, Vector2 location, bool displayFloatingText = true)
    {
        Money += amount;
        if(displayFloatingText) 
             DisplayMoneyGainAsFloatingText(amount, location);

    }

    /// <summary>
    ///  Increase the player money
    /// </summary>
    /// <param name="amount"></param>
    public void GainMoney(int amount)
    {
        GainMoney(amount, Vector2.zero, false);
    }

    private static void DisplayMoneyGainAsFloatingText(int amount, Vector2 location)
    {
        // Display a floating text, green if positive , red if negative  
        Color color;
        string text = "";
        if (amount > 0)
        {
            color = Color.green;
            text += "+ ";
        }
        else
        {
            color = Color.red;
            text += "- ";
        }

        text += Mathf.Abs(amount) + "$";
        FloatingText.current.ShowText(text, color, location, 1f);
    }
}
