using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreatFloor : MonoBehaviour
{
    public float length;
    public float width;
    public float inradius;
    public Pivot pivot;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;
    private BoxCollider box;

    private float rWidth;
    private float rInr;

    public bool isEdit=false;

    private Pivot borderPoint;

    void Start()
    {
        BulidRender();
        //StartCoroutine(OnMouseDown());
    }

    private void BulidRender()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Star Mesh";
        box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(length, width, 0);
        //三角形顶点的坐标数组    
        vertices = new Vector3[12];
        //三角形顶点ID数组    
        triangles = new int[24];
        //三角形顶点UV坐标  
        uv = new Vector2[12];
        SetPivot();

        rWidth = width / length;
        rInr = inradius / length;
    }

    private void SetPivot()
    {
        //Debug.Log(pivot);
        switch (pivot)
        {
            case Pivot.LeftTop:
                SetLeftTopPivot();
                break;
            case Pivot.Top:
                SetTopPivot();
                break;
            case Pivot.RightTop:
                SetRightTopPivot();
                break;
            case Pivot.Left:
                SetLeftPivot();
                break;
            case Pivot.Center:
                SetCenterPivot();
                break;
            case Pivot.Right:
                SetRightPivot();
                break;
            case Pivot.LeftBottom:
                SetLeftBottomPivot();
                break;
            case Pivot.Bottom:
                SetBottomPivot();
                break;
            case Pivot.RightBottom:
                SetRightBottomPivot();
                break;
        }
        DrawMesh();
    }

    private void SetLeftTopPivot()
    {
        //三角形三个定点坐标，为了显示清楚忽略Z轴    
        vertices[0] = new Vector3(0, -width, 0);
        vertices[1] = new Vector3(0, -width + inradius, 0);
        vertices[2] = new Vector3(0, -inradius, 0);
        vertices[3] = new Vector3(0, 0, 0);

        vertices[4] = new Vector3(length, 0, 0);
        vertices[5] = new Vector3(length, -inradius, 0);
        vertices[6] = new Vector3(length, -width + inradius, 0);
        vertices[7] = new Vector3(length, -width, 0);

        vertices[8] = new Vector3(length - inradius, -width + inradius, 0);
        vertices[9] = new Vector3(inradius, -width + inradius, 0);
        vertices[10] = new Vector3(inradius, -inradius, 0);
        vertices[11] = new Vector3(length - inradius, -inradius, 0);

        box.center = new Vector3(length / 2, -width / 2, 0);
    }

    private void SetTopPivot()
    {
        vertices[0] = new Vector3(-length / 2, -width, 0);
        vertices[1] = new Vector3(-length / 2, -width + inradius, 0);
        vertices[2] = new Vector3(-length / 2, - inradius, 0);
        vertices[3] = new Vector3(-length / 2, 0, 0);

        vertices[4] = new Vector3(length / 2, 0, 0);
        vertices[5] = new Vector3(length / 2, - inradius, 0);
        vertices[6] = new Vector3(length / 2, -width + inradius, 0);
        vertices[7] = new Vector3(length / 2, -width, 0);

        vertices[8] = new Vector3(length / 2 - inradius, -width + inradius, 0);
        vertices[9] = new Vector3(-length / 2 + inradius, -width + inradius, 0);
        vertices[10] = new Vector3(-length / 2 + inradius, -inradius, 0);
        vertices[11] = new Vector3(length / 2 - inradius, - inradius, 0);

        box.center = new Vector3(0, -width / 2, 0);
    }

    private void SetRightTopPivot()
    {
        vertices[0] = new Vector3(-length, -width, 0);
        vertices[1] = new Vector3(-length, -width + inradius, 0);
        vertices[2] = new Vector3(-length, -inradius, 0);
        vertices[3] = new Vector3(-length, 0, 0);

        vertices[4] = new Vector3(0, 0, 0);
        vertices[5] = new Vector3(0, -inradius, 0);
        vertices[6] = new Vector3(0, -width + inradius, 0);
        vertices[7] = new Vector3(0, -width, 0);

        vertices[8] = new Vector3(-inradius, -width + inradius, 0);
        vertices[9] = new Vector3(-length + inradius, -width + inradius, 0);
        vertices[10] = new Vector3(-length + inradius, -inradius, 0);
        vertices[11] = new Vector3(-inradius, -inradius, 0);

        box.center = new Vector3(-length / 2, -width / 2, 0);
    }

    private void SetLeftPivot()
    {
        vertices[0] = new Vector3(0, -width / 2, 0);
        vertices[1] = new Vector3(0, -width / 2 + inradius, 0);
        vertices[2] = new Vector3(0, width / 2 - inradius, 0);
        vertices[3] = new Vector3(0, width / 2, 0);

        vertices[4] = new Vector3(length, width / 2, 0);
        vertices[5] = new Vector3(length, width / 2 - inradius, 0);
        vertices[6] = new Vector3(length, -width / 2 + inradius, 0);
        vertices[7] = new Vector3(length, -width / 2, 0);

        vertices[8] = new Vector3(length - inradius, -width / 2 + inradius, 0);
        vertices[9] = new Vector3(inradius, -width / 2 + inradius, 0);
        vertices[10] = new Vector3(inradius, width / 2 - inradius, 0);
        vertices[11] = new Vector3(length - inradius, width / 2 - inradius, 0);

        box.center = new Vector3(length / 2, 0, 0);
    }

    private void SetCenterPivot()
    {
        vertices[0] = new Vector3(-length/2, -width/2, 0);
        vertices[1] = new Vector3(-length / 2, -width / 2 + inradius, 0);
        vertices[2] = new Vector3(-length / 2, width/2 - inradius, 0);
        vertices[3] = new Vector3(-length / 2, width/2, 0);

        vertices[4] = new Vector3(length / 2, width/2, 0);
        vertices[5] = new Vector3(length / 2, width/2 - inradius, 0);
        vertices[6] = new Vector3(length / 2, -width / 2+inradius, 0);
        vertices[7] = new Vector3(length / 2, -width / 2, 0);

        vertices[8] = new Vector3(length/2 - inradius, -width / 2 + inradius, 0);
        vertices[9] = new Vector3(-length / 2 + inradius, -width / 2 + inradius, 0);
        vertices[10] = new Vector3(-length / 2 + inradius, width / 2 - inradius, 0);
        vertices[11] = new Vector3(length / 2 - inradius, width/2 - inradius, 0);

        box.center = new Vector3(0, 0, 0);
    }

    private void SetRightPivot()
    {
        vertices[0] = new Vector3(-length, -width/2, 0);
        vertices[1] = new Vector3(-length, -width / 2+inradius, 0);
        vertices[2] = new Vector3(-length, width / 2 - inradius, 0);
        vertices[3] = new Vector3(-length, width/2, 0);

        vertices[4] = new Vector3(0, width/2, 0);
        vertices[5] = new Vector3(0, width/2 - inradius, 0);
        vertices[6] = new Vector3(0, -width / 2 + inradius, 0);
        vertices[7] = new Vector3(0, -width / 2, 0);

        vertices[8] = new Vector3(-inradius, -width / 2 + inradius, 0);
        vertices[9] = new Vector3(-length + inradius, -width / 2 + inradius, 0);
        vertices[10] = new Vector3(-length + inradius, width / 2 - inradius, 0);
        vertices[11] = new Vector3(-inradius, width / 2 - inradius, 0);

        box.center = new Vector3(-length / 2, 0, 0);
    }

    private void SetLeftBottomPivot()
    {
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, inradius, 0);
        vertices[2] = new Vector3(0, width - inradius, 0);
        vertices[3] = new Vector3(0, width, 0);

        vertices[4] = new Vector3(length, width, 0);
        vertices[5] = new Vector3(length, width - inradius, 0);
        vertices[6] = new Vector3(length, inradius, 0);
        vertices[7] = new Vector3(length, 0, 0);

        vertices[8] = new Vector3(length - inradius, inradius, 0);
        vertices[9] = new Vector3(inradius, inradius, 0);
        vertices[10] = new Vector3(inradius, width - inradius, 0);
        vertices[11] = new Vector3(length - inradius, width - inradius, 0);

        box.center = new Vector3(length / 2, width / 2, 0);
    }

    private void SetBottomPivot()
    {
        vertices[0] = new Vector3(-length / 2, 0, 0);
        vertices[1] = new Vector3(-length / 2, inradius, 0);
        vertices[2] = new Vector3(-length / 2, width - inradius, 0);
        vertices[3] = new Vector3(-length / 2, width, 0);

        vertices[4] = new Vector3(length / 2, width, 0);
        vertices[5] = new Vector3(length / 2, width - inradius, 0);
        vertices[6] = new Vector3(length / 2, inradius, 0);
        vertices[7] = new Vector3(length / 2, 0, 0);

        vertices[8] = new Vector3(length / 2 - inradius, inradius, 0);
        vertices[9] = new Vector3(-length / 2 + inradius, inradius, 0);
        vertices[10] = new Vector3(-length / 2 + inradius, width - inradius, 0);
        vertices[11] = new Vector3(length / 2 - inradius, width - inradius, 0);

        box.center = new Vector3(0, width / 2, 0);
    }

    private void SetRightBottomPivot()
    {
        vertices[0] = new Vector3(-length, 0, 0);
        vertices[1] = new Vector3(-length, inradius, 0);
        vertices[2] = new Vector3(-length, width-inradius, 0);
        vertices[3] = new Vector3(-length, width, 0);

        vertices[4] = new Vector3(0, width, 0);
        vertices[5] = new Vector3(0, width-inradius, 0);
        vertices[6] = new Vector3(0, inradius, 0);
        vertices[7] = new Vector3(0, 0, 0);

        vertices[8] = new Vector3(-inradius, inradius, 0);
        vertices[9] = new Vector3(-length + inradius, inradius, 0);
        vertices[10] = new Vector3(-length + inradius, width-inradius, 0);
        vertices[11] = new Vector3(-inradius, width-inradius, 0);

        box.center = new Vector3(-length / 2, width / 2, 0);
    }

    void DrawMesh()
    {
        //Debug.Log("DrawMesh");
        //三角形绘制顶点的数组    
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 6;
        triangles[3] = 0;
        triangles[4] = 6;
        triangles[5] = 7;

        triangles[6] = 2;
        triangles[7] = 10;
        triangles[8] = 9;
        triangles[9] = 2;
        triangles[10] = 9;
        triangles[11] = 1;

        triangles[12] = 3;
        triangles[13] = 4;
        triangles[14] = 5;
        triangles[15] = 3;
        triangles[16] = 5;
        triangles[17] = 2;

        triangles[18] = 11;
        triangles[19] = 5;
        triangles[20] = 6;
        triangles[21] = 11;
        triangles[22] = 6;
        triangles[23] = 8;

        uv[0] = new Vector2(0f, 0f);
        uv[1] = new Vector2(0, inradius / width);
        uv[2] = new Vector2(0f, (width - inradius) / width);
        uv[3] = new Vector2(0f, 1);
        uv[4] = new Vector2(1f, 1f);
        uv[5] = new Vector2(1, (width - inradius) / width);
        uv[6] = new Vector2(1, inradius / width);
        uv[7] = new Vector2(1, 0f);
        uv[8] = new Vector2((length - inradius) / length, inradius / width);
        uv[9] = new Vector2(inradius / length, inradius / width);
        uv[10] = new Vector2(inradius / length, (width - inradius) / width);
        uv[11] = new Vector2((length - inradius) / length, (width - inradius) / width);


        //注释1    
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.normals = vertices;
        box.size = new Vector3(length, width, 0);
    }

    private void Update()
    {
        //鼠标滚轮的效果
        //Camera.main.fieldOfView 摄像机的视野
        //Camera.main.orthographicSize 摄像机的正交投影
        //Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (isEdit)
            {
                if(length>0.5 && width>0.1 && inradius>0)
                {
                    length -= 0.5f;
                    width = rWidth * length;
                    inradius = rInr * length;
                    SetPivot();
                }
            }
            else
            {
                if (Camera.main.fieldOfView <= 100)
                    Camera.main.fieldOfView += 2;

                if (Camera.main.orthographicSize <= 20)
                    Camera.main.orthographicSize += 0.5F;
            }
        }

        //Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (isEdit)
            {
                if(length <20 && width < 10 )
                {
                    length += 0.5f;
                    width = rWidth * length;
                    inradius = rInr * length;
                    SetPivot();
                }
            }
            else
            {
                if (Camera.main.fieldOfView > 2)
                    Camera.main.fieldOfView -= 2;

                if (Camera.main.orthographicSize >= 1)
                    Camera.main.orthographicSize -= 0.5F;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            length -= 0.5f;
            width = rWidth*length;
            inradius = rInr * length;
            SetPivot();
        }
        else if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            length += 0.5f;
            width = rWidth * length;
            inradius = rInr * length;
            SetPivot();
        }
    }


   

    private void GetBoderInfo(Vector3 hit, Vector3 center,out Pivot borderPoint)
    {
        float horizontal;
        float vertical;

        float halfLength= length *0.5f;
        float halfWidth = width * 0.5f;

        horizontal = hit.x - center.x;
        vertical = hit.y - center.y;

        if (horizontal > -halfLength && horizontal < -halfLength + inradius)
        {
            if (vertical > halfWidth - inradius && vertical < halfWidth)
                borderPoint = Pivot.LeftTop;
            else if (vertical >= -halfWidth + inradius && vertical <= halfWidth - inradius)
                borderPoint = Pivot.Left;
            else if (vertical > -halfWidth && vertical < -halfWidth + inradius)
                borderPoint = Pivot.LeftBottom;
            else
                borderPoint = Pivot.Empty;
        }
        else if (horizontal >= -halfLength + inradius && horizontal <= halfLength - inradius)
        {
            if (vertical > halfWidth - inradius && vertical < halfWidth)
                borderPoint = Pivot.Top;
            else if (vertical >= -halfWidth + inradius && vertical <= halfWidth - inradius)
                borderPoint = Pivot.Center;
            else if (vertical > -halfWidth && vertical < -halfWidth + inradius)
                borderPoint = Pivot.Bottom;
            else
                borderPoint = Pivot.Empty;
        }
        else if (horizontal > halfLength - inradius && horizontal < halfLength)
        {
            if (vertical > halfWidth - inradius && vertical < halfWidth)
                borderPoint = Pivot.RightTop;
            else if (vertical >= -halfWidth + inradius && vertical <= halfWidth - inradius)
                borderPoint = Pivot.Right;
            else if (vertical > -halfWidth && vertical < -halfWidth + inradius)
                borderPoint = Pivot.RightBottom;
            else
                borderPoint = Pivot.Empty;
        }
        else
            borderPoint = Pivot.Empty;
    }

    IEnumerator OnMouseDown()
    {
        //将物体由世界坐标系转换为屏幕坐标系
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//三维物体坐标转屏幕坐标

        //完成两个步骤 1.由于鼠标的坐标系是2维，需要转换成3维的世界坐标系  
        //            2.只有3维坐标情况下才能来计算鼠标位置与物理的距离，offset即是距离
        //将鼠标屏幕坐标转为三维坐标，再算出物体位置与鼠标之间的距离
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition)/*new Ray(Camera.main.transform.position, transform.forward)*/;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GetBoderInfo(hit.point, hit.transform.position, out borderPoint);
            Debug.Log(borderPoint);
        }


        while (Input.GetMouseButton(0))
        {
            //得到现在鼠标的2维坐标系位置
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            //将当前鼠标的2维位置转换成3维位置，再加上鼠标的移动量
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            if (borderPoint.Equals(Pivot.Center) || borderPoint.Equals(Pivot.Empty))
            {
                //curPosition就是物体应该的移动向量赋给transform的position属性
                transform.position = curPosition;
            }
            else
            {
                DragBorder(curPosition);
            }

            yield return new WaitForFixedUpdate(); //这个很重要，循环执行
        }
    }

    private void DragBorder(Vector3 offset)
    {
        switch (borderPoint)
        {
            case Pivot.LeftTop:
                DragLeftTop(offset);
                break;
            case Pivot.Left:
                DragLeft(offset);
                break;
            case Pivot.LeftBottom:
                DragLeftBottom(offset);
                break;
            case Pivot.Top:
                DragTop(offset);
                break;
            case Pivot.Bottom:
                DragBottom(offset);
                break;
            case Pivot.RightTop:
                DragRightTop(offset);
                break;
            case Pivot.Right:
                DragRight(offset);
                break;
            case Pivot.RightBottom:
                DragRightBottom(offset);
                break;
        }
    }

    

    private void DragLeft(Vector3 offset)
    {
         if (offset.x<0 && length < 16)
         {
            length -= offset.x / 10;
            SetPivot();
         }

        if (offset.x > 0 && length > 2*inradius)
        {
            length -= offset.x / 10;
            SetPivot();
        }
    }

    private void DragRight(Vector3 offset)
    {
        if (offset.x < 0 && length > 2 * inradius)
        {
            length += offset.x / 10;
            SetPivot();
        }

        if (offset.x > 0 && length < 16)
        {
            length += offset.x / 10;
            SetPivot();
        }
    }

    private void DragTop(Vector3 offset)
    {
        if (offset.y < 0 && width > 2 * inradius)
        {
            width += offset.y / 10;
            SetPivot();
        }

        if (offset.y > 0 && width < 8)
        {
            width += offset.y / 10;
            SetPivot();
        }
    }

    private void DragBottom(Vector3 offset)
    {
        if (offset.y < 0 && width < 8)
        {
            width -= offset.y / 10;
            SetPivot();
        }

        if (offset.y > 0 && width > 2 * inradius)
        {
            width -= offset.y / 10;
            SetPivot();
        }
    }

    private void DragLeftTop(Vector3 offset)
    {
        if (offset.x < 0 && offset.y > 0)
        {
            length += 0.1f;
            width = rWidth * length;
           
        }
        else if (offset.x < 0 && offset.y < 0)
        {
            length -= 0.1f;
            width = rWidth * length;
            
        }
        else if (offset.x > 0 && offset.y > 0)
        {
            length += 0.1f;
            width = rWidth * length;
            
        }
        else if (offset.x > 0 && offset.y < 0)
        {
            length -= 0.1f;
            width = rWidth * length;
            
        }
        else
            return;
        SetPivot();
    }

    private void DragLeftBottom(Vector3 offset)
    {
        throw new NotImplementedException();
    }

    private void DragRightTop(Vector3 offset)
    {
        throw new NotImplementedException();
    }

    private void DragRightBottom(Vector3 offset)
    {
        throw new NotImplementedException();
    }






}

public enum Pivot
{
    Empty,
    LeftTop,
    Top,
    RightTop,
    Left,
    Center,
    Right,
    LeftBottom,
    Bottom,
    RightBottom,
    X,
}
