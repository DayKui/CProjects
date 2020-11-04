#include <string.h>
#include <stdio.h>
#include <stdlib.h>

#include <iostream>
#include <string>
#include "../../netbus/netbus.h"
using namespace std;


void SysCheck()
{
	union IsLitte_Endian
	{
		int i;
		char c;
	};
	IsLitte_Endian Check;
	Check.i = 1;
	bool Flag = Check.c == 1;	//Flag为true表示是小端模式，Flag为false表示为大端模式，此时Flag为true。
	return;
}
int main(int*argc,char**argv)
{
	SysCheck();
	netbus::instance()->init();
	netbus::instance()->start_tcp_server(6080);
	netbus::instance()->start_ws_server(8001);
	netbus::instance()->run();
	return 0;
}
