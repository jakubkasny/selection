#include <stdio.h>
#include <stdlib.h>

int
main(
     int argc,
     char* argv[]
     )
{
  char* arg1 = argv[1];
  int a1n = atoi(argv[1]+2);
  char* arg2 = argv[2];
  int a2n = atoi(argv[2]+2);  
  
  if ( arg1[1] == 'a' && arg2[1] == 'b')
    fprintf(stdout, "A is %i and B is %i.\n", a1n, a2n);
  else if ( arg1[1] == 'b' && arg2[1] == 'a')
    fprintf(stdout, "A is %i and B is %i.\n", a2n, a1n);
  
  return 0;
}
