using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 4f;
    public float impactForce = 50f;
    public Camera RaycastCam;
    public ParticleSystem muzzleFlash;
    public GameObject bulletEffect;
    private float nextTimeToFire = 0f;
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 2f / fireRate;
            Pewpew();
        }
    }

    void Pewpew()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(RaycastCam.transform.position, RaycastCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactObject = Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObject, 2f);
        }
    }
}
