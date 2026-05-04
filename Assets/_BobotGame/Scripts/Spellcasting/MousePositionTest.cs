using UnityEngine;

public class MousePositionTest : MonoBehaviour
{

    [SerializeField]
    private SpellCasting.CameraController cameraController;
    void Update()
    {
        transform.position = cameraController.GetAimPointFromMouse(Input.mousePosition);
    }
}
