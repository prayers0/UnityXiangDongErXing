using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField]private float offest;
    private Transform targetTransform;

    public void Init(Transform targetTransform)
    {
        this.targetTransform = targetTransform;

    }

    private void LateUpdate()
    {
        Vector3 target = transform.position;
        target.x = targetTransform.position.x + offest;
        transform.position=Vector3.Lerp(transform.position, target, moveSpeed*Time.deltaTime);
    }
    
}
