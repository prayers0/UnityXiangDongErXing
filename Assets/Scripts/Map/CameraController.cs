using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField]private float offest;
    private Transform targetTransform;
    public float screenWidth { get;private set; }
    private float maxPosX;

    public void Init(Transform targetTransform,float maxPosX)
    {
        this.targetTransform = targetTransform;
        Camera camera=GetComponent<Camera>();
        screenWidth = camera.aspect * camera.orthographicSize*2;
        if (maxPosX < 0) this.maxPosX = float.MaxValue;
        else
        {
            this.maxPosX = maxPosX - screenWidth / 2;
        }
        transform.position = new Vector3(ClampX(targetTransform.position.x + offest), transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (targetTransform != null)
        {
            Vector3 target = transform.position;
            target.x = ClampX(targetTransform.position .x+ offest);
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
        }
    }

    private float ClampX(float x)
    {
        return Mathf.Clamp(x, screenWidth / 2f, maxPosX);
    }
    
}
