using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    public int[,] map;
    public int[,] maptmp;
    public GameObject[,] mapGO;

    public GameObject cell;
    public Camera cam;

    public Color32 seaColor;
    public Color32 landColor;
    public int size;

    void Start(){
        Debug.Log("<b><size=25>Space: Egy iteráció lejátszása</size></b>");
        map = new int[size,size];
        maptmp = new int[size,size];
        mapGO = new GameObject[size,size];
        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                int rand = Random.Range(0,2);
                if(rand == 0) map[i,j] = 1;
                else map[i,j] = 0;
                maptmp[i,j] = 0;
                mapGO[i,j] = Instantiate(cell);
                mapGO[i,j].transform.position = new Vector2(i-(size/2),(size/2)-j);
            }
        }
        cam.orthographicSize = size/2+2;
        RefreshMap();
    }

    void Update(){
        if (Input.GetKeyDown("space")){
            Cellular();
        }
    }

    void RefreshMap(){
        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                if(map[i,j] == 0){
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = seaColor;
                }
                else{
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = landColor;
                }
            }
        }
    }

    public int liveCells(int x, int y){
        int aliveCount = 0;
        if (x > 0){
            aliveCount += map[x-1,y];
            if (y > 0){
                aliveCount += map[x-1,y-1];
            }
        }

        if (y > 0){
            aliveCount += map[x,y-1];
            if (x < size - 1){
                aliveCount += map[x+1,y-1];
            }
        }

        if (x < size-1){
            aliveCount += map[x+1,y];
            if (y < size - 1){
                aliveCount += map[x+1,y+1];
            }
        }

        if (y < size - 1){
            aliveCount += map[x,y+1];
            if (x > 0){
                aliveCount += map[x-1,y+1];
            }
        }
        return aliveCount;
    }

    void Cellular(){
        for(int i = 1; i < size-1; i++){
            for(int j = 1; j < size-1; j++){
                int landCount = map[i,j] + liveCells(i,j);
                maptmp[i,j] = landCount > 4 ? 1 : 0;
            }
        }

        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                map[i,j] = maptmp[i,j];
            }
        }
        RefreshMap();
    }
}
