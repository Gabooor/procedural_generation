using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPointDisplacementBlocks : MonoBehaviour
{
    int numberOfPoints = 513;
    int[] terrain;
    float roughness = 15f;
    int displacement = 10;
    int initRange = 50;

    public GameObject cell;
    public GameObject[] surface;
    
    void Start()
    {
        terrain = new int[numberOfPoints];
        surface = new GameObject[numberOfPoints];
        GenerateSurface();
        int min = 9999;
        for(int i = 0; i < numberOfPoints; i++){
            if(terrain[i] < min){
                min = terrain[i];
            }
        }
        Debug.Log("Lowest block: " + min);
        GenerateGround(min);
    }

    void GenerateSurface(){
        terrain[0] = Random.Range(-initRange-30, initRange-30);
        terrain[numberOfPoints-1] = Random.Range(-initRange, initRange);
        
        Displace(0, numberOfPoints-1, roughness);

        for(int i = 0; i < numberOfPoints; i++){
            surface[i] = Instantiate(cell);
            surface[i].transform.position = new Vector2(i, terrain[i]);
            surface[i].GetComponent<SpriteRenderer>().color = new Color32(30,220,30,255);; // FŰ
        }

    }

    void Displace(int start, int end, float roughness)
    {
        if (end - start < 2)
            return;

        int mid = (start + end) / 2;

        int disp = (int) (Random.Range(-displacement, displacement) * roughness);
        terrain[mid] = (int) ((terrain[start] + terrain[end]) / 2 + disp);

        Displace(start, mid, roughness / 2);
        Displace(mid, end, roughness / 2);
    }

    void GenerateGround(int min){
        for(int i = 0; i < numberOfPoints; i++){
            for(int j = 1; j < 5; j++){
                GameObject ground_block = Instantiate(cell);
                ground_block.transform.position = new Vector2(i, terrain[i]-j);
                ground_block.GetComponent<SpriteRenderer>().color = new Color32(158,108,72,255); // FÖLD
            }
        }

        for(int i = 0; i < numberOfPoints; i++){
            for(int j = terrain[i]; j > min -8; j--){
                    GameObject ground_block = Instantiate(cell);
                    ground_block.transform.position = new Vector2(i, j-5);
                    ground_block.GetComponent<SpriteRenderer>().color = new Color32(140,140,140,255); // KŐ
            }
        }
    }
}
