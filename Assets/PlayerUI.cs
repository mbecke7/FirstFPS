using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform healthBarFill;

    [SerializeField]
    GameObject scoreboard;

    [SerializeField]
    Text ammoText;

    private Player player;
    private WeaponManager weaponManager;

    public void SetPlayer(Player _player)
    {
        player = _player;
        weaponManager = player.GetComponent<WeaponManager>();
    }

    private void Update()
    {
        SetHealthAmount(player.GetHealthPct());
        if (!weaponManager.isReloading)
        {
            SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets, weaponManager.GetCurrentWeapon().maxBullets);
        } else
        {
            SetAmmoAmount(0, weaponManager.GetCurrentWeapon().maxBullets);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        } else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R) && !weaponManager.isReloading)
        {
            weaponManager.Reload();
        }
    }

    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(_amount, 1f, 1f);
    }

    void SetAmmoAmount(int _remaining, int _max)
    {
        ammoText.text = _remaining.ToString() + "/" + _max.ToString();
        if (_remaining < _max / 2)
        {
            ammoText.color = Color.red;
        } else
        {
            ammoText.color = Color.white;
        }
        if (_remaining == 0)
        {
            ammoText.text = "Reloading...";
        }
    }
}
