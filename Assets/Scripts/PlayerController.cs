using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _myRidigdboy;

    [SerializeField] private float _speed = 4f;

    private float _xAxis;
    private float _yAxis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _myRidigdboy = GetComponent<Rigidbody>();

        if (_myRidigdboy == null)
        {
            Debug.LogError("Missing Rigidbody!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        _myRidigdboy.AddForce(new Vector3(_xAxis, 0, _yAxis) * _speed);
    }
}
