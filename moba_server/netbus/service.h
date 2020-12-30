#ifndef _SERVICE_H_
#define _SERVICE_H_

class session;
struct cmd_msg;

class service {
public:
	virtual bool on_session_recv_cmd(session* s, struct cmd_msg* msg);
	virtual void on_session_disconnect(session* s);
};

#endif // !_SERVICE_H_
