using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float tileSizeZ;

    private GameObject[] tiles = new GameObject[2];
    private float newPositionX;
    private Camera mainCamera;
    private float screenWidthWorldPos;

    void Start()
    {
        mainCamera = Camera.main;
        float cameraHeight = mainCamera.orthographicSize * 2;
        screenWidthWorldPos = cameraHeight * mainCamera.aspect;

        tiles[0] = transform.GetChild(0).gameObject;
        tileSizeZ = tiles[0].GetComponent<SpriteRenderer>().bounds.size.x;
        newPositionX = tiles[0].transform.position.x + tileSizeZ - 1;

        // Clone the tile and position it next to the original
        var clonedTile = Instantiate(tiles[0], transform);
        clonedTile.transform.position = new Vector3(newPositionX, tiles[0].transform.position.y, tiles[0].transform.position.z);
        clonedTile.transform.localScale = new Vector3(-1, 1, 1);
        tiles[1] = clonedTile;
    }

    void Update()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);

            if (tiles[i].transform.position.x < mainCamera.transform.position.x - screenWidthWorldPos / 2 - tileSizeZ)
            {
                float rightMostXPosition = tiles[1 - i].transform.position.x + tileSizeZ - 1;
                tiles[i].transform.position = new Vector3(rightMostXPosition, tiles[i].transform.position.y, tiles[i].transform.position.z);
            }
        }
    }
}
