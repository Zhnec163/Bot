using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ResourceBalanceView : MonoBehaviour
{
    [SerializeField] private ResourceBalance _resourceBalance;
    
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _resourceBalance.Changed += OnBalanceChanged;
    }

    private void OnDisable()
    {
        _resourceBalance.Changed -= OnBalanceChanged;
    }

    private void OnBalanceChanged()
    {
        if (_resourceBalance != null)
            _text.text = _resourceBalance.Balance.ToString();
    }
}
