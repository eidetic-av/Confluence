
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using UnityEngine.PostProcessing;
using Eidetic.Utility;

namespace Eidetic.Confluence
{
    [CreateNodeMenu("PostProcessingController"),
        NodeTint(Colors.ControllerTint)]
    public class PostProcessingController : RuntimeNode
    {
        [SerializeField] PostProcessingProfile Profile;
        
        [Input] public float DepthOfFieldFocusDistance
        {
            get => Profile.depthOfField.settings.focusDistance;
            set
            {
                var depthOfFieldSettings = Profile.depthOfField.settings;
                depthOfFieldSettings.focusDistance = value.Map(-1, 1, MinimumMap, MaximumMap);
                Profile.depthOfField.settings = depthOfFieldSettings;
            }
        }
        [SerializeField] public float MinimumMap;
        [SerializeField] public float MaximumMap;


        [Input]
        public float BloomThreshold
        {
            get => Profile.bloom.settings.bloom.threshold;
            set
            {
                var bloomSettings = Profile.bloom.settings;
                bloomSettings.bloom.threshold = value;
                Profile.bloom.settings = bloomSettings;
            }
        }

        [Input]
        public float BloomIntensity
        {
            get => Profile.bloom.settings.bloom.intensity;
            set
            {
                var bloomSettings = Profile.bloom.settings;
                bloomSettings.bloom.intensity = value;
                Profile.bloom.settings = bloomSettings;
            }
        }
    }
}