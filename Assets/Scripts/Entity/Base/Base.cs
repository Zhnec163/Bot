using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceBalance)), RequireComponent(typeof(ResourceScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab; // унести в creator
    [SerializeField] private List<Bot> _bots;
    [SerializeField] private int _costOfBot;
    [SerializeField] private Transform _collectZone;

    private bool _isBuildingBase;
    private ResourceBalance _resourceBalance;
    private ResourceScanner _resourceScanner;
    private List<Resource> _processedResources = new();

    public Transform CollectZone => _collectZone;

    private void Awake()
    {
        foreach (Bot bot in _bots)
        {
            bot.Init(this);
            bot.ResourceDelivered += OnResourceDelivered;
        }

        _resourceBalance = GetComponent<ResourceBalance>();
        _resourceScanner = GetComponent<ResourceScanner>();
    }

    private void Update()
    {
        if (_isBuildingBase)
        {
            
        }
        else
        {
            if (_resourceBalance.HasSum(_costOfBot))
                CreateBot();
        }

        List<Bot> freeBots = _bots.Where(bot => bot.IsWorking == false).ToList();

        if (freeBots.Count > 0)
            SendBotsForResources(freeBots);
    }
    
    private void OnDisable()
    {
        _bots.ForEach(bot => { bot.ResourceDelivered -= OnResourceDelivered; });
    }

    private void CreateBot() // вынести в creator
    {
        _resourceBalance.Substract(_costOfBot);
        Bot bot = Instantiate(_botPrefab, _collectZone.transform.position, Quaternion.identity);
        bot.Init(this);
        bot.ResourceDelivered += OnResourceDelivered;
        _bots.Add(bot);
    }

    private void SendBotsForResources(List<Bot> freeBot)
    {
        List<Resource> resources = _resourceScanner.Search();
        resources = resources.Except(_processedResources).ToList();

        foreach (var bot in freeBot)
        {
            if (resources.Count == 0)
                break;

            Resource resource = resources[0];
            _processedResources.Add(resource);
            bot.BringResource(resource);
            resources.Remove(resource);
        }
    }

    private void OnResourceDelivered(Resource resource)
    {
        _resourceBalance.Increment();
        _processedResources.Remove(resource);
    }
}