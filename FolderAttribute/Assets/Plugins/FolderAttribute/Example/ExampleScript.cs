using UnityEngine;
using Folder;

public class ExampleScript : MonoBehaviour
{

    [Folder]
    public string materialsFolder; // A variable with [Folder] must be a string.


    void Start()
    {
        if (string.IsNullOrEmpty(materialsFolder))
        {
            Debug.LogError("Assign a materials folder to start.");
            return;
        }

        Material[] materials = materialsFolder.LoadFolder<Material>(); // Get the content of the folder!

        if (materials != null && materials.Length > 0)
        {
            // Create a cube in front of the camera
            GameObject exampleCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            exampleCube.name = "ExampleCube";
            exampleCube.transform.position = Vector3.back * 7;

            // Assign to the cube a random material in the given folder of materials
            exampleCube.GetComponent<Renderer>().material = materials.GetRandomElement();
        }
        else
        {
            Debug.LogError("No materials found. ");
        }
    }
}