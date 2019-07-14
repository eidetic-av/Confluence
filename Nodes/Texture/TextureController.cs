using System.Linq;
using UnityEngine;
using XNode;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("Texture/TextureController"),
        NodeTint(Colors.ControllerTint)]
    public class TextureController : RuntimeNode
    {
        public Material Material;
        [Input(ShowBackingValue.Always, ConnectionType.Override)] public float ScaleX;
        [Input(ShowBackingValue.Always, ConnectionType.Override)] public float ScaleY;
        [Input(ShowBackingValue.Always, ConnectionType.Override)] public float OffsetX;
        [Input(ShowBackingValue.Always, ConnectionType.Override)] public float OffsetY;

        [Input(ShowBackingValue.Always, ConnectionType.Override)] public float Hue = 0;
        [Input(ShowBackingValue.Always, ConnectionType.Override)] public float Saturation = 0;
        [Input(ShowBackingValue.Always, ConnectionType.Override)] public float Value = 1;

        Vector2 MainTextureScale = new Vector2();
        Vector2 MainTextureOffset = new Vector2();

        Color MainColor = new Color();

        internal override void Update()
        {
            base.Update();
            if (Material == null) return;

            // Update values from each input port
            this.Ports.Where(port => port.IsInput).ToList()
                .ForEach((NodePort port) =>
                {
                    switch (port.MemberName)
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
                            float? offsetY;
                            if (port.TryGetInputValue(out offsetY))
                                OffsetY = offsetY.Value;
                                MainTextureOffset.y = OffsetY;
                            break;
                        case "Hue":
                            float? hue; 
                            if (port.TryGetInputValue(out hue))
                                Hue = hue.Value;
                            break;
                        case "Saturation":
                            float? saturation; 
                            if (port.TryGetInputValue(out saturation))
                                Saturation = saturation.Value;
                            break;
                        case "Value":
                            float? value; 
                            if (port.TryGetInputValue(out value))
                                Value = value.Value;
                            break;
                    }
                });

            Material.SetTextureScale("_MainTex", MainTextureScale);
            Material.SetTextureOffset("_MainTex", MainTextureOffset);

            MainColor = Color.HSVToRGB(Hue, Saturation, Value);

            Material.SetColor("_Color", MainColor);
        }
    }
}