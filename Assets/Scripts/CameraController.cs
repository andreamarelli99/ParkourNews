using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var cameraTransform = transform;
        var targetPosition = target.position;
        cameraTransform.position = new Vector3(targetPosition.x, targetPosition.y, cameraTransform.position.z);
    }
}