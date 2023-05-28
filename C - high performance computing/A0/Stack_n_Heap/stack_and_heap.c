#include <stdio.h>
#include <stdlib.h>

int myPow(int a, int b);

int
main(
     int argc,
     char* argv[]
     )
{
  char stack = '1';
  if (stack=='1'){
    int size = myPow(10,2);
    
    for ( size_t pwr = 0; pwr < 2000; ++pwr)
      {
	int size = myPow(2, (int)pwr);
	printf("Bytes to be taken: %i\n",size*sizeof(int));
	 //printf("Ints to be created: %i\n",size);
	int as[size];
	for ( size_t ix = 0; ix < size; ++ix)
	  as[ix] = 0;
      }
  }else{
    int size = 8388608/sizeof(int);

    int* as = (int*) malloc(sizeof(int)*size);
    for ( size_t ix = 0; ix < size; ++ix)
      as[ix] = ix;
    printf("Bytes to be taken: %i\n",size*sizeof(int)); 
    free(as); 
  }
  
  
}

int myPow(int a, int b)
{
  int num = 1;
  for ( int i = 0; i < b; ++i)
    num *= a;
  
  return(num);
}
