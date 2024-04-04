using System.Collections.Generic;
using UnityEngine;

public class Instancer : Singleton<Instancer>
{
    public Mesh Mesh;
    public Material Material;
    private List<List<Matrix4x4>> Batches = new List<List<Matrix4x4>>();

    private void RenderBatches()
    {
        foreach (var batch in Batches)
        {
            Graphics.DrawMeshInstanced(Mesh, 0, Material, batch);
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
    public void ClearBatches()
    {
        Batches.Clear();
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
