// Kasny.cpp : Tento soubor obsahuje funkci main. Provádění programu se tam zahajuje a ukončuje.
//

#include <iostream>
#include <vector>
#include <string>
#include "dataOperator.h"
#include "armadillo"

using namespace std;
using namespace arma;

bool tryParse(string input, int& output)
{
	bool exc;
	try
	{
		output = stoi(input);
		exc = true;
	}
	catch (const std::exception&)
	{
		cout << "Nebyl zadan spravny vstup. Ujistete se, ze zadavate prirozene cislo.\n";
		exc = false;
	}
	return exc;
}

int getIndex(string hlaska, vector<int> povoleneId)
{
	string inx;
	bool exc;
	int iinx;

	do
	{
		cout << hlaska;
		cin >> inx;
		cout << endl;
		exc = tryParse(inx, iinx);
		if (exc)
		{
			if (!(find(povoleneId.begin(), povoleneId.end(), iinx) != povoleneId.end()))
			{
				cout << "Zadany index neni pripustny.\n";
				exc = false;
			}
		}
	} while (!exc);
	cin.get();
	return iinx;
}

int getIndex(string hlaska)
{
	string inx;
	bool exc;
	int iinx;

	do
	{
		cout << hlaska;
		cin >> inx;
		cout << endl;
		exc = tryParse(inx, iinx);
	} while (!exc);
	cin.get();
	return iinx;
}

double getMean(dataOperator op)
{
	vector<double> data = op.getData();
	vector<double> helper = { data.begin() , data.begin() + data.size() };
	vec subvector = helper;
	return mean(subvector);
}

int main()
{
	vector<int> menuInx = { 0 , 0 , 0 };
	bool ukoncit = false;
	bool exc;
	string inx;
	int iinx;
	string hlaska;

	dataOperator op;

	system("CLS");
	cout << "PROGRAM NA ZPRACOVANI DAT" << endl;
	cout << "-------------------------\n" << endl;
	// Menu
	hlaska = "Menu: \n[1] Nacist data. \n[2] Ukoncit program.\n";
	vector<int> povoleneId = { 1,2 };
	menuInx[0] = getIndex(hlaska, povoleneId);
	
	system("CLS");
	cout << "Menu: " << menuInx[0] << "." << endl;

	switch (menuInx[0])
	{
	case 1:
		// Volba 1.:
		op.nactiData();
		break;

	case 2:
		// Volba 3.:
		return 10;
	}
	
	double mm = getMean(op);


	do
	{	
		system("CLS");
		cout << "PROGRAM NA ZPRACOVANI DAT" << endl;
		cout << "-------------------------\n" << endl;
		// Menu
		hlaska = "Menu: \n[1] Nacist data. \n[2] Zpracovavat data. \n[3] Ukoncit program.\n";
		povoleneId.push_back(3);
		menuInx[0] = getIndex(hlaska,povoleneId);

		system("CLS");
		cout << "Menu: " << menuInx[0] <<"." << endl;

		switch (menuInx[0])
		{
		case 1:	
			// Volba 1.:
			op.nactiData();
			break;

		case 2:
			// Volba 2.:
			hlaska = "[1] Stredni hodnota. \n[2] Rozptyl. \n[3] Nalezeni peaku. \n";
			menuInx[1] = getIndex(hlaska, povoleneId);

			system("CLS");
			cout << "Menu: " << menuInx[0] << "." << menuInx[1] << "." << endl;

			switch (menuInx[1])
			{
			case 1:
			case 2:
				//Volba 2.2./2.1.:
				hlaska = "Zadejte kterou cast dat chcete zpracovavat:\n[1] Celou.\n[2] Jednu cast. \n[3] Vice casti. \n";
				menuInx[2] = getIndex(hlaska, povoleneId);
				op.zpracuj(menuInx[1], menuInx[2]);
				break;
			case 3:
				//Volba 2.3.:
				hlaska = "Zadejte kterou cast dat chcete zpracovavat:\n[1] Celou.\n[2] Jednu cast. \n";
				povoleneId.pop_back();
				menuInx[2] = getIndex(hlaska, povoleneId);
				op.zpracuj(menuInx[1], menuInx[2]);
				break;
			}

			break;

		case 3:
			// Volba 3.:
			ukoncit = true;
		}
		
	} while (!ukoncit);
	return 10;
}
/*
	PROGRAM NA ZPRACOVANI DAT
	Menu:
	[1] nacist data
	[2] zpracovat data
	[3] ukoncit program
	read: <-

	[1]:
	- Zadejte jmeno souboru s daty:
		- Chcete ukladat vysledky do Vami zvoleneho souboru? [A=1/N=0]:
		[1]: read <-

	[2]:
		[2.1] stredni hodnota
			[2.1.1] Jakou cast dat chcete zpracovat? // Obecna funkce, kterou budu volat na barco
				[2.1.1.1] Celou.
				[2.1.1.2] Cast. => read <-
				[2.1.1.3] Vice casti. => read <-

		[2.2] rozptyl
			[] -||- stejna struktura

		[2.3] nalezni vrcholy dat
			[] -||-
			- Kolik vrcholu chcete hledat?  <- read


	V kazde metode: ulozDoSouboru(int dataID, string nazevOperace, string delkaDat ,list<double> vystupniHodnoty) // delkaDat - int/DateTime/timestamp/ neco na ten zpusob ???


	*/