using UnityEngine;

[ExecuteInEditMode]
public class RescaleOneLayerUniform : MonoBehaviour
{
    void Update()
    {
        transform.localScale = Vector3.one / transform.parent.lossyScale.x;
    }
}
