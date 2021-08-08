using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gprotocol;

public class system_service_proxy : Singletom<system_service_proxy>
{
    private int ver_num = 0;
    private List<string> sys_msgs = null;

    void on_get_ugame_info_return(cmd_msg msg)
    {
        GetUgameInfoRes res = proto_man.protobuf_deserialize<GetUgameInfoRes>(msg.body);
        if (res == null)
        {
            return;
        }
        if (res.status != Respones.OK)
        {
            Debug.Log("get ugame info status: " + res.status);
            return;
        }

        UserGameInfo uinfo = res.uinfo;
        ugame.Instance.save_ugame_info(uinfo);

        event_manager.Instance.dispatch_event("get_ugame_info_success", null);
        event_manager.Instance.dispatch_event("sync_ugame_info", null);
    }

    void on_recv_login_bonues_return(cmd_msg msg)
    {
        RecvLoginBonuesRes res = proto_man.protobuf_deserialize<RecvLoginBonuesRes>(msg.body);
        if (res == null)
        {
            return;
        }
        if (res.status != Respones.OK)
        {
            Debug.Log("recv login bonues status: " + res.status);
            return;
        }

        ugame.Instance.ugame_info.uchip += ugame.Instance.ugame_info.bonues;
        ugame.Instance.ugame_info.bonues_status = 1;

        event_manager.Instance.dispatch_event("sync_ugame_info", null);
    }

    void on_get_world_uchip_rank_info_return(cmd_msg msg)
    {
        GetWorldRankUchipRes res = proto_man.protobuf_deserialize<GetWorldRankUchipRes>(msg.body);
        if (res == null)
        {
            return;
        }

        if (res.status != Respones.OK)
        {
            Debug.Log("get_world_uchip_rank_info status: " + res.status);
            return;
        }

        event_manager.Instance.dispatch_event("get_rank_list", res.rank_info);
    }


    void on_get_sys_msg_return(cmd_msg msg)
    {
        GetSysMsgRes res = proto_man.protobuf_deserialize<GetSysMsgRes>(msg.body);
        if (res == null)
        {
            return;
        }

        if (res.status != Respones.OK)
        {
            Debug.Log("get system msg status: " + res.status);
            return;
        }

        Debug.Log("get system msg success");
        if (this.ver_num == res.ver_num)
        { // 本地和服务器的一样，使用本地的数据;
            Debug.Log("the use local data");
        }
        else
        {
            this.ver_num = res.ver_num;
            this.sys_msgs = res.sys_msgs;
            Debug.Log("sync server data");
        }

        if (this.sys_msgs != null)
        {
            for (int i = 0; i < this.sys_msgs.Count; i++)
            {
                Debug.Log(this.sys_msgs[i]);
            }
            event_manager.Instance.dispatch_event("get_sys_email", this.sys_msgs);
        }
    }

    void on_system_server_return(cmd_msg msg)
    {
        switch (msg.ctype)
        {
            case (int)Cmd.eGetUgameInfoRes:
                this.on_get_ugame_info_return(msg);
                break;

            case (int)Cmd.eRecvLoginBonuesRes:
                this.on_recv_login_bonues_return(msg);
                break;
            case (int)Cmd.eGetWorldRankUchipRes:
                this.on_get_world_uchip_rank_info_return(msg);
                break;
            case (int)Cmd.eGetSysMsgRes:
                this.on_get_sys_msg_return(msg);
                break;
        }
    }

    public void init()
    {
        network.Instance.add_service_listener((int)Stype.System, this.on_system_server_return);
    }

    public void load_user_ugame_info()
    {
        network.Instance.send_protobuf_cmd((int)Stype.System, (int)Cmd.eGetUgameInfoReq, null);
    }

    public void recv_login_bonues()
    {
        network.Instance.send_protobuf_cmd((int)Stype.System, (int)Cmd.eRecvLoginBonuesReq, null);
    }

    public void get_world_uchip_rank_info()
    {
        network.Instance.send_protobuf_cmd((int)Stype.System, (int)Cmd.eGetWorldRankUchipReq, null);
    }

    public void get_sys_msg()
    {
        GetSysMsgReq req = new GetSysMsgReq();
        req.ver_num = this.ver_num;

        network.Instance.send_protobuf_cmd((int)Stype.System, (int)Cmd.eGetSysMsgReq, req);
    }
}
