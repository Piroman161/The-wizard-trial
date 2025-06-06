using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; } // Singleton для глобального доступа

    public GameObject bulletImpactEffectPrefab; // Префаб эффекта при ударе пули
    public GameObject grenadeExplosionEffect; // Эффект взрыва гранаты

    private void Awake()
    {
        // Реализация Singleton
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
