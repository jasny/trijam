using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    private Camera _camera;
    private float _lastSpawnTime;
    private float _spawnInterval;
    private float _startTime;

    public float maxSpawnInterval = 1f;
    public float spawnIntervalDecay = 0.1f;
    public float minSpawnInterval = 0.02f;

    private void Start()
    {
        _camera = Camera.main;
        _startTime = Time.time;
        CalculateSpawnInterval();
    }

    private void Update()
    {
        if (!(Time.time - _lastSpawnTime > _spawnInterval)) return;
        
        SpawnPrefab();
        CalculateSpawnInterval();
        _lastSpawnTime = Time.time;
    }

    private void SpawnPrefab()
    {
        var screenHeightInWorld = 2f * _camera.orthographicSize;
        var randomY = Random.Range(-screenHeightInWorld / 2, screenHeightInWorld / 2);

        var spawnPosition = new Vector3(transform.position.x, _camera.transform.position.y + randomY, transform.position.z);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    private void CalculateSpawnInterval()
    {
        var t = Time.time - _startTime;
        _spawnInterval = maxSpawnInterval * Mathf.Exp(-spawnIntervalDecay * t) + minSpawnInterval;
    }
}
