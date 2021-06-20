using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator m_Animator;
    //[SerializeField] public Animator m_Animator;
    //  �� �ڵ�� �Ȱ���
    //  1. [SerializeField] �� �־�� Inspector â�� ���δ�
    //  2. public �� ��� �ڵ����� "[SerializeField]" ������ �ٴ´�
    //  ���� public �ε� Inspector �� �Ⱥ��̰� �ʹٸ�
    //  [System.NonSerialized] �� �ٿ��� �Ѵ�
    public CharacterController m_Controller;
    public Transform m_CameraTransform;
    public float m_MoveSpeed;           //  ĳ���� �̵� �ӵ�
    public float m_RotateSpeed;         //  ī�޶� ȸ�� �ӵ�
    public float m_Gravity = 9.81f;
    public float m_JumpHeight = 2.0f;

    public Transform m_AimPoint;
    public GameObject m_BulletPrefab;
    public float m_ShootPower;

    public Transform m_BodyTransform;

    private Vector2 m_RotateAxis;       //  ȸ���� ������ ��
    private float m_FixRotateX = 0;     //  ���� ī�޶� �����ִ� X ����
    private float m_GravityTimer = 0;
    private float m_VerticalVelocity = 0;
    private bool m_IsEnableCursor = false;

    private void Awake()
    {
        Vector3 euler = m_CameraTransform.eulerAngles;

        m_RotateAxis = new Vector2(euler.y, euler.x);
        m_FixRotateX = m_RotateAxis.x;

        EnableCursor(m_IsEnableCursor);
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject go = Instantiate(m_BulletPrefab, m_AimPoint.position, m_AimPoint.rotation, null);
            go.GetComponent<Bullet>().Shoot(m_ShootPower);
        }

        //  !m_IsEnableCursor
        //  ������ Ʈ���̸� �޽���, �޽��� Ʈ��� �ش�
        if (Input.GetKeyDown(KeyCode.Escape))
            EnableCursor(!m_IsEnableCursor);
    }

    private void EnableCursor(bool isEnable)
    {
        Cursor.lockState = isEnable ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isEnable;

        m_IsEnableCursor = isEnable;
    }

    private void UpdateRotate()
    {
        m_RotateAxis.x += Input.GetAxis("Mouse X") * m_RotateSpeed;
        m_RotateAxis.y += Input.GetAxis("Mouse Y") * m_RotateSpeed;

        //  ���콺�� Y ���� �ݴ��̱� ������, - �� �����ش�
        m_CameraTransform.localRotation = Quaternion.Euler(-m_RotateAxis.y, m_FixRotateX, 0);
        transform.localRotation = Quaternion.Euler(0, m_FixRotateX + m_RotateAxis.x, 0);

        m_BodyTransform.localRotation = Quaternion.Euler(m_RotateAxis.y, 0, 0);
    }

    private void UpdateMove()
    {
        //  Character controller ?
        //  �� �״�� ĳ���͸� ��Ʈ�� �ϴ� �༮�ε�, transform �� ���������� �����ϴ� ���� �ƴ϶�
        //  .Move(), .SimpleMove(), isGrounded ���� �Լ��� �̿��ؼ� �������� �Ѵ�
        //  ������ �ٵ� ���� �����Ѵ� (�־ ��� ����)

        //  ĳ���Ͱ� ���� �پ��� ?
        if (m_Controller.isGrounded) m_GravityTimer = 0.2f;
        if (m_GravityTimer > 0) m_GravityTimer -= Time.deltaTime;

        //  ĳ���Ͱ� ���� �ְ�, ĳ���Ͱ� �������� �ʰ� �ִٸ� (���� ���� ���� 0 ���� �۴ٸ�)
        if (m_Controller.isGrounded && m_VerticalVelocity < 0)
            m_VerticalVelocity = 0;

        if (Input.GetButtonDown("Jump"))
        {
            //  ���� ������ �ƴ��� �˻�
            if (m_GravityTimer > 0)
            {
                m_GravityTimer = 0;
                m_VerticalVelocity += Mathf.Sqrt(m_JumpHeight * 2.0f * m_Gravity);
            }
        }

        m_VerticalVelocity -= m_Gravity * Time.deltaTime;

        float vertical = Input.GetAxis("Vertical");     //  ��Ʈ�ѷ��� ���� ���� �޾ƿ´�
        float horizontal = Input.GetAxis("Horizontal");  //  ��Ʈ�ѷ��� ���� ���� �޾ƿ´�

        //  ������ ���� ���� ������ ���� ���� Vector3 ���� �־��ش�
        Vector3 direction = new Vector3(-horizontal, 0, -vertical);
        //  transform.TransformDirection(Vector3)
        //  -> ���� �������� ���� ���������� ������ �ش�
        //  ���� ? �� �ڱ� �ڽ��� ����
        //  ���� ? �θ� ������ ����
        direction = transform.TransformDirection(direction);
        direction *= m_MoveSpeed;

        direction.y = m_VerticalVelocity;
        if (direction.x != 0 && direction.z != 0) m_Animator.SetBool("isMove", true);
        else m_Animator.SetBool("isMove", false);

        m_Controller.Move(direction * Time.deltaTime);
    }

}
