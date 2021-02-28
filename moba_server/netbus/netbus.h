#ifndef  _NETBUS_H_
#define  _NETBUS_H_

class netbus {
public:
		static netbus* instance();

public:
	void init();
	void tcp_listen(int port);
	void ws_listen(int port);
	void udp_listen(int port);
	void run();
};

#endif
