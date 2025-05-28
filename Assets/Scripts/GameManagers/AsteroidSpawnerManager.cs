using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerManager : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab;
    public float spawnInterval = 3f;
    public int maxAsteroids = 10;
    public int minMineralsPerAsteroid = 1;
    public int maxMineralsPerAsteroid = 5;

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin = new Vector2(-10f, -4f);
    public Vector2 spawnAreaMax = new Vector2(10f, 4f);

    [Header("Avoid Bases")]
    public List<Transform> basePositions;
    public float avoidRadius = 2f;

    private List<GameObject> spawnedAsteroids = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() // restricts astroid to spawn near Bases => change radius with "avoidRadius"
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Remove destroyed asteroids
            spawnedAsteroids.RemoveAll(item => item == null);

            if (spawnedAsteroids.Count < maxAsteroids)
            {
                Vector2 spawnPos = Vector2.zero;
                bool foundValid = false;

                for (int attempt = 0; attempt < 10; attempt++) // 10 attemps to spawn in a suitable position
                {
                    spawnPos = new Vector2(
                        Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                        Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                    );

                    bool tooCloseToBase = false;

                    foreach (Transform basePos in basePositions)
                    {
                        if (Vector2.Distance(spawnPos, basePos.position) < avoidRadius)
                        {
                            tooCloseToBase = true;
                            break;
                        }
                    }

                    if (!tooCloseToBase)
                    {
                        foundValid = true;
                        break;
                    }
                }

                if (foundValid)
                {
                    GameObject asteroidGO = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
                    Asteroid asteroid = asteroidGO.GetComponent<Asteroid>();
                    asteroid.totalMinerals = Random.Range(minMineralsPerAsteroid, maxMineralsPerAsteroid);
                    AsteroidManager.Instance.RegisterAsteroid(asteroid);
                    spawnedAsteroids.Add(asteroidGO);
                }
                else
                {
                    Debug.LogWarning("could not find valid loctaion.");
                }
            }
        }
    }
}
