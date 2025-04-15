using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class DeathController : MonoBehaviour
{
    [SerializeField] private Entity _entity;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private CinemachineVirtualCamera _deathCamera;
    [SerializeField] private CanvasGroup _deathScreen;
   
    void Start()
    {
        foreach (var param in _entity.Params)
        {
            if (param.Type.Equals(ParamType.Health))
            {
                param.OnValueChange.AddListener(OnHealthChange);
                break;
            }
        }
    }

    private void OnHealthChange(int health)
    {
        if (health <= 0)
        {
            _movement.enabled = false;
            _deathCamera.Priority = 100;

            DOVirtual.DelayedCall(2, () => _deathScreen.DOFade(1, 1));
        }
    }

}
