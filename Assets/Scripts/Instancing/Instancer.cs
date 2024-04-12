using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Instancing
{
    public class Instancer<T> : Singleton<T> where T : MonoBehaviour
    {
        public Mesh Mesh;
        public Material Material;

        private RenderParams _rp;
        private MeshPositionJob _job;
        private NativeArray<float3> _nativePositions;
        private NativeArray<float3> _nativeRotations;
        private NativeArray<float3> _nativeScales;
        private NativeArray<Matrix4x4> _nativeMatrices;

        void Start()
        {
            _rp = new RenderParams(Material);
            _nativeMatrices = new NativeArray<Matrix4x4>(1, Allocator.Persistent);
        }

        private void RenderBatches()
        {
            _job.Matrices = _nativeMatrices;
            var jobHandle = _job.Schedule(_nativeMatrices.Length, 64);
            jobHandle.Complete();
            if (_nativeMatrices != null && _nativeMatrices.Length > 0)
                Graphics.RenderMeshInstanced(_rp, Mesh, 0, _nativeMatrices);
        }

        private void Update()
        {
            RenderBatches();
        }

        public void RemoveAll(int timesDifference, int maxMeshCount)
        {
            if (_nativeMatrices.Length >= maxMeshCount)
                return;
            var oldCount = _nativeMatrices.Length;
            _nativePositions.Dispose();
            _nativeRotations.Dispose();
            _nativeScales.Dispose();
            _nativeMatrices.Dispose();
            _nativePositions = new NativeArray<float3>(oldCount * timesDifference, Allocator.Persistent);
            _nativeRotations = new NativeArray<float3>(oldCount * timesDifference, Allocator.Persistent);
            _nativeScales = new NativeArray<float3>(oldCount * timesDifference, Allocator.Persistent);
            _nativeMatrices = new NativeArray<Matrix4x4>(oldCount * timesDifference, Allocator.Persistent);
        }

        public void SpawnMesh(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var pos3 = new float3(position);
            var rot3 = new float3(rotation.eulerAngles);
            var scale3 = new float3(scale);
            _nativePositions.ChangeFirstElementThatIsZero(pos3);
            _nativeRotations.ChangeFirstElementThatIsZero(rot3);
            _nativeScales.ChangeFirstElementThatIsZero(scale3);

            _job.Positions = _nativePositions;
            _job.Rotations = _nativeRotations;
            _job.Scales = _nativeScales;
        }

        void OnDestroy()
        {
            _nativeScales.Dispose();
            _nativeMatrices.Dispose();
            _nativePositions.Dispose();
        }
    }

    [BurstCompile]
    internal struct MeshPositionJob : IJobParallelFor
    {
        public NativeArray<Matrix4x4> Matrices;
        public NativeArray<float3> Positions;
        public NativeArray<float3> Scales;
        public NativeArray<float3> Rotations;

        public void Execute(int index)
        {
            Matrices[index] = Matrix4x4.TRS(Positions[index], Quaternion.Euler(Rotations[index]), Scales[index]);
        }
    }
}