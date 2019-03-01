using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInitializer : MonoBehaviour
{

    public Color initialColor; //defined in editor
    public enum MeshType { Sphere, Cube, Cylinder }; //the types of meshes currently available to create
    public MeshType meshType; //defined in editor (for sphere object, set to "Sphere", etc.)

    // Start is called before the first frame update
    void Start()
    {
        InitializeVertexColors(initialColor);
    }

    //initializes vertex colors when the user creates a shape so that painting can operate on the vertex colors
    void InitializeVertexColors(Color meshColor){
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] originalMeshColors = new Color[mesh.vertices.Length];
        for (int i = 0; i < originalMeshColors.Length; i += 1)
        {
            originalMeshColors[i] = meshColor;
        }
        GetComponent<MeshFilter>().mesh.colors = originalMeshColors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
