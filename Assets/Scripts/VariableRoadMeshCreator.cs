using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PathCreation.Tools
{

    public class VariableRoadMeshCreator : RoadMeshCreator2D
    {

        public enum VariationType { Regular, Sinus, Pipe };
        [Header("Road variation settings")]
        [Range(-0.9f, 0.9f)] public float widthVariation = 0f;
        [Range(0, 1)] public float variationCenter = 0.5f;
        [Range(0, 1)] public float variationSize = 0.5f;

        public VariationType variationType = VariationType.Regular;


        protected override float getCurrentRoadWidth(float progression)
        {
            //Debug.Log("getCurrentRoadWidth(" + progression + ")");
            if( progression > variationCenter + variationSize || progression < variationCenter - variationSize)
            {
                return roadWidth;
            }
            if(variationType == VariationType.Pipe)
            {
                return roadWidth * ( 1f + widthVariation);
            }
            else if(variationType == VariationType.Regular)
            {
                return roadWidth * (1f + widthVariation) - Math.Abs(progression - variationCenter ) * (roadWidth *  widthVariation) / variationSize;
            }
            else
            {
                return roadWidth + (float) Math.Cos( (progression - variationCenter) * Math.PI / ( 2f * variationSize)) * (roadWidth * widthVariation);
            }

        }
    }

}