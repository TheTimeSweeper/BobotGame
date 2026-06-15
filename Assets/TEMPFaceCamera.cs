using UnityEngine;

public class TEMPFaceCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        dir.y = transform.position.y;
        transform.rotation = Util.DirectionQuaternion(dir);
    }
}
