log_debug("HelloWorld")
mysql_wrapper.connect("127.0.0.1", 3306, "class_sql", "root", "123456", function(err, context) 
	log_debug("event call");

	if(err)  then
		print(err)
		return
	end


	mysql_wrapper.close(context);
end)
