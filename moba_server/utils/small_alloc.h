#ifndef _SMALL_ALLOC_H_
#define _SMALL_ALLOC_H_

#ifdef __cplusplus
extern "C" {
#endif

	void* small_alloc(int size);
	void small_free(void* mem);

#ifdef __cplusplus
}
#endif

#endif
