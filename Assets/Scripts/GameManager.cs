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
        if (Input.GetButton("Cancel"))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
