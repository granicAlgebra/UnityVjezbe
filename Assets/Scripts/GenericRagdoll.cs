using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GenericRagdoll))]
public class RagdollEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GenericRagdoll ragdoll = (GenericRagdoll)target;

        if (GUILayout.Button("Cache Ragdoll Rigidbodies"))
        {
            ragdoll.CacheRagdollColliders();
        }
        if (GUILayout.Button("Die"))
        {
            ragdoll.Die();
        }
    }
}
#endif

public class GenericRagdoll : MonoBehaviour
{
    public Rigidbody MyRigidbody;
    public AnimationController AC;
    public Collider MainCollider;
    public GameObject RagdollRootObject;
    public List<Rigidbody> RagdollRigidbodies = new List<Rigidbody>();
    private void OnEnable()
    {
        SetRagdollKinematic(true);
    }
    
    public void SetRagdollKinematic(bool value)
    {
        for (int i = 0; i < RagdollRigidbodies.Count; i++)
            RagdollRigidbodies[i].isKinematic = value;
    }

    public void CacheRagdollColliders()
    {
        RagdollRootObject.GetComponentsInChildren(true, RagdollRigidbodies);
    }

    public void Die()
    {
        if (MyRigidbody)
        {
            MyRigidbody.isKinematic = true;
        }

        AC.Animator.enabled = false;

        if (MainCollider)
        {
            MainCollider.enabled = false;
        }

        SetRagdollKinematic(false);
        RagdollRootObject.SetActive(true);
    }

    public void Die(Vector3 forcePosition, float force, float radius)
    {
        Die();

        for (int i = 0; i < RagdollRigidbodies.Count; i++)
        {
            Vector3 dist = RagdollRigidbodies[i].position - forcePosition;
            float magnitude = dist.magnitude;
            if (magnitude < radius)
            {
                RagdollRigidbodies[i].AddForce((1 - (magnitude / radius)) * force * dist, ForceMode.Impulse);
            }
        }
    }
}
