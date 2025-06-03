using UnityEngine;
using DG.Tweening;

public class DisolveController : MonoBehaviour
{
    [SerializeField] private Renderer _targetRenderer;
    [SerializeField] private float _transitionTime;

    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _material = _targetRenderer.material;
    }

    public void DisolveIn()
    {
        float time = 0;
        DOTween.To(() => time, x => time = x, _transitionTime, _transitionTime).OnUpdate(()=> {
            _material.SetFloat("_Amount", time / _transitionTime);        
        });
    }
}
