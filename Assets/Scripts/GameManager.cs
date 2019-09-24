using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GoodGuy _goodGuy;

    void Start()
    {
        if (null == _goodGuy)
        {
            _goodGuy = FindObjectOfType<GoodGuy>();
        }
        Assert.IsNotNull<GoodGuy>(_goodGuy);

    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _goodGuy.Move(horizontalInput, verticalInput);
    }
}
