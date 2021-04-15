using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] debreePrefabs;

    public PlayerController playerReference;

    public GameObject[] enemiesArray;

    public Transform enemySpawnY;

    public bool isGameRunning;

    public List<GameObject> spawnedEnemies;

    public int currentWave = 0;
    public int[] wavesRowsToInstantiate;

    private Dictionary<DebreeEnum, GameObject> debreePrefabDictionary;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        debreePrefabDictionary = new Dictionary<DebreeEnum, GameObject>();

        foreach (GameObject debreePrefab in debreePrefabs)
        {
            var debreeType = debreePrefab.GetComponent<TechDebree>();

            debreePrefabDictionary.Add(debreeType.type, debreePrefab);
        }

        isGameRunning = true;

        StartCoroutine(WaveSpawner());
    }

    public void Update()
    {
        if(spawnedEnemies.Count == 0)
        {
            currentWave++;

            StartCoroutine(WaveSpawner());
        }
    }

    public void InstantiateDebree(Vector3 position)
    {
        Instantiate(GetRandomDebree(), position, Quaternion.identity);
    }

    public GameObject GetRandomDebree()
    {
        int maxDebreeRange = Mathf.RoundToInt(Mathf.Clamp((playerReference.playerLevel + 2), 1, 5));

        DebreeEnum debree = (DebreeEnum)Mathf.RoundToInt(Random.Range(1, maxDebreeRange));

        return debreePrefabDictionary[debree];
    }

    public void GameOver()
    {
        //disable player controller, show game over screen with restart button.
        PauseGame();

        Debug.Log("Game Over!");
    }

    public void PauseGame()
    {
        isGameRunning = false;
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        isGameRunning = true;
        Time.timeScale = 1;
    }

    public IEnumerator WaveSpawner()
    {
        int waveRowSpawned = 0;

        yield return new WaitForSeconds(2f);

        while (isGameRunning && waveRowSpawned < wavesRowsToInstantiate[currentWave])
        {
            var enemiesToSpawn = Mathf.Clamp(Random.Range(playerReference.playerLevel, 17 + playerReference.playerLevel), playerReference.playerLevel, 17);

            GameObject[] enemyLine = new GameObject[18];

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                var enemyPosition = Random.Range(0, 17);
                
                enemyLine[enemyPosition] = enemiesArray[Mathf.RoundToInt(Random.Range(0, 1))];
            }

            for(int i = 0; i < enemyLine.Length; i++)
            {
                if(enemyLine[i] != null)
                {
                    spawnedEnemies.Add(Instantiate(enemyLine[i], new Vector3((-8.5f + i), enemySpawnY.transform.position.y, 0), Quaternion.identity));
                }
            }

            yield return new WaitForSeconds(2f);

            waveRowSpawned++;
        }

        StopCoroutine(WaveSpawner());
    }

    public void WinScreen()
    {
        PauseGame();

        //show win screen
        
    }

    public void ShowWaveNumeber()
    {
        //show wave number

    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }
}
