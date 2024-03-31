using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnClickAnotherCube : MonoBehaviour
{
    public int cubePerAxis = 4;
    public float force = 300f;
    public float radius = 100f;
    public float delayBeforeDestroy = 1f; // Delay before destroying all cubes
    public float delayBeforeClearing = 1f; // Delay before clearing all objects list

    private List<GameObject> allObjects = new List<GameObject>(); // List to store all created objects
    private Renderer mainCubeRenderer;

    void Start()
    {
        mainCubeRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Check for right mouse button click
        {
            StartCoroutine(ExplodeSequence());
        }
    }

    IEnumerator ExplodeSequence()
    {
        // Disable the main cube renderer
        mainCubeRenderer.enabled = false;

        ExplodeCubes();
        yield return new WaitForSeconds(delayBeforeDestroy);
        DestroyAllObjects();
        yield return new WaitForSeconds(delayBeforeClearing);
        ClearAllObjects();
    }

    void ExplodeCubes()
    {
        for (int x = 0; x < cubePerAxis; x++)
        {
            for (int y = 0; y < cubePerAxis; y++)
            {
                for (int z = 0; z < cubePerAxis; z++)
                {
                    GameObject newCube = CreateCube(new Vector3(x, y, z));
                    allObjects.Add(newCube); // Add the new cube to the list
                }
            }
        }
    }

    GameObject CreateCube(Vector3 coordinates)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = GetComponent<Renderer>().material;

        cube.transform.localScale = transform.localScale / cubePerAxis;

        Vector3 firstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
        cube.transform.position = firstCube + Vector3.Scale(coordinates, cube.transform.localScale);

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.mass = 0.1f; // Setting mass to avoid unrealistic behavior
        rb.AddExplosionForce(force, transform.position, radius);

        return cube;
    }

    void DestroyAllObjects()
    {
        // Destroy all objects in the list
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        // Clear the list after destroying all objects
        allObjects.Clear();

        // Destroy the main cube GameObject
        Destroy(gameObject);
    }

    void ClearAllObjects()
    {
        // Clear the list of all objects
        allObjects.Clear();
    }
}
