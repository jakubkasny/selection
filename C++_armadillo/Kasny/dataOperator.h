#pragma once
#include <string>
#include <vector>
#include <iostream>

using namespace std;

class dataOperator
{
	
	// atributy
private:
	string dataName;
	string outputFile;
	vector<double> data;
	vector<int> timeStamp;
	vector<double> processedData;

	   // metody:
public: 
	void nactiData();
	void zpracuj(int metoda, int delkaDat);
	
vector<double> getData();
vector<int> getTimeStamp();
string getOutputFile();

};
