using System.Collections.Generic;
using PathCreation.Utility;
using UnityEngine;

namespace PathCreation.Examples {
    public class RoadMeshCreator2D : PathSceneTool {
        [Header ("Road settings")]
        public float roadWidth = .4f;

        [Header ("Material settings")]
        public Material roadMaterial;
        public float textureTiling = 1;

        [SerializeField, HideInInspector]
        protected GameObject meshHolder;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        Mesh mesh;

        protected override void PathUpdated () {
            if (pathCreator != null) {
                AssignMeshComponents ();
                AssignMaterials ();
                CreateRoadMesh ();
            }
        }

        void CreateRoadMesh () {
            Vector3[] verts = new Vector3[path.NumPoints * 2];
            Vector2[] uvs = new Vector2[verts.Length];
            Vector3[] normals = new Vector3[verts.Length];

            int numTris = 2 * (path.NumPoints - 1) + ((path.isClosedLoop) ? 2 : 0);
            int[] roadTriangles = new int[numTris * 3];

            int vertIndex = 0;
            int triIndex = 0;


            for (int i = 0; i < path.NumPoints; i++) {
                Vector3 localRight = path.GetNormal(i);

                // Find position to left and right of current path vertex
                Vector3 vertSideA = path.GetPoint (i) - localRight * Mathf.Abs (roadWidth);
                Vector3 vertSideB = path.GetPoint (i) + localRight * Mathf.Abs (roadWidth);

                // Add top of road vertices
                verts[vertIndex] = vertSideA;
                verts[vertIndex + 1] = vertSideB;

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                float completionPercent = i / (float)(path.NumPoints - 1);
                float v = 1 - Mathf.Abs(2 * completionPercent - 1);
                uvs[vertIndex] = new Vector2(0, v);
                uvs[vertIndex + 1] = new Vector2(1, v);

                if (i < path.NumPoints - 1 || path.isClosedLoop)
                {
                    roadTriangles[triIndex] = vertIndex;
                    roadTriangles[triIndex + 1] = (vertIndex + 2) % verts.Length;
                    roadTriangles[triIndex + 2] = vertIndex + 1;

                    roadTriangles[triIndex + 3] = vertIndex + 1;
                    roadTriangles[triIndex + 4] = (vertIndex + 2) % verts.Length;
                    roadTriangles[triIndex + 5] = (vertIndex + 3) % verts.Length;
                }
                normals[vertIndex] = Vector2.up;
                normals[vertIndex + 1] = Vector2.up;
                vertIndex += 2;
                triIndex += 6;
            }
            mesh.Clear ();
            mesh.vertices = verts;
            mesh.uv = uvs;
            //mesh.normals = normals;
            mesh.triangles = roadTriangles;
            mesh.RecalculateBounds ();
        }

        // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
        void AssignMeshComponents () {

            if (meshHolder == null) {
                meshHolder = new GameObject ("Road Mesh Holder");
                meshHolder.transform.parent = gameObject.transform;
                meshHolder.transform.rotation = Quaternion.identity;
                meshHolder.transform.position = gameObject.transform.position;
                meshHolder.transform.localScale = Vector3.one;
            }


            // Ensure mesh renderer and filter components are assigned
            if (!meshHolder.gameObject.GetComponent<MeshFilter> ()) {
                meshHolder.gameObject.AddComponent<MeshFilter> ();
            }
            if (!meshHolder.GetComponent<MeshRenderer> ()) {
                meshHolder.gameObject.AddComponent<MeshRenderer> ();
            }

            meshRenderer = meshHolder.GetComponent<MeshRenderer> ();
            meshFilter = meshHolder.GetComponent<MeshFilter> ();
            if (mesh == null) {
                mesh = new Mesh ();
            }
            meshFilter.sharedMesh = mesh;
        }

        void AssignMaterials () {
            if (roadMaterial != null ) {
                meshRenderer.sharedMaterial = roadMaterial;
                int textureRepeat = Mathf.RoundToInt(textureTiling * path.NumPoints * 1 * .05f);
                meshRenderer.sharedMaterial.mainTextureScale = new Vector3 (1, textureRepeat);
            }
        }

    }
}