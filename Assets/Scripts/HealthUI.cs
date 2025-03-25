using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Entity _entity;
    private ParamData _paramData;

    void Start()
    {
        _paramData = _entity.GetParam(ParamType.Health);
        _paramData.OnValueChange.AddListener(UpdateHealthBar);
        UpdateHealthBar(_paramData.Value);
    }

    private void UpdateHealthBar(int arg0)
    {
        _healthBar.fillAmount = arg0 / (float)_paramData.MinMax.y;
    }
}
