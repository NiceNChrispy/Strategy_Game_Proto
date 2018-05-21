using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public static class DrawGL
{
    static Material DrawMat;
    public static Color Color { get; set; }

    static DrawGL()
    {
        InitMaterial();
        Color = Color.white;
    }

    private static void InitMaterial()
    {
        DrawMat = new Material(Shader.Find("Unlit/Color"));
    }

    public static void DrawVector(Vector3 origin, Vector3 direction, float length = 1)
    {
        DrawLine(origin, origin + (direction * length));
    }

    public static void DrawRay(Ray ray)
    {
        DrawVector(ray.origin, ray.direction);
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
        if(!DrawMat)
        {
            InitMaterial();
        }
        DrawMat.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(start);
        GL.Color(Color);
        GL.Vertex(end);
        GL.End();
    }
}
