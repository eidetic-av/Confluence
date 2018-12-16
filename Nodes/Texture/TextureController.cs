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

    Vector2 MainTextureScale = new Vector2();

    public override void Update()
    {
        MainTextureScale.x = ScaleX = GetInputValue<float>("ScaleX");
        MainTextureScale.y = ScaleY = GetInputValue<float>("ScaleY");
        try
        {
            Material.SetTextureScale("_MainTex", MainTextureScale);
        }
        catch (UnassignedReferenceException exception)
        {
            Debug.LogWarning("This TextureController isn't assigned to any Texture.");
        }
    }
}