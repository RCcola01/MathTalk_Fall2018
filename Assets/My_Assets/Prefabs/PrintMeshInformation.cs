using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintMeshInformation : MonoBehaviour
{
    //material: colored lines
    //shader: particles/standard surface
    //mush have a mesh collider in order to detect triangle index collisions

    //issue: all objects appear to share the same mesh/material: coloring the vertex of one shape will color the same vertex of another
    //issue: certain builtin unity meshes have more detailed meshes (spheres a lot vs cylinders a little) so the coloring looks more accurate for them. Make custom more detailed meshes for all shapes
    //issue: blended vs flat shading? more detailed meshes therefore make it harder to have the painting color completely pure, it is always blurred/mixed with the original color
    //issue: make the highlight for selected objects indicated by a way that doesnt affect mesh color: maybe some kind of outline or particle glow


    public Camera testCam;
    public Color paintingColor; //color the user is currently painting with
    // Start is called before the first frame update
    void Start()
    {

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        print(mesh.vertices.ToString());
        print(mesh.vertexCount);
        print(mesh.vertices[1].ToString());

        Color[] originalMeshColors = new Color[mesh.vertices.Length];
        for (int i = 0; i < originalMeshColors.Length; i += 1)
        {
            //originalMeshColors[i] = Color.red;
        }
        GetComponent<MeshFilter>().mesh.colors = originalMeshColors;
        Vector3[] originalNormals = GetComponent<MeshFilter>().mesh.normals;
        Vector3[] invertedNormals = new Vector3[originalNormals.Length];
        for (int i = 0; i < originalNormals.Length; i += 1)
        {
            invertedNormals[i] = -1 * originalNormals[i];
        }
        GetComponent<MeshFilter>().mesh.normals = invertedNormals;

    }

    // Update is called once per frame
    void Update()
    {
        print(GetComponent<MeshFilter>().mesh.triangles.Length.ToString());
        RaycastHit hit;
        if (Physics.Raycast(testCam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID()) //only paint the mesh of the collider that was hit
            {
                //must add condition here to check that the collided mesh belongs to THIS object (or else every object is painted)
                int triangleIndex = hit.triangleIndex;
                print(triangleIndex);
                MeshFilter meshFilter = GetComponent<MeshFilter>();
                Mesh mesh = meshFilter.mesh;
                //print("Colors: " + mesh.colors[0].ToString());
                // getting the vertices of the triangle; mesh.triangles contains the indices to the vertices

                /*
                int closestVertexIndex = -1;
                float closestDistance = float.MaxValue;
                for (int i = 0; i < mesh.vertices.Length; i++){
                    Vector3 translatedMeshVertex = new Vector3(mesh.vertices[i].x + transform.position.x, mesh.vertices[i].y + transform.position.y, mesh.vertices[i].z + transform.position.z);
                    if (Vector3.Distance(hit.point, translatedMeshVertex) < closestDistance){
                        closestVertexIndex = i;
                        closestDistance = Vector3.Distance(hit.point, translatedMeshVertex);
                        //print(mesh.vertices[i]);
                    }
                }
                */

                List<int> paintedVertexIndicies = new List<int>();

                /*
                int brushSize = 1; //number of vertices that will be painted
                float lowerDistanceBound = 0f; //distance to vertex must be larger than this (used to find the 2nd closest, 3rd closest, etc)
                float currentClosestDistance = float.MaxValue;
                for (int numPaintedVertices = 0; numPaintedVertices < brushSize; numPaintedVertices++)
                {
                    int paintedIndex = -1; //the index of the vertex we choose to paint for this portion of the brush size
                    for (int i = 0; i < mesh.vertices.Length; i++)
                    {
                        //Vector3 translatedMeshVertex = new Vector3((mesh.vertices[i].x * transform.localScale.x) + transform.position.x, (mesh.vertices[i].y * transform.localScale.y) + transform.position.y, (mesh.vertices[i].z * transform.localScale.z) + transform.position.z);
                        Vector3 translatedMeshVertex = transform.TransformPoint(mesh.vertices[i]);
                        if (Vector3.Distance(hit.point, translatedMeshVertex) < currentClosestDistance && Vector3.Distance(hit.point, translatedMeshVertex) > lowerDistanceBound)
                        {
                            paintedIndex = i;
                            currentClosestDistance = Vector3.Distance(hit.point, translatedMeshVertex);
                        }
                    }
                    paintedVertexIndicies.Add(paintedIndex);
                    float colorRadius = 0.05f * transform.TransformVector(new Vector3(1f, 0f, 0f)).magnitude; //color radius scales as the mesh expands/shrinks
                    for (int i = 0; i < mesh.vertices.Length; i++){
                        Vector3 translatedMeshVertex = transform.TransformPoint(mesh.vertices[i]);
                        if(Vector3.Distance(translatedMeshVertex, mesh.vertices[paintedIndex]) < colorRadius){
                            paintedVertexIndicies.Add(i); //paint all vertices within the color radius of the closest vertex to the tap
                        }
                    }
                    //Vector3 translatedClosestVertex = new Vector3((mesh.vertices[paintedIndex].x * transform.localScale.x) + transform.position.x, (mesh.vertices[paintedIndex].y * transform.localScale.y) + transform.position.y, (mesh.vertices[paintedIndex].z * transform.localScale.z) + transform.position.z);
                    Vector3 translatedClosestVertex = transform.TransformPoint(mesh.vertices[paintedIndex]);
                    lowerDistanceBound = Vector3.Distance(hit.point, translatedClosestVertex);
                    currentClosestDistance = float.MaxValue;
                }
                //print(closestVertexIndex);
                //print(hit.point);
                */
                float colorRadius = 0.05f * transform.TransformVector(new Vector3(1f, 0f, 0f)).magnitude * 4f; //color radius scales as the mesh expands/shrinks
                print("color Radius " + colorRadius.ToString());
                for (int i = 0; i < mesh.vertices.Length; i++)
                {
                    Vector3 translatedMeshVertex = transform.TransformPoint(mesh.vertices[i]);
                    if (Vector3.Distance(translatedMeshVertex, hit.point) < colorRadius)
                    {
                        paintedVertexIndicies.Add(i); //paint all vertices within the color radius of the closest vertex to the tap
                    }
                }

                /*
                Vector3 a = mesh.vertices[mesh.triangles[3 * triangleIndex + 0]]; // +0 is not needed, I just want to make it clear
                Vector3 b = mesh.vertices[mesh.triangles[3 * triangleIndex + 1]];
                Vector3 c = mesh.vertices[mesh.triangles[3 * triangleIndex + 2]];
                Vector3 localA = hit.transform.TransformPoint(a);
                Vector3 localB = hit.transform.TransformPoint(b);
                Vector3 localC = hit.transform.TransformPoint(c);
                float distanceToA = Vector3.Distance(localA, hit.point);
                float distanceToB = Vector3.Distance(localB, hit.point);
                float distanceToC = Vector3.Distance(localC, hit.point);
                // sorry for the confusing ternary operator chain, but I don't have much time
                int closestVertexIndex = -1;
                if (distanceToA < distanceToB && distanceToA < distanceToC)
                {
                    closestVertexIndex = mesh.triangles[3 * triangleIndex];
                }
                else if (distanceToB < distanceToA && distanceToB < distanceToC)
                {
                    closestVertexIndex = mesh.triangles[3 * triangleIndex + 1];
                }
                else
                {
                    closestVertexIndex = mesh.triangles[3 * triangleIndex + 2];
                }
                //change the color of he closest vertex
                /*
                Color[] newMeshColors = new Color[mesh.vertices.Length];
                for (int i = 0; i < newMeshColors.Length; i+=1){
                    if (i == closestVertexIndex)
                    {
                        newMeshColors[i] = Color.red;
                    }
                    else
                    {
                        newMeshColors[i] = Color.blue;
                    }
                }
                GetComponent<MeshFilter>().mesh.colors = newMeshColors;
                */
                Color[] oldColors = GetComponent<MeshFilter>().mesh.colors;
                Color[] newColors = oldColors;
                foreach (int index in paintedVertexIndicies)
                {
                    newColors[index] = paintingColor;
                }
                GetComponent<MeshFilter>().mesh.colors = newColors;
                //Vector3 closestVertexToMouseCursor = distanceToA < distanceToB ? distanceToA < distanceToC ? a : c : distanceToB < distanceToC ? b : c;
                //print(closestVertexToMouseCursor.ToString());
            }
        }
    }

}


