using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gprotocol;

public class user_login : Singletom<user_login> {
    private string g_key = null;
    private bool is_save_gkey = false;

    private EditProfileReq temp_req = null;


    void on_edit_profile_return(cmd_msg msg)
    {
        EditProfileRes res = proto_man.protobuf_deserialize<EditProfileRes>(msg.body);
        if (res == null)
        {
            return;
        }

        if (res.status != Respones.OK)
        {
            Debug.Log("edit profle status: " + res.status);
            return;
        }

        ugame.Instance.save_edit_profile(this.temp_req.unick, this.temp_req.uface, this.temp_req.usex);
        this.temp_req = null;

        event_manager.Instance.dispatch_event("sync_uinfo", null);
    }

    void on_guest_login_return(cmd_msg msg) {
        GuestLoginRes res = proto_man.protobuf_deserialize<GuestLoginRes>(msg.body);
        if (res == null){
            return;
        }

        if (res.status != Respones.OK) {
            Debug.Log("Guest Login status: " + res.status);
            return;
        }

        UserCenterInfo uinfo = res.uinfo;
        ugame.Instance.save_uinfo(uinfo, true);

        // 保存一下这个游客的key到我们的本地
        if (this.is_save_gkey) {
            this.is_save_gkey = false;
            PlayerPrefs.SetString("bycw_moba_guest_key", this.g_key);
        }
        // end

        event_manager.Instance.dispatch_event("login_success", null);
        event_manager.Instance.dispatch_event("sync_uinfo", null);
    }

    void on_auth_server_return(cmd_msg msg) {
        switch (msg.ctype) { 
            case (int)Cmd.eGuestLoginRes:
                this.on_guest_login_return(msg);
            break;
            case (int)Cmd.eEditProfileRes:
                this.on_edit_profile_return(msg);
                break;
        }
    }

    public void init() {
        network.Instance.add_service_listener((int)Stype.Auth, this.on_auth_server_return);
    }

    public void guest_login() {
        this.g_key = PlayerPrefs.GetString("bycw_moba_guest_key");
        this.is_save_gkey = false;
        if (string.IsNullOrEmpty(this.g_key) || this.g_key.Length != 32) {
            this.g_key = utils.rand_str(32);
            this.is_save_gkey = true;
        }

        // this.g_key = "Hello";
        GuestLoginReq req = new GuestLoginReq();
        req.guest_key = this.g_key;

        network.Instance.send_protobuf_cmd((int)Stype.Auth, (int)Cmd.eGuestLoginReq, req);
    }


    public void edit_profile(string unick, int uface, int usex)
    {
        if (unick.Length <= 0)
        {
            return;
        }
        if (uface <= 0 || uface > 9)
        {
            return;
        }

        if (usex != 0 && usex != 1)
        {
            return;
        }

        // 提交我们修改资料的请求;
        EditProfileReq req = new EditProfileReq();
        req.unick = unick;
        req.uface = uface;
        req.usex = usex;

        this.temp_req = req;
        network.Instance.send_protobuf_cmd((int)Stype.Auth, (int)Cmd.eEditProfileReq, req);
        // end 
    }
}
