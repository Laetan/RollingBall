using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PathCreation.Examples
{
    public class RoadMeshColliderCreator2D : RoadMeshCreator2D
    {
        EdgeCollider2D edgeCollider;

        protected override void PathUpdated()
        {
            base.PathUpdated();
            if (pathCreator != null)
            {
                AddCollider();
            }
        }
        void AddCollider()
        {
            if (!meshHolder.gameObject.GetComponent<EdgeCollider2D>())
            {
                meshHolder.gameObject.AddComponent<EdgeCollider2D>();
                edgeCollider = meshHolder.gameObject.GetComponent<EdgeCollider2D>();
            }
            Vector2[] points = new Vector2[path.NumPoints];
            for(int i = 0; i < path.NumPoints; i++)
            {
                points[i] = path.GetPoint(i);
            }
            edgeCollider.points = points;
            edgeCollider.edgeRadius = roadWidth;
        }

    }
}