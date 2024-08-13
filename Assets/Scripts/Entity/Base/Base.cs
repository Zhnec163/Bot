using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceBalance)), RequireComponent(typeof(BotCreator)), RequireComponent(typeof(BaseCreator)), RequireComponent(typeof(ResourceScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private Transform _collectZone;
    [SerializeField] private Transform _botSpawn;
    [SerializeField] private List<Bot> _attachedBots;
    [SerializeField] private Flag _flag;

    private bool _isBuildBase;
    private List<Bot> _bots = new();
    private ResourceBalance _resourceBalance;
    private ResourceScanner _resourceScanner;
    private BaseCreator _baseCreator;
    private BotCreator _botCreator;

    public bool IsBuildBase => _isBuildBase;

    private void Awake()
    {
        if (_attachedBots != null)
            _bots.AddRange(_attachedBots);

        foreach (Bot bot in _bots)
            InitializeBot(bot);

        _resourceBalance = GetComponent<ResourceBalance>();
        _resourceScanner = GetComponent<ResourceScanner>();
        _baseCreator = GetComponent<BaseCreator>();
        _botCreator = GetComponent<BotCreator>();
    }

    private void Update()
    {
        Production();
        GatherResource();
    }

    private void OnDisable() => _bots.ForEach(bot => { bot.ResourceDelivered -= OnResourceDelivered; });

    public void Init(Bot bot, ResourceFinder resourceFinder)
    {
        _resourceScanner.Init(resourceFinder);
        InitializeBot(bot);
        _bots.Add(bot);
    }

    public void ChangeFlagPosition(Vector3 position)
    {
        _flag.transform.position = position;
    }

    public void StartBuildingBase(Vector3 position)
    {
        _isBuildBase = true;
        ChangeFlagPosition(position);
        // _flag.gameObject.SetActive(true);
    }

    private void Production()
    {
        if (_isBuildBase)
        {
            if (_resourceBalance.HasSum(_baseCreator.Cost))
            {
                if (TryGetFreeBot(out Bot bot))
                {
                    _resourceBalance.Substract(_baseCreator.Cost);
                    bot.ConstructionCompleted += OnConstructionCompleted;
                    bot.BuildBase(_baseCreator, _flag.transform.position, _resourceScanner.ResourceFinder);
                    _isBuildBase = false;
                }
            }
        }
        else
        {
            if (_resourceBalance.HasSum(_botCreator.Cost))
            {
                _resourceBalance.Substract(_botCreator.Cost);
                Bot bot = _botCreator.Create(_botSpawn.position);
                InitializeBot(bot);
                _bots.Add(bot);
            }
        }
    }

    private void InitializeBot(Bot bot)
    {
        bot.Init(_collectZone.position);
        bot.ResourceDelivered += OnResourceDelivered;
    }

    private void GatherResource()
    {
        if (TryGetFreeBot(out Bot bot))
        {
            if (_resourceScanner.TryGetNearestResource(out Resource resource))
                bot.BringResource(resource);
        }
    }

    private void OnConstructionCompleted(Bot bot)
    {
        _bots.Remove(bot);
        _flag.gameObject.SetActive(false);
        bot.ConstructionCompleted -= OnConstructionCompleted;
    }

    private bool TryGetFreeBot(out Bot bot)
    {
        bot = _bots.FirstOrDefault(bot => bot.IsWorking == false);

        if (bot == null)
            return false;

        return true;
    }

    private void OnResourceDelivered(Resource resource)
    {
        _resourceBalance.Increment();
    }
}