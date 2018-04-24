using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { set; get; }

    public GameObject mainMenu, serverMenu, connectMenu;

    public GameObject serverPrefab, clientPrefab;

    public InputField input, nameInput;

	void Start () {
        Instance = this;
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        DontDestroyOnLoad(gameObject);	
	}

    public void ConnectButton()
    {
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }

    public void HostButton()
    {
        try
        {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Begin();
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.Name = nameInput.text;
            c.isHost = true;
            if (c.Name == "")
            {
                c.Name = "Host";
            }
            c.ConnectToServer("127.0.0.1", 6321);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        mainMenu.SetActive(false);
        serverMenu.SetActive(true);
    }

    public void ConnectToServer()
    {
        string hostAddress = input.text;
        if (hostAddress == "")
        {
            hostAddress = "127.0.0.1";
        }

        try
        {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.Name = nameInput.text;
            if (c.Name == "")
            {
                c.Name = "Client";
            }
            c.ConnectToServer(hostAddress, 6321);
            connectMenu.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);

        Server s = FindObjectOfType<Server>();
        if (s != null)
        {
            Destroy(s.gameObject);
        }

        Client c = FindObjectOfType<Client>();
        if (c != null)
        {
            Destroy(c.gameObject);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
