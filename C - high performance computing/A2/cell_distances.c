#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <omp.h>

static inline
void
dataParse(
      char b_data[],
      short  b_arr[],
      size_t bs
      ){
  size_t inx=0;
  for (size_t ix = 0; ix < bs*24; ix += 24){
    b_arr[inx+0] = (b_data[1+ix] - '0')*10000 + (b_data[2+ix] - '0')*1000 +
                  (b_data[4+ix] - '0')*100   + (b_data[5+ix] - '0')*10 + (b_data[6+ix] - '0');
    if (b_data[0+ix] == '-')
      b_arr[inx+0] *= -1;

    b_arr[inx+1] = (b_data[1+8+ix] - '0')*10000 + (b_data[2+8+ix] - '0')*1000 +
                  (b_data[4+8+ix] - '0')*100   + (b_data[5+8+ix] - '0')*10 + (b_data[6+8+ix] - '0');
    if (b_data[8+ix] == '-')
      b_arr[inx+1] *= -1;

    b_arr[inx+2] = (b_data[1+16+ix] - '0')*10000 + (b_data[2+16+ix] - '0')*1000 +
	          (b_data[4+16+ix] - '0')*100   + (b_data[5+16+ix] - '0')*10 + (b_data[6+16+ix] - '0');
    if (b_data[16+ix] == '-')
      b_arr[inx+2] *= -1;
    inx+=3;
  }
    
  return;  
}

int
main(
     int argc,
     char* argv[]
     )
{
  // parse argument:
  int threads_num = 1; 
  if (argc > 1 )    
    threads_num = atoi(argv[1]+2);
  omp_set_num_threads(threads_num);

  // variables:
  const unsigned short max_dist = 3464;
  unsigned short distance[max_dist];
  unsigned long* heap_dist = (unsigned long*) malloc(sizeof(unsigned long)*max_dist);

  // create file pointer:
  FILE* file_pointer = fopen("cells","r");
  if ( file_pointer == NULL){
    printf("error opening file :(\n");
    return -1;
  }

  // size of whole file:
  fseek( file_pointer, 0 , SEEK_END);
  int sz = ftell(file_pointer)/24;
  
  // file data:
  const size_t bb_size_prim = 512;
  //  const size_t bb_size_prim = 2;
  const size_t bb_size = bb_size_prim*threads_num;
  char bb_data[bb_size*24];
  short bb_arr[bb_size*3];

  const size_t sb_size = 128;
  //const size_t sb_size = 2;
  char sb_data[sb_size*24];
  short sb_arr[sb_size*3];

  
  // initialize heap_distance
  for (size_t ix = 0; ix < max_dist; ix+=4){
    heap_dist[ix] = 0;
    heap_dist[ix+1] = 0;
    heap_dist[ix+2] = 0;
    heap_dist[ix+3] = 0;
  }
  

  // for each big block:
  for ( size_t bbi = 0; bbi < sz; bbi += bb_size){
    
    size_t bbs = (bbi + bb_size < sz) ? bb_size : sz-bbi;

    // read big block:
    fseek( file_pointer, bbi*24 , SEEK_SET);
    fread(bb_data, 1, bbs*24, file_pointer);
    
    // save bb to short[];
    dataParse(bb_data, bb_arr, bbs);

    // initialize distance
    for (size_t ix = 0; ix < max_dist; ix+=4){
      distance[ix] = 0;
      distance[ix+1] = 0;
      distance[ix+2] = 0;
      distance[ix+3] = 0;
    }
  
#pragma omp parallel for default(shared) reduction(+:distance[:max_dist]) schedule(static, bb_size_prim)
    // compute big block;
    for (size_t ix = 0; ix < bbs*3; ix +=3){
      for (size_t jx = ix + 3; jx < bbs*3; jx +=3){
        float diff0 = bb_arr[ix+0] - bb_arr[jx+0];
        float diff1 = bb_arr[ix+1] - bb_arr[jx+1];
        float diff2 = bb_arr[ix+2] - bb_arr[jx+2];
	unsigned int d = (sqrtf((diff0*diff0 + diff1*diff1 + diff2*diff2))*0.1f);
	distance[d+1] += 1;
	//printf("ix: %i | jx: %i | dist: %i \n",ix,jx,d);
      }
    }
    
    // save to heap:
    for ( size_t di = 0; di < max_dist; di+=4){
      heap_dist[di] += distance[di];
      heap_dist[di+1] += distance[di+1];
      heap_dist[di+2] += distance[di+2];
      heap_dist[di+3] += distance[di+3];
    }
    // for each small block:
    for ( size_t sbi = bbi+bb_size; sbi < sz; sbi += sb_size){
      size_t sbs = (sbi + sb_size < sz) ? sb_size : sz-sbi;

      fseek(file_pointer, sbi*24, SEEK_SET);
      // read small block:
      fread(sb_data, 1, sbs*24, file_pointer);

      // save bb to short[]; 
      dataParse(sb_data, sb_arr, sbs);
      
      for (size_t ix = 0; ix < max_dist; ix+=4){
	distance[ix] = 0;
	distance[ix+1] = 0;
      	distance[ix+2] = 0;
      	distance[ix+3] = 0;
      }
      
#pragma omp parallel for default(shared) reduction(+:distance[:max_dist]) schedule(static, bb_size_prim)
      for ( size_t ix = 0; ix < bbs*3; ix += 3){
	for ( size_t jx = 0; jx < sbs*3; jx += 3){
	  float diff0 = bb_arr[ix+0] - sb_arr[jx+0];
	  float diff1 = bb_arr[ix+1] - sb_arr[jx+1];
	  float diff2 = bb_arr[ix+2] - sb_arr[jx+2];
	  unsigned int d = (sqrtf((diff0*diff0 + diff1*diff1 + diff2*diff2))*.1f);
	  distance[d+1] += 1;
	  //printf("ix: %i | jx: %i | dist: %i \n",ix,jx,d);
	}
      }

      // save to heap:
      for ( size_t di = 0; di < max_dist; di+=4){
	heap_dist[di] += distance[di];
	heap_dist[di+1] += distance[di+1];
	heap_dist[di+2] += distance[di+2];
	heap_dist[di+3] += distance[di+3];
      }	
    }
  }
 
  for (size_t di = 0; di < max_dist; di+=4){ //max_dist
    //    if (heap_dist[di] != 0)
      fprintf( stdout,"%.2f %i\n",di*.01f, heap_dist[di]);
      //if (heap_dist[di+1] != 0)
      fprintf( stdout,"%.2f %i\n",(di+1)*.01f, heap_dist[di+1]);
      //if (heap_dist[di+2] != 0)
      fprintf( stdout,"%.2f %i\n",(di+2)*.01f, heap_dist[di+2]);
      //if (heap_dist[di+3] != 0)
      fprintf( stdout,"%.2f %i\n",(di+3)*.01f, heap_dist[di+3]);
  }
  fclose(file_pointer);
  free(heap_dist);
  
  return 0;
}


/*
  For loop 0 
    // Read large block
    
    For loop 1 (Adin (<3) double for loop) 
      //compute elements in loaded large block
    end loop 1
    
    For loop 2.1 (foreach small block)
      // Read smaller block
      // initialize distance[d] = 0;
      For loop 2.2 (foreach element_large_block in large_block) 
          For loop 2.3 (foreach element_small_block in small_block)
	      // Compute (element_large_block x element_small_block)
	  end
      end
      // save to heap (some freaking large memory) :) 
    end
  end

  // Save to file
  
  */
