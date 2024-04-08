using System.Collections.Generic;
using UnityEngine;

public class Instancer : Singleton<Instancer>
{
    public Mesh Mesh;
    public Material Material;
    private List<List<Matrix4x4>> Batches = new();
    private List<List<Matrix4x4>> CurrentScalingMeshes = new();


    [HideInInspector] public float AnimationTime = 1f;
    private float _scalingDeltatime;
    private Vector3 _startingAnimationScale;
    private Vector3 _endingAnimationScale;
    Vector3 currentScale;
    private bool _canScale = false;
    private void RenderBatches()
    {
        foreach (var batch in Batches)
            Graphics.DrawMeshInstanced(Mesh, 0, Material, batch);

        if (_canScale)
        {
            if (CurrentScalingMeshes.Count > 0)
                currentScale = Vector3.Lerp(_startingAnimationScale, _endingAnimationScale, _scalingDeltatime / AnimationTime);

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
                CurrentScalingMeshes.Clear();
            }
        }
    }

    private void ChangeScaleInBatch(ref List<Matrix4x4> batch, Vector3 scale)
    {
        for (int j = 0; j < batch.Count; j++)
        {
            var matrix = batch[j];
            ModifyScale(ref matrix, scale);
            batch[j] = matrix;
        }
    }

    private void Update()
    {
        RenderBatches();
    }

    public void SpawnMesh(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var elementsCountInLastBatch = Batches.Count > 0 ? Batches[^1].Count : 1000; // if there is no batches yet, create new one
        var matrix = Matrix4x4.TRS(position, rotation, scale);
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
        _startingAnimationScale = CurrentScalingMeshes[0][0].lossyScale;
    }

    void ModifyScale(ref Matrix4x4 matrix, Vector3 newScale)
    {
        // Set the scale components of the matrix to the new scale values
        matrix.m00 = newScale.x; // Set x component of the X axis
        matrix.m11 = newScale.y; // Set y component of the Y axis
        matrix.m22 = newScale.z; // Set z component of the Z axis
    }

    public void DespawnMesh(Vector3 position, Vector3 scale)
    {
        for (int i = 0; i < Batches.Count; i++)
        {
            var batch = Batches[i];
            for (int j = 0; j < batch.Count; j++)
            {
                var matrix = batch[j];
                var batchPos = matrix.GetPosition();
                var batchScale = matrix.lossyScale;

                if (batchPos == position && batchScale == scale)
                {
                    batch.RemoveAt(j);
                    break;
                }
            }
        }
    }
}
