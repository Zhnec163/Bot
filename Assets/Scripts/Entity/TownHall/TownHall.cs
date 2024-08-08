using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceBalance)), RequireComponent(typeof(ResourceScanner))]
public class TownHall : MonoBehaviour
{
    [SerializeField] private List<Slave> _slaves;
    [SerializeField] private float _updateRate;

    private ResourceBalance _resourceBalance;
    private ResourceScanner _resourceScanner;
    private List<Resource> _processedResources = new();
    private WaitForSeconds _updateDelay;
    
    private void Awake()
    {
        _slaves.ForEach(slave => slave.Init(this));
        _resourceBalance = GetComponent<ResourceBalance>();
        _resourceScanner = GetComponent<ResourceScanner>();
        _updateDelay = new WaitForSeconds(_updateRate);
        StartCoroutine(Working());
    }
    
    public void AddCollectedResource(Resource resource)
    {
        _processedResources.Remove(resource);
        _resourceBalance.Increment(); 
    }

    private IEnumerator Working()
    {
        while (enabled)
        {
            yield return _updateDelay;
            List<Slave> _freeSlave = _slaves.Where(slave => slave.IsWorking == false).ToList();

            if (_freeSlave.Count > 0)
            {
                List<Resource> resources = _resourceScanner.Search();
                resources = resources.Except(_processedResources).ToList();
                
                foreach (var slave in _freeSlave)
                {
                    if (resources.Count == 0)
                        break;
                    
                    Resource resource = resources[0];
                    _processedResources.Add(resource);
                    slave.BringResource(resource);
                    resources.Remove(resource);
                }
            }
        }
    }
}
