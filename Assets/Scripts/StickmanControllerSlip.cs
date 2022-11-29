using UnityEngine;

public class StickmanControllerSlip : MonoBehaviour
{
    [SerializeField] private float slipForce = 10f;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // check if a slippery ground is touched by the stickman
        if (col.gameObject.CompareTag("Slip"))
        {
            Debug.Log("Slipping");
            var dir = Quaternion.Euler(0, 0, col.transform.rotation.z) * Vector3.down;
            _rigidbody2D.AddForce(slipForce * dir.normalized);
            _animator.SetBool("IsSlipping", true);
            _transform.Rotate(0, 180, 0);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        //_animator.SetBool("IsSlipping", false);
    }
}