using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BadGuy : MonoBehaviour
{
    enum BadGuyState
    {
        Alive,
        Dead
    };
    private BadGuyState _state = BadGuyState.Alive;

    [SerializeField]
    private float _speed = 1.0F;

    private Vector3 startMarker;
    private Vector3 endMarker;
    private float startTime;
    private float journeyLength = 0f;

    private BoxCollider2D _playerCollider;
    private BoxCollider2D _punchCollider;

    void Start()
    {
        // get a reference to the player's collider for punch-checking
        _playerCollider = FindObjectOfType<GoodGuy>().GetComponent<BoxCollider2D>();
        Assert.IsNotNull<BoxCollider2D>(_playerCollider);
        _punchCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        switch (_state)
        {
            case BadGuyState.Alive:
                if (_playerCollider.IsTouching(_punchCollider))
                {
                    GetComponentInChildren<Animator>().SetTrigger("onPunch");
                }
                Move();
                break;
            case BadGuyState.Dead:
                break;
        }
    }

    private void Move()
    {
        if (journeyLength == 0f)
        {
            // pick a new LERP destination
            startMarker = transform.position;
            endMarker = new Vector3(Random.Range(-9f, 9f), transform.position.y - 1f, transform.position.z);
            startTime = Time.time;
            journeyLength = Vector3.Distance(startMarker, endMarker);
        }

        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * _speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);

        if (distCovered >= 1f)
        {
            journeyLength = 0f; // reset
        }
    }

    public bool Hit()
    {
        if (_state != BadGuyState.Alive)
        {
            return false;
        }

        Destroy(this.GetComponent<BoxCollider2D>());
        _state = BadGuyState.Dead;
        GetComponentInChildren<Animator>().SetTrigger("onDeath");
        GetComponent<AudioSource>().Play();
        Destroy(this.gameObject, 5f);
        return true;
    }

    public void ChangeSpeed(float newSpeed)
    {
        this._speed = newSpeed;
    }
}
