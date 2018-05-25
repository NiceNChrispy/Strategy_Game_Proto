using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        List<T> randomList = new List<T>();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (list.Count > 0)
        {
            randomIndex = r.Next(0, list.Count); //Choose a random object in the list
            randomList.Add(list[randomIndex]); //add it to the new, random list
            list.RemoveAt(randomIndex); //remove to avoid duplicates
        }
        return randomList;
    }

    public static T ObjectOfTypeUnderCursor<T>() where T : MonoBehaviour
    {
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(cursorPos);

        RaycastHit hitInfo;

        Physics.Raycast(ray, out hitInfo, 1000);

        if (hitInfo.collider != null)
        {
            T hitObject = hitInfo.collider.GetComponent<T>();

            if (hitObject != null)
            {
                return hitObject;
            }
        }
        return null;
    }
}
