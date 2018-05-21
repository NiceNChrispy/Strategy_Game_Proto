using UnityEngine;

public class GLTEST : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Draw();    
    }

    private void OnRenderObject()
    {
        Draw();
    }

    private void Draw()
    {
        DrawGL.DrawVector(transform.position, transform.forward, 100);
        DrawGL.DrawLine(Vector3.zero, new Vector3(100,100,100));
    }
}
