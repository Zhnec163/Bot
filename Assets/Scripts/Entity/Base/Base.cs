using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(ResourceBalance)), RequireComponent(typeof(BotSpawner)),
 RequireComponent(typeof(ResourceScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private int _costOfBot;
    [SerializeField] private int _costOfBase;
    [SerializeField] private Transform _collectZone;
    [SerializeField] private Transform _botSpawn;
    [SerializeField] private List<Bot> _attachedBots;
    [SerializeField] private Flag _flag;
    [SerializeField] private BaseBuilder baseBuilder;

    private bool _isBuildBase;
    private List<Bot> _bots = new();
    private ResourceBalance _resourceBalance;
    private ResourceScanner _resourceScanner;
    private BotSpawner _botSpawner;

    public bool IsBuildBase => _isBuildBase;

    private void Awake()
    {
        if (_attachedBots != null)
            _bots.AddRange(_attachedBots);

        foreach (Bot bot in _bots)
            InitializeBot(bot);

        _resourceBalance = GetComponent<ResourceBalance>();
        _resourceScanner = GetComponent<ResourceScanner>();
        _botSpawner = GetComponent<BotSpawner>();
    }

    private void Update()
    {
        Production();
        GatherResource();
    }

    private void OnDisable() => _bots.ForEach(bot => { bot.ResourceDelivered -= OnResourceDelivered; });

    public void Init(Bot bot, ResourceFinder resourceFinder, BaseBuilder baseBuilder)
    {
        _resourceScanner.Init(resourceFinder);
        this.baseBuilder = baseBuilder;
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
        _flag.transform.position = position;
        _flag.gameObject.SetActive(true);
    }

    private void Production()
    {
        if (_isBuildBase)
        {
            if (TryGetFreeBot(out Bot bot) == false)
                return;

            if (_resourceBalance.TrySubstract(_costOfBase) == false)
                return;

            bot.ConstructionCompleted += OnConstructionCompleted;
            bot.BuildBase(_flag.transform.position);
            _isBuildBase = false;
        }
        else
        {
            if (_resourceBalance.TrySubstract(_costOfBot) == false)
                return;

            Bot bot = _botSpawner.Spawn(_botSpawn.position);
            InitializeBot(bot);
            _bots.Add(bot);
        }
    }

    private void InitializeBot(Bot bot)
    {
        bot.Init(_collectZone.position, baseBuilder);
        bot.ResourceDelivered += OnResourceDelivered;
    }

    private void GatherResource()
    {
        if (TryGetFreeBot(out Bot bot) == false)
            return;

        if (_resourceScanner.TryGetNearestResource(out Resource resource))
            bot.BringResource(resource);
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