#include <stdio.h>
#include <stdlib.h>

int main(
	 int arfc,
	 char *argv[]
	 )
{
  int size = 10;

  int* asentries = (int*) malloc(sizeof(int)*size*size); // allocate 4*100 Bytes of memory (one block) 
  int** as = (int**) malloc(sizeof(int*)*size); // allocate 8*10 Bytes of memory

  for ( size_t ix = 0, jx = 0; ix < size; ++ix, jx+=size )
    as[ix] = asentries + jx;
 
  for ( size_t ix = 0; ix < size; ++ix )
    for ( size_t jx = 0; jx < size; ++jx )
      as[ix][jx] = 0;

  printf("%d\n", as[0][0]);

  free(as);
  free(asentries);
}
