using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -7f);
    public float smoothing = 8f;

    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.3f);
    }
}
