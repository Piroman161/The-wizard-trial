using UnityEngine;

public class Mouse : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    private void Start()
    {
        // Лочим курсор на месте
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Реагирование на нажатие
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Исправлено

        // Вертеть камерой вверх и вниз
        xRotation -= mouseY;

        // Фиксируем вращение X
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Вертеть камерой влево и вправо
        yRotation += mouseX; // Исправлено

        // Преобразуем повороты
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
