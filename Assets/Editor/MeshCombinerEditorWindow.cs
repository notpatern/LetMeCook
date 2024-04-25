using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
#if UNITY_EDITOR
public class MeshCombinerEditorWindow : EditorWindow
{
    GameObject meshToCombine;
    bool isAddMeshCollider;
    PhysicMaterial physicMaterial;

    [MenuItem("LetMeCook/Tools/MeshCombiner")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(MeshCombinerEditorWindow));
        window.Show();
    }

    void OnGUI()
    {
        meshToCombine = (GameObject)EditorGUILayout.ObjectField("Parent à combiner:", meshToCombine, typeof(GameObject), true);
        isAddMeshCollider = EditorGUILayout.Toggle("Ajouter un MeshCollider:", isAddMeshCollider);
        if (isAddMeshCollider)
            physicMaterial = (PhysicMaterial)EditorGUILayout.ObjectField("physics component (optional):", physicMaterial, typeof(PhysicMaterial), false);

        if (GUILayout.Button("Combine"))
        {
            Combine(meshToCombine, isAddMeshCollider, physicMaterial);

            meshToCombine = null;
        }
    }

    public void Combine(GameObject meshToCombine, bool isAddMeshCollider, PhysicMaterial physicMaterial)
    {
        if (!meshToCombine)
            return;

        Vector3 oldPos = meshToCombine.transform.position;
        Quaternion oldRot = meshToCombine.transform.rotation;
        meshToCombine.transform.position = Vector3.zero;
        meshToCombine.transform.rotation = Quaternion.identity;

        MeshFilter[] meshFilters = meshToCombine.GetComponentsInChildren<MeshFilter>();
        Matrix4x4[] oldPosMatrix = new Matrix4x4[meshFilters.Length];

        List<Material> materials = new List<Material>();
        MeshRenderer[] renderers = meshToCombine.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.transform == meshToCombine.transform)
                continue;
            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localMat in localMats)
                if (!materials.Contains(localMat))
                    materials.Add(localMat);
        }

        List<Mesh> submeshes = new List<Mesh>();
        foreach (Material material in materials)
        {
            //faire un combiner pour chaque (sub)mesh qui est mapped sur le bon material
            List<CombineInstance> combiners = new List<CombineInstance>();
            for (int i = 0; i < meshFilters.Length; i++)
            {
                //Set la pos matriciale(se dit?) de chaque mesh
                oldPosMatrix[i] = meshFilters[i].transform.localToWorldMatrix;
                if (meshFilters[i].transform == meshToCombine.transform) continue;
                MeshRenderer renderer = renderers[i];

                if (renderer == null)
                {
                    Debug.LogError(meshFilters[i].name + " has no MeshRenderer");
                    continue;
                }

                //regarde si les materiaux sont ceux qu'on veut maintenant
                Material[] localMaterials = renderer.sharedMaterials;
                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
                {
                    if (localMaterials[materialIndex] != material)
                        continue;

                    //ce submesh est le material qu'on cherche maintenant (on lui set la pos matriciale(vraiment jsp si ça se dit))
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = meshFilters[i].sharedMesh;
                    ci.subMeshIndex = materialIndex;
                    ci.transform = oldPosMatrix[i];
                    combiners.Add(ci);
                }
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray(), true);
            submeshes.Add(mesh);
        }

        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        foreach (Mesh mesh in submeshes)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiners.Add(ci);
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(finalCombiners.ToArray(), false);

        if (!meshToCombine.GetComponent<MeshFilter>())//si pas de component meshFilter en ajoute un
            meshToCombine.AddComponent<MeshFilter>();
        meshToCombine.GetComponent<MeshFilter>().sharedMesh = finalMesh;
        if (!meshToCombine.GetComponent<MeshRenderer>())//si pas de component meshRenderer en ajoute un
            meshToCombine.AddComponent<MeshRenderer>();
        meshToCombine.GetComponent<MeshRenderer>().materials = materials.ToArray();

        /*#if UNITY_EDITOR
        Unwrapping.GenerateSecondaryUVSet(finalMesh); remetre si bake avec lightmap
        #endif*/

        if (isAddMeshCollider)
        {
            if (!meshToCombine.GetComponent<MeshCollider>())
                meshToCombine.AddComponent<MeshCollider>();
            meshToCombine.GetComponent<MeshCollider>().material = physicMaterial;
        }

        foreach (MeshFilter filter in meshFilters)
            if (filter != meshToCombine.GetComponent<MeshFilter>())
                filter.gameObject.SetActive(false);

        meshToCombine.transform.position = oldPos;
        meshToCombine.transform.rotation = oldRot;
    }
}
#endif