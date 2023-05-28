#include <stdio.h>

int main(
	 int argc,
	 char* argv[]
	 )
{
  int size = 10;
  /*
  // Option 1: contiguous memory with pointers 
  int ** as = (int**) malloc(sizeof(int*)*size);

  for ( size_t ix = 0; ix < size; ++ix)
    as[ix] = (int*) malloc(sizeof(int)*size);

  for ( size_t ix = 0; ix < size; ++ix)
    for ( size_t jx = 0; jx < size; ++jx)
      as[ix][jx] = ix*jx;
  //*/

  // Option 2 - stack memory:
  int as[size][size];
  for ( int ix = 0; ix < size; ++ix)
    for ( int jx = 0; jx < size; ++jx)
      as[ix][jx] = ix*jx;

  printf("%p\n",&as);
  //---------------------------
  // File writing:
  FILE* file = fopen("output.dat", "w");

  if ( file == NULL ){
    printf("error opening file\n");
    return -1;
  }

  // Option Stack:
  for ( int i = 0; i < size; ++i)
    for ( int j = 0; j < size; ++j)
      fwrite(&as[i][j], sizeof(int), 1, file);
  
  // Option 0: write all at once
  /*
  fwrite( as, sizeof(int), size*size, file);
  //*/


  // Option 1: write by 10 bunches
  /*
  for ( size_t ix = 0; ix < size-1; ++ix){
    fwrite( as+(ix*size), sizeof(int)*size, 10, file);
    fwrite( "\n", sizeof(char), 1, file);
  }
  fwrite( as+(size-1)*size, sizeof(int)*size, 10, file);
  //*/

  /*
  for ( size_t ix = 0; ix < size-1; ++ix){
    for ( size_t jx = 0; jx < size; ++jx){
      fwrite( &(as[ix][jx]), sizeof(int), 1, file);
      //  fwrite( &(ix*jx), sizeof(int), 1, file);
    }
    fwrite("\n",sizeof(char), 1, file);
  }
  for ( size_t jx = 0; jx < size; ++jx)
    fwrite( &(as[size-1][jx]), sizeof(int), 1, file);
    //fwrite((size-1)*jx, sizeof(int), 1, file);
  */	    
  fclose(file);

  //----------------------------
  // File .txt writing
  /*
  file = fopen("output.txt", "w");

  if ( file == NULL ){
    printf("error opening file\n");
    return -1;
  }

  // Option 1: write by 10 bunches
  //*
  fwrite( &as, sizeof(int)*size*size, size*size, file);
    
  for ( int ix = 0; ix < size-1; ++ix){
    //fwrite( &as+ix*size, sizeof(int)*size, size, file);
    //printf("Write to output.\n");
    //fflush(file);
    //  fwrite( "\n", sizeof(char), 1, file);
  }
  //fwrite( &as+(size-1)*size, sizeof(int)*size, size, file);
  
  
  fclose(file);
  */
  return 0;
}