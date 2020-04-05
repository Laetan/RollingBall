using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Tools
{
    public class SimpleEdgeCreator : PathCreation.Examples.PathSceneTool
    {
        enum Side { left, right };
        //public PathCreator pathCreator;
        public bool createLeftEdge;
        public bool createRightEdge;
        public float roadWidth = 1f;
        public float edgeWidth = .05f;

        GameObject leftEdge;
        GameObject rightEdge;

        Mesh mesh;

        override protected void PathUpdated()
        {
            if (pathCreator != null && mesh == null)
            {
                mesh = pathCreator.GetComponentInChildren<MeshFilter>().sharedMesh;
            }
            if (createRightEdge)
            {
                if (rightEdge == null)
                {
                    rightEdge = new GameObject("RightEdge");
                    rightEdge.transform.rotation = Quaternion.identity;
                    rightEdge.transform.position =  Vector3.back;
                    rightEdge.transform.localScale = Vector3.one;
                    rightEdge.transform.parent = gameObject.transform;
                    rightEdge.AddComponent<PathCreator>();
                    rightEdge.AddComponent<RoadMeshColliderCreator2D>();
                }
                CreateEdgePath(Side.right);
            }
            else if(rightEdge != null)
            {
                DestroyImmediate(rightEdge);
                rightEdge = null;
            }
            if (createLeftEdge)
            {
                if (leftEdge == null)
                {
                    leftEdge = new GameObject("LeftEdge");
                    leftEdge.transform.rotation = Quaternion.identity;
                    leftEdge.transform.position =  Vector3.back;
                    leftEdge.transform.localScale = Vector3.one;
                    leftEdge.transform.parent = gameObject.transform;
                    leftEdge.AddComponent<PathCreator>();
                    leftEdge.AddComponent<RoadMeshColliderCreator2D>();

                }
                CreateEdgePath(Side.left);
            }
            else if (leftEdge != null){
                DestroyImmediate(leftEdge);
                leftEdge = null;
            }

            

        }
        
        void CreateEdgePath(Side side)
        {

            Vector3[] vertices = mesh.vertices;
            Vector2[] sidePoints = new Vector2[mesh.vertexCount / 2];
            for (int i = 0, v = (side == Side.left ? 0 : 1); v < mesh.vertexCount; i++, v += 2)
            {
                sidePoints[i] = vertices[v];
            }

            BezierPath sidePath = new BezierPath(sidePoints, false, PathSpace.xy);
            if (side == Side.right)
            {
                rightEdge.GetComponent<PathCreator>().bezierPath = sidePath;
                rightEdge.GetComponent<RoadMeshColliderCreator2D>().roadWidth = edgeWidth;

            }
            else
            {
                leftEdge.GetComponent<PathCreator>().bezierPath = sidePath;
                leftEdge.GetComponent<RoadMeshColliderCreator2D>().roadWidth = edgeWidth;

            }

        }
    }
}