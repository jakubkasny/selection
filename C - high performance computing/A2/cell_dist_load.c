#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>

int
main(
     int argc,
     char* argv[]
     )
{
  FILE* file_pointer = fopen("cell","r");
  if ( file_pointer == NULL){
    printf("error opening file\n");
    return -1;
  }

  int fsize = 0;
  fseek(file_pointer, 0, SEEK_END); // set position pointer to end of file
  fsize = ftell(file_pointer); // get pointer position
  rewind(file_pointer); // set pointer to beginning of file

  char* fdata = (char*) malloc(sizeof(char)*fsize); // allocate memory for data
  size_t readsize = fread(fdata, 1, fsize, file_pointer); //
  printf("%i\n",sizeof(fdata));
  
  int sz = (int)fsize/24;
  float* accs = (float*) malloc(sizeof(float)*3*(fsize/24));
  float** fldata = (float**) malloc(sizeof(float*)*(fsize/24));

  printf("size: %i\n",fsize);
  for (size_t ix = 0, jx = 0; ix < fsize/24; ++ix, jx+=3)
    fldata[ix] = accs + jx;

  float tmp;
  int inx;
  for ( int ix = 0; ix < fsize; ix+=24){
    //printf("ix: %i\n",ix);
    tmp = (*(fdata+1+ix) - 48)*10.f + (*(fdata+2+ix) - 48) +
      (*(fdata+4+ix) - 48)*0.1f + (*(fdata+5+ix) - 48)*0.01f + (*(fdata+6+ix) - 48)*0.001f;
    fldata[inx][1] = tmp - tmp*(*(fdata+ix) - 43);
    printf("%f ", fldata[inx][1]);
    
    tmp = (*(fdata+9+ix) - 48)*10.f + (*(fdata+10+ix) - 48) +
      (*(fdata+12+ix) - 48)*0.1f + (*(fdata+13+ix) - 48)*0.01f + (*(fdata+14+ix) - 48)*0.001f;
    fldata[inx][2] = tmp - tmp*(*(fdata+ix+8) - 43);
    printf("%f ", fldata[inx][2]);

    tmp = (*(fdata+17+ix) - 48)*10.f + (*(fdata+18+ix) - 48) +
      (*(fdata+20+ix) - 48)*0.1f + (*(fdata+21+ix) - 48)*0.01f + (*(fdata+22+ix) - 48)*0.001f;
    fldata[inx][3] = tmp - tmp*(*(fdata+ix+16) - 43);
    printf("%f\n", fldata[inx][3]);
  }

  /*
  float fldata = (*(chdata+1) - 48)*10.f + (*(chdata+2) - 48) +
    (*(chdata+4) - 48)*0.1f + (*(chdata+5) - 48)*0.01f + (*(chdata+6) - 48)*0.001f;
  fldata = fldata - fldata*(*(chdata+ix) - 43);

  printf("fread return size: %i\n", readsize);
  printf("File read data:\n %s\n",fdata);
  */

  
  /*
  printf("fread return size: %i\n", readsize);
  printf("File read data:\n %s\n",fdata);
  */
  //==============================
  //Char to float:
  /*
  char* chdata = (char*) malloc(sizeof(char)*8);
  strcpy(chdata, "-96.254");
  float fldata = (*(chdata+1) - 48)*10.f + (*(chdata+2) - 48) +
    (*(chdata+4) - 48)*0.1f + (*(chdata+5) - 48)*0.01f + (*(chdata+6) - 48)*0.001f;
  printf("Data: %f\n", fldata);
*/
  free(fdata);
  // free(chdata);
  return 0;
}

  /*
  //==============================
  // Benchamrking:
  const long int bench_iter = 10000000000;
  struct timespec bench_start_time;
  struct timespec bench_stop_time;
  double bench_diff_time;
  { volatile int tmp = 0;
    for (size_t heat = 0; heat < 100000; heat++)
      tmp += heat*heat;
  }
  ///*
  float fldata = 1.;
  char* chdata = "-++--+--+-+-+-+++++-+-+++---+"; //(char*) malloc(sizeof(char)*3); 
  //printf("chdata: %i\n", strlen(chdata));

  volatile float signum;
  /*
  for ( size_t ix = 0; ix < strlen(chdata); ix++){
    fldata = 1; 
    signum = *(chdata+ix) - 43;//chdata[ix] - 43;
    fldata = fldata - fldata*(*(chdata+ix) - 43);
    printf("Val: %f | ",fldata);
    printf("sign: %c |",chdata[ix]);
    printf("sign val: %f \n",signum);
  }

  fldata = 1;
  fldata = fldata - fldata*(*(chdata+0) - 43);
  printf("%f\n",fldata);
  //
  timespec_get(&bench_start_time, TIME_UTC);
  for ( size_t iter = 0; iter < bench_iter; iter++){
    for ( size_t ix = 0; ix < strlen(chdata); ix++){
      fldata = 1; 
      signum = *(chdata+ix) - 43;//chdata[ix] - 43;
      fldata = fldata - fldata*(*(chdata+ix) - 43);
    }
  }
  timespec_get(&bench_stop_time, TIME_UTC);

  bench_diff_time =
  difftime(bench_stop_time.tv_sec, bench_start_time.tv_sec) * 1000000 +
    (bench_stop_time.tv_nsec - bench_start_time.tv_nsec) / 1000;
  
  printf("Data: %f\n", fldata);
  printf("Length of one cycle (NO IF): %f mus \n", bench_diff_time/bench_iter);
  double bench1 = bench_diff_time/bench_iter;

  
  timespec_get(&bench_start_time, TIME_UTC);
  for ( size_t iter = 0; iter < bench_iter; iter++){
    fldata = 1; 
    signum = *(chdata+0) - 43;//chdata[ix] - 43;
    fldata = fldata - fldata*(*(chdata+0) - 43);
  }
  timespec_get(&bench_stop_time, TIME_UTC);

  bench_diff_time =
  difftime(bench_stop_time.tv_sec, bench_start_time.tv_sec) * 1000000 +
    (bench_stop_time.tv_nsec - bench_start_time.tv_nsec) / 1000;
  
  printf("Data: %f\n", fldata);
  printf("Length of one cycle (NO IF - NO OUTER LOOP): %f mus \n", bench_diff_time/bench_iter);
  double bench11 = bench_diff_time/bench_iter;

  // Values check
  /*
  for ( size_t ix = 0; ix < strlen(chdata); ix++){
    fldata = 1; 
    if (*(chdata+ix ) == 45)
      fldata = -fldata;
    printf("Val: %f | ",fldata);
    printf("sign: %c \n",*(chdata+ix));
  }
  //
  
  timespec_get(&bench_start_time, TIME_UTC);
  for ( size_t iter = 0; iter < bench_iter; iter++){
    for ( size_t ix = 0; ix < strlen(chdata); ix++){
      fldata = 1;
      if (*(chdata+ix) == 45)
	fldata = -fldata;
    }
  }
  timespec_get(&bench_stop_time, TIME_UTC);

  bench_diff_time =
    difftime(bench_stop_time.tv_sec, bench_start_time.tv_sec) * 1000000 +
    (bench_stop_time.tv_nsec - bench_start_time.tv_nsec) / 1000;
  
  printf("Data: %f\n", fldata);
  printf("Length of one cycle (IF): %f mus \n", bench_diff_time/bench_iter);
  double bench2 = bench_diff_time/bench_iter;

  
  timespec_get(&bench_start_time, TIME_UTC);
  for ( size_t iter = 0; iter < bench_iter; iter++){
    fldata = 1;
    if (*(chdata+0) == 45)
      fldata = -fldata;
  }
  timespec_get(&bench_stop_time, TIME_UTC);

  bench_diff_time =
    difftime(bench_stop_time.tv_sec, bench_start_time.tv_sec) * 1000000 +
    (bench_stop_time.tv_nsec - bench_start_time.tv_nsec) / 1000;
  
  printf("Data: %f\n", fldata);
  printf("Length of one cycle (IF): %f mus \n", bench_diff_time/bench_iter);
  printf("Ratio if/noIf: %f %\n", (bench_diff_time/bench_iter)/bench1);
  double bench2 = bench_diff_time/bench_iter;


  
  while( fgets(out
  int r = fscanf(file, "%s", out);
  if (r != 1)
    printf("error reading from file\n");
  printf("read %s \n", out);
  */
