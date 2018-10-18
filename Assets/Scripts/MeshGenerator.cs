using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MeshGenerator : MonoBehaviour {
    [SerializeField] private Object meshFile;
    [SerializeField] private string meshName = "GeneratedObject";
    [SerializeField] private GameObject quadToInstantiate;
    [SerializeField] private Vector3 scale = Vector3.one;
    [SerializeField] private GameObject parent = null;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material material;
    [SerializeField] private TextureData[] textureMapping;

    public void GenerateMesh() {
        MeshDataset dataset = new MeshDataset(AssetDatabase.GetAssetPath(meshFile));

        // Creates root object to hold all the instantiated quads
        GameObject rootObject = new GameObject();
        rootObject.name = meshName;
        
        if (parent != null) rootObject.transform.parent = parent.transform;

        for (int faceIdx = 0; faceIdx < dataset.numFaces; faceIdx++) {
            Vector4 face = dataset.faces[faceIdx];
            GameObject quad = Instantiate(quadToInstantiate, rootObject.transform, true);

            Vector3 normal = CalculateQuadNormal(dataset, quad, face);
            quad.transform.position = CalculateQuadPosition(dataset, quad, face);
            quad.transform.localRotation = CalculateQuadRotation(normal);
            ApplyMaterial(dataset, quad, face, normal);
        }

        rootObject.transform.localScale = scale;
    }

    private Vector3 CalculateQuadNormal(MeshDataset dataset, GameObject quad, Vector4 face) {
        List<Vector3> datasetVertices = new List<Vector3>();
        for (int vertIndex = 0; vertIndex < 4; vertIndex++) {
            int datasetVertIndex = Mathf.RoundToInt(face[vertIndex]);
            datasetVertices.Add(dataset.vertices[datasetVertIndex]);
        }

        return Vector3.Cross(datasetVertices[1] - datasetVertices[0], datasetVertices[2] - datasetVertices[1]);
    }

    private Vector3 CalculateQuadPosition(MeshDataset dataset, GameObject quad, Vector4 face) {
        Vector3 position = Vector3.zero;

        for (int vertIndex = 0; vertIndex < 4; vertIndex++) {
            int datasetVertIndex = Mathf.RoundToInt(face[vertIndex]);
            position += dataset.vertices[datasetVertIndex];
        }
        return position / 4.0f;
    }

    private Quaternion CalculateQuadRotation(Vector3 normal) {
        if (normal == Vector3.forward) {
            return Quaternion.Euler(0, 90, 90);
        }
        else if (normal == Vector3.back) {
            return Quaternion.Euler(0, -90, 90);
        }
        else if (normal == Vector3.left) {
            return Quaternion.Euler(0, 0, 90);
        }
        else if (normal == Vector3.right) {
            return Quaternion.Euler(0, 180, 90);
        }
        else if (normal == Vector3.down) {
            return Quaternion.Euler(0, 90, 180);
        }
        else {
            return Quaternion.Euler(0, -90, 0);
        }
    }

    private void ApplyMaterial(MeshDataset dataset, GameObject quad, Vector4 face, Vector3 normal) {
        Vector3 color = dataset.colors[Mathf.RoundToInt(face[0])];
        Renderer renderer = quad.GetComponentInChildren<Renderer>();
        Material mat = Instantiate(material);

        foreach (TextureData t in textureMapping) {
            if (t.ColorEqual(color)) {
                if (normal == Vector3.up) {
                    mat.mainTexture = t.topTexture;
                    break;
                }
                else if (normal == Vector3.down) {
                    mat.mainTexture = t.bottomTexture;
                    break;
                }
                else {
                    mat.mainTexture = t.sideTexture;
                    break;
                }
            }
        }

#if UNITY_EDITOR
        renderer.sharedMaterial = mat;
#else
        renderer.material = mat;
#endif
    }

    class MeshDataset {
        public int numVertices { get; private set; }
        public int numFaces { get; private set; }

        public List<Vector3> vertices { get; private set; }
        public List<Vector3> colors { get; private set; }
        public List<Vector4> faces { get; private set; }

        private StreamReader reader;

        public MeshDataset(string filePath) {
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

            InitVerticesAndColors();
            InitFaces();
        }

        private void InitVerticesAndColors() {
            vertices = new List<Vector3>();
            colors = new List<Vector3>();
            for (int i = 0; i < numVertices; i++) {
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
        private void InitFaces() {
            faces = new List<Vector4>();
            for (int i = 0; i < numFaces; i++) {
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

    [System.Serializable]
    public class TextureData {
        public Color color;
        public Texture2D sideTexture;
        public Texture2D topTexture;
        public Texture2D bottomTexture;

        public bool ColorEqual(Vector3 c) {

            return Mathf.RoundToInt(color.r * 255) == c.x && Mathf.RoundToInt(color.g * 255) == c.y && Mathf.RoundToInt(color.b * 255) == c.z;
        }
    }
}