using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator m_Animator;
    //[SerializeField] public Animator m_Animator;
    //  요 코드랑 똑같음
    //  1. [SerializeField] 가 있어야 Inspector 창에 보인다
    //  2. public 인 경우 자동으로 "[SerializeField]" 문구가 붙는다
    //  만약 public 인데 Inspector 에 안보이고 싶다면
    //  [System.NonSerialized] 를 붙여야 한다
    public CharacterController m_Controller;
    public Transform m_CameraTransform;
    public float m_MoveSpeed;           //  캐릭터 이동 속도
    public float m_RotateSpeed;         //  카메라 회전 속도
    public float m_Gravity = 9.81f;
    public float m_JumpHeight = 2.0f;

    public Transform m_AimPoint;
    public GameObject m_BulletPrefab;
    public float m_ShootPower;

    public Transform m_BodyTransform;

    private Vector2 m_RotateAxis;       //  회전을 저장할 축
    private float m_FixRotateX = 0;     //  기존 카메라가 갖고있는 X 각도
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
        //  변수가 트루이면 펄스로, 펄스면 트루로 준다
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

        //  마우스는 Y 축이 반대이기 때문에, - 를 곱해준다
        m_CameraTransform.localRotation = Quaternion.Euler(-m_RotateAxis.y, m_FixRotateX, 0);
        transform.localRotation = Quaternion.Euler(0, m_FixRotateX + m_RotateAxis.x, 0);

        m_BodyTransform.localRotation = Quaternion.Euler(m_RotateAxis.y, 0, 0);
    }

    private void UpdateMove()
    {
        //  Character controller ?
        //  말 그대로 캐릭터를 컨트롤 하는 녀석인데, transform 을 직접적으로 조종하는 것이 아니라
        //  .Move(), .SimpleMove(), isGrounded 같은 함수를 이용해서 움직여야 한다
        //  리지드 바디 없이 동작한다 (있어도 상관 없다)

        //  캐릭터가 땅에 붙었냐 ?
        if (m_Controller.isGrounded) m_GravityTimer = 0.2f;
        if (m_GravityTimer > 0) m_GravityTimer -= Time.deltaTime;

        //  캐릭터가 땅에 있고, 캐릭터가 점프하지 않고 있다면 (수직 낙하 값이 0 보다 작다면)
        if (m_Controller.isGrounded && m_VerticalVelocity < 0)
            m_VerticalVelocity = 0;

        if (Input.GetButtonDown("Jump"))
        {
            //  점프 중인지 아닌지 검사
            if (m_GravityTimer > 0)
            {
                m_GravityTimer = 0;
                m_VerticalVelocity += Mathf.Sqrt(m_JumpHeight * 2.0f * m_Gravity);
            }
        }

        m_VerticalVelocity -= m_Gravity * Time.deltaTime;

        float vertical = Input.GetAxis("Vertical");     //  컨트롤러의 수직 값을 받아온다
        float horizontal = Input.GetAxis("Horizontal");  //  컨트롤러의 수평 값을 받아온다

        //  수직과 수평 값의 방향을 갖기 위해 Vector3 에다 넣어준다
        Vector3 direction = new Vector3(-horizontal, 0, -vertical);
        //  transform.TransformDirection(Vector3)
        //  -> 로컬 공간에서 월드 공간으로의 방향을 준다
        //  로컬 ? 내 자기 자신의 공간
        //  월드 ? 부모를 포함한 공간
        direction = transform.TransformDirection(direction);
        direction *= m_MoveSpeed;

        direction.y = m_VerticalVelocity;
        if (direction.x != 0 && direction.z != 0) m_Animator.SetBool("isMove", true);
        else m_Animator.SetBool("isMove", false);

        m_Controller.Move(direction * Time.deltaTime);
    }

}
