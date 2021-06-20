using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    //����Ƽ �÷ο� ��Ʈ
    //

    // Initialization : �ʱ�ȭ �Լ�
    // �� �Լ����� �ʱ�ȭ �� �� ���� ���ȴ�
    // void Awake()     // Start�Լ� ���� ������ ����ȭ ���� ȣ��
    // void OnEnable()  // ������Ʈ�� Ȱ��ȭ �� �� Awake������ ȣ���
    // void Start       // ��ũ��Ʈ(�ڱ� �ڽ�)�� ȣ��� �� ȣ��

    // Updates : ��� ���ư��� �Լ�
    // void FixedUpdate()   // Update���� ���� ȣ��� �� �ִ�. �������� ���� ���� ȣ��� Ȯ�� ����
    // void Update()        // �����Ӹ��� �� ���� ȣ��
    // void LateUpdate()    // Update�� ���� ���� �����Ӹ��� �� ���� ȣ��

    // Options..
    // void OnDisable()     // ��ũ��Ʈ�� ��Ȱ��ȭ �� �� ȣ��� (OnEnable�� �ݴ�)
    // void OnDestroy()     // ������Ʈ�� �ı��� �� ȣ��� (�� ������ �����ӿ��� ȣ���)

    // C#?
    // class�� ���� ��ũ��Ʈ�� ����?
    // class�� ��ü, ��ũ��Ʈ�� �ڵ�(code)��.

    // ����Ƽ �ν����Ϳ��� ������Ʈ�� ���� �� �ִ� ��ü�� ����� ���
    // 1. �ݵ�� public class �����̾�� ��
    // 2. �ݵ�� MonoBehavior�� ��ӹ޾ƾ� ��
    // 3. class�̸��� ��ũ��Ʈ ���� ���� �����ؾ� ��

    // C#���� ����, �Լ�, Ŭ���� ���� ��
    // C++�� �����
    // public, private, protected, internal ��� ���� ������
    // 1. ������ �տ� �����ڸ� �ٿ��� ��, ������ ������ �ڵ����� private�� �ȴ�.
    //int value = 0; // -> private int value = 0;
    // public int value =0; // -> �տ� public�� �����Ƿ� �ڵ����� ����Ƽ Inspector�� �����ȴ�
    // Inspector�� �����ȴ�? -> Inspector �󿡼� ���� ��ĥ �� �ִ�!
    // �׷��� public class�� ����Ͽ� ����Ƽ�� ������ �϶�� ��.
    // public ���̸� �ڵ����� [SerializeField]��� Attribute(�Ӽ�) Ŭ������ �ٴ´�.
    // �̰� �پ��־�� ����Ƽ�� �����ȴ�!
    // [SerializeField] private int value = 0; -> private �̶� �ص� �տ� �ø�������� �Ӽ� Ŭ������ ������ ���� ��!!

    // ���� �����ڸ� ���� ���� ?
    // A Ŭ���� �ȿ� �� public���� �ϰ� �ٸ� ����� AŬ������ ����Ѵٸ�
    // A Ŭ���� �ȿ� �ִ� �ۺ����� �� ���δ�.
    // 1. �ǵ帮�� ���ƾ� �� �κ��� �ٸ� �����ڰ� �ǵ帱 �� �ִ�
    // 2. ���� �Ǵ� �Լ� �翡 ���� ���ڸ������� ���������ϰ� ��������
    // 3. ��ŷ ������ ���� ����!

    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private Transform m_AimPointTransform;
    public float m_MoveSpeed = 6.0f;
    public float m_RotateSpeed = 1.0f;

    private void Update()
    {
        // Ű ���� �G Ȯ���ϴ� ���
        // File > Build Settings > Input Manager
        float translation = Input.GetAxis("Vertical")*m_MoveSpeed*Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal")*m_RotateSpeed*Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if(Input.GetButtonDown("Fire1"))
        {
            // Instantiate -> Ŭ�� ����
            var go = Instantiate(m_Prefab, transform.position, transform.rotation, null);
            // GetComponent?
            // �ϴ�, transform, gameObject ��� ��������
            // �� ��ũ��Ʈ�� ���� ��ü�� ����̴�
            var rigidbody = go.GetComponent<Rigidbody>();
            rigidbody.AddRelativeForce(Vector3.forward * 1000f);
        }

    }












}
