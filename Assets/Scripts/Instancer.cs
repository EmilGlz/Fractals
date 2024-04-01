using System.Collections.Generic;
using UnityEngine;

public class Instancer : Singleton<Instancer>
{
    public int Instances;
    public Mesh Mesh;
    public Material[] Materials;
    private List<List<Matrix4x4>> Batches = new List<List<Matrix4x4>>();

    private void RenderBatches()
    {
        foreach (var batch in Batches)
        {
            for (int i = 0; i < Mesh.subMeshCount; i++)
            {
                Graphics.DrawMeshInstanced(Mesh, i, Materials[i], batch);
            }
        }
    }

    private void Update()
    {
        RenderBatches();
    }

    public void Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (Batches.Count == 0 || Batches[Batches.Count - 1].Count >= 1000)
        {
            Batches.Add(new List<Matrix4x4>());
        }
        Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
        Batches[^1].Add(matrix);
    }

    public void DeSpawn(Vector3 position, float thresholdDistance = 0.1f)
    {
        // Find the nearest object within the threshold distance from the provided position
        float minDistance = float.MaxValue;
        GameObject nearestObject = null;

        foreach (var batch in Batches)
        {
            foreach (var matrix in batch)
            {
                Vector3 objectPosition = matrix.GetColumn(3);
                float distance = Vector3.Distance(position, objectPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = GetGameObjectFromMatrix(matrix); // Get the GameObject from the matrix
                }
            }
        }

        // If a nearest object is found and it's within the threshold distance, despawn it
        if (nearestObject != null && minDistance <= thresholdDistance)
        {
            foreach (var batch in Batches)
            {
                int index = batch.FindIndex(matrix => matrix == nearestObject.transform.localToWorldMatrix);
                if (index != -1)
                {
                    batch.RemoveAt(index);
                    break;
                }
            }

            Destroy(nearestObject);
        }
    }

    private GameObject GetGameObjectFromMatrix(Matrix4x4 matrix)
    {
        // Extract the GameObject from the transformation matrix
        Vector3 position = matrix.GetColumn(3);
        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.transform.position == position)
            {
                return obj;
            }
        }
        return null;
    }


    private void Start()
    {
        int addedMatrices = 0;
        Batches.Add(new List<Matrix4x4>());
        //for (int i = 0; i < Instances; i++)
        //{
        //    if(addedMatrices < 1000)
        //    {
        //        Batches[Batches.Count - 1].Add(Matrix4x4.TRS(new Vector3(Random.Range(0, 50), Random.Range(0, 50), Random.Range(0, 50)),
        //            Quaternion.identity, Vector3.one));
        //    }
        //    else
        //    {
        //        Batches.Add(new List<Matrix4x4>());
        //        addedMatrices = 0;
        //    }
        //}
    }
}
