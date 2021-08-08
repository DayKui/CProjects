using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gprotocol;

public class auth_service_proxy : Singletom<auth_service_proxy>
{
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

    void on_guest_account_upgrade_return(cmd_msg msg)
    {
        AccountUpgradeRes res = proto_man.protobuf_deserialize<AccountUpgradeRes>(msg.body);
        if (res.status == Respones.OK)
        {
            ugame.Instance.is_guest = false;
        }
        event_manager.Instance.dispatch_event("upgrade_account_return", res.status);
        // 本地保存的游客的key,给它换掉
        PlayerPrefs.SetString("bycw_moba_guest_key", "");
    }

    void on_uname_login_return(cmd_msg msg)
    {
        UnameLoginRes res = proto_man.protobuf_deserialize<UnameLoginRes>(msg.body);
        if (res == null)
        {
            return;
        }

        if (res.status != Respones.OK)
        {
            Debug.Log("Uname Login status: " + res.status);
            return;
        }

        UserCenterInfo uinfo = res.uinfo;
        ugame.Instance.save_uinfo(uinfo, false);

        event_manager.Instance.dispatch_event("login_success", null);
        event_manager.Instance.dispatch_event("sync_uinfo", null);
    }

    void on_guest_login_return(cmd_msg msg)
    {
        GuestLoginRes res = proto_man.protobuf_deserialize<GuestLoginRes>(msg.body);
        if (res == null)
        {
            return;
        }

        if (res.status != Respones.OK)
        {
            Debug.Log("Guest Login status: " + res.status);
            return;
        }

        UserCenterInfo uinfo = res.uinfo;
        ugame.Instance.save_uinfo(uinfo, true, this.g_key);

        // 保存一下这个游客的key到我们的本地
        if (this.is_save_gkey)
        {
            this.is_save_gkey = false;
            PlayerPrefs.SetString("bycw_moba_guest_key", this.g_key);
        }
        // end

        event_manager.Instance.dispatch_event("login_success", null);
        event_manager.Instance.dispatch_event("sync_uinfo", null);
    }

    void on_user_loginout_return(cmd_msg msg)
    {
        LoginOutRes res = proto_man.protobuf_deserialize<LoginOutRes>(msg.body);
        if (res == null)
        {
            return;
        }
        if (res.status != Respones.OK)
        {
            Debug.Log("Guest Login status: " + res.status);
            return;
        }

        // 注销成功了
        ugame.Instance.user_login_out();
        // end 

        event_manager.Instance.dispatch_event("login_out", null);
    }

    void on_auth_server_return(cmd_msg msg)
    {
        switch (msg.ctype)
        {
            case (int)Cmd.eGuestLoginRes:
                this.on_guest_login_return(msg);
                break;
            case (int)Cmd.eEditProfileRes:
                this.on_edit_profile_return(msg);
                break;

            case (int)Cmd.eAccountUpgradeRes:
                this.on_guest_account_upgrade_return(msg);
                break;

            case (int)Cmd.eUnameLoginRes:
                this.on_uname_login_return(msg);
                break;

            case (int)Cmd.eLoginOutRes:
                this.on_user_loginout_return(msg);
                break;
        }
    }

    public void init()
    {
        network.Instance.add_service_listener((int)Stype.Auth, this.on_auth_server_return);
    }

    public void guest_login()
    {
        this.g_key = null;// PlayerPrefs.GetString("bycw_moba_guest_key");
        this.is_save_gkey = false;
        if (this.g_key == null || this.g_key.Length != 32)
        {
            this.g_key = utils.rand_str(32);
            // this.g_key = "8JvrDstUNDuTNnnCKFEw4pKFs27z9xSr";
            this.is_save_gkey = true;
        }

        // this.g_key = "Hello";
        GuestLoginReq req = new GuestLoginReq();
        req.guest_key = this.g_key;

        network.Instance.send_protobuf_cmd((int)Stype.Auth, (int)Cmd.eGuestLoginReq, req);
    }

    public void uname_login(string uname, string upwd)
    {
        string upwd_md5 = utils.md5(upwd);

        Debug.Log(uname + " " + upwd_md5);

        UnameLoginReq req = new UnameLoginReq();
        req.uname = uname;
        req.upwd = upwd_md5;

        network.Instance.send_protobuf_cmd((int)Stype.Auth, (int)Cmd.eUnameLoginReq, req);
    }

    public void do_account_upgrade(string uname, string upwd_md5)
    {
        AccountUpgradeReq req = new AccountUpgradeReq();
        req.uname = uname;
        req.upwd_md5 = upwd_md5;

        network.Instance.send_protobuf_cmd((int)Stype.Auth, (int)Cmd.eAccountUpgradeReq, req);
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

    public void user_login_out()
    {
        network.Instance.send_protobuf_cmd((int)Stype.Auth, (int)Cmd.eLoginOutReq, null);
    }
}
