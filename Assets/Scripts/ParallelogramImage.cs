using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

[RequireComponent(typeof(CanvasRenderer))]
public class ParallelogramImage : Image {
    [SerializeField]
    [Range(-45f, 45f)]
    private float skewAngle = 15f;

    [SerializeField]
    private Color edgeColor = new Color(0.5f, 0.8f, 1f, 1f);

    [SerializeField]
    [Range(0f, 20f)]
    private float edgeThickness = 3f;

    // Force refresh when values change in inspector
    void OnValidate() {
        base.SetVerticesDirty();
        base.SetMaterialDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh) {
        Debug.Log($"OnPopulateMesh called. SkewAngle: {skewAngle}, EdgeThickness: {edgeThickness}"); // Debug log

        vh.Clear();

        Rect r = GetPixelAdjustedRect();
        float skewAmount = Mathf.Tan(skewAngle * Mathf.Deg2Rad) * r.height;
        float halfSkew = skewAmount / 2f;  // Calculate half the skew amount

        Vector2[] vertices = new Vector2[]
        {
            new Vector2(r.x + halfSkew + edgeThickness, r.y),            // Bottom-left
            new Vector2(r.x - halfSkew + edgeThickness, r.y + r.height), // Top-left
            new Vector2(r.x + r.width - halfSkew, r.y + r.height),       // Top-right
            new Vector2(r.x + r.width + halfSkew, r.y),                  // Bottom-right
        };

        UIVertex vert = UIVertex.simpleVert;

        // Main parallelogram
        for (int i = 0; i < 4; i++) {
            vert.position = vertices[i];
            vert.color = color;
            vert.uv0 = new Vector2(i == 1 || i == 2 ? 1 : 0, i >= 2 ? 1 : 0);
            vh.AddVert(vert);
        }

        // Edge vertices
        Vector2[] edgeVerts = new Vector2[]
        {
            new Vector2(r.x + halfSkew, r.y),                           // Bottom-left edge
            new Vector2(r.x - halfSkew, r.y + r.height),                // Top-left edge
            new Vector2(r.x - halfSkew + edgeThickness, r.y + r.height),// Top-left inner
            new Vector2(r.x + halfSkew + edgeThickness, r.y),           // Bottom-left inner
        };

        for (int i = 0; i < 4; i++) {
            vert.position = edgeVerts[i];
            vert.color = edgeColor;
            vert.uv0 = new Vector2(i >= 2 ? 1 : 0, i == 1 || i == 2 ? 1 : 0);
            vh.AddVert(vert);
        }

        // Add triangles
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
        vh.AddTriangle(4, 5, 6);
        vh.AddTriangle(4, 6, 7);

        Debug.Log("Mesh populated"); // Debug log
    }

    public void ForceUpdate() {
        SetVerticesDirty();
        SetMaterialDirty();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ParallelogramImage))]
public class ParallelogramImageEditor : ImageEditor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.LabelField("Parallelogram Settings", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        var skewProp = serializedObject.FindProperty("skewAngle");
        EditorGUILayout.PropertyField(skewProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Edge Settings", EditorStyles.boldLabel);
        var edgeColorProp = serializedObject.FindProperty("edgeColor");
        var edgeThicknessProp = serializedObject.FindProperty("edgeThickness");
        EditorGUILayout.PropertyField(edgeColorProp);
        EditorGUILayout.PropertyField(edgeThicknessProp);

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
            (target as ParallelogramImage).ForceUpdate();
            Debug.Log("Properties changed in inspector"); // Debug log
        }

        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}
#endif