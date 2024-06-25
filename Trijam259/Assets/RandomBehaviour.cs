using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBehaviour : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    public Vector3 initialLocation = new (0, 0, -18.5f);
    public Vector3 gridLocation = new (0, 0f, 0);
    public Vector2 gridSize = new (28, 36); // Size of the grid
    public float rotationSpeed = 3f; // Speed of rotation
    public float idleSitTime = 3f; // Sit down if idling longer than this
    public float walkSpeed = 0.5f; // Speed of walking
    public float runSpeed = 2.5f; //
    public float runDistance = 10f;
    public float minIdle = 1f;
    public float maxIdle = 15f;

    private Vector3 _destination;
    private float _speed;
    private float _timeMoving;
    
    private float _idleTime;
    private bool _isIdle = false;
    private static readonly int SpeedF = Animator.StringToHash("Speed_f");
    private static readonly int SitB = Animator.StringToHash("Sit_b");
    private static readonly int BarkB = Animator.StringToHash("Bark_b");

    public GameObject hungryIndicator;
    private float _timeToHungry;
    private bool _isBarking;

    public float Unhappy => _isBarking ? Mathf.Clamp(_timeToHungry / 10f, -1f, 0f) : 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        _timeToHungry = Random.Range(10f, 60f);
        
        var indicator = Instantiate(hungryIndicator, transform, true);
        indicator.transform.localPosition = new Vector3(0f, 0.24f, 0.11f);
        indicator.SetActive(false);
        hungryIndicator = indicator;
    }

    private void Start()
    {
        initialLocation.x = transform.position.x;
        
        _destination = initialLocation;
        _speed = runSpeed;
    }

    private void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void Idle()
    {
        _isIdle = true;
        _idleTime = Random.Range(minIdle, maxIdle);

        _animator.SetFloat(SpeedF, 0);
        if (_idleTime > idleSitTime + 2f) _animator.SetBool(SitB, true);
    }

    private void Wander()
    {
        var x = Random.Range(-0.5f * gridSize.x, 0.5f * gridSize.x);
        var z = Random.Range(-0.5f * gridSize.y, 0.5f * gridSize.y);
        
        _destination = gridLocation + new Vector3(x, 0, z);
        _timeMoving = 0;
        
        var distance = Vector3.Distance(transform.position, _destination);
        _speed = distance < runDistance ? walkSpeed : runSpeed;
        
        _isIdle = false;
    }
    
    private void Update()
    {
        _timeToHungry -= Time.deltaTime;
        
        if (_isIdle)
        {
            HandleIdleBehavior();
        }
        else
        {
            MoveTowardsDestination();
        }
    }

    private void FixedUpdate()
    {
        ResetVelocity();
    }

    private void HandleIdleBehavior()
    {
        _idleTime -= Time.deltaTime;

        if (_timeToHungry <= 0)
        {
            Bark();
            return;
        }
        
        if (_idleTime <= idleSitTime) _animator.SetBool(SitB, false);
        if (_idleTime <= 0) Wander();
    }

    private void Bark()
    {
        if (_isBarking) return;
        
        _animator.SetBool(SitB, false);
        _animator.SetBool(BarkB, true);

        hungryIndicator.SetActive(true);
        _audioSource.Play();
        
        _isBarking = true;
    }

    private void Feed()
    {
        _animator.SetBool(BarkB, false);
        _audioSource.Stop();

        hungryIndicator.SetActive(false);

        _timeToHungry = Random.Range(15f, 90f);
        
        _isBarking = false;
    }

    private void OnMouseDown()
    {
        if (_isBarking) Feed();
    }

    private void MoveTowardsDestination()
    {
        _timeMoving += Time.deltaTime;

        if (_timeMoving > 20f)
        {
            Wander();
        }
        
        if (Vector3.Distance(transform.position, _destination) < 0.01f)
        {
            if (_destination == initialLocation)
            {
                Wander();
            }
            else
            {
                Idle();
            }
            return;
        }
        
        var direction = (_destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Start moving if almost facing the destination
        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction)) < 5f)
        {
            _animator.SetFloat(SpeedF, _speed); // Set speed when actually starting to move
            transform.position = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
        }
    }
}
