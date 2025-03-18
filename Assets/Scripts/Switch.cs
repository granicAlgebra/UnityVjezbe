using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, Interactable
{
    [SerializeField] private ScriptedMovement _door;

    public void InvokeInteraction(Entity entity)
    {
        _door.OpenDoor();
    }
}
