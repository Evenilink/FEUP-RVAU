using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MeshGenerator : MonoBehaviour {
    public string MeshName;
	public Object MeshFile;
    public GameObject Parent;
    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateMesh() {
        MeshDataset dataset = new MeshDataset(AssetDatabase.GetAssetPath(MeshFile));

        GameObject scene = new GameObject();
        scene.name = MeshName;
        if (Parent != null) scene.transform.parent = Parent.transform;
        
        for (int faceIdx = 0; faceIdx < dataset.numFaces; faceIdx++) {
            // Creates quad
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

            Mesh mesh;
            // Retreives quad mesh
#if UNITY_EDITOR
            MeshFilter meshFilter = quad.GetComponent<MeshFilter>();
            Mesh meshCopy = Instantiate(meshFilter.sharedMesh) as Mesh;
            mesh = meshFilter.sharedMesh = meshCopy;
#else
            mesh = quad.GetComponent<MeshFilter>().mesh;
#endif

            Vector3[] vertices = mesh.vertices;

            // Gets ply defined vertices and assigns them to the quad
            Vector4 face = dataset.faces[faceIdx];
            for (int vertIndex = 0; vertIndex < vertices.Length; vertIndex++) {
                int plyVertIndex = Mathf.RoundToInt(face[vertIndex]);
                vertices[vertIndex] = dataset.vertices[plyVertIndex];
            }

            // Reaclculates mesh with new vertices
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            // Add to parent object
            quad.transform.parent = scene.transform;
        }
    }
}

class MeshDataset {
    public int numVertices { get; private set; }
    public int numFaces { get; private set; }

    public List<Vector3> vertices { get; private set; }
    public List<Vector3> colors { get; private set; }
    public List<Vector4> faces { get; private set; }

    private StreamReader reader;

    public MeshDataset(string filePath)
    {
        reader = new StreamReader(filePath);

        reader.ReadLine(); // File format
        reader.ReadLine(); // File format

        // Ignore comments
        string line;
        while ((line = reader.ReadLine()).StartsWith("comment")) continue;

		// Parse number of vertices
        line = line.Substring(line.LastIndexOf(' ') + 1);
        numVertices = int.Parse(line);
		// Ignore vertices properties definition
        for (int i = 0; i < 6; i++) reader.ReadLine();

		// Parse number of faces
        line = reader.ReadLine();
        line = line.Substring(line.LastIndexOf(' ') + 1);
        numFaces = int.Parse(line);
        
		reader.ReadLine(); // Ignore face property definition
        reader.ReadLine(); // Ignore end_header tag

        initVerticesAndColors();
        initFaces();
    }

    private void initVerticesAndColors()
    {
        vertices = new List<Vector3>();
        colors = new List<Vector3>();
        for (int i = 0; i < numVertices; i++)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(' ');
            
            float x = float.Parse(values[0]);
            float y = float.Parse(values[2]); // Switch for Y up
            float z = float.Parse(values[1]);
            int r = int.Parse(values[3]);
            int g = int.Parse(values[4]);
            int b = int.Parse(values[5]);
            Vector3 vertex = new Vector3(x, y, z);
            Vector3 color = new Vector3(r, g, b);
            vertices.Add(vertex);
            colors.Add(color);
        }
    }
    private void initFaces()
    {
        faces = new List<Vector4>();
        for (int i = 0; i < numFaces; i++)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(' ');
            int x = int.Parse(values[3]);
            int y = int.Parse(values[1]);
            int z = int.Parse(values[4]);
            int w = int.Parse(values[2]);
            Vector4 face = new Vector4(x, y, z, w);
            faces.Add(face);
        }
    }
}