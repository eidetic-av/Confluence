using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eidetic.URack
{
    [CreateAssetMenu, Serializable]
    public class Rack : ScriptableObject
    {
        /// <summary> All modules in the rack. <para/>
        /// See: <see cref="AddModule{T}"/> </summary>
        [SerializeField] public List<Module> modules = new List<Module>();
    }
}