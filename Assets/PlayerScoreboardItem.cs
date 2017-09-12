using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardItem : MonoBehaviour {

    [SerializeField]
    Text playerNameText;
    [SerializeField]
    Text damageText;
    [SerializeField]
    Text killsText;
    [SerializeField]
    Text deathsText;

    public void Setup(string username, float damage, int kills, int deaths) 
    {
        playerNameText.text = username;
        damageText.text = damage.ToString();
        killsText.text = kills.ToString();
        deathsText.text = deaths.ToString();
    }

}
