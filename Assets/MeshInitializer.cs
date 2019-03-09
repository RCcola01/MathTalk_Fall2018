using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LylekGames;

public class MeshInitializer : MonoBehaviour
{

    public Color initialColor; //defined in editor
    public enum MeshType { Sphere, Cube, Cylinder }; //the types of meshes currently available to create
    public MeshType meshType; //defined in editor (for sphere object, set to "Sphere", etc.)

    // Start is called before the first frame update
    void Start()
    {

        if(meshType == MeshType.Cylinder || meshType == MeshType.Cube){
            GetComponent<CombineMeshes>().EnableMesh();
        }
        PaintVertexColors(initialColor); //initialize the color of the object

    }

    //paints all vertex colors of the shape's mesh to be the same color
    public void PaintVertexColors(Color meshColor){
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] originalMeshColors = new Color[mesh.vertices.Length];
        for (int i = 0; i < originalMeshColors.Length; i += 1)
        {
            originalMeshColors[i] = meshColor;
        }
        GetComponent<MeshFilter>().mesh.colors = originalMeshColors;
    }

    //maintain the current colors of each mesh vertex but give them 60% transparency
    public void TurnTransparent(){
        /*
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] originalMeshColors = mesh.colors;
        print(mesh.colors.Length);
        for (int i = 0; i < originalMeshColors.Length; i += 1)
        {
            Color highlight = originalMeshColors[i];
            highlight.r = Mathf.Clamp(highlight.r + 0.2f, 0f, 1f);
            highlight.g = Mathf.Clamp(highlight.g - 0.1f, 0f, 1f);
            highlight.b = Mathf.Clamp(highlight.b - 0.1f, 0f, 1f);
            originalMeshColors[i] = highlight;
        }
        GetComponent<MeshFilter>().mesh.colors = originalMeshColors;*/
        /*
        Color transparent = GetComponent<Renderer>().material.color;
        transparent.a = 0.5f;
        Material newMaterial = new Material(GetComponent<Renderer>().material);
        newMaterial.color = transparent;
        GetComponent<Renderer>().material = newMaterial;
        */
    }

    //maintain the current colors of each mesh vertex but give them 100% opacity
    public void TurnOpaque()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] originalMeshColors = mesh.colors;
        for (int i = 0; i < originalMeshColors.Length; i += 1)
        {
            Color opaque = originalMeshColors[i];
            opaque.a = 1.0f;
            originalMeshColors[i] = opaque;
        }
        GetComponent<MeshFilter>().mesh.colors = originalMeshColors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
