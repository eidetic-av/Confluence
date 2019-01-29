using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (Material == null) return;

        // Update values from each input port
        this.Ports.Where(port => port.IsInput).ToList()
            .ForEach((NodePort port) =>
            {
                switch (port.fieldName)
                {
                    case "ScaleX":
                        float? scaleX;
                        if (port.TryGetInputValue(out scaleX))
                            ScaleX = scaleX.Value;
                            MainTextureScale.x = ScaleX;
                        break;
                    case "ScaleY":
                        float? scaleY;
                        if (port.TryGetInputValue(out scaleY))
                            ScaleY = scaleY.Value;
                            MainTextureScale.y = ScaleY;
                        break;
                    case "OffsetX":
                        float? offsetX;
                        if (port.TryGetInputValue(out offsetX))
                            OffsetX = offsetX.Value;
                            MainTextureOffset.x = OffsetX;
                        break;
                    case "OffsetY":
                        float? offsetY;;
                        if (port.TryGetInputValue(out offsetY))
                            OffsetY = offsetY.Value;
                            MainTextureOffset.y = OffsetY;
                        break;
                }
            });

        Material.SetTextureScale("_MainTex", MainTextureScale);
        Material.SetTextureOffset("_MainTex", MainTextureOffset);
    }
}