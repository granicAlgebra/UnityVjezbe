using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private Entity _player;
    [SerializeField] private TextMeshProUGUI _goldTxt;
    private ParamData _paramData;

    void Start()
    {
        _paramData = _player.GetParam(ParamType.Gold);
        _paramData.OnValueChange.AddListener(UpdateGoldText);
    }

    private void UpdateGoldText(int value)
    {
        _goldTxt.SetText(value.ToString());
    }
}