using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody m_Rigidbody;

    public void Shoot(float power)
    {
        m_Rigidbody.AddRelativeForce(Vector3.forward * m_Rigidbody.mass * power);
    }
}
