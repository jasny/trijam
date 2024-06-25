using UnityEngine;
using Random = UnityEngine.Random;

public class Rotate : MonoBehaviour
{
    public float rotationSpeedMin;
    public float rotationSpeedMax;
    private float _rotationSpeed;

    private void Awake()
    {
        _rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    private void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
