using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] Transform cam;
    [SerializeField] Transform muzzle;
    [SerializeField] Rigidbody playerRB;

    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
        }
    }
    private IEnumerator Reload()
    {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    public void Shoot()
    {
        //Debug.Log("Shoot Gun!");
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                //Cambiar segun sea necesario el origen y direccion del disparo
                //Calculos con la camara y Efectos con el muzzle
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    print(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);

                    //hit.rigidbody.AddForce(ray.direction * hitForce);
                    //rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                    playerRB.AddForce(-cam.forward * gunData.nockback, ForceMode.Impulse);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                onGunShot();

            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance, Color.blue);
        Debug.DrawRay(muzzle.position, cam.forward * gunData.maxDistance, Color.red);
        
    }

    private void onGunShot()
    {
    }
}
