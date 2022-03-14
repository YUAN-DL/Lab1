#include"pch.h"
#include <iostream>
#include <time.h>
#include <mkl.h>
using namespace std;
extern "C" _declspec(dllexport)
void VMS_LN(const MKL_INT n, float* a, float* y_HA, float* y_EP, double* time, int& ret)
{
	try
	{
		clock_t time_start_HA = clock();
		vmsLn(n, a, y_HA, VML_HA);
		clock_t time_end_HA = clock();
		time[0] = (time_end_HA - time_start_HA) * 1.0 / CLOCKS_PER_SEC * 1000;
		clock_t time_strat_EP = clock();
		vmsLn(n, a, y_EP, VML_EP);
		clock_t time_end_EP = clock();
		time[1] = (time_end_EP - time_strat_EP) * 1.0 / CLOCKS_PER_SEC * 1000;
		ret = 0;
	}
	catch (...)
	{
		ret = -1;
	}
}
extern "C" _declspec(dllexport)
void VMD_LN(const MKL_INT n, double* a, double* y_HA, double* y_EP, double* time, int& ret)
{
	try
	{
		clock_t time_start_HA = clock();
		vmdLn(n, a, y_HA, VML_HA);
		clock_t time_end_HA = clock();
		time[0] = (time_end_HA - time_start_HA) * 1.0 / CLOCKS_PER_SEC * 1000;
		clock_t time_strat_EP = clock();
		vmdLn(n, a, y_EP, VML_EP);
		clock_t time_end_EP = clock();
		time[1] = (time_end_EP - time_strat_EP) * 1.0 / CLOCKS_PER_SEC * 1000;
		ret = 0;
	}
	catch (...)
	{
		ret = -1;
	}
}

extern "C" _declspec(dllexport)
void VMSL_Gamma(const MKL_INT n, float* a, float* y_HA, float* y_EP, double* time, int& ret)
{
	try
	{
		clock_t time_start_HA = clock();
		vmsLGamma(n, a, y_HA, VML_HA);
		clock_t time_end_HA = clock();
		time[0] = (time_end_HA - time_start_HA) * 1.0 / CLOCKS_PER_SEC * 1000;

		clock_t time_strat_EP = clock();
		vmsLGamma(n, a, y_EP, VML_EP);
		clock_t time_end_EP = clock();
		time[1] = (time_end_EP - time_strat_EP) * 1.0 / CLOCKS_PER_SEC * 1000;

		ret = 0;
	}
	catch (...)
	{
		ret = -1;
	}
}
extern "C" _declspec(dllexport)
void VMDL_Gamma(const MKL_INT n, double* a, double* y_HA, double* y_EP, double* time, int& ret)
{
	try
	{
		clock_t time_start_HA = clock();
		vmdLGamma(n, a, y_HA, VML_HA);
		clock_t time_end_HA = clock();
		time[0] = (time_end_HA - time_start_HA) * 1.0 / CLOCKS_PER_SEC * 1000;
		clock_t time_strat_EP = clock();
		vmdLGamma(n, a, y_EP, VML_EP);
		clock_t time_end_EP = clock();
		time[1] = (time_end_EP - time_strat_EP) * 1.0 / CLOCKS_PER_SEC * 1000;
		ret = 0;
	}
	catch (...)
	{
		ret = -1;
	}
}