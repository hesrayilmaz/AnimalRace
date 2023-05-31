using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public enum CollectibleTypes {NoType, Coin, Chest, Type3, Type4, Type5}; // you can replace this with your own labels for the types of collectibles in your game!

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	private CoinManager coinManager;

	private PhotonView pv;

	private GameObject colliderObject;


	// Use this for initialization
	void Start () 
	{
		pv = GetComponent<PhotonView>();
		coinManager = GameObject.Find("CoinManager").GetComponent<CoinManager>();
	}


	// Update is called once per frame
	void Update () 
	{
        if (pv.IsMine)
        {
			if (rotate)
				transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && pv.IsMine) 
		{
			colliderObject = other.gameObject;
			Collect();
		}
		if(other.tag == "AI")
        {
			PhotonNetwork.Destroy(gameObject);
        }
	}

	public void Collect()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		//Below is space to add in your code for what happens based on the collectible type

		if (CollectibleType == CollectibleTypes.NoType) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.Coin) {

			coinManager.IncreaseCoin(1);

			Debug.Log ("Coin");
		}
		if (CollectibleType == CollectibleTypes.Chest) {

			colliderObject.GetComponent<PhotonView>().RPC("RPC_SpeedUp", RpcTarget.All, null);

			Debug.Log ("Chest");
		}
		if (CollectibleType == CollectibleTypes.Type3) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.Type4) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.Type5) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}

		PhotonNetwork.Destroy(gameObject);
	}
}
