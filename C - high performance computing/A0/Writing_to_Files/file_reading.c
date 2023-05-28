#include <stdio.h>

int
main(
     int argc,
     char* argv[]
     )
{
  int size = 10;
  int as[size][size];
  
  FILE* file = fopen("output.dat", "r");

  for ( int i = 0; i < size; ++i)
      fread( (void*)as[i], sizeof(int), size, file);
  fclose(file);

  int eqls = 1;
   
  for ( int i = 0; i < size; ++i)
  {
    for ( int j = 0; j < size; ++j)
    {
      printf("%i ",as[i][j]);
      if (as[i][j] != i*j)
	eqls = 0;	
    }
      printf("\n");
  }

  if (eqls == 1)
    printf("Matrix is correct.\n");
  else
    printf("Error occured, matrix is wrong.\n");
    
  return 0;
}
