
using UnityEngine;

public class LandMine : MonoBehaviour, Interactable
{
    [SerializeField] private ParamType _paramType;
    [SerializeField] private int _amount;
    [SerializeField] private float _force;
    [SerializeField] private AudioSource _SFXprefab;
    [SerializeField] private AudioClip _SFXclip;
    [SerializeField] private ParticleSystem _VFXprefab;

    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<Entity>();
        if (entity != null)
        {
            entity.ChangeParam(_paramType, _amount);
            SFXManager.Instance.PlaySFX(_SFXprefab, transform.position, _SFXclip);
            VFXManager.Instance.PlayVFX(_VFXprefab, transform.position);
            var dir =  other.transform.position - transform.position;
            other.GetComponent<Rigidbody>().AddForce(dir.normalized * _force, ForceMode.Impulse);
            gameObject.SetActive(false);    
        }
    }

    public void InvokeInteraction(Entity entity)
    {
        entity.ChangeParam(_paramType, _amount);
        gameObject.SetActive(false);
    }
}
