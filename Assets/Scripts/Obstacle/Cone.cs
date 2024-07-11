using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinCastejon.ConeMesh
{
    //public enum ConeOrientation
    //{
    //    X,
    //    Y,
    //    Z
    //}

    public class Cone : MonoBehaviour
    {
        //[SerializeField]
        //private bool m_PivotAtTop = true;
        //[SerializeField]
        //private ConeOrientation m_Orientation = ConeOrientation.Z;
        //[SerializeField]
        //private bool m_InvertDirection;
        //[SerializeField]
        //private bool m_IsTrigger;
        //[SerializeField]
        //private Material m_Material;
        //[Min(3)]
        //[SerializeField]
        //private int m_ConeSides = 24;
        //[SerializeField]
        //private bool m_ProportionalRadius;
        //[Min(float.Epsilon)]
        //[SerializeField]
        //private float m_ConeRadius = 0.5f;
        //[SerializeField]
        //private float m_ConeHeight = 1.2f;

        //private Mesh m_ConeMesh;
        //private MeshFilter m_MeshFilter;
        //private MeshRenderer m_MeshRenderer;
        //private MeshCollider m_MeshCollider;

        //public bool PivotAtTop { get => m_PivotAtTop; set { m_PivotAtTop = value; GenerateCone(); } }
        //public Material Material { get => m_Material; set { m_Material = value; m_MeshRenderer = m_MeshRenderer ? m_MeshRenderer : gameObject.GetComponent<MeshRenderer>(); m_MeshRenderer.material = m_Material; } }
        //public int ConeSubdivisions { get => m_ConeSides; set { m_ConeSides = value; GenerateCone(); } }
        //public float ConeRadius { get => m_ConeRadius; set { m_ConeRadius = value; GenerateCone(); } }
        //public float ConeHeight { get => m_ConeHeight; set { m_ConeHeight = value; GenerateCone(); } }
        //public ConeOrientation Orientation { get => m_Orientation; set { m_Orientation = value; GenerateCone(); } }
        //public bool IsConeGenerated { get => m_ConeMesh != null; }
        //public bool IsTrigger
        //{
        //    get => m_IsTrigger;
            
        //    set
        //    {
        //        m_IsTrigger = value; m_MeshCollider = m_MeshCollider ? m_MeshCollider : gameObject.GetComponent<MeshCollider>();
        //        if (m_IsTrigger)
        //        {
        //            m_MeshCollider.convex = true;
        //        }
        //        m_MeshCollider.isTrigger = value;
        //    }
        //}
        //public bool ProportionalRadius { get => m_ProportionalRadius; set { m_ProportionalRadius = value; GenerateCone(); } }

        //internal void GenerateCone()
        //{
        //    m_ConeMesh = CreateConeMesh(m_ConeSides + 1, m_ConeRadius, m_ConeHeight, m_PivotAtTop, m_Orientation, m_InvertDirection, m_ProportionalRadius);
        //    m_MeshFilter = m_MeshFilter ? m_MeshFilter : gameObject.GetComponent<MeshFilter>();
        //    m_MeshRenderer = m_MeshRenderer ? m_MeshRenderer : gameObject.GetComponent<MeshRenderer>();
        //    m_MeshCollider = m_MeshCollider ? m_MeshCollider : gameObject.GetComponent<MeshCollider>();

        //    m_MeshFilter.sharedMesh = m_ConeMesh;

        //    m_MeshRenderer.additionalVertexStreams = m_ConeMesh;
        //    m_MeshRenderer.material = m_Material;
        //    m_MeshCollider.sharedMesh = m_ConeMesh;
        //    m_MeshCollider.convex = true;
        //    m_MeshCollider.isTrigger = m_IsTrigger;
        //}

        //private static Mesh CreateConeMesh(int p_subdivisions, float p_radius, float p_height, bool p_pivotAtTop, ConeOrientation p_orientation, bool p_invertDirection, bool p_proportionalRadius)
        //{
        //    if (p_proportionalRadius)
        //    {
        //        p_radius *= p_height;
        //    }
        //    if (p_invertDirection)
        //    {
        //        p_height = -p_height;
        //    }
        //    Mesh mesh = new Mesh();
        //    Vector3[] vertices = new Vector3[p_subdivisions + 2];
        //    Vector2[] uv = new Vector2[vertices.Length];
        //    int[] triangles = new int[(p_subdivisions * 2) * 3];

        //    if (p_orientation == ConeOrientation.X)
        //    {
        //        vertices[0] = p_pivotAtTop ? Vector3.right * p_height : Vector3.zero;
        //    }
        //    else if (p_orientation == ConeOrientation.Y)
        //    {
        //        vertices[0] = p_pivotAtTop ? Vector3.up * p_height : Vector3.zero;
        //    }
        //    else
        //    {
        //        vertices[0] = p_pivotAtTop ? Vector3.forward * p_height : Vector3.zero;
        //    }
        //    uv[0] = new Vector2(0.5f, 0f);
        //    for (int i = 0, n = p_subdivisions - 1; i < p_subdivisions; i++)
        //    {
        //        float ratio = (float)i / n;
        //        float r = ratio * (Mathf.PI * 2f);
        //        float x = Mathf.Cos(r) * p_radius;
        //        float z = Mathf.Sin(r) * p_radius;
        //        if (p_orientation == ConeOrientation.X)
        //        {
        //            vertices[i + 1] = new Vector3(p_pivotAtTop ? p_height : 0f, x, z);
        //        }
        //        else if (p_orientation == ConeOrientation.Y)
        //        {
        //            vertices[i + 1] = new Vector3(x, p_pivotAtTop ? p_height : 0f, z);
        //        }
        //        else
        //        {
        //            vertices[i + 1] = new Vector3(x, z, p_pivotAtTop ? p_height : 0f);
        //        }
        //        uv[i + 1] = new Vector2(ratio, 0f);
        //    }
        //    if (p_orientation == ConeOrientation.X)
        //    {
        //        vertices[p_subdivisions + 1] = !p_pivotAtTop ? Vector3.right * p_height : Vector3.zero;
        //    }
        //    else if (p_orientation == ConeOrientation.Y)
        //    {
        //        vertices[p_subdivisions + 1] = !p_pivotAtTop ? Vector3.up * p_height : Vector3.zero;
        //    }
        //    else
        //    {
        //        vertices[p_subdivisions + 1] = !p_pivotAtTop ? Vector3.forward * p_height : Vector3.zero;
        //    }
        //    uv[p_subdivisions + 1] = new Vector2(0.5f, 1f);

        //    for (int i = 0, n = p_subdivisions - 1; i < n; i++)
        //    {
        //        int offset = i * 3;
        //        triangles[offset] = 0;
        //        triangles[offset + 1] = i + 1;
        //        triangles[offset + 2] = i + 2;
        //    }

        //    int bottomOffset = p_subdivisions * 3;
        //    for (int i = 0, n = p_subdivisions - 1; i < n; i++)
        //    {
        //        int offset = i * 3 + bottomOffset;
        //        triangles[offset] = i + 1;
        //        triangles[offset + 1] = p_subdivisions + 1;
        //        triangles[offset + 2] = i + 2;
        //    }

        //    mesh.vertices = vertices;
        //    mesh.uv = uv;
        //    mesh.triangles = triangles;
        //    mesh.RecalculateBounds();
        //    mesh.RecalculateNormals();

        //    return mesh;
        //}

        // 폭발 또는 가스폭발을 봤을 경우
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Explosion" || other.gameObject.tag == "GasExplosion")
            {
                // 스테이지 클리어 실패
                StartCoroutine(GameOver());
            }
        }

        // 게임 오버
        private IEnumerator GameOver()
        {
            yield return new WaitForSeconds(2.0f);

            GameManager.Instance.m_IsFailed = true;
            GameManager.Instance.m_IsGameOver = true;
        }
    }
}
