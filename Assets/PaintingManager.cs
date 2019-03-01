using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingManager : MonoBehaviour
{

    //material: colored lines
    //shader: particles/standard surface

    //issue: certain builtin unity meshes have more detailed meshes (spheres a lot vs cylinders a little) so the coloring looks more accurate for them. Make custom more detailed meshes for all shapes
    //issue: blended vs flat shading? more detailed meshes therefore make it harder to have the painting color completely pure, it is always blurred/mixed with the original color
    //issue: make the highlight for selected objects indicated by a way that doesnt affect mesh color: maybe some kind of outline or particle glow
    //issue: make the gray mesh of magnetic objects changed in a way that doesn't affect painting
    //when we have a more detailed mesh, make brush size independent of object size. This will allow the user to expand the object to make more detailed drawings

    //right now dragging is disabled by one finger touches when in paint mode (Drag object script)
    //object creation is disabled when in paint mode by single touches as well
    //also, activate and deactivate highlight functions in Select Manager are temporarily disabled when in paint mode
    //this is because they overwrite the material each time; need to find a way to handle highlighting
    //that does not change material, shader, color, etc
    //at the beginning when select tracker sets overall color to mesh, this makes paint colors off (yellow for sphere leads to blue paint looking green because it blends)
    //so take out any parts where I assign color as a whole to the material , for initial color just keep the vertex color setting inStart() of mesh script
    //this is also apparent in highlighting color (red highlight on blue vertex color cube now looks black, highlight on yellow sphere looks orange, etc)
    //confirm that scaling works
    //material selection stopped working??

    public static bool InPaintMode; //indicates whether one finger touches are used for painting or for object movement

    public Camera testCam;
    public Color paintingColor; //color the user is currently painting with
    public float brushSize; //where 1 is the normal size

    public Text mode;

    // Start is called before the first frame update
    void Start()
    {
        InPaintMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1 && InPaintMode){
            HandlePainting();
        }
    }

    void HandlePainting(){
        RaycastHit hit;
        if (Physics.Raycast(testCam.ScreenPointToRay(Input.touches[0].position), out hit) && InPaintMode)
        {
            if (hit.collider.gameObject.GetComponent<MeshFilter>().mesh.colors.Length > 0) //only paint objects that are able to accept vertex colors
            {
                GameObject paintedObject = hit.transform.gameObject; //the game object currently being painted
                print(paintedObject.transform.position);
                MeshFilter meshFilter = paintedObject.GetComponent<MeshFilter>();
                Mesh mesh = meshFilter.mesh;


                List<int> paintedVertexIndicies = new List<int>(); //the indices of the vertices that will be painted

                float colorRadius = 0.05f * paintedObject.transform.TransformVector(new Vector3(1f, 0f, 0f)).magnitude * brushSize; //color radius scales as the mesh expands/shrinks
                print("color Radius " + colorRadius.ToString());
                for (int i = 0; i < mesh.vertices.Length; i++)
                {
                    Vector3 translatedMeshVertex = paintedObject.transform.TransformPoint(mesh.vertices[i]);
                    if (Vector3.Distance(translatedMeshVertex, hit.point) < colorRadius)
                    {
                        paintedVertexIndicies.Add(i); //paint all vertices within the color radius of the closest vertex to the tap
                    }
                }

                Color[] oldColors = paintedObject.GetComponent<MeshFilter>().mesh.colors;
                Color[] newColors = oldColors;
                foreach (int index in paintedVertexIndicies)
                {
                    newColors[index] = paintingColor;
                }
                paintedObject.GetComponent<MeshFilter>().mesh.colors = newColors;
            }
        }
    }

    public void EnterPaintMode(){
        InPaintMode = true;
        mode.text = "Paint Mode";
    }

    public void LeavePaintMode(){
        InPaintMode = false;
        mode.text = "Build Mode";
    }

    public void ChangeBrushSize(float size){
        brushSize = size;
    }

    public void Purple(){
        paintingColor = new Color(0.3f, 0.1f, 0.7f);
    }

    public void Pink(){
        paintingColor = new Color(1f, 0.5f, 1f);
    }

    public void Orange()
    {
        paintingColor = new Color(1f, 0.6f, 0f);
    }

    public void Red()
    {
        paintingColor = new Color(0.7f, 0.1f, 0f);
    }

    public void Green()
    {
        paintingColor = new Color(0f, 1f, 0f);
    }

    public void Turquoise()
    {
        paintingColor = new Color(0.4f, 0.9f, 0.8f);
    }

    public void Blue(){
        paintingColor = new Color(0.4f, 0.6f, 1f);
    }

    public void Yellow(){
        paintingColor = new Color(0.9f, 0.9f, 0.1f);
    }

    public void HotPink(){
        paintingColor = new Color(1f, 0f, 0.6f);
    }

}
