using UnityEngine;
using Random = UnityEngine.Random;

public class Drift : MonoBehaviour
{
    public float minMoveSpeed = 10f;
    public float maxMoveSpeed = 10f;
    private float moveSpeed;
    private Camera mainCamera;
    private float leftBoundaryOffset = 5f; // How far left of the camera the object can go before being destroyed

    private void Awake()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        mainCamera = Camera.main; // Cache the main camera
    }
    
    private void Update()
    {
        transform.Translate(Vector3.left * (moveSpeed * Time.deltaTime), Space.World);

        // Calculate the left boundary of the camera's view in world coordinates and subtract the offset
        float leftBoundary = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x - leftBoundaryOffset;

        // Check if the object has moved past this boundary
        if (transform.position.x < leftBoundary)
        {
            Destroy(gameObject); // Destroy the object
        }
    }
}
