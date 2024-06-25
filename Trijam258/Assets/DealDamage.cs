using UnityEngine;

public class DealDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        
        var ship = other.gameObject.GetComponent<Ship>();
        if (ship) ship.Explode();
    }
}
