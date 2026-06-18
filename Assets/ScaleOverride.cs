using UnityEngine;

public class ScaleOverride : MonoBehaviour
{
    [SerializeField]
    private Vector3 scale;
    private void Reset()
    {
        scale = transform.localScale;
    }
    private void LateUpdate()
    {
        transform.localScale = scale;
    }
}
