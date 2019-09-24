using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Periodically instantiate a new game object, such as an enemy, using a prefab.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// List of prefabs to use as templates for instantiating objects.
    /// </summary>
    [SerializeField]
    private GameObject[] _prefabs = null;

    /// <summary>
    /// Minimum position of new objects.
    /// </summary>
    [SerializeField]
    private Vector3 _positionMin = Vector3.zero;

    /// <summary>
    /// Maximum position of new objects.
    /// </summary>
    [SerializeField]
    private Vector3 _positionMax = Vector3.zero;

    /// <summary>
    /// Whether the position is absolute world coordinates or a percentage.
    /// </summary>
    [SerializeField]
    private bool _positionIsPercentage = false;

    /// <summary>
    /// Minimum number of seconds before a new object is spawned.
    /// </summary>
    [SerializeField]
    private float _delayMin = 1f;

    /// <summary>
    /// Maximum number of seconds before a new object is spawned.
    /// </summary>
    [SerializeField]
    private float _delayMax = 10f;

    /// <summary>
    /// Enables spawning from the editor and is used to control the coroutine
    /// loop.
    /// </summary>
    [SerializeField]
    private bool _spawning = true;

    /// <summary>
    /// Reference to running co-routine, if any.
    /// </summary>
    IEnumerator _spawningCoRoutine = null;

    void Start()
    {
        Assert.IsNotNull<GameObject[]>(_prefabs);
        if (_spawning)
        {
            StartSpawning();
        }
    }

    /// <summary>
    /// Main spawning loop.
    /// </summary>
    /// <returns>Nothing special.</returns>
    IEnumerator Spawning()
    {
        while (_spawning)
        {
            float seconds = Random.Range(_delayMin, _delayMax);
            yield return new WaitForSeconds(seconds);

            if (_spawning)
            {
                SpawnOne();
            }
        }

        // since Unity runs this on the main thread, this is guaranteed to be
        // prior to any call to StartSpawning() or if it happens after then
        // _spawning will be TRUE and the loop above continues
        _spawningCoRoutine = null;
    }

    /// <summary>
    /// Makes sure the spawning co-routine is running. Note that new objects
    /// will NOT be spawned immediately.
    /// </summary>
    public void StartSpawning()
    {
        if (null == _spawningCoRoutine)
        {
            _spawningCoRoutine = Spawning();
            StartCoroutine(_spawningCoRoutine);
        }
        _spawning = true;
    }

    public void StopSpawning()
    {
        _spawning = false;
    }

    /// <summary>
    /// Instantiates a random object from the list of prefabs.
    /// </summary>
    public void SpawnOne()
    {
        int prefabIndex = Random.Range(0, _prefabs.Length);
        Vector3 position = new Vector3(
            Random.Range(_positionMin.x, _positionMax.x),
            Random.Range(_positionMin.y, _positionMax.y),
            Random.Range(_positionMin.z, _positionMax.z));

        // convert percentages to actual world coordinates
        if (_positionIsPercentage)
        {
            float height = Camera.main.orthographicSize * 2.0f;
            float width = height * Camera.main.aspect;
            position.x *= width;
            position.y *= height;
        }

        Quaternion rotation = Quaternion.identity;  // no special rotation at this time
        Instantiate(_prefabs[prefabIndex], position, rotation);
    }

    /// <summary>
    /// Allows other objects to modify the delay. This will take effect only
    /// after the previous delay is completed.
    /// </summary>
    /// <param name="minSeconds">Minimum seconds before new object is spawned.</param>
    /// <param name="maxSeconds">Maximum seconds before new object is spawned.</param>
    public void ChangeDelay(float minSeconds, float maxSeconds)
    {
        _delayMin = minSeconds;
        _delayMax = maxSeconds;
    }
}
