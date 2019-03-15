using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Eidetic.Unity.Utility
{
    public static partial class UnityEngineExtensionMethods
    {
        public static GameObject WithComponent<T>(this GameObject gameObject) where T : Component
        {
            gameObject.AddComponent<T>();
            return gameObject;
        }
        public static GameObject WithHideFlags(this GameObject gameObject, params HideFlags[] flags)
        {
            HideFlags applyFlags = HideFlags.None;
            foreach(var flag in flags) applyFlags |= flag;
            gameObject.hideFlags = applyFlags;
            return gameObject;
        }
        public static GameObject InDontDestroyMode(this GameObject gameObject)
        {
            GameObject.DontDestroyOnLoad(gameObject);
            return gameObject;
        }
        public static void Destroy(this GameObject gameObject)
        {
            if (gameObject == null) return;
            GameObject.Destroy(gameObject);
        }
    }
}