using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour {
    private byte[,,] _map = new byte[Constants.chunkWidth, Constants.chunkHeight, Constants.chunkWidth];
    private MeshCollider _collider;
    private MeshRenderer _renderer;
    private MeshFilter _filter;

    void Awake()
    {
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();
        _filter = GetComponent<MeshFilter>();
    }

    // Use this for initialization
    void Start()
    {

    }

    public void setInfo(ChunkInfo info)
    {
        this._map = info.map;
        _filter.mesh = info.mesh;
        _collider.sharedMesh = info.mesh;
    }
}
