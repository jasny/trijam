using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AstroidType
{
    Normal,
    Hot,
    Cold
}

public class Astroid : MonoBehaviour
{
    private SpriteRenderer _sprite;

    [SerializeField] private AstroidType type;

    public Color colorNormal;
    public Color colorHot;
    public Color colorCold;

    public float chanceNormal = 0.5f;
    
    public GameObject crumbleEffect;
    
    public AstroidType Type
    {
        get => type;
        set
        {
            type = value;
            UpdateSpriteColor();
        }
    }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.one * Random.Range(0.5f, 2f);

        AssignRandomType();
        
        UpdateSpriteColor();
    }
    
    private void AssignRandomType()
    {
        if (Random.value < chanceNormal) return;
        
        // Define the time scale for a full cycle
        float cyclePeriod = 30f; // Full cycle duration in seconds

        // Calculate the sine of the current time, oscillating between -1 and 1
        float sinValue = Mathf.Sin((Time.time % cyclePeriod) / cyclePeriod * 2 * Mathf.PI);

        // Adjust the sinValue to range between 0 and 1 for probability comparison
        float adjustedSinValue = (sinValue + 1) / 2;

        // Determine the asteroid type based on the adjustedSinValue
        if (Random.value < adjustedSinValue)
        {
            type = AstroidType.Hot;
        }
        else
        {
            type = AstroidType.Cold;
        }
    }

    private void UpdateSpriteColor()
    {
        _sprite.color = type switch
        {
            AstroidType.Normal => colorNormal,
            AstroidType.Hot => colorHot,
            AstroidType.Cold => colorCold,
        };
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var ship = other.gameObject.GetComponent<Ship>();
        if (!ship) return;

        var shipCanDestroy = ship.isHot ? AstroidType.Hot : AstroidType.Cold;
        if (type == shipCanDestroy)
        {
            Explode();
        }
        else
        {
            ship.Explode();;
        }
    }

    private void Explode()
    {
        var effect = Instantiate(crumbleEffect, transform.position, Quaternion.identity);
        effect.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        gameObject.SetActive(false);
    }
}
