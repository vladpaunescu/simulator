/**
 * Copyright (c) 2018 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */


﻿using UnityEditor;
using UnityEngine;
using static Map.Autoware.VectorMapUtility;

[CustomEditor(typeof(MapSegmentBuilder)), CanEditMultipleObjects]
public class MapSegmentBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapSegmentBuilder mapSegment = (MapSegmentBuilder)target;
        Undo.RecordObject(mapSegment, "change points");

        if (GUILayout.Button("Add Point"))
        {
            mapSegment.AddPoint();
        }

        if (GUILayout.Button("Remove Point"))
        {
            mapSegment.RemovePoint();
        }

        if (GUILayout.Button("Reverse Points"))
        {
            mapSegment.ReversePoints();
        }

        if (GUILayout.Button("Reset Points"))
        {
            mapSegment.ResetPoints();
        }      
    }

    protected virtual void OnSceneGUI()
    {
        MapSegmentBuilder vmSegBuilder = (MapSegmentBuilder)target;
        Undo.RecordObject(vmSegBuilder, "Segment points change");

        var localPositions = vmSegBuilder.segment.targetLocalPositions;

        var pointCount = localPositions.Count;

        if (pointCount < 1)
        {
            return;
        }

        Transform mainTrans = vmSegBuilder.transform;

        if (vmSegBuilder.showHandles)
        {
            for (int i = 0; i < pointCount - 1; i++)
            {
                Vector3 newTargetPosition = Handles.PositionHandle(mainTrans.TransformPoint(localPositions[i]), Quaternion.identity);
                localPositions[i] = mainTrans.InverseTransformPoint(newTargetPosition);
            }
            Vector3 lastPoint = Handles.PositionHandle(mainTrans.TransformPoint(localPositions[pointCount - 1]), Quaternion.identity);
            localPositions[pointCount - 1] = mainTrans.InverseTransformPoint(lastPoint);
        }
    }
}