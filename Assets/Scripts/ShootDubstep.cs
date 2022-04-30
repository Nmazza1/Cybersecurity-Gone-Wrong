using UnityEngine;
using System.Collections;

public class ShootDubstep : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0f;
    public float impactForce = 50f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera RaycastCam;
    public ParticleSystem muzzleFlash;
    public GameObject bulletEffect;
    public Animator animator;
    private float nextTimeToFire = 0f;


    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            // if fire rate is 4, divide 1/4 which means bullets will shoot every 0.25 seconds
            nextTimeToFire = Time.time + 1f / fireRate;
            Pewpew();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Pewpew()
    {
        muzzleFlash.Play();

        currentAmmo--;

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
        }
        
        GameObject impactObject = Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactObject, 1f);
        
    }
}
