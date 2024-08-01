using TMPro;
using UnityEngine;

public class ResourceBalanceView : MonoBehaviour
{
    [SerializeField] private ResourceBalance _resourceBalance;
    
    private TMP_Text _text;

    private void Awake()
    {
        if (TryGetComponent(out TMP_Text text))
            _text = text;
        
        _resourceBalance.OnChange += HandleBalanceChangeAction;
    }

    private void HandleBalanceChangeAction()
    {
        if (_resourceBalance != null)
            _text.text = _resourceBalance.Balance.ToString();
    }
}
