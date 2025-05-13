using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ArcMeshGenerator : MonoBehaviour
{
    public int segments = 32;
    public float radius = 0.2f;

    private Mesh mesh;

    public void GenerateArc(Vector3 center, Vector3 dir1, Vector3 dir2)
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        Vector3 normal = Vector3.Cross(dir1, dir2).normalized;
        float angle = Vector3.Angle(dir1, dir2);

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float currentAngle = Mathf.Lerp(0, angle, t) * Mathf.Deg2Rad;
            Quaternion rot = Quaternion.AngleAxis(currentAngle, normal);
            vertices[i + 1] = rot * dir1.normalized * radius;
        }

        for (int i = 0; i < segments; i++)
        {
            int start = i + 1;
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = start;
            triangles[i * 3 + 2] = start + 1;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        transform.position = center;
        transform.rotation = Quaternion.LookRotation(normal);
    }

    public void ClearArc()
    {
        if (mesh != null)
            mesh.Clear();
    }
}
