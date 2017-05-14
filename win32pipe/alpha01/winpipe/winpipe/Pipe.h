#ifndef __IPIPE_H__
#define __IPIPE_H__

#include <string>

class Pipe
{
public:
	static Pipe *create_pipe(const char *readpipe_name,const char *writepipe_name);
	static void   delete_pipe(Pipe *p);

	virtual std::string *read_msg(){return NULL;}
	virtual bool write_msg(const char *msg){ msg; return false;} 
};


#endif // __IPIPE_H__
