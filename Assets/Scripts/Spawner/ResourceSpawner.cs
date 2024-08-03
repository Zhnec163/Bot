using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnTimeStep;

    private ObjectPool<Resource> _pool;
    private WaitForSeconds _spawnDelay;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: HandleActionOnCreate,
            actionOnGet: HandleActionOnGet,
            actionOnRelease: obj => HandleActionOnRelease(obj),
            actionOnDestroy: obj => HandleActionOnDestroy(obj));
        
        _spawnDelay = new WaitForSeconds(_spawnTimeStep);
        StartCoroutine(SpawningResources());
    }

    private Resource HandleActionOnCreate()
    {
        Resource resource = Instantiate(_prefab, CalculateSpawnPosition(), Quaternion.identity);
        resource.Init(this);
        return resource;
    }
    
    private void HandleActionOnGet(Resource resource)
    {
        resource.transform.position = CalculateSpawnPosition();
        resource.gameObject.SetActive(true);
    }

    private void HandleActionOnRelease(Resource resource)
    {
        resource.gameObject.SetActive(false);
    }

    private void HandleActionOnDestroy(Resource resource)
    {
        Destroy(resource.gameObject);
    }

    public void Release(Resource resource)
    {
        _pool.Release(resource);
    }

    private IEnumerator SpawningResources()
    {
        while (enabled)
        {
            yield return _spawnDelay;
            _pool.Get();
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        return new Vector3(Random.Range(transform.position.x - _spawnRadius, transform.position.x + _spawnRadius),
            _prefab.transform.position.y,
            Random.Range(transform.position.z - _spawnRadius, transform.position.z + _spawnRadius));
    }
}