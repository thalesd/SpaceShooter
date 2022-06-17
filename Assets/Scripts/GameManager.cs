using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject PauseCanvas;

    public GameObject GameOverCanvas;
    public GameObject YouWinCanvas;

    private bool isWaveSpawning = false;

    public Text currentWaveIndicator;

    void Awake()
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

        GameOverCanvas.SetActive(false);
        YouWinCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
    }

    public void Update()
    {
        if(!isWaveSpawning && spawnedEnemies.Count == 0)
        {
            StartCoroutine(WaveSpawner());

            currentWave++;
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

        PauseCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
    }

    public void PauseGame()
    {
        PauseCanvas.SetActive(true);

        isGameRunning = false;
    }

    public void UnPauseGame()
    {
        PauseCanvas.SetActive(false);

        isGameRunning = true;
    }

    public IEnumerator WaveSpawner()
    {
        int waveRowSpawned = 0;
        isWaveSpawning = true;

        StartCoroutine(ShowWaveNumber());

        yield return new WaitForSeconds(2f);

        while (isGameRunning && currentWave < wavesRowsToInstantiate.Length && waveRowSpawned < wavesRowsToInstantiate[currentWave])
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

        isWaveSpawning = false;

        StopCoroutine(nameof(WaveSpawner));
    }

    public void WinScreen()
    {
        PauseGame();

        PauseCanvas.SetActive(false);
        YouWinCanvas.SetActive(true);
    }

    public IEnumerator ShowWaveNumber()
    {
        currentWaveIndicator.text = "Wave " + (currentWave + 1).ToString() + " / " + wavesRowsToInstantiate.Length.ToString();

        currentWaveIndicator.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        currentWaveIndicator.gameObject.SetActive(false);
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
