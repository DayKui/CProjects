using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_manager : UnitySingletom<game_manager> {

	// Use this for initialization
	void Start () {
        event_manager.Instance.init();
        ulevel.Instance.init();
        auth_service_proxy.Instance.init();
        system_service_proxy.Instance.init();
        logic_service_proxy.Instance.init();
    }
	
}
