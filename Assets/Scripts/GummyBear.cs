using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GummyBear : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _spinSpeed = 500f;

    void Start()
    {
        
    }

    void Update()
    {
        // move independent of rotation (always constant direction)
        Vector3 movementVector = Vector3.up * _moveSpeed * Time.deltaTime;
        transform.position = transform.position + movementVector;
        transform.Rotate(0f, 0f, _spinSpeed * Time.deltaTime);

        // remove object once it has left the viewable screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height || screenPosition.y < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BadGuy>(out BadGuy badguy) && badguy.Hit())
        {
            Destroy(this.gameObject);
        }
    }
}
