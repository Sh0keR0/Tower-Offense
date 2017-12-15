using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public int EnemyCount;
    public int EnemyID;
    public float SpawnRate;
    public float CooldownTillNextWave;
}

public class EnemySpawner : MonoBehaviour
{
  

    public static EnemySpawner current;


    public GameObject EnemyPrefab;
    private Wave[] WavesToSpawn;

    private int waveIndex;
    private int spawnCount = 0;
    private float remaningSpawnCooldown = 0;

    private bool isWorking = false;

    public bool IsWorking { get { return isWorking; } }

    private void SpawnEnemy(EnemyData data)
    {

        Enemy enemy_GO =  Instantiate(EnemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
        enemy_GO.InitializeEnemy(data);
    }



    private void Awake()
    {
        isWorking = false;
        current = this;
        waveIndex = 0;
    }

    public void StartSpawner(Wave[] wavesToSpawn = null)
    {
        if (wavesToSpawn != null)
            WavesToSpawn = wavesToSpawn;

        waveIndex = 0;
        spawnCount = 0;
        isWorking = true;
           
    }

    public void StopSpawner()
    {
        isWorking = false;
    }

    public void ContinueSpawner()
    {
        isWorking = true;
    }

    private void Update()
    {
        if (GameController.instance.GamePaused) return;

        // Returns if the enemy spawner is not working
        if (isWorking == false) return;
        
        // Check if we spawned all the waves already
        if(waveIndex > WavesToSpawn.Length - 1)
        {
            // All the waves were spawned so we are stoping the spawner, ready for the next wave.
            StopSpawner();
            return;
        }


        if (spawnCount >= WavesToSpawn[waveIndex].EnemyCount)
        {
            // We spawned all the enemies needed for the current wave and now we are moving to the next wave
            spawnCount = 0;
            remaningSpawnCooldown = WavesToSpawn[waveIndex].CooldownTillNextWave;
            waveIndex++;
            return;
        }

        if (remaningSpawnCooldown > 0)
        {
            remaningSpawnCooldown -= Time.deltaTime;
        }
        else
        {

            // spawn an enemy at the spawner location
            SpawnEnemy(GameData.GetEnemyByID(WavesToSpawn[waveIndex].EnemyID));
            spawnCount++;
            remaningSpawnCooldown = WavesToSpawn[waveIndex].SpawnRate;
        }
    }



}
