using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private Human human;
    [SerializeField] private MyWebReader myWebReader;
    [SerializeField] private List<Texture2D> textures;
    [SerializeField] private GameObject prefabWall;
    [SerializeField] private Transform spawnPointWall;
    [SerializeField] private Transform endPointWall;
    [SerializeField] private float timeWall;
    
    private MeshGenerator _meshGenerator;
    private bool _wallLogicFail;
    
    private void Awake()
    {
        _meshGenerator = new MeshGenerator(2, 2, 0.5f);
        human.Trigger += HumanOnTrigger;
        myWebReader.DataTrigger += DataTrigger;
    }
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        myWebReader.NeedData();
    }
    
    private void DataTrigger(WallDataModel data)
    {
        Spawn(data.Data.ToArray(), 128, 128);
    }
    
    private void HumanOnTrigger(GameObject obj)
    {
        Fail();
    }
    
    private void Fail()
    {
        _wallLogicFail = true;
    }
    
    private void Success()
    {
        
    }
    
    private void EndWall()
    {
        if (!_wallLogicFail)
        {
            Success();
        }
        myWebReader.NeedData();
    }
    
    [Button]
    private void Generate()
    {
        var tex = textures[Random.Range(0, textures.Count)];
        var pixels = tex.GetPixels();
        var data = pixels.Select(x => x.a < 0.5 ? 1 : 0).ToArray();
        Spawn(data, tex.width, tex.height);
    }
    
    [Button]
    private void Spawn(int[] data, int width, int height)
    {
        var obj = Instantiate(prefabWall);
        obj.transform.position = spawnPointWall.position;
        var meshFilter = obj.GetComponent<MeshFilter>();
        meshFilter.mesh = _meshGenerator.GenerateCutoutMesh(data, width, height);
        obj.transform.DOMove(endPointWall.position, timeWall).OnComplete(() =>
        {
            EndWall();
            Destroy(obj);
        });
    }
}
