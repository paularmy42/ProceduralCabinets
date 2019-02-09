using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralObjectConstructor : MonoBehaviour
{
    private float worldLength;
    private float worldDepth;
    private float worldHeight;
    private float worldThickness;

    // Start is called before the first frame update
    void Start()
    {
        PlacementManager manager = Object.FindObjectOfType<PlacementManager>();
        
        worldLength = manager.length / 1000f;
        worldDepth = manager.depth / 1000f;
        worldHeight = manager.height / 1000f;
        worldThickness = manager.thickness / 1000f;

        GameObject leftEnd = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftEnd.tag = "Carcase";
        leftEnd.transform.parent = this.transform;
        leftEnd.transform.position = new Vector3(worldThickness / 2, worldHeight / 2, 0);
        leftEnd.transform.localScale = new Vector3(worldThickness, worldHeight, worldDepth);

        GameObject rightEnd = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightEnd.tag = "Carcase";
        rightEnd.transform.parent = this.transform;
        rightEnd.transform.position = new Vector3(worldLength - worldThickness / 2, worldHeight / 2, 0);
        rightEnd.transform.localScale = new Vector3(worldThickness, worldHeight, worldDepth);

        GameObject bottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottom.tag = "Carcase";
        bottom.transform.parent = this.transform;
        bottom.transform.position = new Vector3(worldLength / 2, worldThickness / 2, -worldThickness / 2);
        bottom.transform.localScale = new Vector3(worldLength - (worldThickness * 2), worldThickness, worldDepth - worldThickness);

        GameObject back = GameObject.CreatePrimitive(PrimitiveType.Cube);
        back.tag = "Carcase";
        back.transform.parent = this.transform;
        back.transform.position = new Vector3(worldLength / 2, worldHeight / 2, worldDepth / 2 - worldThickness / 2);
        back.transform.localScale = new Vector3(worldLength - worldThickness * 2, worldHeight, worldThickness);

        GameObject rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rail.tag = "Carcase";
        rail.transform.parent = this.transform;
        rail.transform.position = new Vector3(worldLength / 2, worldHeight - worldThickness / 2, -worldDepth / 2 + 0.130f / 2);
        rail.transform.localScale = new Vector3(worldLength - worldThickness * 2, worldThickness, .13f);

        CombineMesh();

        GameObject doorLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
        doorLeft.tag = "Front";
        doorLeft.layer = 2;
        doorLeft.name = "DoorLeft";
        doorLeft.GetComponent<MeshRenderer>().material = manager.doorMat;
        doorLeft.transform.parent = this.transform;
        doorLeft.transform.localPosition = new Vector3(worldLength / 4 + 0.001f, worldHeight / 2, -worldDepth / 2 - worldThickness / 2 - 0.002f);
        doorLeft.transform.localScale = new Vector3(worldLength / 2 - 0.003f, worldHeight - 0.002f, worldThickness);

        GameObject doorRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        doorRight.tag = "Front";
        doorRight.layer = 2;
        doorRight.name = "DoorRight";
        doorRight.GetComponent<MeshRenderer>().material = manager.doorMat;
        doorRight.transform.parent = this.transform;
        doorRight.transform.localPosition = new Vector3(worldLength - (worldLength / 4 + 0.001f), worldHeight / 2, -worldDepth / 2 - worldThickness / 2 - 0.002f);
        doorRight.transform.localScale = new Vector3(worldLength / 2 - 0.003f, worldHeight - 0.002f, worldThickness);
    }

    void CombineMesh()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        MeshUtility.Optimize(transform.GetComponent<MeshFilter>().mesh);
        this.gameObject.AddComponent<MeshCollider>();
        //transform.gameObject.layer = 0;
        transform.gameObject.SetActive(true);
        //Find all children with tag "Carcase" and Destroy
        for (int j = 0; j < transform.childCount; j++)
        {
            Transform child = transform.GetChild(j);
            if (child.tag == "Carcase")
            {
                Destroy(child.gameObject);
            }
        }
    }
}
