using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeshRenderTest : MonoBehaviour
{
    Mesh mesh;
    public MeshRenderer meshRender;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    //public int xSize = 20;
    //public int zSize = 20;
    public float deep;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    string myText;
    string mySizex;
    string mySizey;
    string info;

    // Start is called before the first frame update
    void Start()
    {
        myText = "";
        mySizex = "";
        mySizey = "";
        info = "";
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


    }

    void Update()
    {
        var setCam = Camera.main;
        setCam.transform.LookAt(meshRender.bounds.center);
    }



    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 60, 20), "Height");
        myText = GUI.TextField(new Rect(80, 20, 140, 20), myText, 15); // 15 为最大字符串长度

        GUI.Label(new Rect(20, 40, 60, 20), "Length");
        mySizex = GUI.TextField(new Rect(80, 40, 40, 20), mySizex, 15); // 15 为最大字符串长度

        GUI.Label(new Rect(140, 40, 60, 20), "Width");
        mySizey = GUI.TextField(new Rect(180, 40, 40, 20), mySizey, 15); // 15 为最大字符串长度

        GUI.Label(new Rect(20, 80, 200, 80), info);


        if (GUI.Button(new Rect(50, 140, 60, 20), "Confirm"))
        {
            info = "The steepness of the terrain is " + myText + "\nThe size of the terrain is " + mySizex + " * " + mySizey ;

            CreateShape();
            UpdateMesh();
        }

        if (GUI.Button(new Rect (130,140,60,20),"Reset"))
        {
            print("重新开始键ok");
            SceneManager.LoadScene(0);
            
        }

        if (GUI.RepeatButton(new Rect (400,700,60,20),"Left"))
        {
            RotationL();
        }
        if (GUI.RepeatButton(new Rect(580, 700,60, 20), "Right"))
        {
            RotationR();
        }
    }


    
    void CreateShape()
    {
        int xSize = int.Parse(mySizex);
        int zSize = int.Parse(mySizey);
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float deep = float.Parse(myText);

        for (int i= 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * deep;//使得地面有起伏
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z< zSize; z++)
        {

            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;



                vert++;
                tris += 6;
            }

            vert++;        
        
        }

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }


    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

     void RotationL()
    {
        transform.RotateAround(meshRender.bounds.center, new Vector3(0, 1, 0), -20* Time.deltaTime);
    }

    void RotationR()
    {
        transform.RotateAround(meshRender.bounds.center, new Vector3(0, 1, 0), 20 * Time.deltaTime);
    }


}
