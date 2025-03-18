using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{
    [SerializeField] private Entity _entity;
    [SerializeField] private Vector3 _boxSize = Vector3.one;
    [SerializeField] private float _boxForwardOffset = 1f;
    [SerializeField] private float _boxVerticalOffset = -1f;


    private void OnDrawGizmos()
    {
        Vector3 boxPosition = transform.position + (transform.forward * _boxForwardOffset) + (transform.up * _boxVerticalOffset);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxPosition, _boxSize);
    }

    void Update()
    {
        Interact();
    }
    
    private void Interact()
    {
        if (!Input.GetKeyDown(KeyCode.E))
        {
            return;
        }

        Vector3 boxPosition = transform.position + (transform.forward * _boxForwardOffset) + (transform.up * _boxVerticalOffset);

        Collider[] hits = Physics.OverlapBox(boxPosition, _boxSize / 2, transform.rotation);

        for (int i = 0; i < hits.Length; i++)
        {
            var interactable = hits[i].GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.InvokeInteraction(_entity);
                break;
            }
        }
    }
}

