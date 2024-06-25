using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public float scrollSpeed;

    void Update()
    {
        transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
    }
}
