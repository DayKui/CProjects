local Stype = require("Stype")
local Cmd = require("Cmd")
local guest = require("auth_server/guest")


local auth_service_handlers = {}
auth_service_handlers[Cmd.eGuestLoginReq] = guest.login

-- {stype, ctype, utag, body}
function on_auth_recv_cmd(s, msg)
	if auth_service_handlers[msg[2]] then 
		auth_service_handlers[msg[2]](s, msg)
	end
end

function on_auth_session_disconnect(s, stype) 
end

local auth_service = {
	on_session_recv_cmd = on_auth_recv_cmd,
	on_session_disconnect = on_auth_session_disconnect,
}

return auth_service