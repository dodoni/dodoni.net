/* MIT License
Copyright (c) 2011-2019 Markus Wendt (http://www.dodoni-project.net)

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 
Please see http://www.dodoni-project.net/ for more information concerning the Dodoni.net project. 
*/
using System;

using Dodoni.MathLibrary;
using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics
{
    //public class DenseMatrixExponentialTaylorSeries : DenseMatrix.IExponential
    //{
    //    #region public constructors

    //    public DenseMatrixExponentialTaylorSeries(int order)
    //    {
    //    }
    //    #endregion

    //    #region public properties

    //    #region IIdentifierNameable Members

    //    public IdentifierString LongName
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public IdentifierString Name
    //    {
    //        get { throw new NotImplementedException(); }
    //    }
    //    #endregion

    //    #region IAnnotatable Members

    //    public string Annotation
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public bool HasReadOnlyAnnotation
    //    {
    //        get { throw new NotImplementedException(); }
    //    }
    //    #endregion

    //    public int Order
    //    {
    //        get;
    //        private set;
    //    }
    //    #endregion

    //    #region IExponential Members

    //    public DenseMatrixRealScalarFunction Create(DenseMatrix a)
    //    {
    //        if (a == null)
    //        {
    //            throw new ArgumentNullException("a");
    //        }
    //        if (a.RowCount != a.ColumnCount)
    //        {
    //            throw new ArgumentException("a");
    //        }
    //        int d = a.RowCount;
    //        int n = d * d;

    //        return (t, data, work) =>
    //        {
    //            return GetTruncatedTaylorSeries(a, data, work, t);
    //        };
    //    }


    //    public DenseMatrix GetValue(DenseMatrix a, double[] data, double[] work)
    //    {
    //        if (a == null)
    //        {
    //            throw new ArgumentNullException("a");
    //        }
    //        if (a.RowCount != a.ColumnCount)
    //        {
    //            throw new ArgumentException("a");
    //        }
    //        return GetTruncatedTaylorSeries(a, data, work);
    //    }

    //    public DenseMatrix GetValue(DenseMatrix a)
    //    {
    //        if (a == null)
    //        {
    //            throw new ArgumentNullException("a");
    //        }
    //        if (a.RowCount != a.ColumnCount)
    //        {
    //            throw new ArgumentException("a");
    //        }
    //        int n = a.RowCount * a.ColumnCount;

    //        return GetTruncatedTaylorSeries(a, data: new double[n], work: new double[n]);
    //    }
    //    #endregion

    //    #region IAnnotatable Members

    //    public bool TrySetAnnotation(string annotation)
    //    {
    //        return false;
    //    }
    //    #endregion

    //    #region private methods


    //    private DenseMatrix GetTruncatedTaylorSeries(DenseMatrix a, double[] data, double[] work, double factor = 1.0)
    //    {
    //        int d = a.RowCount;
    //        int n = d * d;

    //        var expOfA = new DenseMatrix(d, d, data);   // = I + t * A + 1/2! * (t*A)^2 + 1/3! * (t*A)^3 + ..., initialized with identity matrix I
    //        BLAS.Level1.dscal(n, 0.0, data);
    //        for (int j = 0; j < d; j++)
    //        {
    //            data[j * (1 + d)] = 1.0;
    //        }

    //        var scaledPowerMatrix = new DenseMatrix(d, d, data, createDeepCopyOfArgument: false);  // = 1/j! * (t*A)^j
    //        BLAS.Level1.dcopy(n, a.Data, data);
    //        BLAS.Level1.dscal(n, factor, data);

    //        for (int j = 1; j < Order; j++)
    //        {
    //            VectorUnit.Basics.Add(d * d, expOfA.Data, scaledPowerMatrix.Data);

    //            BLAS.Level3.dgemm(d, d, d, factor / SpecialFunction.Gamma.Factorial[j + 1], a.Data, scaledPowerMatrix.Data, SpecialFunction.Gamma.Factorial[j], scaledPowerMatrix.Data);
    //        }
    //        return expOfA;
    //    }
    //    #endregion

    //    #region IExponential Members


    //    public int GetWorkspaceLength(int dimension)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion
    //}
}