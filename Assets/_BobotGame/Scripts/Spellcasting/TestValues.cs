using SpellCasting;
using UnityEngine;

public class TestValues : Singleton<TestValues>
{
    public static float[] StaticFloats;
    [SerializeField]
    public float[] floats;

    protected override void HandleAdditionalInstance() { }
    private void Update()
    {
        StaticFloats = floats;
    }
}
