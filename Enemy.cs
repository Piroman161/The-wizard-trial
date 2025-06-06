using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

// ������ ��������� ���������� �����, � �������� � ���������� ����� � ������������
public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100; // �������� �����, ������������ � ������������� � ����������

    // ����� ��� ��������� ����� �����
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount; // ��������� �������� �� ���������� �����

        // ���������, ���� �������� ����� 0 ��� ������, ���������� �����
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
