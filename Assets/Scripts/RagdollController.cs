using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(RagdollController))]
public class RagdollEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RagdollController ragdoll = (RagdollController)target;  

        if (GUILayout.Button("Cache ragdoll rigidbodies"))
        {
            ragdoll.CacheRagdollColliders();    
        }
        if (GUILayout.Button("Activate Ragdoll"))
        {
            ragdoll.ActivateRagdoll();
        }
    }
}
#endif

public class RagdollController : MonoBehaviour
{
    public Rigidbody MyRigidbody;
    public AnimationController AC;
    public Collider MainCollider;
    public GameObject RagdollRootObject;
    public List<Rigidbody> RagdollBodies;

    private void Awake()
    {
        SetRagdollKinematic(false);
    }

    public void CacheRagdollColliders()
    {
        RagdollRootObject.GetComponentsInChildren(true, RagdollBodies);
    }


    private void SetRagdollKinematic(bool value)
    {
        for (int i = 0; i < RagdollBodies.Count; i++)
        {
            RagdollBodies[i].isKinematic = value;
        }
    }

    public void ActivateRagdoll()
    {
        MyRigidbody.isKinematic = true;

        AC.MyAnimator.enabled = false;

        MainCollider.enabled = false;

        SetRagdollKinematic(false);

    }

    public void Die(Vector3 forcePosition, float force, float radius)
    {
        ActivateRagdoll();

        for (int i = 0; i < RagdollBodies.Count; i++)
        {
            Vector3 dist = RagdollBodies[i].position - forcePosition;   
            float magnitude = dist.magnitude;
            if (magnitude < radius)
            {
                RagdollBodies[i].AddForce((1 - (magnitude / radius)) * force * dist, ForceMode.Impulse);    
            }
        }
    }
}
