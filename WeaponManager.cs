using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using static Throwable; // Использование статического класса Throwable
using System.Data;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; } // Singleton для доступа из других скриптов

    public Weapon hoverdWeapon = null; // Оружие, над которым наведен курсор (если нужно)

    public List<GameObject> weaponSlots; // Слоты для оружия
    public GameObject activeWeaponSlot; // Текущий активный слот

    [Header("Thtowables")]
    public int grenades = 0; // Количество гранат
    public float throwForce = 10f; // Сила броска гранаты
    public GameObject grenadePrefab; // Префаб гранаты
    public GameObject throwableSpawn; // Точка появления гранаты
    public float forceMultiplier = 0; // Множитель силы броска
    public float forceMultiolierLimit = 2f; // Максимальный множитель силы

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

    private void Start()
    {
        // Изначально активен первый слот
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        // Переключение отображения оружий в слотах
        foreach (GameObject weaponSlot in weaponSlots)
        {
            weaponSlot.SetActive(weaponSlot == activeWeaponSlot);
        }

        // Нажатие G для увеличения множителя силы броска
        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiolierLimit)
            {
                forceMultiplier = forceMultiolierLimit;
            }
        }

        // Переключение оружия по клавишам 1 и 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        // Отпускание G — бросить гранату, если есть
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grenades > 0)
            {
                ThrowLethal();
            }
        }

        // Обнуляем множитель силы
        forceMultiplier = 0;
    }

    // Метод для подбора оружия
    public void PickupWeapon(GameObject pickerdupWeapon)
    {
        DropCurrentWeapon(pickerdupWeapon);
        pickerdupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickerdupWeapon.GetComponent<Weapon>();
        // Установка позиции и rotation оружия внутри слота
        pickerdupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickerdupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
    }

    // Метод для сброса текущего оружия при подборе нового
    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            // Перемещаем оружие в родительский объект (например, в инвентарь)
            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    // Переключение активного слота
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

    // Подбор throwable (гранаты)
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupGrenade();
                break;
        }
    }

    // Увеличиваем количество гранат
    private void PickupGrenade()
    {
        grenades += 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }

    // Метод для броска гранаты
    private void ThrowLethal()
    {
        GameObject throwableObject = Instantiate(grenadePrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwableObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Обеспечиваем, что Rigidbody активен
        }

        Throwable throwableScript = throwableObject.GetComponent<Throwable>();
        Vector3 throwDirection = Camera.main.transform.forward;
        float calculatedThrowForce = throwForce * forceMultiplier;

        throwableScript.Throw(throwDirection, calculatedThrowForce);

        grenades -= 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }
}