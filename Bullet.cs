using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Скрипт управляет поведением пули при столкновениях
public class Bullet : MonoBehaviour
{
    public int bulletDamage; // Урон, наносимый при попадании

    // Этот метод вызывается при столкновении пули с другим объектом
    private void OnCollisionEnter(Collision ObjectWeHit)
    {
        // Если объект, с которым столкнулись, имеет тег "Target"
        if (ObjectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit " + ObjectWeHit.gameObject.name + " !");
            CreateBulletImpactEffect(ObjectWeHit); // Создаем эффект попадания
            Destroy(gameObject); // Уничтожаем пулю
        }
        // Если объект — стена (таг "Wall")
        if (ObjectWeHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall ");
            CreateBulletImpactEffect(ObjectWeHit); // Создаем эффект попадания
            Destroy(gameObject); // Уничтожаем пулю
        }
        // Если объект — враг (таг "Enemy")
        if (ObjectWeHit.gameObject.CompareTag("Enemy"))
        {
            // Вызываем метод получения урона у врага
            ObjectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            Destroy(gameObject); // Уничтожаем пулю
        }
    }

    // Метод создает эффект попадания (например, дырку или искры)
    void CreateBulletImpactEffect(Collision ObjectWeHit)
    {
        // Берем точку контакта
        ContactPoint contact = ObjectWeHit.contacts[0];

        // Создаем эффект в месте контакта, ориентированный по нормали поверхности
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab, // Префаб эффекта
            contact.point, // Место появления
            Quaternion.LookRotation(contact.normal) // Ориентация по нормали поверхности
        );

        // Назначаем эффект как дочерний объект к объекту, с которым произошло столкновение
        hole.transform.SetParent(ObjectWeHit.gameObject.transform);
    }
}