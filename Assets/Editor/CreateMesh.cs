//Create a folder (right click in the Assets directory, click Create>New Folder) and name it “Editor” if one doesn’t exist already.
//Place this script in that folder

//This script creates a new menu and a new menu item in the Editor window
// Use the new menu item to create a prefab at the given path. If a prefab already exists it asks if you want to replace it
//Click on a GameObject in your Hierarchy, then go to Examples>Create Prefab to see it in action.

using UnityEngine;
using UnityEditor;

public class CreateMesh : EditorWindow {
    private static MeshGenerator meshGeneratorInstance = null;
    private static string pathToSave = "Assets/Prefabs/WorldStructures/";
    private static Vector3 scale = new Vector3(2.1f, 2.1f, 2.1f);

    [MenuItem("Mesh Generator/Create Prefab")]
    static void CreatePrefab() {
        MeshGenerator meshGenerator = GetMeshGenerator();

        //Keep track of the currently selected GameObject(s)

        foreach (string guid in Selection.assetGUIDs) {
            // Assets/Maps/ExportedDatasets/Start.ply
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string name = path.Substring(path.LastIndexOf("/") + 1);
            name = name.Substring(0, name.Length - 4);

            GameObject mesh = meshGenerator.GenerateMesh(path, name, scale);
            string nameToSave = pathToSave + mesh.name + ".prefab";

            //Check if the Prefab and/or name already exists at the path
            if (AssetDatabase.LoadAssetAtPath(nameToSave, typeof(GameObject))) {
                //Create dialog to ask if User is sure they want to overwrite existing prefab
                if (EditorUtility.DisplayDialog("Are you sure?",
                        "The prefab " + name + " already exists. Do you want to overwrite it?",
                        "Yes",
                        "No"))
                //If the user presses the yes button, create the Prefab
                {
                    CreateNew(mesh, nameToSave);
                }
            }
            //If the name doesn't exist, create the new Prefab
            else {
                Debug.Log(mesh.name + " is not a prefab, will convert");
                CreateNew(mesh, nameToSave);
            }

            DestroyImmediate(mesh);
        }
    }

    // Disable the menu item if no selection is in place
    [MenuItem("Mesh Generator/Create Prefab", true)]
    static bool ValidateCreatePrefab() { 
        foreach (string guid in Selection.assetGUIDs) {
            if (AssetDatabase.GUIDToAssetPath(guid).EndsWith(".ply")) return true;
        }
        return false;
    }

    static void CreateNew(GameObject obj, string localPath) {
        //Create a new prefab at the path given
        Object prefab = PrefabUtility.CreatePrefab(localPath, obj);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }

    static MeshGenerator GetMeshGenerator() {
        if (meshGeneratorInstance == null) {
            // Gets the quad to instantiate
            GameObject quad = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/QuadPrefab.prefab");

            // Gets the material pallete
            Texture2D palette = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Maps/MagicaVoxel Projects/Palette.png");

            // Loads the materials
            string[] guids = AssetDatabase.FindAssets("M_I  t:material", new[] { "Assets/Materials" });
            Material[] mats = new Material[guids.Length];

            foreach (string guid in guids) {

                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                string name = mat.name;
                string id = name.Substring(3);
                id = id.Substring(0, id.IndexOf("_"));
                int index = int.Parse(id) - 1;

                mats[index] = mat;
            }

            meshGeneratorInstance = new MeshGenerator(quad, palette, mats);
        }
        return meshGeneratorInstance;
    }
}