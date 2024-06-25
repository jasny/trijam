using System;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{
    public GameObject explosion;
    public GameObject[] related;

    public bool isHot;

    public UnityEvent onExplode;

    public void Explode()
    {
        gameObject.SetActive(false);

        foreach (var rel in related)
        {
            rel.SetActive(false);
        }
        
        explosion.SetActive(true);
        
        Invoke(nameof(TriggerExplodeEvent), 1f);
    }
    
    private void TriggerExplodeEvent()
    {
        Debug.Log("On explode");
        onExplode?.Invoke();
    }    
}
