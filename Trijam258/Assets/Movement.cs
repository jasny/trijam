using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    public Transform jetStream;
    public float jetStreamScaleFactor = 5;
    private Vector3 _jetStreamScale;

    private Camera _camera;

    private void Start()
    {
        if (jetStream) _jetStreamScale = jetStream.localScale;
        _camera = Camera.main;
    }
    
    private void Update()
    {
        var moveDirection = Vector3.zero;

        if (Input.GetKey(moveUp))
        {
            moveDirection.y += 1;
        }
        if (Input.GetKey(moveDown))
        {
            moveDirection.y -= 1;
        }
        if (Input.GetKey(moveLeft))
        {
            moveDirection.x -= 1;
        }
        if (Input.GetKey(moveRight))
        {
            moveDirection.x += 1;
        }

        var delta = moveDirection.normalized * (moveSpeed * Time.deltaTime);
        var newPosition = transform.position + delta;

        // Ensure the new position is within the screen bounds
        var newPositionInViewport = _camera.WorldToViewportPoint(newPosition);
        newPositionInViewport.x = Mathf.Clamp(newPositionInViewport.x, 0f, 1f);
        newPositionInViewport.y = Mathf.Clamp(newPositionInViewport.y, 0f, 1f);
        
        // Convert the position back to world space and apply it
        transform.position = _camera.ViewportToWorldPoint(newPositionInViewport);

        if (jetStream)
        {
            var scale = _jetStreamScale;
            scale *= Mathf.Pow(2, delta.magnitude * jetStreamScaleFactor); // Use magnitude for uniform scaling
            
            jetStream.localScale = scale;
        }
    }
}
