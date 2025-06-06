using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

// Скрипт управляет поведением врага, в основном — получением урона и уничтожением
public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100; // Здоровье врага, отображается и редактируется в инспекторе

    // Метод для нанесения урона врагу
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount; // Уменьшаем здоровье на полученную сумму

        // Проверяем, если здоровье стало 0 или меньше, уничтожаем врага
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
