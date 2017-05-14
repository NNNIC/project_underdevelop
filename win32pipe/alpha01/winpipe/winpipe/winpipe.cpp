// winpipe.cpp : DLL アプリケーション用にエクスポートされる関数を定義します。
//

#include "stdafx.h"
#include "winpipe.h"
#include "Pipe.h"
#include <string>
using namespace std;

WINPIPE_API Pipe *g_pipe = NULL;
WINPIPE_API char  *g_buf = NULL;

WINPIPE_API void create(int *readpipename, int *writepipename)
{
	g_pipe = Pipe::create_pipe((const char*)readpipename, (const char *)writepipename);
	g_buf = NULL;
}
WINPIPE_API void destroy()
{
	if (g_pipe != NULL)
	{
		Pipe::delete_pipe(g_pipe);
	}
	g_pipe = NULL;
}
WINPIPE_API int read()
{
	if (g_pipe != NULL)
	{
		string *s = g_pipe->read_msg();
		if (s != NULL)
		{
			if (g_buf != NULL)
			{
				free(g_buf);
				g_buf = NULL;
			}

			const char *src = s->c_str();
			const int  size = (int)strlen(src);
			g_buf = (char *)malloc(size + 1);
			memset(g_buf, 0, size + 1);
			memmove(g_buf, src, size);
			return size;
		}
	}
	return 0;
}
WINPIPE_API void write(int *msg)
{
	if (g_pipe != NULL && msg != NULL)
	{
		g_pipe->write_msg((const char *)msg);
	}
}

WINPIPE_API int *get_buf()
{
	return (int *)g_buf;
}