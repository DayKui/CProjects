#ifndef _SERVER_EXPORT_TO_LUA_H_
#define _SERVER_EXPORT_TO_LUA_H_

struct lua_State;
int register_service_export(lua_State* tolua_S);

#endif 
