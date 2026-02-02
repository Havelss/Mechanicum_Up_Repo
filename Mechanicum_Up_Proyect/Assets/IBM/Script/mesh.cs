using UnityEngine;
public class FaceMeshDeformer : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    void Update()
    {
        // prueba: mover el vértice 0 al presionar espacio
        if (Input.GetKey(KeyCode.Space))
        {
            vertices[0] += Vector3.up * Time.deltaTime;
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
        }
    }
}
