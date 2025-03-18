using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Interactable
{
    [SerializeField] private ParamType _paramType;
    [SerializeField] private int _amount;

    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<Entity>();
        if (entity != null)
        {
            entity.ChangeParam(_paramType, _amount);
            gameObject.SetActive(false);    
        }
    }

    public void InvokeInteraction(Entity entity)
    {
        entity.ChangeParam(_paramType, _amount);
        gameObject.SetActive(false);
    }
}
