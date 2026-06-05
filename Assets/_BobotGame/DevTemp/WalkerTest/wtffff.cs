using SpellCasting;
using UnityEngine;


[ExecuteAlways]
public class wtffff : MonoBehaviour
{
    public Transform aimtransform;
    public float maximumYAngle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 startAimDirection = aimtransform.forward;
        TESTstartaim = startAimDirection;
        //get forward forwardRotation from our forward direction
        Vector3 forward = transform.forward;
        TESTtempforward = forward;
        //get aim angle and modify our forwardRotation, with a limit
        float aimanglebutactually = Vector3.Angle(startAimDirection, forward);
        TESTaimanglebutactually = aimanglebutactually / 360f;
        float aimAngle = Mathf.Min(maximumYAngle, aimanglebutactually) * (startAimDirection.y > 0 ? 1 : -1);
        TESTaimanglebutactually2 = aimAngle / 360f;
        Quaternion forwardRotation = Quaternion.LookRotation(forward);
        forwardRotation = forwardRotation * Quaternion.AngleAxis(aimAngle, -Vector3.right);
        TESTangleaxis = Quaternion.AngleAxis(aimAngle, -Vector3.right) * Vector3.forward;
        TESTfinal = forwardRotation * Vector3.forward;
        startAimDirection = (forwardRotation * Vector3.forward);

        DoDebug();
    }

    Vector3 TESTstartaim;
    Vector3 TESTtempforward;
    float TESTaimanglebutactually;
    float TESTaimanglebutactually2;
    Vector3 TESTangleaxis;
    Vector3 TESTfinal;

    void DoDebug()
    {
        DrawVector(TESTstartaim, Color.green);
        DrawVector(TESTtempforward, Color.blue);
        DrawVector(Vector3.up * TESTaimanglebutactually, Color.red, TESTtempforward);
        DrawVector(Vector3.up * TESTaimanglebutactually2, Color.yellow, TESTtempforward*1.1f);
        DrawVector(TESTangleaxis, Color.magenta);
        DrawVector(TESTfinal, Color.cyan);
    }

    void DrawVector(Vector3 vector, Color color, Vector3 offset = default)
    {
        Debug.DrawLine(transform.position + offset, transform.position + offset + vector, color);
    }
}
