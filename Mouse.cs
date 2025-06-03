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
        // ����� ������ �� �����
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // ������������ �� �������
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // ����������

        // ������� ������� ����� � ����
        xRotation -= mouseY;

        // ��������� �������� X
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // ������� ������� ����� � ������
        yRotation += mouseX; // ����������

        // ����������� ��������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
