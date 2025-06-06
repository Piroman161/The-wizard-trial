using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; } // Singleton для глобального доступа

    public Weapon hoverdWeapon = null; // Оружие, над которым в данный момент курсор
    public Throwable hoverdThrowable = null; // throwable (граната или другой подобный предмет)

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

    private void Update()
    {
        // Создаем луч, исходящий из центра экрана (персонажа)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Выполняем Raycast
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // Обработка взаимодействия с оружием
            if (objectHitByRaycast.GetComponent<Weapon>() &&
                objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoverdWeapon = objectHitByRaycast.GetComponent<Weapon>();
                // Включаем эффект обводки (Outline) для подсветки
                hoverdWeapon.GetComponent<Outline>().enabled = true;

                // Если нажата клавиша 'E', подбираем оружие
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast);
                }
            }
            else
            {
                // Если ранее было подсвеченное оружие, отключаем подсветку
                if (hoverdWeapon)
                {
                    hoverdWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            // Обработка взаимодействия с throwable (гранатами и т.п.)
            if (objectHitByRaycast.GetComponent<Throwable>())
            {
                hoverdThrowable = objectHitByRaycast.GetComponent<Throwable>();
                // Включаем эффект обводки
                hoverdThrowable.GetComponent<Outline>().enabled = true;

                // Если нажата клавиша 'E', подбираем throwable
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupThrowable(hoverdThrowable);
                    Destroy(objectHitByRaycast); // Удаляем предмет из сцены
                }
            }
            else
            {
                // Если ранее было подсвеченное throwable, отключаем подсветку
                if (hoverdThrowable)
                {
                    hoverdThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}