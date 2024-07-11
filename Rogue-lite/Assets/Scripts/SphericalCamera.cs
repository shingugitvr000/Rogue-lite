using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCamera : MonoBehaviour
{
    public Transform target;                        //ī�޶� �ٶ� ���
    public float distance = 10.0f;                  //������κ����� �Ÿ� 
    public float mouseSensitivity = 100.0f;         //���콺 ����
    public float scrollSensitivity = 2.0f;          //��ũ�� ����
    public float minYAngle = 5.0f;                 //�ּ� ���� ����
    public float maxYAngle = 85.0f;                 //�ִ� ���� ���� 

    private float currentHorizontalAngle = 0.0f;        //���� ȸ�� ����
    private float currentVerticalAngle = 4.5f;          //���� ȸ�� ����

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }    
    void Update()
    {
        if(target != null)
        {
            HandleInput();
            UpdateCameraPosition();
        }
    }

    private void HandleInput()
    {
        //���콺 �̵��� ���� ���� ����
        currentHorizontalAngle -= Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;   
        currentVerticalAngle += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minYAngle, maxYAngle);         //�ִ� �ּҰ����� ���� �̵��� �ݴ´�. 

        //��ũ�ѿ� ���� �Ÿ� ����
        distance += -Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        distance = Mathf.Clamp(distance, 2.0f, 100.0f);                             //�Ÿ� ���� 
    }

    private void UpdateCameraPosition()
    {
        //���� ��ǥ�踦 ����� ��ġ ���
        float verticalAngleRadians = currentVerticalAngle * Mathf.Deg2Rad;          //������ ��������� ��ȯ 
        float horizontalAngleRadians = currentHorizontalAngle * Mathf.Deg2Rad;

        float x = distance * Mathf.Sin(verticalAngleRadians) * Mathf.Cos(horizontalAngleRadians);
        float z = distance * Mathf.Sin(verticalAngleRadians) * Mathf.Sin(horizontalAngleRadians);
        float y = distance * Mathf.Cos(verticalAngleRadians);

        Vector3 newPosition = new Vector3(x, y, z);
        newPosition = target.position + newPosition;

        //���� ��ġ���� �� ��ġ�� �̵�
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 6); //���� �ӵ� ���� 
        transform.LookAt(target);
    }
}
