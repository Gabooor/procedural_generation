using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Voronoi : MonoBehaviour
{
    [SerializeField] private Color[] possibleColors;

    private int imgSize;
    private int gridSize = 15;
    private int pixelsPerCell;
    private RawImage image;
    private Color[,] colors;
    private Vector2Int[,] pointPositions;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);
        
    }

    private void Start(){
        GenerateMap();
    }

    private void Update(){
        if (Input.GetKeyDown("0")){
            ShowMap();
        }
    }

    private void GenerateMap(){
        Texture2D texture = new Texture2D(imgSize, imgSize);
        texture.filterMode = FilterMode.Point;
        pixelsPerCell = imgSize / gridSize;
        for(int i = 0; i < imgSize; i++){
            for(int j = 0; j < imgSize; j++){
                texture.SetPixel(i, j, Color.white);
            }
        }
 
        GeneratePoints();
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                texture.SetPixel(pointPositions[i,j].x, pointPositions[i,j].y, Color.black);
                texture.SetPixel(pointPositions[i,j].x-1, pointPositions[i,j].y-1, Color.black);
                texture.SetPixel(pointPositions[i,j].x-1, pointPositions[i,j].y, Color.black);
                texture.SetPixel(pointPositions[i,j].x, pointPositions[i,j].y-1, Color.black);
                texture.SetPixel(pointPositions[i,j].x+1, pointPositions[i,j].y+1, Color.black);
                texture.SetPixel(pointPositions[i,j].x+1, pointPositions[i,j].y, Color.black);
                texture.SetPixel(pointPositions[i,j].x, pointPositions[i,j].y+1, Color.black);
                texture.SetPixel(pointPositions[i,j].x+1, pointPositions[i,j].y-1, Color.black);
                texture.SetPixel(pointPositions[i,j].x-1, pointPositions[i,j].y+1, Color.black);
            }
        }

        texture.Apply();
        image.texture = texture;
    }

    private void ShowMap(){        
        Texture2D texture = new Texture2D(imgSize, imgSize);
        texture.filterMode = FilterMode.Point;
        pixelsPerCell = imgSize / gridSize;
        for(int i = 0; i < imgSize; i++){
            for(int j = 0; j < imgSize; j++){
                int gridX = i / pixelsPerCell;
                int gridY = j / pixelsPerCell;

                float nearestDistance = Mathf.Infinity;
                Vector2Int nearestPoint = new Vector2Int();

                for(int a = -2; a <= 2; a++){
                    for(int b = -2; b <= 2; b++){
                        int X = gridX + a;
                        int Y = gridY + b;
                        if (X < 0 || Y < 0 || X >= gridSize || Y >= gridSize) continue;

                        float distance = Vector2Int.Distance(new Vector2Int(i,j), pointPositions[X, Y]);
                        if(distance < nearestDistance){
                            nearestDistance = distance;
                            nearestPoint = new Vector2Int(X, Y);
                        }
                    }
                }
                texture.SetPixel(i,j, colors[nearestPoint.x, nearestPoint.y]);
            }
        }
        texture.Apply();
        image.texture = texture;
    }

    private void GeneratePoints(){
        pointPositions = new Vector2Int[gridSize, gridSize];
        colors = new Color[gridSize, gridSize];
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                pointPositions[i, j] = new Vector2Int(i * pixelsPerCell + Random.Range(0, pixelsPerCell), j * pixelsPerCell + Random.Range(0, pixelsPerCell));
                colors[i, j] = possibleColors[Random.Range(0, possibleColors.Length)];
                Debug.Log(pointPositions[i,j]);
            }
        }
    }
}