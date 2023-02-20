using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshHell : MonoBehaviour
{

    public float Displacment = 0.05f;
    public float speed = 1f;
    public float Amp = 1f;

    public List<MeshFilter> Meshes = new List<MeshFilter>();

    private Mesh _mesh;

    private Vector3[] _verts = new Vector3[4];
    private Vector2[] _uv = new Vector2[4];
    private int[] _tris = new int[6];


    private void Start() {
        _mesh = new Mesh();

        _verts[0] = new Vector3(-0.5f,-0.5f);
        _verts[1] = new Vector3(-0.5f, 0.5f);
        _verts[2] = new Vector3( 0.5f, 0.5f);
        _verts[3] = new Vector3( 0.5f,-0.5f);

        _uv[0] = new Vector2(0,0);
        _uv[1] = new Vector2(0,1);
        _uv[2] = new Vector2(1,1);
        _uv[3] = new Vector2(1,0);

        _tris[0] = 0;
        _tris[1] = 1;
        _tris[2] = 2;
        
        _tris[3] = 0;
        _tris[4] = 2;
        _tris[5] = 3;

        _mesh.vertices = _verts;
        _mesh.uv = _uv;
        _mesh.triangles = _tris;

        foreach (var mfilter in Meshes)
        {
            mfilter.sharedMesh = _mesh;
        }
    }

    private void Update() {

        _verts[1] = new Vector3(-0.5f,f(0)-0.5f);
        _verts[2] = new Vector3(0.5f,f(1)-0.5f);

        _uv[1] = new Vector2(0,f(0));
        _uv[2] = new Vector2(1,f(1));

        _mesh.uv = _uv;
        _mesh.vertices = _verts;

        foreach (var mfilter in Meshes)
        {
            mfilter.sharedMesh = _mesh;
        }
    }


    private float f(int x)
    {
        double fx = Math.Sin((Amp*x)+(Time.time*speed))*Math.Sin(0.6*((Amp*x)+(Time.time*speed)));
        double gx = ((Displacment*fx)+(1-Displacment));
        return (float)gx;
    }
}
