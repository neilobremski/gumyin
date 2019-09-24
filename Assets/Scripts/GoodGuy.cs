using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GoodGuy : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 10.0f;

    [SerializeField]
    private float _shootSpeed = 1;

    [SerializeField]
    private GameObject _projectilePrefab = null;

    private float _nextFireTime = 0;
    private Animator _anim;

    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        Assert.IsNotNull<Animator>(_anim);
    }

    public void Move(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new Vector3(
            horizontalInput * _moveSpeed * Time.deltaTime,
            verticalInput * _moveSpeed * Time.deltaTime);
        transform.Translate(movement);
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    /// <summary>
    /// Fire ze gummybear! But I am le' tired ... well then take a nap. AND ZEN FIRE ZE GUMMYBEAR!
    /// </summary>
    public void Shoot()
    {
        if (_nextFireTime > Time.time)
        {
            return;
        }

        GetComponent<AudioSource>().Play();

        _anim.SetTrigger("onThrow");
        Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        _nextFireTime = Time.time + _shootSpeed;
    }

    public void OnDeathComplete()
    {
        Debug.Break();
    }
}
