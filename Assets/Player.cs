using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead {
        get {
            return _isDead;
        }
        protected set {
            _isDead = value;
        }
    }

    [SyncVar]
    public string username;

    public int kills;
    public int deaths;
    [SyncVar]
    public float damage;

    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;

    public float GetHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++ )
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    [Command]
    public void CmdUpdateUsername(string _username)
    {
        RpcUpdateUsername(_username);
    }

    [ClientRpc]
    void RpcUpdateUsername(string _username)
    {
        username = _username;
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourceID)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= _amount;
        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        sourcePlayer.damage += _amount;

        if (currentHealth <= 0)
        {
            Die();
            deaths++;
            sourcePlayer.kills++;
            GameManager.instance.onPlayerKilledCallback.Invoke(this.username, sourcePlayer.username);
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }

        // Call Respawn Method.
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }
    }
}
