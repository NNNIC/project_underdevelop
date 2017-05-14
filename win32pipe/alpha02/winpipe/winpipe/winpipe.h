// 以下の ifdef ブロックは DLL からのエクスポートを容易にするマクロを作成するための 
// 一般的な方法です。この DLL 内のすべてのファイルは、コマンド ラインで定義された WINPIPE_EXPORTS
// シンボルを使用してコンパイルされます。このシンボルは、この DLL を使用するプロジェクトでは定義できません。
// ソースファイルがこのファイルを含んでいる他のプロジェクトは、 
// WINPIPE_API 関数を DLL からインポートされたと見なすのに対し、この DLL は、このマクロで定義された
// シンボルをエクスポートされたと見なします。
#ifdef WINPIPE_EXPORTS
#define WINPIPE_API __declspec(dllexport)
#else
#define WINPIPE_API __declspec(dllimport)
#endif

#include "Pipe.h"

extern "C" WINPIPE_API Pipe *g_pipe;
extern "C" WINPIPE_API char  *g_buf;

extern "C" WINPIPE_API void create(int *readpipename, int *writepipename);
extern "C" WINPIPE_API void destroy();
extern "C" WINPIPE_API int read();
extern "C" WINPIPE_API void write(int *);

extern "C" WINPIPE_API int *get_buf();
