using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] private float _slicerRailLimit = 3.5f;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.acceleration);

        if (transform.localPosition.x < -_slicerRailLimit) 
            transform.localPosition = new Vector3(-_slicerRailLimit, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x > _slicerRailLimit)
            transform.localPosition = new Vector3(_slicerRailLimit, transform.localPosition.y, transform.localPosition.z);
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(Input.acceleration.x) > 0.1f)
            _rigidbody2D.AddForce(new Vector2(Input.acceleration.x, 0) * 20.0f);
        if (transform.localPosition.x < -_slicerRailLimit) _rigidbody2D.velocity = Vector2.zero;
        if (transform.localPosition.x > _slicerRailLimit) _rigidbody2D.velocity = Vector2.zero;
        if (_rigidbody2D.velocity.magnitude > 6) 
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * 6;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.TakeDeath();
        }
    }
}
