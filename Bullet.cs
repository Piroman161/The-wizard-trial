using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// ������ ��������� ���������� ���� ��� �������������
public class Bullet : MonoBehaviour
{
    public int bulletDamage; // ����, ��������� ��� ���������

    // ���� ����� ���������� ��� ������������ ���� � ������ ��������
    private void OnCollisionEnter(Collision ObjectWeHit)
    {
        // ���� ������, � ������� �����������, ����� ��� "Target"
        if (ObjectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit " + ObjectWeHit.gameObject.name + " !");
            CreateBulletImpactEffect(ObjectWeHit); // ������� ������ ���������
            Destroy(gameObject); // ���������� ����
        }
        // ���� ������ � ����� (��� "Wall")
        if (ObjectWeHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall ");
            CreateBulletImpactEffect(ObjectWeHit); // ������� ������ ���������
            Destroy(gameObject); // ���������� ����
        }
        // ���� ������ � ���� (��� "Enemy")
        if (ObjectWeHit.gameObject.CompareTag("Enemy"))
        {
            // �������� ����� ��������� ����� � �����
            ObjectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            Destroy(gameObject); // ���������� ����
        }
    }

    // ����� ������� ������ ��������� (��������, ����� ��� �����)
    void CreateBulletImpactEffect(Collision ObjectWeHit)
    {
        // ����� ����� ��������
        ContactPoint contact = ObjectWeHit.contacts[0];

        // ������� ������ � ����� ��������, ��������������� �� ������� �����������
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab, // ������ �������
            contact.point, // ����� ���������
            Quaternion.LookRotation(contact.normal) // ���������� �� ������� �����������
        );

        // ��������� ������ ��� �������� ������ � �������, � ������� ��������� ������������
        hole.transform.SetParent(ObjectWeHit.gameObject.transform);
    }
}