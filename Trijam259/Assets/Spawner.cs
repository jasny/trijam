using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefab objects to spawn
    public float spawnRate = 0.5f;
    private float timeSinceLastSpawn; // Time elapsed since the last spawn

    private List<RandomBehaviour> dogs;

    public Slider slider;
    public TextMeshProUGUI dogCountLabel;
    public GameObject menu;

    private void Awake()
    {
        dogs = new List<RandomBehaviour>();
    }

    void Start()
    {
        timeSinceLastSpawn = 0; // Reset time since last spawn
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime; // Update time since last spawn

        // Calculate the delay based on the current spawn rate
        float spawnDelay = 1f / spawnRate;
        
        if (timeSinceLastSpawn >= spawnDelay)
        {
            SpawnPrefab(); // Spawn a new prefab
            timeSinceLastSpawn = 0; // Reset time since last spawn
        }

        dogCountLabel.text = $"{dogs.Count} dogs";

        var happiness = 15f + dogs.Sum(dog => dog.Unhappy);
        slider.value = happiness;

        if (happiness <= 0)
        {
            gameObject.SetActive(false);
            menu.SetActive(true);
        }
    }

    private void SpawnPrefab()
    {
        if (prefabs.Length == 0) return; // Check if there are prefabs to spawn

        int index = Random.Range(0, prefabs.Length); // Pick a random prefab

        var position = transform.position;
        position.x += Random.Range(-1f, 1f);
        var dog = Instantiate(prefabs[index], position, Quaternion.identity); // Instantiate it at the spawner's position
        
        dogs.Add(dog.GetComponent<RandomBehaviour>());
    }
}
