using System;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Eidetic.Confluence.MeshTools
{
    public class Violator : RuntimeNode
    {
        // Inputs
        public Mesh InputMesh;
        [Input] public float NoiseIntensity;
        public int SmoothingTimes;
        public bool Bypass;
        public bool Freeze;

        // Outputs
        public Mesh OutputMesh;

        public string OutputMeshId;

        public bool RefreshOutput;

        void Start()
        {
            if (InputMesh)
                OutputMesh = Instantiate(InputMesh);
        }

        internal override void Update()
        {
            if (RefreshOutput)
            {
                MeshPublisher.Meshes.TryGetValue(OutputMeshId, out var mesh);
                OutputMesh = mesh;
                RefreshOutput = false;
            }

            if (Bypass) OutputMesh = InputMesh;

            if (Bypass || Freeze) return;

            // Allocate memory for the vertices because we
            // calculate their new positions on multiple threads
            var vertices = new NativeArray<Vector3>(InputMesh.vertices, Allocator.TempJob);
            var normals = new NativeArray<Vector3>(InputMesh.normals, Allocator.TempJob);

            var noiseIntensity = GetInputPort("NoiseIntensity").GetInputValue();
            if (noiseIntensity == null)
                noiseIntensity = NoiseIntensity;

            new NoiseJob(vertices, normals, (float)noiseIntensity)
                .Schedule(InputMesh.vertexCount, 50)
                .Complete();

            OutputMesh.SetVertices(vertices.ToList());

            vertices.Dispose();
            normals.Dispose();

            // OutputMesh = MeshSmoothing.LaplacianFilter(OutputMesh, SmoothingTimes);
        }

        struct NoiseJob : IJobParallelFor
        {
            NativeArray<Vector3> vertices;
            NativeArray<Vector3> normals;
            float intensity;

            public NoiseJob(NativeArray<Vector3> vertices, NativeArray<Vector3> normals, float intensity)
            {
                this.vertices = vertices;
                this.normals = normals;
                this.intensity = intensity;
            }

            public void Execute(int i)
            {
                vertices[i] = vertices[i] + (normals[i] * RandomGenerator.NextFloat() * intensity);
            }
        }
    }
}
