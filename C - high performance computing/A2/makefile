CC = -O2 -fopenmp -lm -o

.PHONY : all
all: cell_distances

cell_distances: cell_distances.c
	gcc $(CC) $@ $< 
