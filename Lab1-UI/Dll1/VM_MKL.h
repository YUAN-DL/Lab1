//#pragma once
//#include "mkl.h"
//
//extern "C"  _declspec(dllexport)
//void VM_Sqrt_Double(MKL_INT n, double* x, double* y, int& ret);
//
//extern "C"  _declspec(dllexport)
//void VM_Sqrt_Single(MKL_INT n, float* x, float* y, int& ret);
#pragma once
#include"pch.h"
#include "mkl.h"
#include <time.h>
extern "C" _declspec(dllexport)
void VMS_LN(const MKL_INT n, float* a, float* y_HA, float* y_EP, double* time, int& ret);
extern "C" _declspec(dllexport)
void VMD_LN(const MKL_INT n, double* a, double* y_HA, double* y_EP, double* time, int& ret);
extern "C" _declspec(dllexport)
void VMSL_Gamma(const MKL_INT n, float* a, float* y_HA, float* y_EP, double* time, int& ret);
extern "C" _declspec(dllexport)
void VMDL_Gamma(const MKL_INT n, double* a, double* y_HA, double* y_EP, double* time, int& ret);