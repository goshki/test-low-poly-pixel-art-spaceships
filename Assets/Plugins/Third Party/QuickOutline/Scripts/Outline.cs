//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//
//  see: https://github.com/chrisnolet/QuickOutline/tree/feature/per-object/QuickOutline

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Outline : MonoBehaviour {

    private static HashSet<Mesh> registeredMeshes = new();

    public enum Mode {
        OutlineAll,
        OutlineVisible,
        OutlineVisibleAll,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly
    }

    public Mode OutlineMode {
        get => outlineMode;
        set {
            outlineMode = value;
            needsUpdate = true;
        }
    }

    public Color OutlineColor {
        get => outlineColor;
        set {
            outlineColor = value;
            needsUpdate = true;
        }
    }

    public float OutlineWidth {
        get => outlineWidth;
        set {
            outlineWidth = value;
            needsUpdate = true;
        }
    }

    [Serializable]
    private class ListVector3 {

        public List<Vector3> data;
    }

    [SerializeField]
    private Mode outlineMode;

    [SerializeField]
    private Color outlineColor = Color.white;

    [SerializeField, Range(0f, 10f)]
    private float outlineWidth = 2f;

    [Header("Optional"), Tooltip(
        "Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
        + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes."
    )]
    [SerializeField]
    private bool precomputeOutline;

    [SerializeField]
    private bool includeInactive; // include inactive renderers
    
    private string tagToIgnore = "NoMask"; // renderer tag to ignore

    [SerializeField]
    private int renderQueueOffset;

    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new();

    private Renderer[] renderers;

    private Material outlineMaterial;

    private bool needsUpdate;

    private void Awake() {
        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>(includeInactive).Where(r => r.CompareTag(tagToIgnore) == false).ToArray();

        // Instantiate outline materials
        outlineMaterial = Instantiate(Resources.Load<Material>(@"Materials/Outline"));

        outlineMaterial.name = "Outline (Instance)";

        // Retrieve or generate smooth normals
        LoadSmoothNormals();

        // Apply material properties immediately
        needsUpdate = true;
    }

    private void OnEnable() {
        foreach (var cachedRenderer in renderers) {
            // Append outline shaders
            var materials = cachedRenderer.sharedMaterials.ToList();

            materials.Add(outlineMaterial);

            // Assign materials
            cachedRenderer.materials = materials.ToArray();
        }
    }

    private void OnValidate() {
        // Update material properties
        needsUpdate = true;

        // Clear cache when baking is disabled or corrupted
        if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count) {
            bakeKeys.Clear();
            bakeValues.Clear();
        }

        // Generate smooth normals when baking is enabled
        if (precomputeOutline && bakeKeys.Count == 0)
            Bake();
    }

    private void Update() {
        if (needsUpdate) {
            needsUpdate = false;

            UpdateMaterialProperties();
        }
    }

    private void OnDisable() {
        foreach (var cachedRenderer in renderers) {
            // Remove outline shaders
            var materials = cachedRenderer.sharedMaterials.ToList();

            materials.Remove(outlineMaterial);

            // Assign materials
            cachedRenderer.materials = materials.ToArray();
        }
    }

    private void OnDestroy() {
        // Destroy material instance
        Destroy(outlineMaterial);
    }

    private void Bake() {
        // Generate smooth normals for each mesh
        var bakedMeshes = new HashSet<Mesh>();

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>()) {
            // Skip duplicates
            if (!bakedMeshes.Add(meshFilter.sharedMesh))
                continue;

            // Serialize smooth normals
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            bakeKeys.Add(meshFilter.sharedMesh);
            bakeValues.Add(
                new ListVector3() {
                    data = smoothNormals
                }
            );
        }
    }

    private void LoadSmoothNormals() {
        // Retrieve or generate smooth normals
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>()) {
            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh)) {
                continue;
            }

            // Retrieve or generate smooth normals
            int index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = index >= 0 ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Combine submeshes
            if (meshFilter.TryGetComponent<Renderer>(out var meshRenderer)) {
                CombineSubmeshes(meshFilter.sharedMesh, meshRenderer.sharedMaterials);
            }
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
            // Skip if UV3 has already been reset
            if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
                continue;

            // Clear UV3
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

            // Combine submeshes
            CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
        }
    }

    private List<Vector3> SmoothNormals(Mesh mesh) {
        if (mesh.isReadable == false) {
            Debug.LogWarning($"[Outline][SmoothNormals] Mesh not readable. ({name})", gameObject);
            return mesh.normals.ToList();
        }
        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups) {
            // Skip single vertices
            if (group.Count() == 1)
                continue;

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach ((_, int index) in group) {
                if (index >= 0 && index < smoothNormals.Count) {
                    smoothNormal += smoothNormals[index];
                } else {
                    Debug.LogWarning($"[SmoothNormals][Outline] Invalid normal index: {index}, count: {smoothNormals.Count} ({name})", gameObject);
                }
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach ((_, int index) in group) {
                if (index >= 0 && index < smoothNormals.Count) {
                    smoothNormals[index] = smoothNormal;
                }
            }
        }

        return smoothNormals;
    }

    private void CombineSubmeshes(Mesh mesh, Material[] materials) {
        // Skip meshes with a single submesh
        if (mesh.subMeshCount == 1) {
            return;
        }

        // Skip if submesh count exceeds material count
        if (mesh.subMeshCount > materials.Length) {
            return;
        }

        // Append combined submesh
        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        mesh.UploadMeshData(true);
    }
    
    private static readonly int OutlineColorShaderProperty = Shader.PropertyToID("_OutlineColor");
    
    private static readonly int OutlineWidthShaderProperty = Shader.PropertyToID("_OutlineWidth");

    private static readonly int ZTestMaskShaderProperty = Shader.PropertyToID("_ZTestMask");

    private static readonly int ZTestFillShaderProperty = Shader.PropertyToID("_ZTestFill");

    private void UpdateMaterialProperties() {
        // Apply properties according to mode
        outlineMaterial.renderQueue = 3100 + renderQueueOffset; // using 3100 (i.e. “Transparent+100”) queue to render over billboarded sprites
        outlineMaterial.SetColor(OutlineColorShaderProperty, outlineColor);
        outlineMaterial.SetFloat(OutlineWidthShaderProperty, outlineWidth);

        switch (outlineMode) {
            case Mode.OutlineAll:
                outlineMaterial.SetFloat(ZTestMaskShaderProperty, (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaterial.SetFloat(ZTestFillShaderProperty, (float)UnityEngine.Rendering.CompareFunction.Always);
                break;

            case Mode.OutlineVisible:
                outlineMaterial.SetFloat(ZTestMaskShaderProperty, (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaterial.SetFloat(ZTestFillShaderProperty, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                break;
            
            case Mode.OutlineVisibleAll:
                outlineMaterial.SetFloat(ZTestMaskShaderProperty, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineMaterial.SetFloat(ZTestFillShaderProperty, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                break;

            case Mode.OutlineHidden:
                outlineMaterial.SetFloat(ZTestMaskShaderProperty, (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaterial.SetFloat(ZTestFillShaderProperty, (float)UnityEngine.Rendering.CompareFunction.Greater);
                break;

            case Mode.OutlineAndSilhouette:
                outlineMaterial.SetFloat(ZTestMaskShaderProperty, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineMaterial.SetFloat(ZTestFillShaderProperty, (float)UnityEngine.Rendering.CompareFunction.Always);
                break;

            case Mode.SilhouetteOnly:
                outlineMaterial.SetFloat(OutlineWidthShaderProperty, 0f);
                outlineMaterial.SetFloat(ZTestMaskShaderProperty, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineMaterial.SetFloat(ZTestFillShaderProperty, (float)UnityEngine.Rendering.CompareFunction.Greater);
                break;
        }
    }
}
