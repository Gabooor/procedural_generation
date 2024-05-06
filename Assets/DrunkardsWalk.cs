using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkardsWalk : MonoBehaviour
{
    private int[,] map;
    private int[,] footstep;
    private GameObject[,] mapGO;

    public GameObject cell;
    public Camera cam;
    
    public Color32 seaColor;
    public Color32 landColor;
    public Color32 smoothingColor;

    public int size;
    public int amountOfWalks = 0;
    public int stepsPerWalk = 0;

    void Start(){
        Debug.Log("<b><size=25>0: A pálya simítása</size></b>");
        Debug.Log("<b><size=25>1: Minden lépés csoport különböző színnel való megjelenítése</size></b>");
        Debug.Log("<b><size=25>2: Térkép normális megjelenítése</size></b>");
        map = new int[size,size];
        footstep = new int[size,size];
        mapGO = new GameObject[size,size];
        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                map[i,j] = 0;
                mapGO[i,j] = Instantiate(cell);
                mapGO[i,j].transform.position = new Vector2(i-(size/2),(size/2)-j);
                cam.orthographicSize = size/2+2;
            }
        }
        Walk();
        SwitchToGrass();
    }

    void Update()
    {
        if (Input.GetKeyDown("0")){
            SmoothTerrain(7);
        }
        if (Input.GetKeyDown("1")){
            ShowStepColors();
        }
        if (Input.GetKeyDown("2")){
            SwitchToGrass();
        }
    }

    void ResetFootsteps(){
        for(int j = 0; j < size; j++){ // Lépés tömb kiürítése
            for(int k = 0; k < size; k++){
                footstep[j,k] = 0;
            }
        }
    }

    void SimulateIteration(int x, int y){
        footstep[x,y] = 1;
        for(int j = 0; j < stepsPerWalk; j++){ // Lépés valamely irányba
            int rand = Random.Range(0,4);
            if((rand == 0 && y > 0) || (rand == 1 && x < size-1) || (rand == 2 && y < size-1) || (rand == 3 && x > 0)){
                switch(rand){
                    case 0: y--; break;
                    case 1: x++; break;
                    case 2: y++; break;
                    case 3: x--; break;
                    default: break;
                }
                footstep[x,y] = 1;
            }
        }
    }

    void AddNewFootsteps(int color){
        for(int j = 0; j < size; j++){ // Pályához hozzáadjuk az új utat
            for(int k = 0; k < size; k++){
                if(footstep[j,k] == 1){
                    map[j,k] = color;
                }
            }
        }
    }

    void Walk(){
        for(int i = 0; i < amountOfWalks; i++){
            int x = 0;
            int y = 0;
            if(i == 0){ // Először a pálya közepéről indul a szimuláció
                x = (int) Mathf.Floor(size/2);
                y = (int) Mathf.Floor(size/2);
            }
            else{ // Utána pedig a bejárt út egy pontjából
                bool found = false;
                while(!found){
                    int xRand = Random.Range(0,size);
                    int yRand = Random.Range(0,size);
                    if(map[xRand,yRand] > 0){
                        x = xRand;
                        y = yRand;
                        found = true;
                    }
                }
            }
            ResetFootsteps();
            SimulateIteration(x,y);
            AddNewFootsteps(i+1);
        }
    }

    void ShowStepColors(){
        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                if(map[i,j] == 0){
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = seaColor; // VÍZ
                }
                else if(map[i,j] == 555){
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = smoothingColor; // SIMÍTÁS
                }
                else{
                    int colorValue = 0 + (255/amountOfWalks*map[i,j]);
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = new Color32((byte) colorValue,(byte) colorValue,(byte) colorValue,255); // FÖLD 
                }
            }
        }
    }

    void SwitchToGrass(){
        for(int i = 0; i < size; i++){
            for(int j = 0; j < size; j++){
                if(map[i,j] == 0){
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = seaColor; // VÍZ
                }
                else if(map[i,j] == 150){
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = new Color32(0,0,0,255); // FÖLD
                }
                else{
                    mapGO[i,j].GetComponent<SpriteRenderer>().color = landColor; // FÖLD
                }
            }
        }
    }

    void SmoothTerrain(int value){
        for(int j = 0; j < size; j++){
            for(int k = 0; k < size; k++){
                footstep[j,k] = 0;
            }
        }
        for(int j = 1; j < size-1; j++){
            for(int k = 1; k < size-1; k++){
                int neighbours = 0;
                if(map[j,k] == 0){
                    if(map[j-1,k-1] > 0) neighbours++;
                    if(map[j-1,k] > 0) neighbours++;
                    if(map[j-1,k+1] > 0) neighbours++;
                    if(map[j,k-1] > 0) neighbours++;
                    if(map[j,k+1] > 0) neighbours++;
                    if(map[j+1,k-1] > 0) neighbours++;
                    if(map[j+1,k] > 0) neighbours++;
                    if(map[j+1,k+1] > 0) neighbours++;
                }
                if(neighbours >= value) footstep[j,k] = 1;
            }
        }
        for(int j = 0; j < size; j++){
            for(int k = 0; k < size; k++){
                if(footstep[j,k] == 1){
                    map[j,k] = 555;
                }
            }
        }
        SwitchToGrass();
    }
}
