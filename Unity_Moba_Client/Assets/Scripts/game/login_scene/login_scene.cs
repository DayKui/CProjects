using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class login_scene : MonoBehaviour {

    void on_login_success(string name, object udata) {
        SceneManager.LoadScene("home_scene");
    }

	// Use this for initialization
	void Start () {
        event_manager.Instance.add_event_listener("login_success", this.on_login_success);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy() {
        event_manager.Instance.remove_event_listener("login_success", this.on_login_success);
    }

    public void on_guest_login_click() {
        user_login.Instance.guest_login();
    }
}
