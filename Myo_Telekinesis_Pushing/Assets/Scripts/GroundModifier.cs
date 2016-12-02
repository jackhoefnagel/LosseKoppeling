using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundModifier : MonoBehaviour {

    public GameObject modifiedObject;
    public bool wave = false;
    public bool moveGround = false;

    Mesh modifiedMesh;
    Vector3[] vertices;
    Vector3[] normals;
    MeshCollider collider;


    public Vector3 point = Vector3.zero;
    public float height = 0;
    public float range = 2;
    public float strength = 0;

    void Start()
    {
        modifiedMesh = modifiedObject.GetComponent<MeshFilter>().mesh;
        vertices = modifiedMesh.vertices;
        normals = modifiedMesh.normals;
        collider = modifiedObject.GetComponent<MeshCollider>();
    }

    void Update () {
        //Wave the mesh and return
        if (wave)
            Wave();

        if (moveGround)
        {
            Debug.Log(point);
            MoveGround(point, range, height, strength);
        }
    }

    private void MoveGround(Vector3 location, float range, float height, float strength)
    {
        int i = 0;
        while (i < vertices.Length)
        {
            if (Vector3.Distance(new Vector3(vertices[i].x, 0, vertices[i].z), new Vector3(location.x,0, location.z)) < range)
                vertices[i] = Vector3.Lerp(vertices[i], normals[i] * height + new Vector3(vertices[i].x, 0, vertices[i].z), strength * Time.deltaTime); //normals[i] * Mathf.Sin(Time.time * 5 + (-Mathf.Abs(vertices[i].x) + -Mathf.Abs(vertices[i].z)) / 2) / 50;
            i++;
        }
        modifiedMesh.vertices = vertices;
        collider.sharedMesh = modifiedMesh;
    }

    private void Wave()
    {
        int i = 0;
        while (i < vertices.Length)
        {
            vertices[i] += normals[i] * Mathf.Sin(Time.time * 5 + (-Mathf.Abs(vertices[i].x) + -Mathf.Abs(vertices[i].z)) / 2) / 50;
            i++;
        }
        modifiedMesh.vertices = vertices;
        collider.sharedMesh = modifiedMesh;
    }
}
