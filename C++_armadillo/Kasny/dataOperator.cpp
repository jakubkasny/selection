#include "dataOperator.h"
#include <fstream>
#include <iostream>
#include <armadillo>
#include <vector>
#include <list>
#include "PeakFinder.h"


using namespace std;
using namespace arma;

// Funkce:
vector<double> zpracujMean(vector<pair<int, int>> intervalyDat, vector<double> dataM);
vector<double> zpracujVar(vector<pair<int, int>> intervalyDat, vector<double> dataM);
vector<double> najdiPeak(vector<pair<int, int>> intervalyDat,vector<double> dataM);
pair<int, int> zadejInterval(int intervalType, vector<int> tmStmp);
void saveOutput(int metoda, vector<pair<int, int>> intervalyDat, string dataName, string outputFile, vector<double> processedData);
bool tryParse(string, int&);
int getIndex(string, vector<int>);
int getIndex(string); 
double  mEAN(vec subvector);

// Metody:
vector<double> dataOperator::getData()
{
	return dataOperator::data;
}

vector<int> dataOperator::getTimeStamp()
{
	return dataOperator::timeStamp;
}

string dataOperator::getOutputFile()
{
	return dataOperator::outputFile;
}

void dataOperator::nactiData()
{
	bool out = false;
	string cesta;
	string hlaska;
	do
	{
		cout << "Zadejte nazev souboru i s priponou (soubor musi byt ve slozce projektu): ";
		cin >> cesta;
		ifstream classfile(cesta);
		
		
		vector<string> data;
		string line;
		vector<int> timeStampM;
		vector <double> dataM;
		string sTS;
		string sD;

		if (!getline(classfile, line, '\n'))
		{
			out = false;
			cout << "\nDoslo k problemu pri nacitani ze souboru." << endl;
		}
		else
		{
			out = true;
			while (getline(classfile, line, '\n'))
			{
				sTS = line.substr(0, line.find(","));
				sD = line.substr(line.find(",") + 1, line.length());
				try
				{
					timeStampM.push_back(stoi(sTS));
					dataM.push_back(stod(sD));
				}
				catch (const std::exception&)
				{
				}
			}
			dataOperator::data = dataM;
			dataOperator::timeStamp = timeStampM;
			dataOperator::dataName = cesta;
		}
	} while (!out);
	

	cout << "Data byla uspesne nactena.\n";
	cout << endl;
	cout << "Vystup programu bude automaticky ulozen do .csv souboru." << endl;
	hlaska = "Chcete zadat nazev souboru pro ukladani vystupu ze zpracovani? [Ano=1/Ne=0]: ";
	vector<int> povoleneId = { 0,1 };
	int inx = getIndex(hlaska, povoleneId);
	string soubor;
	
	if (inx == 1)	
	{
		do
		{
			cout << "Zadejte nazev souboru bez pripony(napr.: vystup): ";
			cin >> soubor;
			cin.get();

			if (soubor.find(",") != string::npos || soubor.find(".") != string::npos)
			{
				out = false;
				cout << "\n V nazvu souboru se objevila tecka, nebo carka. Prosim opravte tuto chybu.\n";
			}
			else
			{
				soubor = soubor + ".csv";
				out = true;
			}
		} while (!out);
	}
	else
		soubor = cesta.substr(0, cesta.find(".")) + "Out.csv";
	

	dataOperator::outputFile = soubor;

	ofstream outFile;	
	outFile.open(soubor, ios::app);
	outFile << "";
	outFile.close();
	cout << "Soubor pro ukladani dat byl uspesne vytvoren, nebo nalezen.\n";
	cout << "Pokracujte stisknutim klavesy enter...";
	cin.get();

	return;
}

void dataOperator::zpracuj(int metoda, int delkaDat)
{
	system("CLS");
	cout << "Menu: " << "2." << metoda << "." << delkaDat << endl;

	// Nacti datove intervaly pro zpracovani: 
	vector<pair<int, int>> intervalyDat;
	pair<int, int> intervalDat(0, data.size());
	int intervalType;
	string hlaska;
	vector<int> povoleneId = { 1,2 };
	int intervalCount;

	switch (delkaDat)
	{
	case 1:
		intervalyDat.push_back(intervalDat);
		break;

	case 2:
		hlaska = "Jakym zpusobem chcete zadavat body intervalu? \n[1] Pomoci timeStamp. \n[2] Pomoci poradi v datove rade.\n";		
		intervalType = getIndex(hlaska, povoleneId);
		intervalyDat.push_back(zadejInterval(intervalType, dataOperator::timeStamp));
		break;

	case 3:
		hlaska = "Jakym zpusobem chcete zadavat body intervalu? \n[1] Pomoci timeStamp. \n[2] Pomoci poradi v datove rade.\n";
		intervalType = getIndex(hlaska, povoleneId);

		hlaska = "Zadejte pocet zadavanych intervalu: ";
		intervalCount = getIndex(hlaska);
		for (size_t i = 0; i < intervalCount; i++)
			intervalyDat.push_back(zadejInterval(intervalType, dataOperator::timeStamp));
		break;
	}

	// Zpracuj datovou radu:
	vector<double> helper;
	vector<double> prsdData;
	vec subvector;
	int i = 0;

	switch (metoda)
	{
	case 1:
		processedData = zpracujMean(intervalyDat,dataOperator::data);
		break;
	case 2:
		processedData = zpracujVar(intervalyDat, dataOperator::data);
		break;

	case 3:
		processedData = najdiPeak(intervalyDat,dataOperator::data);
		break;
	}


//	cout << "Data byla uspesne zpracovana.\n";
//	cout << "Probiha ukladani dat ... \n";
	
	saveOutput(metoda, intervalyDat, dataName, outputFile, processedData);
	
	cout << "Dala byla ulozena.\n";
	cout << "Pokracujte stisknutim klavesy enter...";
	cin.get();

	system("CLS");
	return;
}

//========================================================================
// Definice funkcí:
// Funkce pro zpracovani stredni hodnoty:
vector<double> zpracujMean(vector<pair<int, int>> intervalyDat, vector<double> dataM)
{
	vector<double> prsdData = {};
	vector<double> helper;
	vec subvector;
	double dh;

	for (pair<int,int> interv : intervalyDat)
	{
		if (interv.second != dataM.size())
			interv.second++;
		helper = { dataM.begin() + interv.first, dataM.begin() + interv.second };
		subvector = helper;
		prsdData.push_back(mean(subvector));
		helper.clear();
	}

	return prsdData;
}

// Funkce pro zpracovani rozptylu:
vector<double> zpracujVar(vector<pair<int, int>> intervalyDat, vector<double> dataM)
{
	vector<double> prsdData = {};
	vector<double> helper;
	vec subvector;

	for (pair<int, int> interv : intervalyDat)
	{
		helper = { dataM.begin() + interv.first, dataM.begin() + interv.second };
		subvector = helper;
		prsdData.push_back(var(subvector));
		helper.clear();
	}
	return prsdData;
}

// Funkce pro nalezeni peaku:
vector<double> najdiPeak(vector<pair<int, int>> intervalyDat, vector<double> dataM)
{
	vector<int> out;
	pair<int, int> interv = intervalyDat[0];
	vector<double> sub = { dataM.begin() + interv.first, dataM.begin() + interv.second };
	PeakFinder::findPeaks(sub, out, true); 
	vector<double> prsdData(out.begin(), out.end());
	return prsdData;
}
//-----------------------------------------------------------------

// Funkce pro ulozeni do souboru
void saveOutput(int metoda, vector<pair<int, int>> intervalyDat, string dataName, string outputFile, vector<double> processedData)
{
	string metody[3] = { "stredni hodnota","rozptyl","vrcholy" };
	cout << "\nVysledek:" << endl;

	ofstream soubor;
	soubor.open(outputFile, ios::app);
	if (soubor.is_open())
	{
		cout << "data set: " << dataName << "  |  " << "metoda: " << metody[metoda - 1] << endl;
		soubor << "data set: " << dataName << "  |  " << "metoda: " << metody[metoda - 1] << endl;
		
		for (size_t i = 0; i < intervalyDat.size(); i++)
		{
			soubor << "interval: [" << to_string(intervalyDat[i].first) << " , " << to_string(intervalyDat[i].second) << "]" << endl;
			cout << "interval: [" << to_string(intervalyDat[i].first) << " , " << to_string(intervalyDat[i].second) << "]" << endl;

			if (metoda == 3)
			{
				for (double data : processedData)
				{
					if (data == processedData[processedData.size() - 1])
					{
						cout << (int)data;
						soubor << (int)data;
					}
					else
					{
						cout << (int)data << " | ";
						soubor << (int)data << " | ";
					}
						
				}
				soubor << endl;
				cout << endl;
			}
			else
			{
				cout << processedData[i] << endl;
				soubor << processedData[i] << endl;
			}
				
		}
	}
	else
		cout << "Pri otevirani souboru doslo k chybe.";

	soubor.close();
	return;
}


// Funkce pro nacitani intervalu:
pair<int, int> zadejInterval(int intervalType,vector<int> tmStmp)
{
	pair<int, int> interval;
	int iinx;
	string inx;
	bool exc = true;
	string hlaska;

	// nacti levy krajni bod intervalu:
	do
	{
		hlaska = "Zadejte levy bod intervalu: ";
		iinx = getIndex(hlaska);

		// najdi jeho index pomoci timeStamp
		if (intervalType == 1)
		{
			if (!(find(tmStmp.begin(), tmStmp.end(), iinx) != tmStmp.end()))
			{
				cout << "Levy bod intervalu v timeStamp neexistuje.\n";
				iinx = -1;
			}
			else
				iinx = find(tmStmp.begin(), tmStmp.end(), iinx) - tmStmp.begin();
		}
		// najdi index
		if (intervalType == 2)
		{
			if (iinx<0 || iinx> tmStmp.size())
			{
				cout << "Levy bod intervalu neni pripustny.\n";
				iinx = -1;
			}
		}
	} while (iinx < 0);
	interval.first = iinx;
	
	// nacti pravy krajni bod intervalu:
	do
	{
		hlaska = "Zadejte pravy bod intervalu: ";
		iinx = getIndex(hlaska);

		// najdi jeho index pomoci timeStamp
		if (intervalType == 1)
		{
			if (!(find(tmStmp.begin(), tmStmp.end(), iinx) != tmStmp.end()))
			{
				cout << "Pravy bod intervalu v timeStamp neexistuje.\n";
				iinx = -1;
			}
			else
				iinx = find(tmStmp.begin(), tmStmp.end(), iinx) - tmStmp.begin();
		}
		if (intervalType == 2)
		{
			if (iinx<0 || iinx> tmStmp.size())
			{
				cout << "Pravy bod intervalu neni pripustny.\n";
				iinx = -1;
			}
		}
		if (iinx > -1 && iinx <= interval.first)
		{
			iinx = -1;
			cout << "Pravy index musi byt vetsti, nez levy.\n";
			
			hlaska = "Prejete si opravit levy bod intervalu? [Ano=1/Ne=0]: ";
			vector<int> povoleneId = { 0,1 };
			int repair = getIndex(hlaska, povoleneId);
			if (repair == 1)
			{
				return zadejInterval(intervalType, tmStmp);
			}
		}
			
	} while (iinx < 0);
	interval.second = iinx;

	return interval;
}
