using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveTut : MonoBehaviour
{
    #region Variebels

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject impactPrefab;
    
    [SerializeField]
    private List<GameObject> trails;
    
    private Rigidbody _rigidbody;

    #endregion
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (speed != 0 && _rigidbody != null)
        {
            _rigidbody.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;

        ContactPoint contact = collision.contacts[0];

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);

        Vector3 position = contact.point;
        
        if (impactPrefab != null)
        {
            Instantiate(impactPrefab, position, rotation);
        }

        TrailsChecker();
        
        Destroy(gameObject);
    }


    private void TrailsChecker()
    {
        if (trails.Count > 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].transform.parent = null;
                var ps = trails[i].GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Stop();
                    Destroy(ps.gameObject,ps.main.duration + ps.main.startLifetime.constantMax);
                }
                
            }
        }
    }
}
