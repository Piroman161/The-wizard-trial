using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; } // Singleton ��� ����������� �������

    public Weapon hoverdWeapon = null; // ������, ��� ������� � ������ ������ ������
    public Throwable hoverdThrowable = null; // throwable (������� ��� ������ �������� �������)

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

    private void Update()
    {
        // ������� ���, ��������� �� ������ ������ (���������)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // ��������� Raycast
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // ��������� �������������� � �������
            if (objectHitByRaycast.GetComponent<Weapon>() &&
                objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoverdWeapon = objectHitByRaycast.GetComponent<Weapon>();
                // �������� ������ ������� (Outline) ��� ���������
                hoverdWeapon.GetComponent<Outline>().enabled = true;

                // ���� ������ ������� 'E', ��������� ������
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast);
                }
            }
            else
            {
                // ���� ����� ���� ������������ ������, ��������� ���������
                if (hoverdWeapon)
                {
                    hoverdWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            // ��������� �������������� � throwable (��������� � �.�.)
            if (objectHitByRaycast.GetComponent<Throwable>())
            {
                hoverdThrowable = objectHitByRaycast.GetComponent<Throwable>();
                // �������� ������ �������
                hoverdThrowable.GetComponent<Outline>().enabled = true;

                // ���� ������ ������� 'E', ��������� throwable
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupThrowable(hoverdThrowable);
                    Destroy(objectHitByRaycast); // ������� ������� �� �����
                }
            }
            else
            {
                // ���� ����� ���� ������������ throwable, ��������� ���������
                if (hoverdThrowable)
                {
                    hoverdThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}