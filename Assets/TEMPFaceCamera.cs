using System.Xml.Serialization;
using UnityEngine;

public class TEMPFaceCamera : MonoBehaviour
{
    [SerializeField]
    public Transform cameraPosition;
    private void Awake()
    {
        if (!cameraPosition)
        {
            cameraPosition = Camera.main.transform;
        }
    }
    void Update()
    {
        Vector3 dir = transform.position - cameraPosition.position;
        dir.y = transform.position.y;
        transform.rotation = Util.DirectionQuaternion(dir);
    }
}
