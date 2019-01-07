using UnityEngine;

public class Example : MonoBehaviour
{
    // Draws a line from "startVertex" var to the curent mouse position.
    public Material mat;
    private Vector3 startVertex;
    private Vector3 mousePos;

    private void Start()
    {
        Debug.Log("Startup");
        startVertex = Vector3.zero;
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        // Press space to update startVertex
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Updated point");
            startVertex = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);
        }
    }

    private void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        Debug.Log("Drawing line");
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(startVertex);
        GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
        GL.End();

        GL.PopMatrix();
    }
}