using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;              //RIgidbody �� ������� �ʴ� �浹ü ĳ���� ��Ʈ�� ��
    public Animator animator;

    //�⺻ ���� ����
    public float speed = 5.0f;
    public float runSpeed = 10.0f;
    public float acceleration = 10.0f;
    public float currentSpeed = 0.0f;
    //������ ����
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    //��� ���� ����
    public float dashSpeed = 20.0f;
    public float dashTime = 2.0f;
    public float dashCooldown = 5.0f;

    private float dashCounter;
    private float dashTimer;
    private bool isDashing = false;
    //�׼� ���� 
    public bool isAction = false;
    //�ӵ� �� ���� �� 
    private Vector3 velocity;
    private bool canJumpAgain = true;
    public Transform cam;
    Vector3 moveDir;

    public ActionData[] actionDataList = new ActionData[10];
    public ComboSystem comboSystem;
    public Transform fxTransform;

    //�׼� ���� ����
    private float actionTimer = 0f;
    private ActionData currentAction = null;
    private GameObject currentFx = null;


    void Update()
    {
        bool isGrounded = controller.isGrounded;                //CharacterController���� ���� �ִ��� �Ǻ� ���ش�. 

        if(!isAction)
        {
            //�׼� �Լ����� �����. 
            MoveLogic(isGrounded);
            JumpLogic(isGrounded);
        }
        else
        {
            UpdateAction();
        }

        //�߷� ����
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void MoveLogic(bool isGrounded)
    {
        if(isGrounded)                              //�׶��� ������ �Ǿ��� �� 
        {
            velocity.y = -2f;
            canJumpAgain = true;
            animator.SetBool("IsFalling", false);       //������ false
            animator.SetBool("IsLanding", true);        //���� True
        }  
        else
        {
            if(velocity.y < 0)                              //y�� �ӵ����� ���� �Ǵ��Ѵ�.
            {
                animator.SetBool("IsFalling", true);        //�����̹Ƿ� �������� ��
            }
            else
            {
                animator.SetBool("IsFalling", false);     
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal");      //�⺻ �̵�Ű �Է� �� 
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;   //�̵����� 0 ~ 1 ���̷� ������ش�. 

        animator.SetFloat("moveSpeed", Mathf.Lerp(0, 1, currentSpeed / speed));

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;      //ī�޶� ���� ������ ������ �ǰ� �Ѵ�. 
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleration * Time.deltaTime);

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0.0f;
        }

    }

    public void JumpLogic(bool isGrounded)                                      //���� �Լ�
    {
        if(Input.GetKeyDown(KeyCode.Space))                                     //�����̽��� ��������
        {
            if(isGrounded)                                                      //���� ���� �� 
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);            //���� �ִ� ���� 
            }
            else if(canJumpAgain)                                               //2�� ���� ����� ���� ��� 
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);            //���� �ִ� ���� 
                canJumpAgain = false;
            }
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
    }

    void UpdateAction()         
    {
        if (currentAction == null) return;                  //�׼��� ���ٸ� ���� 
        actionTimer += Time.deltaTime;

        if(actionTimer >= currentAction.fxTime && currentFx == null)        //VFX ���� �ð��� �Ǹ� 
        {
            SpawnFx();
        }

        if(actionTimer >= currentAction.waitTime)                           //��ٸ��� �ð����� �� �׼��� ���� ��Ų��.
        {
            EndAction();
        }
    }

    void SpawnFx()                                          //VFX ���� �Լ�
    {
        if(currentAction.fxObject != null)                  //VFX�� NULL ���� �ƴҋ�
        {
            currentFx = Instantiate(currentAction.fxObject, fxTransform);       //������ VFX ��ġ�� ȸ������ �����ͼ� �����Ѵ�. 
            currentFx.transform.localPosition = Vector3.zero;
            currentFx.transform.localRotation = Quaternion.identity;    
        }
    }

    void EndAction()                        //�׼� ����� ���� �ִ°͵� �� �ʱ�ȭ 
    {
        isAction = false;
        if(currentFx != null)
        {
            Destroy(currentFx);
        }
        currentAction = null;
        currentFx = null;
    }
}
