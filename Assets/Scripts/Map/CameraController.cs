using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField]private float offest;
    private Transform targetTransform;
    public float screenWidth { get;private set; }

    public void Init(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        Camera camera=GetComponent<Camera>();
        screenWidth = camera.aspect * camera.orthographicSize*2;
        transform.position = new Vector3(targetTransform.position.x+offest, transform.position.y, transform.position.z);
    }

    private void LateUpdate()
    {
        if (targetTransform != null)
        {
            Vector3 target = transform.position;
            target.x = Mathf.Clamp(targetTransform.position.x + offest, screenWidth / 2f, float.MaxValue);
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
        }
    }
    
}
