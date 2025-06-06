using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; } // Singleton ��� ����������� �������

    public GameObject bulletImpactEffectPrefab; // ������ ������� ��� ����� ����
    public GameObject grenadeExplosionEffect; // ������ ������ �������

    private void Awake()
    {
        // ���������� Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
