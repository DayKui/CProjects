using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login_bonues : MonoBehaviour {
    public GameObject[] fingerprint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void show_login_bonues(int days) {
        int i;
        for (i = 0; i < days; i++) {
            this.fingerprint[i].SetActive(true);
        }

        for (; i < 5; i++) {
            this.fingerprint[i].SetActive(false);
        }
    }

    public void on_recv_login_bonues_click() {
        this.gameObject.SetActive(false);
        system_service_proxy.Instance.recv_login_bonues();
    }

    public void on_close_login_bonues() {
        this.gameObject.SetActive(false);
    }
}
