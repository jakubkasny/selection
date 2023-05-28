.PHONY : all
all : benchmarking_exe benchmarking_exe_O0 benchmarking_exe_O1 benchmarking_exe_O2 benchmarking_exe_O3 benchmarking_exe_Os benchmarking_exe_Og 

benchmarking_exe : benchmarking.c
	gcc -o benchmarking_exe benchmarking.c && ./benchmarking_exe

benchmarking_exe_O0 : benchmarking.c
	gcc -o benchmarking_exe_O0 benchmarking.c -O0 && ./benchmarking_exe_O0

benchmarking_exe_O1 : benchmarking.c
	gcc -o benchmarking_exe_O1 benchmarking.c -O1 && ./benchmarking_exe_O1

benchmarking_exe_O2 : benchmarking.c
	gcc -o benchmarking_exe_O2 benchmarking.c -O2 && ./benchmarking_exe_O2

benchmarking_exe_O3 : benchmarking.c
	gcc -o benchmarking_exe_O3 benchmarking.c -O3 && ./benchmarking_exe_O3

benchmarking_exe_Os : benchmarking.c
	gcc -o benchmarking_exe_Os benchmarking.c -Os && ./benchmarking_exe_Os

benchmarking_exe_Og : benchmarking.c
	gcc -o benchmarking_exe_Og benchmarking.c -Og && ./benchmarking_exe_Og
