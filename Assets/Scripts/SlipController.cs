using UnityEngine;

public class SlipController : MonoBehaviour
{
    [SerializeField] private float slipForce = 10f;
    private bool _isSlipping;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // check if a slippery ground is touched by the stickman
        if (col.gameObject.CompareTag("Slip"))
        {
            _isSlipping = true;
            var dir = Quaternion.Euler(0, 0, col.transform.rotation.z) * Vector3.down;
            dir = dir.normalized;
            _rigidbody2D.AddForce(slipForce * dir);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        _isSlipping = false;
    }
}