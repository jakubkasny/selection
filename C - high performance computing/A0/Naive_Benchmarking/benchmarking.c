#include <stdio.h>
#include <time.h>

int
main(
     int argc,
     char* argv[]
     )
{
  long int sum = 0;

  struct timespec bench_start_time;
  struct timespec bench_stop_time;
  double bench_diff_time;
  
  timespec_get(&bench_start_time, TIME_UTC);
  for ( size_t i = 0;  i <= 1000000000; ++i){
    sum += i;
  }
  timespec_get(&bench_stop_time, TIME_UTC);
  
  fprintf(stdout,"sum: %li\n", sum);
   
  bench_diff_time =
    difftime(bench_stop_time.tv_sec, bench_start_time.tv_sec)*1000000 +
    (bench_stop_time.tv_nsec - bench_start_time.tv_nsec) / 1000;
    
  printf("Benchmark time: %f s\n",bench_diff_time/1000000);
  printf("Benchmark one loop time: %f mus\n",bench_diff_time/1000000000);
  
  return 0;
}
