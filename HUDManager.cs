using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Для работы с текстом в UI (TextMeshPro)
using UnityEngine.UI; // Для работы с UI-элементами (изображения)
using System;

// Класс управления интерфейсом HUD (Head-Up Display)
public class HUDManager : MonoBehaviour
{
    // Реализуем паттерн Singleton для глобального доступа к HUDManager
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;   // Текст для отображения патронов в магазине
    public TextMeshProUGUI totalAmmoUI;      // Текст для отображения общего количества патронов
    public Image ammoTypeUI;                   // Иконка типа боеприпасов

    [Header("Weapon")]
    public Image activeWeaponUI;               // Иконка текущего активного оружия
    public Image unActiveWeaponUI;             // Иконка неактивного оружия

    [Header("Throwables")]
    public Image lethalUI;                     // Иконка гранаты или другого lethal throwable
    public TextMeshProUGUI lethalAmountUI;     // Количество гранат/гранатометов

    public Image tacticalUI;                   // Иконка тактического выбрасываемого устройства
    public TextMeshProUGUI tactticalAmountUI; // Количество тактических устройств

    public Sprite emptySlot;                   // Спрайт для пустого слота

    private void Awake()
    {
        // Реализация Singleton: если уже есть экземпляр, уничтожаем текущий объект
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; // Назначаем текущий объект синглтоном
        }
    }

    private void Update()
    {
        // Получаем активное оружие из менеджера оружия
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        // Получаем неактивное оружие (второй слот)
        Weapon unActiveWeapon = GetnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            // Обновляем отображение количества патронов
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";

            // Получаем модель оружия для определения спрайтов
            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            // Обновляем иконку типа боеприпасов
            ammoTypeUI.sprite = GetAmmoSprite(model);
            // Обновляем иконку активного оружия
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                // Обновляем иконку неактивного оружия
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            // Если оружие не выбрано, очищаем UI
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }
    }

    // Получение спрайта оружия по модели
    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Red_wand:
                return (Resources.Load<GameObject>("Red_wand_Weapon")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Blue_wand:
                return (Resources.Load<GameObject>("Blue_wand_weapon")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    // Получение спрайта боеприпасов по модели оружия
    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Red_wand:
                return (Resources.Load<GameObject>("Red_wand_Ammo")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Blue_wand:
                return (Resources.Load<GameObject>("Blue_wand_Ammo")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    // Метод для получения неактивного слота оружия
    private GameObject GetnActiveWeaponSlot()
    {
        // Проходим по слотам оружия
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            // Возвращаем первый, который не является активным
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null; // Если нет другого, возвращаем null
    }

    // Обновление UI для бросаемых предметов (гранат, тактических устройств)
    internal void UpdateThrowables(Throwable.ThrowableType throwable)
    {
        switch (throwable)
        {
            case Throwable.ThrowableType.Grenade:
                // Обновляем количество гранат и иконку
                lethalAmountUI.text = $"{WeaponManager.Instance.grenades}";
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
                // Можно добавить обработку других типов бросаемых предметов
        }
    }
}