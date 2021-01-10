#include <string.h>
#include <stdio.h>
#include <stdlib.h>

#include <iostream>
#include <string>
using namespace std;

#include "../../netbus/proto_man.h"
#include "../../netbus/netbus.h"
#include "proto/pf_cmd_map.h"
#include "../../utils/logger.h"
#include "../../utils/time_list.h"
#include "../../utils/timestamp.h"
#include "../../database/mysql_wrapper.h"

static void on_logger_timer(void* udata) {
	log_debug("on_logger_timer");
}

static void
on_query_cb(const char* err, std::vector<std::vector<std::string>>* result) {
	if (err) {
		printf("err");
		return;
	}
	if (result!=NULL)
	{
		int count = result->size();
		for (int i = 0; i < count; i++)
		{
			std::vector < std::string > vec = result->at(i);
			for (vector<std::string>::const_iterator iter = vec.cbegin(); iter != vec.cend(); iter++)
			{
				cout << (*iter) << endl;
			}
		}
	}
	printf("success");
}


static void
on_open_cb(const char* err, void* context) {
	if (err != NULL) {
		printf("%s\n", err);
		return;
	}
	printf("connect success");

	//mysql_wrapper::query(context, "update class_test set name = \"blake haha\" where id = 8", on_query_cb);
	mysql_wrapper::query(context, "select * from class_test", on_query_cb);

	// mysql_wrapper::close(context);
}

static void
test_db() {
	mysql_wrapper::connect("127.0.0.1", 3306, "class_sql", "root", "123456", on_open_cb);
}

int main(int*argc, char**argv)
{
	test_db();
	proto_man::init(PROTO_BUF);
	init_pf_cmd_map();

	logger::init("logger/gateway/", "gateway", true);

	log_debug("%d", timestamp());
	log_debug("%d", timestamp_today());
	log_debug("%d", date2timestamp("%Y-%m-%d %H:%M:%S", "2018-02-01 00:00:00"));


	unsigned long yesterday = timestamp_yesterday();
	char out_buf[64];
	timestamp2date(yesterday, "%Y-%m-%d %H:%M:%S", out_buf, sizeof(out_buf));
	log_debug("%s", out_buf);
	//schedule(on_logger_timer, NULL, 3000, -1);

	netbus::instance()->init();
	netbus::instance()->start_tcp_server(6080);
	netbus::instance()->start_upd_server(8002);
	netbus::instance()->start_ws_server(8001);
	netbus::instance()->run();
	return 0;
}

