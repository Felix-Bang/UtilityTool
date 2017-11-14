using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatWall : MonoBehaviour
{
	private float _wallHeight = 3.5f;
    private float _wallLength = 3.0f;
    private float _wallWidth = 0.24f;//默认的墙的厚度  

    Mesh createObject(Vector2 v1, Vector2 v2, float wallWidth, float wallHeight)
    {
        Mesh mesh = new Mesh();

        Vector3[] points_bottom = getRectPositions(v1, v2, wallWidth, 0);

        for (int m = 0; m < points_bottom.Length; m++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere) as GameObject;
            obj.transform.position = points_bottom[m];
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }


        Vector3[] points_top = getRectPositions(v1, v2, wallWidth, wallHeight);

        for (int p = 0; p < points_top.Length; p++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
            obj.transform.position = points_top[p];
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }



        Vector3[] points_six = getRectPoints(points_top, points_bottom);

        Vector3[] points = new Vector3[points_six.Length];


        points_six.CopyTo(points, 0);

        int[] triangles = new int[points.Length];
        int j = 0;
        while (j < points.Length)
        {
            triangles[j] = j;
            j++;
        }
        Vector2[] uvs = new Vector2[points.Length];
        int i = 0;
        while (i < points.Length)
        {
            uvs[i] = new Vector2(points[i].x, points[i].z);
            i++;
        }
        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        return mesh;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "draw"))
        {
            GameObject wallone = GameObject.Find("wall1") as GameObject;
            Mesh mesh = wallone.GetComponent<MeshFilter>().mesh;
            mesh = createObject(new Vector2(0, 0), new Vector2(-5, -5), _wallWidth, _wallHeight);
            wallone.GetComponent<MeshFilter>().mesh = mesh;

            /** 
            GameObject walltwo= GameObject.Find("wall2") as GameObject; 
            mesh=walltwo.GetComponent<MeshFilter>().mesh; 
            mesh=createObject(new Vector2(0,0),new Vector2(10,10),_wallWidth,0); 
            walltwo.GetComponent<MeshFilter>().mesh=mesh; 
           **/
        }
    }

    Vector3[] getRectPositions(Vector2 v1, Vector2 v2, float _width, float _height)
    {
        Vector3[] positions = new Vector3[4];

        float x0 = v1.x;
        float y0 = v1.y;

        float x1 = v2.x;
        float y1 = v2.y;

        if (x0 == x1)
        {
            if (y0 <= y1)
            {
                positions[0] = new Vector3(x0 - _width, _height, y0);
                positions[1] = new Vector3(x0 + _width, _height, y0);
                positions[2] = new Vector3(x1 + _width, _height, y1);
                positions[3] = new Vector3(x1 - _width, _height, y1);
            }
            else
            {
                positions[0] = new Vector3(x0 + _width, _height, y0);
                positions[1] = new Vector3(x0 - _width, _height, y0);
                positions[2] = new Vector3(x1 - _width, _height, y1);
                positions[3] = new Vector3(x1 + _width, _height, y1);
            }
        }
        else if (y0 == y1)
        {
            if (x0 <= x1)
            {
                positions[0] = new Vector3(x0, _height, y0 + _width);
                positions[1] = new Vector3(x0, _height, y0 - _width);
                positions[2] = new Vector3(x1, _height, y1 - _width);
                positions[3] = new Vector3(x1, _height, y1 + _width);
            }
            else
            {
                positions[0] = new Vector3(x0, _height, y0 - _width);
                positions[1] = new Vector3(x0, _height, y0 + _width);
                positions[2] = new Vector3(x1, _height, y1 + _width);
                positions[3] = new Vector3(x1, _height, y1 - _width);
            }
        }
        else
        {
            float k1 = (y1 - y0) / (x1 - x0);
            float k2 = (x0 - x1) / (y1 - y0);

            float realX = _width * Mathf.Sqrt(1 / (1 + k2 * k2));
            float realY = _width * Mathf.Sqrt((k2 * k2) / (1 + k2 * k2));

            if (k1 > 0)
            {
                if (x0 < x1)
                {
                    positions[0] = new Vector3(x0 - realX, _height, y0 + realY);
                    positions[1] = new Vector3(x0 + realX, _height, y0 - realY);
                    positions[2] = new Vector3(x1 + realX, _height, y1 - _width);
                    positions[3] = new Vector3(x1 - realX, _height, y1 + _width);
                }
                else
                {
                    positions[0] = new Vector3(x0 + realX, _height, y0 - realY);
                    positions[1] = new Vector3(x0 - realX, _height, y0 + realY);
                    positions[2] = new Vector3(x1 - realX, _height, y1 + _width);
                    positions[3] = new Vector3(x1 + realX, _height, y1 - _width);
                }
            }
            else
            {
                if (x0 < x1)
                {
                    positions[0] = new Vector3(x0 + realX, _height, y0 + realY);
                    positions[1] = new Vector3(x0 - realX, _height, y0 - realY);
                    positions[2] = new Vector3(x1 - realX, _height, y1 - _width);
                    positions[3] = new Vector3(x1 + realX, _height, y1 + _width);
                }
                else
                {
                    positions[0] = new Vector3(x0 - realX, _height, y0 - realY);
                    positions[1] = new Vector3(x0 + realX, _height, y0 + realY);
                    positions[2] = new Vector3(x1 + realX, _height, y1 + _width);
                    positions[3] = new Vector3(x1 - realX, _height, y1 - _width);
                }
            }
        }
        return positions;
    }

    Vector3[] getRectPoints(Vector3[] pointsTop, Vector3[] pointsBottom)
    {

        Vector3[] points_bottom = new Vector3[6];

        points_bottom[0] = pointsBottom[0];
        points_bottom[1] = pointsBottom[1];
        points_bottom[2] = pointsBottom[2];

        points_bottom[3] = pointsBottom[0];
        points_bottom[4] = pointsBottom[2];
        points_bottom[5] = pointsBottom[3];


        Vector3[] points_top = new Vector3[6];

        points_top[0] = pointsTop[0];
        points_top[1] = pointsTop[1];
        points_top[2] = pointsTop[2];

        points_top[3] = pointsTop[0];
        points_top[4] = pointsTop[2];
        points_top[5] = pointsTop[3];


        Vector3[] points_face = new Vector3[6];

        points_face[0] = pointsBottom[0];
        points_face[1] = pointsBottom[1];
        points_face[2] = pointsTop[1];

        points_face[3] = pointsBottom[0];
        points_face[4] = pointsTop[1];
        points_face[5] = pointsTop[0];


        Vector3[] points_back = new Vector3[6];

        points_back[0] = pointsBottom[2];
        points_back[1] = pointsBottom[3];
        points_back[2] = pointsTop[3];

        points_back[3] = pointsBottom[2];
        points_back[4] = pointsTop[3];
        points_back[5] = pointsTop[2];

        Vector3[] points_left = new Vector3[6];

        points_left[0] = pointsBottom[3];
        points_left[1] = pointsBottom[0];
        points_left[2] = pointsTop[0];

        points_left[3] = pointsBottom[3];
        points_left[4] = pointsTop[0];
        points_left[5] = pointsTop[3];



        Vector3[] points_right = new Vector3[6];

        points_right[0] = pointsBottom[1];
        points_right[1] = pointsBottom[2];
        points_right[2] = pointsTop[2];

        points_right[3] = pointsBottom[1];
        points_right[4] = pointsTop[2];
        points_right[5] = pointsTop[1];


        Vector3[] points = new Vector3[points_top.Length + points_bottom.Length + points_face.Length + points_back.Length + points_left.Length + points_right.Length];

        points_top.CopyTo(points, 0);
        points_bottom.CopyTo(points, points_top.Length);
        points_face.CopyTo(points, points_top.Length + points_bottom.Length);
        points_back.CopyTo(points, points_top.Length + points_bottom.Length + points_face.Length);
        points_left.CopyTo(points, points_top.Length + points_bottom.Length + points_face.Length + points_back.Length);
        points_right.CopyTo(points, points_top.Length + points_bottom.Length + points_face.Length + points_back.Length + points_left.Length);

        return points;
    }

}
