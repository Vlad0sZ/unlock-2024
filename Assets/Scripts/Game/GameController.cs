using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Texture2D> _texture2Ds;
    [SerializeField] private MeshFilter meshFilter;
    
    private MeshGenerator _meshGenerator;

    private void Awake()
    {
        _meshGenerator = new MeshGenerator(2, 2, 0.5f);
    }

    [Button]
    private void Generate()
    {
        _meshGenerator = new MeshGenerator(2, 2, 0.5f);
        var tex = _texture2Ds[Random.Range(0, _texture2Ds.Count)];
        var pixels = tex.GetPixels();
        var data = pixels.Select(x => x.a < 0.5 ? 1 : 0).ToArray();
        var mesh = _meshGenerator.GenerateCutoutMesh(data, tex.width, tex.height);
        meshFilter.mesh = mesh;
    }
}
