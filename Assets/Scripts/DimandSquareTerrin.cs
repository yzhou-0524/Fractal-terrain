using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimandSquareTerrin : MonoBehaviour
{
    public int mDivisons;
    public float mSize;
    public float mHeight;
    public Gradient gradient;
    // Color[] colors;


    Vector3[] mVerts;
    int mVertCount;
    // Start is called before the first frame update
    void Start()
    {
        CreateTerrain();
    }

    void CreateTerrain()
    {
        mVertCount = (mDivisons + 1) * (mDivisons + 1);
        mVerts = new Vector3[mVertCount];
        Vector2[] uvs = new Vector2[mVertCount];
        int[] tris = new int[mDivisons * mDivisons * 6];

        float halfSize = mSize * 0.5f;
        float divisionSize = mSize / mDivisons;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int triOffset = 0;

        for (int i =0; i<= mDivisons; i++)
        {
            for(int j = 0; j<= mDivisons; j++)
            {
                mVerts[i * (mDivisons + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (mDivisons + 1) + j] = new Vector2((float)i / mDivisons, (float)j / mDivisons);

                if(i< mDivisons && j< mDivisons)
                {
                    int topLeft = i * (mDivisons + 1) + j;
                    int botLeft = (i + 1) * (mDivisons + 1) + j;

                    tris[triOffset] = topLeft;
                    tris[triOffset +1] = topLeft + 1;
                    tris[triOffset + 2] = botLeft + 1;

                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = botLeft + 1;
                    tris[triOffset + 5] = botLeft;

                    triOffset += 6;


                }
            }
        }

        mVerts[0].y = Random.Range(-mHeight, mHeight);
        mVerts[mDivisons].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1 - mDivisons].y = Random.Range(-mHeight, mHeight);

        int iterations = (int)Mathf.Log(mDivisons, 2);
        int numSquare = 1;
        int squareSize = mDivisons;
       // colors = new Color[mVerts.Length];
        for (int i =0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquare; j++)
            {
                int col = 0;
                for (int k = 0; k< numSquare; k++)
                {
                    DiamondSquare(row, col, squareSize, mHeight);
                    col += squareSize;
                    //float height = Mathf.InverseLerp(-mDivisons, mDivisons, mVerts[i].y);
                   // colors[i] = gradient.Evaluate(height);
                }
                row += squareSize;
            }
            numSquare *= 2;
            squareSize /= 2;
            mHeight *= 0.5f;
        }

        //mesh.Clear();
        mesh.vertices = mVerts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        //mesh.colors = colors;


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }    

    void DiamondSquare(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (mDivisons + 1) + col;
        int botLeft = (row + size) * (mDivisons + 1) + col;

        int mid = (int)(row + halfSize) * (mDivisons + 1) + (int)(col + halfSize);
        mVerts[mid].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset) ;

        mVerts[topLeft + halfSize].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid - halfSize].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y)/3 + Random.Range(-offset, offset);
        mVerts[mid + halfSize].y = (mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[botLeft + halfSize].y = (mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }

}
