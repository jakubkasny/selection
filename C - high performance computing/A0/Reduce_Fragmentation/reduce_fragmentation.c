#include <stdio.h>
#include <stdlib.h>

int main(
	 int argc,
	 char *argv[]
	 )
{
  int size = 10;
  int ** as = (int**) malloc(sizeof(int*)*size);
  // allocating 10*8 Bytes for array of double pointers on heap
  for ( size_t ix = 0; ix < size; ++ix )
    as[ix] = (int*) malloc(sizeof(int)*size);
  // for each position in 'as' allocate 10 positions for int* pointers
  // => create 10 bunches of memory (of size 10*8 Bytes each) - these bunches are spread around stack
  for ( size_t ix = 0; ix < size; ++ix)
    for ( size_t jx = 0; jx < size; ++jx)
      as[ix][jx] = 0;
  // set each point in "matrix" to zero

  printf("%d\n", as[0][0]);

  for ( size_t ix = 0; ix < size; ++ix)
    free(as[ix]); // free each of 10 bunches of memory (in double pointer array)
  free(as); // free double pointer
}
