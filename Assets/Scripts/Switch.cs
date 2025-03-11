using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private ScriptedMovement _door;

    public void Hodor()
    {
        _door.Hodor();
    }
}
