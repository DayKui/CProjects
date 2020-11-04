#ifndef  _NETBUS_H_
#define  _NETBUS_H_

class netbus {
public:
		static netbus* instance();

public:
	void init();
	void start_tcp_server(int port);
	void start_ws_server(int port);
	void run();
};

#endif
