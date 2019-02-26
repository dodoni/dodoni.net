Release notes:

01.xx.2019: 0.1

After a long period of inactivity the Dodoni.net project was relaunched.

* Migration of code and documentation from codeplex.com to github.com 
* Change license to MIT license
* Most of the assemblies are now based on .net Standard.
* change versioning scheme
* The wrapper for the following libraries has been migrated, but:
  - Yeppp!: last change on https://bitbucket.org/MDukhan/yeppp was 2016, official homepage is down.
  - LibM: is still active on https://developer.amd.com/amd-cpu-libraries/amd-math-library-libm/, but available for Ubuntu only; one may think about a wrapper for OpenLibm instead
  - ACML (AMD Core Math Library): state unclear


01.07.2015: 1.0.0 preview 8

* Dodoni.BasicMathLibrary: (stable, release candidate)
  - add interface structure for special functions (erf(x), 1F1(x) etc.) similar to BLAS, LAPACK etc., i.e. a external library can be used
  - improve interface for probability distribution functions [includes estimator]
  - NormalDistribution and LogNormalDistribution moved to Dodoni.MathLibrary, instead a StandardNormalDistribution has been established, where the commulative distribution function etc. is taken from the special functions API
  - add interface structure for one-dimensional root finder algorithms

* Dodoni.ExternalMathLibrary.Yeppp:
  - this new assembly serves a wrapper for the Yeppp! library 

* Dodoni.CommonMathLibrary: ("Dodoni.MathLibrary" has been renamed to "Dodoni.CommonMathLibrary")
  - start to implement some special functions (erf(x), erfc(x), 1F1(x), W(x) etc.) which are needed for the improved implementation of (Log)NormalDistribution etc.
  - add further probability distributions 

etc.


11.09.2014: 1.0.0 preview 7

* Dodoni.BasicMathLibrary:
 - improve LAPACK interface structure, add further LAPACK routines (further on demand)
 - improve/bugfixing DenseMatrix, GeneralBandMatrix etc.
 - improve interface structure for correlation matrix factory (Pseudo-Sqrt matrix)
 - improve interface structure for optimizer
 - further bugfixing and unit tests

* Dodoni.FinanceBasics:
 - add further Implied Black/Bachelier volatility algorithms

* Dodoni.MathLibrary:
 - add the following optimizer: 
   o 1-dimensional: Brent, Golden Search, Simulated Annealing
   o Multi-dimensional: Nelder-Mead, Powell, PRAXIS
 - further bugfixing and unit tests

* Dodoni.ExternalMathLibrary.NLopt:
 - bugfixing

* Dodoni.XLBasicComponents:
 - upgrade to ExcelDNA 0.32

etc. 

More than 8000 unit tests.


31.12.2013: 1.0.0 preview 6

* Dodoni.BasicComponents: (stable, release candiate)
 - some bugfixing

* Dodoni.BasicLowLevelMathLibrary:
 - has been removed (reorganized, see BasicMathLibrary)

* Dodoni.BasicMathLibrary: (stable, some features are missing)
 - contains the infastructure for BLAS, LAPACK, FFT but as well for numerical integration, interpolation, optimizer etc.

* Dodoni.MathLibrary.Native.ACML: (stable, release candidate)
 - native wrapper for AMD's Core Math Library

* Dodoni.MathLibrary.Native.BLAS: (stable, release candidate)
 - native wrapper for BLAS library (Fortran interface)

* Dodoni.MathLibrary.Native.CBLAS: (stable, release candidate)
 - native wrapper for BLAS library (C interface)

* Dodoni.MathLibrary.Native.FFTW: (stable, release candidate)
 - native wrapper for FFTW library 

* Dodoni.MathLibrary.Native.MKL: (stable, release candidate)
 - native wrapper for Intel's MKL library

* Dodoni.MathLibrary.Native.NLopt: (stable, release candidate)
 - native wrapper for NLopt library

* Dodoni.MathLibrary: (alpha)
 - contains some numerical integration routines etc.


* Dodoni.FinanceBasics: (beta)
* Dodoni.FinanceCommonMarketUsages: (alpha, contains a few holiday calendar etc. only)
* Dodoni.XLBasicComponents: (stable, release candidate)
* Dodoni.XLIntegrationGnuplot: (beta - it is a simple example for the use of Dodoni.XlBasicComponents only)

More than 7200 unit tests.


17.02.2013: 1.0.0 preview 5

* Dodoni.BasicComponents:
 - bugfixing configuration file(s)

* Dodoni.BasicLowLevelMathLibrary:
 - managed implementation of BLAS established, unit test for level 1, 2, 3 and bugfixing of the native BLAS wrapper
 - start to establish a new LAPACK interface (not yet ready)

minor bugfixes in the other projects; especially the packaging into one xll file works now and is part of the distribution.


16.12.2012: 1.0.0 preview 4

* Dodoni.BasicLowLevelMathLibrary (beta):
 - use System.Numerics.Complex instead of an individual implementation
 - add vector unit operations (support of MKL only)
 - Fast Fourier Transformation ready (including Fractional FFT); except the real FFT using AMD's ACML
 - BLAS level 1 includes a managed implementation [BLAS level 2, level 3 are untested, the LAPACK interfaces will be changed] 
 - add unit tests (now more than 3.000 unit tests in total!)

minor bugfixes in the other projects, upgrade to ExcelDNA 0.30 for the Excel interface.
 

03.10.2012: 1.0.0 preview 3

Establish further projects, add unit tests and do some bug fixing

* Dodoni.BasicComponents (stable, release candidate)
* Dodoni.BasicLowLevelMathLibrary (alpha)
* Dodoni.FinanceBasics (beta)
* Dodoni.FinanceCommonMarketUsages (alpha, contains a few holiday calendar etc. only)
* Dodoni.XLBasicComponents (stable, release candidate)
* Dodoni.XLIntegrationGnuplot (alpha - but it is a simple example for the use of Dodoni.XlBasicComponents only)

The source code of the Dodoni.BasicMathLibrary project as well as the Dodoni.XlIntegration project that contains several Excel UDF's 
are not published yet. Both projects are quite incomplete and the API is still under construction. To allow the use of some functionality 
in Excel, the binaries are added to the distribution of Dodoni.net. 
If you want to compile the source code, there are a few dependencies to the Dodoni.BasicMathLibrary assembly only, mainly some constants and 
for example the distribution function of the normal distribution.


05.05.2012
Publish the projects:

 + Dodoni.BasicComponents
 + Dodoni.XLBasicComponents
 + Dodoni.XLGuidedTour

on Codeplex.com; 1.0.0 preview 2


21.08.2011
First public release (binary only); 1.0.0 preview 1