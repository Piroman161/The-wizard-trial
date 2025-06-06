using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting; // Возможно, не нужен, если не используете визуальное скриптование
using UnityEngine;

// Класс, отвечающий за поведение оружия
public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon; // Указывает, активно ли оружие у игрока
    public int weaponDamage; // Урон, наносимый при попадании

    public Camera playerCamera; // Камера игрока для определения направления стрельбы

    public bool isShooting, readyToShoot; // Флаги, управляющие состоянием стрельбы
    bool allowReset = true; // Для контроля задержки между выстрелами
    public float shootingDelay = 2f; // Задержка между выстрелами (особенно для автоматического режима)

    public int bulletsPerBurst = 3; // Количество выстрелов в серии (burst)
    public int burstBulletsLeft; // Остаток выстрелов в текущей серии

    public float spreadIntensity; // Величина разброса при стрельбе

    public GameObject bulletPrefab; // Префаб пули
    public Transform bulletSpawn; // Точка, откуда вылетают пули
    public float bulletVelocity = 50; // Скорость пули
    public float bulletPrefabLifeTime = 3f; // Время жизни пули до уничтожения

    public float reloadTime; // Время перезарядки
    public int magazineSize, bulletsLeft; // Размер магазина и текущие патроны
    public bool isReloading; // Флаг, перезаряжается ли оружие

    // Перечисление моделей оружия
    public enum WeaponModel
    {
        Red_wand,
        Blue_wand
    }
    public WeaponModel thisWeaponModel; // Текущая модель оружия

    public Vector3 spawnPosition; // Позиция спавна (может быть для анимаций или других целей)
    public Vector3 spawnRotation; // Вращение при спавне

    // Перечисление режимов стрельбы
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode; // Текущий режим

    private void Awake()
    {
        // Инициализация переменных
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            // Устанавливаем слой для рендеринга оружия
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }

            // Обработка режима стрельбы
            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0); // Автоматический режим: держим кнопку
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0); // Одноразовый или серия: по нажатию
            }

            // Обработка перезарядки
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
            {
                Reload();
            }

            // Автоматическая перезарядка при пустом магазине
            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                Reload();
            }

            // Если можно стрелять и есть патроны
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst; // Обнуляем серию
                FireWeapon(); // Вызываем стрельбу
            }
        }
        else
        {
            // Если оружие не активное, сбрасываем слой
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--; // Уменьшаем патроны
        readyToShoot = false; // Устанавливаем флаг, что сейчас стреляем

        // Рассчитываем направление стрельбы с учетом разброса
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // Создаем пулю
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage; // Передаем урон пули

        // Направляем пулю
        bullet.transform.forward = shootingDirection;

        // Применяем силу к пуле для движения
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Уничтожаем пулю через заданное время
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Контроль задержки между выстрелами
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Если режим серия (Burst) и есть еще выстрелы в серии
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay); // Следующий выстрел через задержку
        }
    }

    private void Reload()
    {
        isReloading = true; // Начинаем перезарядку
        Invoke("ReloadCompleted", reloadTime); // Завершение через время
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize; // Восстанавливаем магазин
        isReloading = false; // Закончили перезарядку
    }

    private void ResetShot()
    {
        readyToShoot = true; // Можно стрелять снова
        allowReset = true;
    }

    // Расчет направления выстрела с учетом разброса
    public Vector3 CalculateDirectionAndSpread()
    {
        // Луч идет из центра экрана
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // Точка попадания, если есть объект
        }
        else
        {
            targetPoint = ray.GetPoint(100); // Дальняя точка, если ничего не попало
        }

        Vector3 direction = targetPoint - bulletSpawn.position; // Направление к цели

        // Добавляем разброс по X и Y
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    // Корутин для уничтожения пули через время
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}