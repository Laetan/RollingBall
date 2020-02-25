using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    public class SimpleEdgeCreator : PathSceneTool
    {
        enum Side { left, right };
        //public PathCreator pathCreator;
        public bool createLeftEdge;
        public bool createRightEdge;
        public float roadWidth = .4f;
        public float edgeWidth = .05f;

        GameObject leftEdge;
        GameObject rightEdge;

        override protected void PathUpdated()
        {
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

        }



        void CreateEdgePath(Side side)
        {
            Vector2[] sidePoints = new Vector2[pathCreator.path.NumPoints];
            for (int i = 0; i < pathCreator.path.NumPoints; i++)
            {
                sidePoints[i] = pathCreator.path.GetPoint(i) + (side == Side.right ? 1 : -1) * pathCreator.path.GetNormal(i) * Mathf.Abs(roadWidth);
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