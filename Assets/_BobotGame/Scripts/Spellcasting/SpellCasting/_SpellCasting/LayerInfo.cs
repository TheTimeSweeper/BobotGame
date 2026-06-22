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
    public static readonly LayerInfo Destructible = new LayerInfo("Destructible");

    public class CommonMasks
    {
        public static LayerMask WorldOrBody = Default.layerMask.value | Hurtbox.layerMask.value;
        public static LayerMask Hittable = Destructible.layerMask.value | Hurtbox.layerMask.value;
    }
}
