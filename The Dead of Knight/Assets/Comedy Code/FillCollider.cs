using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Triangulate;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]

public class FillCollider : MonoBehaviour
{
    [FormerlySerializedAs("Mesh Filter")]
    public MeshFilter _mf;

    private PolygonCollider2D _polyCollider ;
    
    private Mesh _mesh;

    void Start ()
    {
        _polyCollider = gameObject.GetComponent<PolygonCollider2D>();

        _mesh = new Mesh();
    }

    private void Update() {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>(); 

        List<Vector2> points = new List<Vector2>();
        

        Vector2[] arrayPoints = _polyCollider.points;

        for (int i = 0; i < _polyCollider.GetTotalPointCount(); i++)
        {
            points.Add(new Vector2(arrayPoints[i].x, arrayPoints[i].y));
        }

        var triangles = Triangulator.Triangulate(points);
        Triangulator.AddTrianglesToMesh(ref verts, ref tris, triangles, -4, true);

        _mesh.vertices = verts.ToArray();
        _mesh.triangles = tris.ToArray();

        _mf.sharedMesh = _mesh;
    }
}