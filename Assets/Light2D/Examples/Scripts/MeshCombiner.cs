using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Light2D.Examples
{
    /// <summary>
    /// Combines child mesh renderers into one mesh at startup.
    /// It gives much better performance for small meshes than StaticBatchingUtility.Combine.
    /// </summary>
    public class MeshCombiner : MonoBehaviour
    {
        private bool _isDone = false;

        void OnEnable()
        {
            if (!_isDone)
            {
                StopCoroutine("Combine");
                StartCoroutine(Combine());
            }
        }

        private IEnumerator Combine()
        {
            yield return null;
            List<IGrouping<GroupKey, Renderer>> groups = GetComponentsInChildren<Renderer>()
                .Where(r => r.GetComponent<MeshRenderer>() != null && r.GetComponent<MeshFilter>() != null &&
                     r.GetComponent<MeshFilter>().sharedMesh != null)
                .GroupBy(r => new GroupKey
                {
                    material = r.sharedMaterial,
                    sortingOrder = r.sortingOrder,
                    layer = r.gameObject.layer
                })
                .ToList();

            List<Vector3> vertices = new List<Vector3>(1024);
            List<int> triangles = new List<int>(1024);
            List<Vector2> uv0 = new List<Vector2>(1024);
            List<Vector2> uv1 = new List<Vector2>(1024);
            List<Color> colors = new List<Color>(1024);
            List<Vector4> tangents = new List<Vector4>(1024);

            foreach (IGrouping<GroupKey, Renderer> group in groups)
            {
                GameObject obj = new GameObject("Combined Mesh");
                obj.transform.parent = transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                obj.layer = group.Key.layer;

                MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
                meshRenderer.material = Instantiate(group.Key.material);
                meshRenderer.sortingOrder = group.Key.sortingOrder;

                Mesh firstMesh = group.First().GetComponent<MeshFilter>().mesh;

                bool useUv0 = firstMesh.uv != null && firstMesh.uv.Length != 0;
                bool useUv1 = firstMesh.uv2 != null && firstMesh.uv2.Length != 0;
                bool useColors = firstMesh.colors != null && firstMesh.colors.Length != 0;

                vertices.Clear();
                triangles.Clear();
                tangents.Clear();

                if (useUv0) uv0.Clear();
                if (useUv1) uv1.Clear();
                if (useColors) colors.Clear();

                foreach (Renderer meshPart in group)
                {
                    MeshFilter filter = meshPart.GetComponent<MeshFilter>();
                    Mesh smallMesh = filter.mesh;
                    int startVertexIndex = vertices.Count;
                    vertices.AddRange(smallMesh.vertices.Select(v => filter.transform.TransformPoint(v)));
                    triangles.AddRange(smallMesh.triangles.Select(t => t + startVertexIndex));

                    IEnumerable<Vector4> localTangents = smallMesh.tangents == null || smallMesh.tangents.Length == 0
                        ? Enumerable.Repeat(new Vector4(1, 0), smallMesh.vertexCount)
                        : smallMesh.tangents;
                    tangents.AddRange(localTangents.Select(t => (Vector4)filter.transform.TransformVector(t)));

                    if (useUv0) uv0.AddRange(smallMesh.uv);
                    if (useUv1) uv1.AddRange(smallMesh.uv2);
                    if (useColors) colors.AddRange(smallMesh.colors);

                    Destroy(meshPart);
                    Destroy(filter);
                    CustomSprite customSprite = meshPart.GetComponent<CustomSprite>();
                    if (customSprite != null)
                        Destroy(customSprite);
                }

                MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
                Mesh mesh = meshFilter.mesh = new Mesh();

                mesh.vertices = vertices.ToArray();
                mesh.triangles = triangles.ToArray();
                mesh.tangents = tangents.ToArray();
                if (useUv0) mesh.uv = uv0.ToArray();
                if (useUv1) mesh.uv2 = uv1.ToArray();
                if (useColors) mesh.colors = colors.ToArray();
            }

            _isDone = true;
        }

        struct GroupKey : IEquatable<GroupKey>
        {
            public Material material;
            public int sortingOrder;
            public int layer;

            public bool Equals(GroupKey other)
            {
                return Equals(material, other.material) && sortingOrder == other.sortingOrder && layer == other.layer;
            }

            private sealed class MaterialSortingOrderLayerEqualityComparer : IEqualityComparer<GroupKey>
            {
                public bool Equals(GroupKey x, GroupKey y)
                {
                    return Equals(x.material, y.material) && x.sortingOrder == y.sortingOrder && x.layer == y.layer;
                }

                public int GetHashCode(GroupKey obj)
                {
                    unchecked
                    {
                        int hashCode = (obj.material != null ? obj.material.GetHashCode() : 0);
                        hashCode = (hashCode*397) ^ obj.sortingOrder;
                        hashCode = (hashCode*397) ^ obj.layer;
                        return hashCode;
                    }
                }
            }

            private static readonly IEqualityComparer<GroupKey> MaterialSortingOrderLayerComparerInstance = new MaterialSortingOrderLayerEqualityComparer();

            public static IEqualityComparer<GroupKey> MaterialSortingOrderLayerComparer
            {
                get { return MaterialSortingOrderLayerComparerInstance; }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is GroupKey && Equals((GroupKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = (material != null ? material.GetHashCode() : 0);
                    hashCode = (hashCode*397) ^ sortingOrder;
                    hashCode = (hashCode*397) ^ layer;
                    return hashCode;
                }
            }

            public static bool operator ==(GroupKey left, GroupKey right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(GroupKey left, GroupKey right)
            {
                return !left.Equals(right);
            }
        }
    }
}
