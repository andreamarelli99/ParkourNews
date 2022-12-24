using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDisappear : MonoBehaviour
{
    [SerializeField] private float _timeToDisappear;// Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _timeToDisappear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
