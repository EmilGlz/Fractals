using System.Collections.Generic;
using System.Linq;
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
        [HideInInspector] public float AnimationTime = 1f;

        protected List<List<Matrix4x4>> Batches = new();
        protected List<List<Matrix4x4>> CurrentScalingMeshes = new();
        protected Vector3 StartingAnimationScale;
        protected Vector3 EndingAnimationScale;

        private float _scalingDeltatime;
        private bool _canScale = false;
        private Vector3 currentScale;

        private MeshPositionJob _job;
        private NativeArray<float3> _nativePositions;
        private NativeArray<float> _nativeScales;
        private NativeArray<NativeArray<Matrix4x4>> _nativeBatches;

        private void Start()
        {
            _job = new MeshPositionJob()
            { };
        }

        private void RenderBatches()
        {
            _job.Batches = _nativeBatches;
            var jobHandle = _job.Schedule(_nativeBatches.Length, 64);
            jobHandle.Complete();

            foreach (var batch in _nativeBatches)
                Graphics.DrawMeshInstanced(Mesh, 0, Material, batch.ToArray());

            if (_canScale)
            {
                if (CurrentScalingMeshes.Count > 0)
                    currentScale = Vector3.Lerp(StartingAnimationScale, EndingAnimationScale, _scalingDeltatime / AnimationTime);

                foreach (var batch in CurrentScalingMeshes)
                {
                    var currentBatch = batch;
                    ChangeScaleInBatch(ref currentBatch, currentScale);
                    Graphics.DrawMeshInstanced(Mesh, 0, Material, currentBatch);
                }
                _scalingDeltatime += Time.deltaTime;

                if (_scalingDeltatime >= AnimationTime)
                {
                    _canScale = false;
                    _scalingDeltatime = 0f;
                    OnAnimationFinished();
                }
            }
        }

        protected virtual void OnAnimationFinished()
        { 
        }

        protected void AddToBatches(List<List<Matrix4x4>> addingBatches)
        {
            // TODO combine to Batches
            foreach (var batch in addingBatches)
                foreach (var matrix in batch)
                    AddMatrix(matrix);
        }

        private void ChangeScaleInBatch(ref List<Matrix4x4> batch, Vector3 scale)
        {
            for (int j = 0; j < batch.Count; j++)
            {
                var matrix = batch[j];
                matrix.ModifyScale(scale);
                batch[j] = matrix;
            }
        }

        private void Update()
        {
            RenderBatches();
        }

        public void SpawnMesh(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            AddMatrix(Matrix4x4.TRS(position, rotation, scale));
        }

        public void AddMatrix(Matrix4x4 matrix)
        {
            var elementsCountInLastBatch = Batches.Count > 0 ? Batches[^1].Count : 1000; // if there is no batches yet, create new one
            if (elementsCountInLastBatch < 1000)
            {
                // Can add to the last batch, limit(1000) is not reached yet
                Batches[^1].Add(matrix);
            }
            else
            {
                // There are 1000 elements in last batch, which is maximum. We need to create new one with the matrix inside of it
                Batches.Add(new List<Matrix4x4>()
                {
                    matrix
                });
            }
        }

        public void SpawnMeshAnim(Vector3 position, Quaternion rotation, Vector3 startingScale, Vector3 endingScale)
        {
            CurrentScalingMeshes ??= new List<List<Matrix4x4>>();
            var elementsCountInLastBatch = CurrentScalingMeshes.Count > 0 ? CurrentScalingMeshes[^1].Count : 1000;
            var matrix = Matrix4x4.TRS(position, rotation, startingScale);
            if (elementsCountInLastBatch < 1000)
            {
                // Can add to the last batch, limit(1000) is not reached yet
                CurrentScalingMeshes[^1].Add(matrix);
            }
            else
            {
                // There are 1000 elements in last batch, which is maximum. We need to create new one with the matrix inside of it
                CurrentScalingMeshes.Add(new List<Matrix4x4>()
                {
                    matrix
                });
            }
            StartingAnimationScale = Vector3.zero;
            EndingAnimationScale = endingScale;
            StartScaleAnimation();
        }

        public void StartScaleAnimation()
        {
            _canScale = true;
            _scalingDeltatime = 0f;
        }

        public void ClearBatches()
        {
            Batches.Clear();
        }

        public void SaveBatches()
        {
            if (Batches.Count == 0 || CurrentScalingMeshes.Count == Batches.Count)
                return;
            CurrentScalingMeshes = new List<List<Matrix4x4>>();
            CurrentScalingMeshes.AddRange(Batches);
            UpdateAnimationParameters(CurrentScalingMeshes[0][0].lossyScale);
        }

        protected virtual void UpdateAnimationParameters(Vector3 parentScale)
        {
        }
    }

    [BurstCompile]
    internal struct MeshPositionJob : IJobParallelFor
    {
        public NativeArray<NativeArray<Matrix4x4>> Batches;
        public NativeArray<float3> Positions;
        public NativeArray<float3> Scales;

        public readonly void Execute(int index)
        {
            //var elementsCountInLastBatch = Batches.Length > 0 ? Batches[^1].Length : 1000; // if there is no batches yet, create new one
            //if (elementsCountInLastBatch < 1000)
            //{
            //    // Can add to the last batch, limit(1000) is not reached yet
            //    Batches[^1].Add(matrix);
            //}
            //else
            //{
            //    // There are 1000 elements in last batch, which is maximum. We need to create new one with the matrix inside of it
            //    Batches.Add(new List<Matrix4x4>()
            //    {
            //        matrix
            //    });
            //}
        }
    }
}