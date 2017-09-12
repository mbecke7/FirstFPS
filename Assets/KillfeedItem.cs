using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillfeedItem : MonoBehaviour {

    [SerializeField]
    Text text;

    public void Setup (string player, string source)
    {
        text.text = "<color=green>" + source + "</color> killed <color=red>" + player + "</color>";
    }
}
