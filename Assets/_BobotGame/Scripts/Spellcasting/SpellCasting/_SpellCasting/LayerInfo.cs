using UnityEngine;

public class LayerInfo
{
    public int layerValue;
    public LayerMask layerMask;

    public LayerInfo(string layerName)
    {
        layerValue = LayerMask.NameToLayer(layerName);
        layerMask = LayerMask.GetMask(layerName);
    }

    public static readonly LayerInfo Default = new LayerInfo("Default");
    public static readonly LayerInfo RoomOverlap = new LayerInfo("RoomOverlap");
    public static readonly LayerInfo Hurtbox = new LayerInfo("Hurtbox");
}
