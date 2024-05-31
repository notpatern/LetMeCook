using ItemLaunch;
using UnityEditor;
using UnityEngine;

public class LaunchItemTraceEditorWindow : EditorWindow
{
    ItemLauncher m_ItemLauncher;
    GameObject m_LauncherGo;
    LineRenderer m_LineRenderer;
    [SerializeField] float width;
    int numPoints = 50;
    Vector3[] positions = new Vector3[50];

    [MenuItem("Window/Item Launcher Bezier")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LaunchItemTraceEditorWindow));
    }

    private void OnGUI()
    {
        m_LauncherGo = EditorGUILayout.ObjectField(m_ItemLauncher, typeof(GameObject), true, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as GameObject;

        if (m_LauncherGo)
        {
            m_ItemLauncher = m_LauncherGo.GetComponent<ItemLauncher>();
        }

        if (m_ItemLauncher)
        {
            m_LineRenderer = m_ItemLauncher.lineRenderer;

            m_LineRenderer.positionCount = positions.Length;

            if (GUILayout.Button("Apply Beizer curve"))
            {
                DrawCubicCurve();
            }
        }

    }

    void DrawCubicCurve()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float _t = i / (float)numPoints;
            positions[i] = BezierCubicAlgorithm(_t);
        }
        m_LineRenderer.SetPositions(positions);
    }

    Vector3 BezierCubicAlgorithm(float currentT)
    {
        Vector3 a = Vector3.Lerp(m_ItemLauncher.StartPoint, m_ItemLauncher.StartTangent, currentT);
        Vector3 b = Vector3.Lerp(m_ItemLauncher.StartTangent, m_ItemLauncher.EndTangent, currentT);
        Vector3 c = Vector3.Lerp(m_ItemLauncher.EndTangent, m_ItemLauncher.EndPoint, currentT);
        Vector3 d = Vector3.Lerp(a, b, currentT);
        Vector3 e = Vector3.Lerp(b, c, currentT);

        return Vector3.Lerp(d, e, currentT);
    }
}