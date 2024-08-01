using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceBalance)), RequireComponent(typeof(ResourceScanner))]
public class TownHall : MonoBehaviour
{
    [SerializeField] private List<Slave> _slaves;

    private ResourceBalance _resourceBalance;
    private ResourceScanner _resourceScanner;
    private List<Resource> _processedResources = new();
    
    private void Awake()
    {
        if (_slaves.Count > 0)
            _slaves.ForEach(slave => slave.Init(this, HandleOnResourceDelivered));
        
        _resourceBalance = GetComponent<ResourceBalance>();
        _resourceScanner = GetComponent<ResourceScanner>();
        _resourceScanner.Init(HandleOnResourcesDiscovered);
    }

    private void HandleOnResourcesDiscovered(List<Resource> resourcesToProcess)
    {
        if (_slaves.Count == 0)
            return;

        List<Slave> _freeSlave = _slaves.Where(slave => slave.IsWorking == false).ToList();

        if (_freeSlave.Count > 0)
        {
            resourcesToProcess = resourcesToProcess.Except(_processedResources).ToList();

            if (resourcesToProcess.Count == 0)
                return;
            
            _freeSlave.ForEach(slave =>
            {
                if (resourcesToProcess.Count > 0)
                {
                    Resource resource = resourcesToProcess[0];
                    _processedResources.Add(resource);
                    slave.Collect(resource);
                    resourcesToProcess.Remove(resource);
                }
            });
        }
    }

    private void HandleOnResourceDelivered(Resource resource)
    {
        _processedResources.Remove(resource);
        _resourceBalance.Increment(); 
    }
}
