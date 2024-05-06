using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPointDisplacement : MonoBehaviour
{
    public LineRenderer lineRenderer;
    int numberOfPoints = 257;
    float spacing = 10f;
    float[] terrain;
    float roughness = 5.0f;
    float displacement = 50f;
    
    void Start()
    {
        terrain = new float[numberOfPoints];
        lineRenderer.positionCount = numberOfPoints;
        GenerateTerrain();
    }

    void GenerateTerrain(){
        terrain[0] = Random.Range(-50, 50);
        terrain[numberOfPoints-1] = Random.Range(-30, 30);
        
        Displace(0, numberOfPoints-1, roughness);

        for (int i = 0; i < numberOfPoints; i++){
            lineRenderer.SetPosition(i, new Vector3(i * spacing-(numberOfPoints*5), terrain[i], 0));
        }
    }

    void Displace(int start, int end, float roughness)
    {
        if (end - start < 2)
            return;

        int mid = (start + end) / 2;

        float disp = (Random.Range(-displacement, displacement) * roughness);
        terrain[mid] = (terrain[start] + terrain[end]) / 2 + disp;
        Debug.Log(terrain[mid]);

        Displace(start, mid, roughness / 2);
        Displace(mid, end, roughness / 2);
    }
}
