using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Eidetic.Confluence;

[CreateNodeMenu("Texture/TextureController"),
    NodeTint(Colors.ControllerTint)]
public class TextureController : RuntimeNode
{
    public Material Material;
    [Input(ShowBackingValue.Always, ConnectionType.Override)] public float ScaleX;
    [Input(ShowBackingValue.Always, ConnectionType.Override)] public float ScaleY;
    [Input(ShowBackingValue.Always, ConnectionType.Override)] public float OffsetX;
    [Input(ShowBackingValue.Always, ConnectionType.Override)] public float OffsetY;

    Vector2 MainTextureScale = new Vector2();
    Vector2 MainTextureOffset = new Vector2();

    public override void Update()
    {
        MainTextureScale.x = ScaleX = GetInputValue<float>("ScaleX");
        MainTextureScale.y = ScaleY = GetInputValue<float>("ScaleY");
        MainTextureOffset.x = OffsetX = GetInputValue<float>("OffsetX");
        MainTextureOffset.y = OffsetY = GetInputValue<float>("OffsetY");
        try
        {
            Material.SetTextureScale("_MainTex", MainTextureScale);
            Material.SetTextureOffset("_MainTex", MainTextureOffset);
        }
        catch (UnassignedReferenceException exception)
        {
            Debug.LogWarning("This TextureController isn't assigned to any Texture.");
        }
    }
}