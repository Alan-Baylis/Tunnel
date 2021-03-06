﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ExtraWeaponController : NetworkBehaviour {

	public ParticleSystem grenadeExplosion;

	public CustomNetworkManager networkManager; // dont initialize

	public override void OnStartLocalPlayer() {
		networkManager = gameObject.GetComponent<CustomNetworkManager> ();
	}


	//GRENADE PARTICLES

	[Command]
	public void CmdGrenadeParticles(Vector3 pos, Vector3 dir) {
		networkManager.SendGrenadeParticleInfo (pos, dir);
	}

	[ClientRpc]
	public void RpcGrenadeParticles(Vector3 pos, Vector3 dir) {
		if (!isLocalPlayer)
			return;

		Instantiate (grenadeExplosion, pos, Quaternion.FromToRotation(Vector3.forward, dir));
		DestroyGrenadeParticles ();
	}

	private void DestroyGrenadeParticles() {
		GameObject[] grenadeParticleSystems = GameObject.FindGameObjectsWithTag ("GrenadeParticles");
		foreach (GameObject gps in grenadeParticleSystems) {
			Destroy (gps, gps.GetComponent<ParticleSystem>().startLifetime);
		}
	}


	//JETPACK PARTICLES

	[Command]
	public void CmdJetpackParticles(NetworkInstanceId id) {
		networkManager.SendJetpackParticleID (id);
	}

	[ClientRpc]
	public void RpcJetpackParticles(NetworkInstanceId id) {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			if (player.GetComponent<NetworkIdentity>().netId == id) {
				player.transform.FindChild ("JetpackTrail").gameObject.GetComponent<ParticleSystem>().Play();
			}
		}
	}

	[Command]
	public void CmdEndJetpackParticles(NetworkInstanceId id) {
		networkManager.SendEndJetpackParticleID (id);
	}

	[ClientRpc]
	public void RpcEndJetpackParticles(NetworkInstanceId id) {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			if (player.GetComponent<NetworkIdentity>().netId == id) {
				player.transform.FindChild ("JetpackTrail").gameObject.GetComponent<ParticleSystem>().Stop();
			}
		}
	}

	//INVISIBLTIY

	[Command]
	public void CmdInvisiblity(NetworkInstanceId id, bool isInvisible) {
		networkManager.SendInvisibilityID (id, isInvisible);
	}

	[ClientRpc]
	public void RpcInvisibility(NetworkInstanceId id, bool isInvisible) {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			if (player.GetComponent<NetworkIdentity>().netId == id) {
				if (isInvisible) {
					player.GetComponent<Renderer> ().enabled = false;
					Transform temp = player.GetComponent<WeaponController> ().weaponHolder;
					player.GetComponent<WeaponController> ().weaponHolder.localPosition = new Vector3(temp.position.x, -1000, temp.position.z);
				} else {
					player.GetComponent<Renderer> ().enabled = true;
					player.GetComponent<WeaponController> ().weaponHolder.localPosition = new Vector3(0.341f, -0.3709f, 0.75f);
				}
			}
		}
	}

	[Command]
	public void CmdPlayerColor(float r, float g, float b, NetworkInstanceId id) {
		networkManager.SendColor (r, g, b, id);
	}

	[ClientRpc]
	public void RpcPlayerColor(float r, float g, float b, NetworkInstanceId id) {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			if (player.GetComponent<NetworkIdentity>().netId == id) {
				player.GetComponent<Renderer> ().material.color = new Color (r, g, b);
			}
		}
	}

}
