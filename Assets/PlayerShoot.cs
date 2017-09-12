using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour {


    private PlayerWeapon currentWeapon;
    
    public float bulletSpeed = 18f;
    public float bulletDelay = .2f;

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>(); 
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        } else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            } else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    // Called on the server when player shoots.
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    // Called on all clients for shooting effect.
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    // Called on the server when player hits something.
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal) 
    {
        RpcDoHitEffect(_pos, _normal);
    }

    // Called on server when something is hit.
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (weaponManager.isReloading)
        {
            return;
        }
        currentWeapon.bullets--;

        // We are shooting, call OnShoot on the server.
        CmdOnShoot();

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage, this.name);
            }

            // We hit something, call on hit on the server.
            CmdOnHit(_hit.point, _hit.normal);
        }

        if (currentWeapon.bullets <= 0)
        {
            weaponManager.Reload();
            return;
        }
    }

    [Command]
    void CmdPlayerShot(string _victimID, int _damage, string _sourceID)
    {
        Debug.Log(_victimID + " has been shot.");
        Player _player = GameManager.GetPlayer(_victimID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }
}
