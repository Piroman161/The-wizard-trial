using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using static Throwable; // ������������� ������������ ������ Throwable
using System.Data;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; } // Singleton ��� ������� �� ������ ��������

    public Weapon hoverdWeapon = null; // ������, ��� ������� ������� ������ (���� �����)

    public List<GameObject> weaponSlots; // ����� ��� ������
    public GameObject activeWeaponSlot; // ������� �������� ����

    [Header("Thtowables")]
    public int grenades = 0; // ���������� ������
    public float throwForce = 10f; // ���� ������ �������
    public GameObject grenadePrefab; // ������ �������
    public GameObject throwableSpawn; // ����� ��������� �������
    public float forceMultiplier = 0; // ��������� ���� ������
    public float forceMultiolierLimit = 2f; // ������������ ��������� ����

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

    private void Start()
    {
        // ���������� ������� ������ ����
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        // ������������ ����������� ������ � ������
        foreach (GameObject weaponSlot in weaponSlots)
        {
            weaponSlot.SetActive(weaponSlot == activeWeaponSlot);
        }

        // ������� G ��� ���������� ��������� ���� ������
        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiolierLimit)
            {
                forceMultiplier = forceMultiolierLimit;
            }
        }

        // ������������ ������ �� �������� 1 � 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        // ���������� G � ������� �������, ���� ����
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grenades > 0)
            {
                ThrowLethal();
            }
        }

        // �������� ��������� ����
        forceMultiplier = 0;
    }

    // ����� ��� ������� ������
    public void PickupWeapon(GameObject pickerdupWeapon)
    {
        DropCurrentWeapon(pickerdupWeapon);
        pickerdupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickerdupWeapon.GetComponent<Weapon>();
        // ��������� ������� � rotation ������ ������ �����
        pickerdupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickerdupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
    }

    // ����� ��� ������ �������� ������ ��� ������� ������
    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            // ���������� ������ � ������������ ������ (��������, � ���������)
            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    // ������������ ��������� �����
    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            var newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    // ������ throwable (�������)
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupGrenade();
                break;
        }
    }

    // ����������� ���������� ������
    private void PickupGrenade()
    {
        grenades += 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }

    // ����� ��� ������ �������
    private void ThrowLethal()
    {
        GameObject throwableObject = Instantiate(grenadePrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // ������������, ��� Rigidbody �������
        }

        Throwable throwableScript = throwableObject.GetComponent<Throwable>();
        Vector3 throwDirection = Camera.main.transform.forward;
        float calculatedThrowForce = throwForce * forceMultiplier;

        throwableScript.Throw(throwDirection, calculatedThrowForce);

        grenades -= 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }
}