using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private LevelData m_LevelData;

    private void Update()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if(m_LevelData)
        {
            m_LevelData.DrawGizmos();
        }
    }
}