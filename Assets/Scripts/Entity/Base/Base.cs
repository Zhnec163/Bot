using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceBalance)), RequireComponent(typeof(ResourceScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private int _costOfBot;
    [SerializeField] private int _costOfBase;
    [SerializeField] private Transform _collectZone;
    [SerializeField] private Transform _botSpawn;
    [SerializeField] private List<Bot> _attachedBots;
    [SerializeField] private Flag _flag;
    [SerializeField] private Bot _botPrefab;

    private List<Bot> _bots = new();
    private Bot _buildingBot;
    private bool _produceBase;
    private ResourceBalance _resourceBalance;
    private ResourceScanner _resourceScanner;
    private List<Resource> _processedResources = new();

    private void Awake()
    {
        if (_attachedBots != null)
            _bots.AddRange(_attachedBots);

        foreach (Bot bot in _bots)
            InitializeBot(bot);

        _resourceBalance = GetComponent<ResourceBalance>();
        _resourceScanner = GetComponent<ResourceScanner>();
    }

    private void Update()
    {
        Produce();
        GatherResources();
    }

    private void OnDisable() => _bots.ForEach(bot => { bot.ResourceDelivered -= OnResourceDelivered; });

    public void Init(Bot bot)
    {
        InitializeBot(bot);
        _bots.Add(bot);
    }

    public void TryBuildBase(Vector3 position)
    {
        if (_produceBase)
        {
            _flag.transform.position = position;
        }
        else
        {
            _produceBase = true;
            _flag.transform.position = position;
            _flag.gameObject.SetActive(true);
        }
    }

    private void Produce()
    {
        if (_produceBase)
        {
            if (_resourceBalance.HasSum(_costOfBase))
            {
                _resourceBalance.Substract(_costOfBase);
                BuildBase();
                _produceBase = false;
            }
        }
        else
        {
            if (_resourceBalance.HasSum(_costOfBot))
            {
                _resourceBalance.Substract(_costOfBot);
                BuildBot();
            }
        }
    }

    private void InitializeBot(Bot bot)
    {
        bot.Init(_collectZone.position);
        bot.ResourceDelivered += OnResourceDelivered;
    }

    private void BuildBase()
    {
        _buildingBot = GetFreeBots()[0];
        _buildingBot.ConstructionCompleted += OnConstructionCompleted;
        _buildingBot.BuildBase(_flag.transform.position);
    }

    private void BuildBot()
    {
        Bot bot = Instantiate(_botPrefab, _botSpawn.position, _botPrefab.transform.rotation);
        InitializeBot(bot);
        _bots.Add(bot);
    }

    private void GatherResources()
    {
        List<Bot> freeBots = GetFreeBots();

        if (freeBots.Count > 0)
        {
            List<Resource> resources = _resourceScanner.Search();
            resources = resources.Except(_processedResources).ToList();

            foreach (var bot in freeBots)
            {
                if (resources.Count == 0)
                    break;

                Resource resource = resources[0];
                _processedResources.Add(resource);
                bot.BringResource(resource);
                resources.Remove(resource);
            }
        }
    }

    private void OnConstructionCompleted()
    {
        _bots.Remove(_buildingBot);
        _flag.gameObject.SetActive(false);
        _buildingBot.ConstructionCompleted -= OnConstructionCompleted;
        _flag.gameObject.SetActive(false);
        _buildingBot = null;
    }

    private List<Bot> GetFreeBots() => _bots.Where(bot => bot.IsWorking == false).ToList();

    private void OnResourceDelivered(Resource resource)
    {
        _resourceBalance.Increment();
        _processedResources.Remove(resource);
    }
}