#include "stdafx.h"

#if	!defined( _CRT_SECURE_NO_DEPRECATE )
#define _CRT_SECURE_NO_DEPRECATE
#endif

#include <windows.h>
#include <cstdio>
#include <cassert>
#include <vector>
#include <string>

using namespace std;
#include "Pipe.h"

inline void create_thread(LPTHREAD_START_ROUTINE proc, LPVOID p)
{
	CreateThread(
		NULL , // セキュリティ記述子.
		0    , // 初期のスタックサイズ.
		proc , // スレッドの機能.
		p    , // スレッドの引数.
		0    , // 作成オプション.
		NULL   // スレッド識別子.
	);
}
//
// 
//
class READ_PIPE
{
	enum
	{
		BUFSIZE=1024*16
	};
	enum
	{
		PIPE_TIMEOUT=5000000
	};
	char pipe_name[128];
	HANDLE mutex;
public:
	vector<string> message_que;
	READ_PIPE(const char *name)// : bInCritical(false)
	{
		mutex = CreateMutexA(NULL, false, "InCritical");
		strcpy_s( pipe_name , sizeof(pipe_name) , name );
		create_thread(&READ_PIPE::process, (LPVOID)this);
	}
	~READ_PIPE()
	{
	}
private:
	static DWORD WINAPI  process(LPVOID p)
	{
		READ_PIPE *thispipe = (READ_PIPE*)p;
		HANDLE hPipe;
		CHAR chRequest[BUFSIZE];
		DWORD cbBytesRead;
		bool fConnected;
		BOOL fSuccess;
		hPipe = CreateNamedPipeA(
			thispipe->pipe_name,
			PIPE_ACCESS_DUPLEX,       // read/write access
			PIPE_TYPE_MESSAGE |       // message type pipe
			PIPE_READMODE_MESSAGE |   // message-read mode
			PIPE_WAIT,                // blocking mode
			PIPE_UNLIMITED_INSTANCES, // max. instances
			BUFSIZE,                  // output buffer size
			BUFSIZE,                  // input buffer size
			PIPE_TIMEOUT,             // client time-out
			NULL);                    // no security attribute
		assert(hPipe != INVALID_HANDLE_VALUE);
		for (;;)
		{
			fConnected = ConnectNamedPipe(hPipe, NULL) ? TRUE : (GetLastError() == ERROR_PIPE_CONNECTED);
			if (fConnected)
			{
				fSuccess = ReadFile(
					hPipe,        // handle to pipe
					chRequest,    // buffer to receive data
					BUFSIZE,      // size of buffer
					&cbBytesRead, // number of bytes read
					NULL);        // not overlapped I/O
				chRequest[cbBytesRead] = '\0';
				WaitForSingleObject(thispipe->mutex, (DWORD)INFINITY);
				{
 					thispipe->message_que.push_back(chRequest);
				}
				ReleaseMutex(thispipe->mutex); //thispipe->bInCritical=false;
				if(! fSuccess || cbBytesRead == 0)
				{
					break;
				}
				FlushFileBuffers(hPipe);
				DisconnectNamedPipe(hPipe);
			}
			else
			{
				CloseHandle(hPipe);
			}
		}
		CloseHandle(hPipe);
		return 0;
	}
private:
	string firstmsg;
public:
	string *get_msg()
	{
		if (message_que.empty())
		{
			return NULL;
		}
		WaitForSingleObject(mutex,(DWORD)INFINITY);
		{
			firstmsg = message_que.at(0);
			message_que.erase(message_que.begin());
		}
		ReleaseMutex(mutex);//  bInCritical = false;
		return &firstmsg;
	}
};

class WRITE_PIPE
{
	char pipe_name[128];
public:
	WRITE_PIPE(const char *name) 
	{
		strcpy_s( pipe_name , sizeof(pipe_name) , name );
	}
	bool operator()(const char *msg)
	{
		HANDLE hFile;
		bool flg = false;
		unsigned long dwWrite = 0;
		std::string szPipeUpdate;
		hFile = CreateFileA( pipe_name , GENERIC_WRITE , 0 , NULL , OPEN_EXISTING , 0 , NULL );
		szPipeUpdate=msg;
		if(hFile == INVALID_HANDLE_VALUE)
		{
			return false;
		}
		else
		{
			if (WriteFile(hFile, szPipeUpdate.c_str(), (unsigned long)(szPipeUpdate.size()), &dwWrite, NULL) == TRUE)
			{
				flg = true;
			}
			FlushFileBuffers(hFile);
			CloseHandle(hFile);
		}
		return flg;
	}
};
/*
	READ/WRITE パイプをまとめる
*/
class PipeWork : public Pipe
{
	READ_PIPE   rpipe;
	WRITE_PIPE  wpipe;
public:
	explicit PipeWork(const char *readpipe_name, const char *writepipe_name) : rpipe(readpipe_name), wpipe(writepipe_name)
	{}
	virtual std::string *read_msg()
	{
		return rpipe.get_msg();
	}
	
	virtual bool write_msg(const char *msg)
	{
		return wpipe(msg);
	}
};
/*
	生成削除
*/
Pipe* Pipe::create_pipe(const char *readpipe_name, const char *writepipe_name)
{
	return new PipeWork(readpipe_name,writepipe_name);	
}
void Pipe::delete_pipe(Pipe *p)
{
	if (p)
	{
		delete p;
	}
}
