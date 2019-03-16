using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Eidetic.Utility;

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
            foreach (var flag in flags) applyFlags |= flag;
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

        /// <summary>
        /// Set the value of a SerializedProperty without knowing the type
        /// </summary>
        public static void SetValue(this SerializedProperty serializedProperty, object value)
        {
            var valueTypeName = value.GetType().CSharpName();
            if (serializedProperty.type != valueTypeName)
                throw new UnityException("You're trying to set a SerializedProperty value with an object of a different Type.");
            switch (valueTypeName)
            {
                case "AnimationCurve":
                    serializedProperty.animationCurveValue = (AnimationCurve)value;
                    break;
                case "bool":
                    serializedProperty.boolValue = (bool)value;
                    break;
                case "BoundsInt":
                    serializedProperty.boundsIntValue = (BoundsInt)value;
                    break;
                case "Bounds":
                    serializedProperty.boundsValue = (Bounds)value;
                    break;
                case "Color":
                    serializedProperty.colorValue = (Color)value;
                    break;
                case "double":
                    serializedProperty.doubleValue = (double)value;
                    break;
                case "Object":
                    serializedProperty.exposedReferenceValue = (UnityEngine.Object)value;
                    break;
                case "float":
                    serializedProperty.floatValue = (float)value;
                    break;
                case "int":
                    serializedProperty.intValue = (int)value;
                    break;
            }
        }

        /// <summary>
        /// Map a float from one range to another.
        /// </summary>
        /// <param name="input">The input float to map</param>
        /// <param name="inputRange">Original range</param>
        /// <param name="outputRange">New range</param>
        /// <returns>Float mapped to the new range.</returns>
        public static float Map(this float input, Vector2 inputRange, Vector2 outputRange)
        {
            return ((input - inputRange.x) / (inputRange.y - inputRange.x)) * (outputRange.y - outputRange.x) + outputRange.x;
        }

        /// <summary>
        /// Map a double from one range to another.
        /// </summary>
        /// <param name="input">The input double to map</param>
        /// <param name="inputRange">Original range</param>
        /// <param name="outputRange">New range</param>
        /// <returns>Double mapped to the new range.</returns>
        public static double Map(this double input, Vector2 inputRange, Vector2 outputRange)
        {
            return ((input - inputRange.x) / (inputRange.y - inputRange.x)) * (outputRange.y - outputRange.x) + outputRange.x;
        }
    }
}