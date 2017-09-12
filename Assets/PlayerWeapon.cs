using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

    public string name = "M4A1";
    public int damage = 10;
    public float range = 600f;
     
    public float fireRate = 4f;

    public int maxBullets = 20;
    [HideInInspector]
    public int bullets;

    public float reloadTime = 2f;

    public GameObject graphics;

    public PlayerWeapon()
    {
        bullets = maxBullets;
    }

}
