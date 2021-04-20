local mysql_center = require("database/mysql_auth_center")
local Respones = require("Respones")
local Stype = require("Stype")
local Cmd = require("Cmd")

-- {stype, ctype, utag, body}
function login(s, msg)
    local g_key = msg[4].guest_key
	local utag = msg[3];
	print(msg[1], msg[2], msg[3], msg[4].guest_key)


	mysql_center.get_guest_uinfo(g_key, function (err, uinfo)
		if err then -- 告诉客户端系统错误信息;
            local msg = {Stype.Auth, Cmd.eGuestLoginRes, utag, {
				status = Respones.SystemErr,
			}}
			Session.send_msg(s, msg)
			return
		end

		if uinfo == nil then -- 没有查到对应的 g_key的信息
			mysql_center.insert_guest_user(g_key, function(err, ret)
				if err then -- 告诉客户端系统错误信息;
                    local msg = {Stype.Auth, Cmd.eGuestLoginRes, utag, {
						status = Respones.SystemErr,
					}}

					Session.send_msg(s, msg)
					return
				end
				login(s, msg)
			end)
			return
		end

		-- 找到了我们gkey所对应的游客数据;
		if uinfo.status ~= 0 then --账号被查封
            local msg = {Stype.Auth, Cmd.eGuestLoginRes, utag, {
				status = Respones.UserIsFreeze,
			}}

			Session.send_msg(s, msg)
		end

		if uinfo.is_guest ~= 1 then  --账号已经不是游客账号了
            local msg = {Stype.Auth, Cmd.eGuestLoginRes, utag, {
				status = Respones.UserIsNotGuest,
			}}

			Session.send_msg(s, msg)
		end
		-- end

		print(uinfo.uid, uinfo.unick) -- 登陆成功返回给客户端;
		local msg = { Stype.Auth, Cmd.eGuestLoginRes, utag, {
			status = Respones.OK,
			uinfo = {
				unick = uinfo.unick,
				uface = uinfo.uface,
				usex  = uinfo.usex,
				uvip  = uinfo.uvip,
				uid   = uinfo.uid, 
			}
		}}
		Session.send_msg(s, msg)
	end)
end

local guest = {
	login = login
}

return guest
