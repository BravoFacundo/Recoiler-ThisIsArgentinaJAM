using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapon/Gun" )]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    //Quizas public new float peso; //Para reducir el momentum del jugador

    [Header("Shooting")]
    public float damage; //Quizas ProjectileDamage* 
    public float maxDistance;
    //Agregar luego:
    //cantidad de proyectiles
    //dispercion 


    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public float nockback;
    [HideInInspector] public bool reloading;


}
