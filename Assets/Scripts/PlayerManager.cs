using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Color[] playerColors;
    public GameObject playerPrefab;
    public List<PlayerController> players = new List<PlayerController>();

    public Transform[] spawnPoints;

    public GameObject deathEffectPrefab;

    public GameObject playerContainerPrefab;
    public Transform playerContainerParent;

    // singleton
    public static PlayerManager instance;

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        //ilk oyuncu için wasd ve bosluk kullanılacak 
        CreatePlayer("WASD", Keyboard.current);

        //ikinci oyuncu için sağ sol tusunu kullanacak
        CreatePlayer("ArrowKeys", Keyboard.current);
    }

    // yeni oyuncu
    void CreatePlayer (string controlScheme, InputDevice inputDevice)
    {
        Vector3 spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // renk değişir 
        playerObj.GetComponentInChildren<SpriteRenderer>().color = playerColors[players.Count];

        playerObj.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme, inputDevice);

        // UI
        PlayerContainerUI containerUI = Instantiate(playerContainerPrefab, playerContainerParent).GetComponent<PlayerContainerUI>();
        playerObj.GetComponent<PlayerController>().containerUI = containerUI;
        containerUI.Initialize(playerColors[players.Count]);

        players.Add(playerObj.GetComponent<PlayerController>());
    }

    // can 0 landıgında cagırılan fonksiyon
    public void OnPlayerDeath (PlayerController player, PlayerController attacker)
    {

        Destroy(Instantiate(deathEffectPrefab, player.transform.position, Quaternion.identity), 1.5f);

        // puan arttırır
        if(attacker != null)
        {
            attacker.score++;

            // UI
            attacker.containerUI.UpdateScoreText(attacker.score);
        }

        // yeniden dogus
        player.Spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].position);
    }
}