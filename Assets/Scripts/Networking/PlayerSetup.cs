﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField] //Disable the components of another player
    Behaviour[] componentsToDisable = null;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    Camera SceneCamera;

    void Start()
    {
		if (isServer)
			return;

		DisableComponents ();

        if(!isLocalPlayer)
        {
            //DisableComponents();
            
            //(temporary)for now, just set all the players in one layer
            //AssignRemoteLayer();
        }
        else
        {
            SceneCamera = Camera.main;
			gameObject.layer = 8;
            if (SceneCamera != null)
            {
                SceneCamera.gameObject.SetActive(false);
            }
        }
        RegisterPlayer();
        
    }

    void RegisterPlayer()
    {
        string _ID = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

	public void EnableComponents()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = true;
		}
	}

    void OnDisable()
    {
        if (SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }
    }
}
