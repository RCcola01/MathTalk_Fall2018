using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPainter : MonoBehaviour
{

    //material: colored lines
    //shader: particles/standard surface
    //mush have a mesh collider in order to detect triangle index collisions

    //issue: all objects appear to share the same mesh/material: coloring the vertex of one shape will color the same vertex of another
    //issue: certain builtin unity meshes have more detailed meshes (spheres a lot vs cylinders a little) so the coloring looks more accurate for them. Make custom more detailed meshes for all shapes
    //issue: blended vs flat shading? more detailed meshes therefore make it harder to have the painting color completely pure, it is always blurred/mixed with the original color
    //issue: make the highlight for selected objects indicated by a way that doesnt affect mesh color: maybe some kind of outline or particle glow
    //issue: cannot add a mesh collider to an object with a rigidbody. Solution: in paint mode: take away all object's
    //rigidbodies and individual non-mesh colliders. Then add a mesh collider to each one. This effectively freezes
    //all objects in place during paint mode, but still allows for transform manipulation (rotation/scaling).
    //when exiting paint mode, reverse this process, which will cause all objects to fall into place

    public Camera testCam;
    public Color paintingColor; //color the user is currently painting with

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(testCam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            int triangleIndex = hit.triangleIndex;
            print(triangleIndex);
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.mesh;
            //print("Colors: " + mesh.colors[0].ToString());
            // getting the vertices of the triangle; mesh.triangles contains the indices to the vertices
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
            print(oldColors.Length);
            Color[] newColors = oldColors;
            newColors[closestVertexIndex] = paintingColor;
            GetComponent<MeshFilter>().mesh.colors = newColors;
            Vector3 closestVertexToMouseCursor = distanceToA < distanceToB ? distanceToA < distanceToC ? a : c : distanceToB < distanceToC ? b : c;
            print(closestVertexToMouseCursor.ToString());
        } 
    }
}
