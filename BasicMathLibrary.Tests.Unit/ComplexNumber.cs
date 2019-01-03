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
using System.Text;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Dodoni.MathLibrary.Basics
{
    /// <summary>Represents a complex number.
    /// </summary>
    /// <remarks>This implementation has been made under .NET 3.5. Since .NET 4.0 the .NET framework contains
    /// an implementation of complex numbers as well. This class is used for some unit tests only. You can copy this 
    /// file to a .NET 3.5 project and use this implementation for your purpose.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]  // was used for calling external DLL's (BLAS etc.) before
    public struct ComplexNumber
    {
        #region static constants

        /// <summary>Represents <c>i</c>.
        /// </summary>
        public readonly static ComplexNumber I = new ComplexNumber(0, 1);

        /// <summary>Represents <c>1.0</c> in some <see cref="Complex"/> representation.
        /// </summary>
        public readonly static ComplexNumber One = new ComplexNumber(1, 0);

        /// <summary>Represents <c>0.0</c> in some <see cref="Complex"/> representation.
        /// </summary>
        public readonly static ComplexNumber Zero = new ComplexNumber(0, 0);

        /// <summary>Represents complex NaN, i.e. 'Not a Number'.
        /// </summary>
        /// <remarks>
        /// <para><c>NaN</c> is returned when the result of an operation involving complex numbers is
        /// undefined. Use <see cref="IsNaN(ComplexNumber)"/> to determine whether a value is 'NaN'.
        /// It is not possible to determine whether a value is 'NaN' by comparing it
        /// to another value equal to <c>NaN</c>.</para>
        /// </remarks>
        public readonly static ComplexNumber NaN = new ComplexNumber(double.NaN, double.NaN);
        #endregion

        #region public members

        /// <summary>The real part.
        /// </summary>
        public readonly double RealPart;

        /// <summary>The imaginary part.
        /// </summary>
        public readonly double ImaginaryPart;
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ComplexNumber"/> struct.
        /// </summary>
        /// <param name="realPart">The real part.</param>
        public ComplexNumber(double realPart)
        {
            RealPart = realPart;
            ImaginaryPart = 0.0;
        }

        /// <summary>Initializes a new instance of the <see cref="ComplexNumber"/> struct.
        /// </summary>
        /// <param name="realPart">The real part.</param>
        /// <param name="imaginaryPart">The imaginary part.</param>
        public ComplexNumber(double realPart, double imaginaryPart)
        {
            RealPart = realPart;
            ImaginaryPart = imaginaryPart;
        }

        /// <summary>Initializes a new instance of the <see cref="ComplexNumber"/> struct.
        /// </summary>
        /// <param name="z">The complex number.</param>
        public ComplexNumber(ComplexNumber z)
        {
            RealPart = z.RealPart;
            ImaginaryPart = z.ImaginaryPart;
        }
        #endregion

        #region public properties

        /// <summary>Gets the argument of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <value>The argument of the <see cref="ComplexNumber"/>, in radians.</value>
        /// <remarks>The argument of a complex number is the angle between the positive real axis
        /// and a line from the origin to complex number, measured counter-clockwise. This is an element
        /// of ]-\pi,\pi]. If the given complex number is 0 <see cref="Double.NaN"/> will be returned.
        /// This property returns the same value as <see cref="Angle(ComplexNumber)"/> applied to the current instance.</remarks>
        public double Argument
        {
            get
            {
                if ((Math.Abs(RealPart) <= MachineConsts.Epsilon) && (Math.Abs(ImaginaryPart) <= MachineConsts.Epsilon))
                {
                    return Double.NaN;
                }
                return Math.Atan2(ImaginaryPart, RealPart);
            }
        }

        /// <summary>Gets the modulus or absolute value of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <value>The modulus of a <see cref="ComplexNumber"/>.</value>
        /// <remarks>The modulus of a complex number is the square root of the sum of the squares of its
        /// real and imaginary components and is equal to its absolute value. Therefore the return value
        /// is the same as the result of <see cref="Abs(ComplexNumber)"/> applied to the current instance.</remarks>
        public double Modulus
        {
            get
            {
                double x = Math.Abs(RealPart);
                double y = Math.Abs(ImaginaryPart);

                if (x <= MachineConsts.Epsilon)
                {
                    return y;
                }
                else if (y <= MachineConsts.Epsilon)
                {
                    return x;
                }
                else if (x > y)
                {
                    double temp1 = y / x;
                    return x * Math.Sqrt(1.0 + temp1 * temp1);
                }
                else
                {
                    double temp2 = x / y;
                    return y * Math.Sqrt(1.0 + temp2 * temp2);
                }
            }
        }
        #endregion

        #region public methods

        /// <summary>Gets a value indicating whether this complex number is real.
        /// </summary>
        /// <param name="tolerance">The (positive) tolerance.</param>
        /// <returns>
        /// 	<c>true</c> if the the complex represented by the current instance is real, i.e. the absolute value of the imaginary part is less or equal
        /// 	<paramref name="tolerance"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool IsReal(double tolerance)
        {
            return (Math.Abs(ImaginaryPart) <= tolerance);
        }

        /// <summary>Gets a value indicating whether this complex number is pure imaginary.
        /// </summary>
        /// <param name="tolerance">The (positive) tolerance.</param>
        /// <value>
        /// 	<c>true</c> if this instance is pure imaginary, i.e. the absolute value of the real part is less or
        /// 	equal <paramref name="tolerance"/>; otherwise, <c>false</c>.
        /// </value>
        public bool IsImaginary(double tolerance)
        {
            return (Math.Abs(RealPart) <= tolerance);
        }

        /// <summary>Gets a value indicating whether this complex number is zero.
        /// </summary>
        /// <param name="tolerance">The (positive) tolerance.</param>
        /// <value><c>true</c> if this instance is zero, i.e. the absolute value of the real as well as of the imaginary 
        /// part is less or equal <paramref name="tolerance"/>; otherwise, <c>false</c>.</value>
        public bool IsZero(double tolerance)
        {
            return ((Math.Abs(RealPart) <= tolerance) && (Math.Abs(ImaginaryPart) <= tolerance));
        }

        /// <summary>Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <remarks>No tolerances are taken into account, i.e. <c>true</c> will be returned if and only if the <see cref="RealPart"/> 
        /// and the <see cref="ImaginaryPart"/> are absolutly equal.</remarks>
        public override bool Equals(object obj)
        {
            if (obj is ComplexNumber)
            {
                ComplexNumber z = (ComplexNumber)obj;
                if ((RealPart == z.RealPart) && (ImaginaryPart == z.ImaginaryPart))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Returns a value indicating whether the current object and a specified <see cref="ComplexNumber"/> 
        /// number represent the same value, taking into account the tolerance <see cref="MachineConsts.Epsilon"/>.
        /// </summary>
        /// <param name="z">A <see cref="ComplexNumber"/> to compare to the current instance.</param>
        /// <returns>A value indicating whether the current instance and <paramref name="z"/> represents 
        /// the same value, taking into account the tolerance <see cref="MachineConsts.Epsilon"/>.</returns>
        public bool NumericalEquals(ComplexNumber z)
        {
            return (Math.Abs(RealPart - z.RealPart) <= MachineConsts.Epsilon) && (Math.Abs(ImaginaryPart - z.ImaginaryPart) <= MachineConsts.Epsilon);
        }

        /// <summary>Returns a value indicating whether the current object and a specified <see cref="Complex"/> 
        /// number represent the same value, taking into account the tolerance <see cref="MachineConsts.Epsilon"/>.
        /// </summary>
        /// <param name="z">A <see cref="ComplexNumber"/> to compare to the current instance.</param>
        /// <param name="tolerance">The (positive) tolerance to take into account.</param>
        /// <returns>A value indicating whether the current instance and <paramref name="z"/> represents 
        /// the same value, taking into account the tolerance <paramref name="tolerance"/>.</returns>
        public bool NumericalEquals(ComplexNumber z, double tolerance)
        {
            return (Math.Abs(RealPart - z.RealPart) <= tolerance) && (Math.Abs(ImaginaryPart - z.ImaginaryPart) <= tolerance);
        }

        /// <summary>Returns the complex number represented by the current object in its <see cref="System.Numerics.Complex"/> representation.
        /// </summary>
        /// <returns>The complex number in its <see cref="System.Numerics.Complex"/> representation.</returns>
        public System.Numerics.Complex AsSystemComplex()
        {
            return new System.Numerics.Complex(this.RealPart, this.ImaginaryPart);
        }

        /// <summary>Returns a <see cref="System.String"/> that represents the current <see cref="Complex"/> number.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current instance.
        /// </returns>
        public override string ToString()
        {
            return "{" + RealPart.ToString() + "; " + ImaginaryPart.ToString() + "i}";
        }

        /// <summary>Returns a <see cref="System.String"/> that represents the current <see cref="Complex"/> number.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <returns>A <see cref="System.String"/> that represents the current instance.
        /// </returns>
        public string ToString(string format)
        {
            return "{" + RealPart.ToString(format) + "; " + ImaginaryPart.ToString(format) + "i}";
        }
        #endregion

        #region public static methods

        #region operator methods

        #region adding methods

        /// <summary>Adds two <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The first <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The second <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="Complex"/> number that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.RealPart + b.RealPart, a.ImaginaryPart + b.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a real number.
        /// </summary>
        /// <param name="a">A real number.</param>
        /// <param name="b">A <see cref="ComplexNumber"/>.</param>        
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(double a, ComplexNumber b)
        {
            return new ComplexNumber(b.RealPart + a, b.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Single"/> number.
        /// </summary>
        /// <param name="a">A <see cref="System.Single"/> number.</param>
        /// <param name="b">A <see cref="ComplexNumber"/>.</param>        
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(System.Single a, ComplexNumber b)
        {
            return new ComplexNumber(b.RealPart + a, b.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Int16"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="System.Int16"/> integer.</param>
        /// <param name="b">A <see cref="ComplexNumber"/>.</param>        
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(Int16 a, ComplexNumber b)
        {
            return new ComplexNumber(b.RealPart + a, b.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Int32"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="System.Int32"/> integer.</param>
        /// <param name="b">A <see cref="ComplexNumber"/>.</param>        
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(Int32 a, ComplexNumber b)
        {
            return new ComplexNumber(b.RealPart + a, b.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Int64"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="System.Int64"/> integer.</param>
        /// <param name="b">A <see cref="ComplexNumber"/>.</param>        
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(Int64 a, ComplexNumber b)
        {
            return new ComplexNumber(b.RealPart + a, b.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a real number.
        /// </summary>
        /// <param name="a">A <see cref="ComplexNumber"/>.</param>
        /// <param name="b">A real number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(ComplexNumber a, double b)
        {
            return new ComplexNumber(a.RealPart + b, a.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Single"/> number.
        /// </summary>
        /// <param name="a">A <see cref="ComplexNumber"/>.</param>
        /// <param name="b">A <see cref="System.Single"/> number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(ComplexNumber a, System.Single b)
        {
            return new ComplexNumber(a.RealPart + b, a.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Int16"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="ComplexNumber"/>.</param>
        /// <param name="b">A <see cref="System.Int16"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(ComplexNumber a, Int16 b)
        {
            return new ComplexNumber(a.RealPart + b, a.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Int32"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="ComplexNumber"/>.</param>
        /// <param name="b">A <see cref="System.Int32"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(ComplexNumber a, Int32 b)
        {
            return new ComplexNumber(a.RealPart + b, a.ImaginaryPart);
        }

        /// <summary>Adds a <see cref="ComplexNumber"/> to a <see cref="System.Int64"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="ComplexNumber"/>.</param>
        /// <param name="b">A <see cref="System.Int64"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the sum of the two operands.</returns>
        public static ComplexNumber operator +(ComplexNumber a, Int64 b)
        {
            return new ComplexNumber(a.RealPart + b, a.ImaginaryPart);
        }
        #endregion

        #region subtracting methods

        /// <summary>Subtracts a <see cref="ComplexNumber"/> from another.
        /// </summary>
        /// <param name="a">The first <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The second <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the difference between <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.RealPart - b.RealPart, a.ImaginaryPart - b.ImaginaryPart);
        }

        /// <summary>Subtracts a real number from a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The real number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(ComplexNumber a, double b)
        {
            return new ComplexNumber(a.RealPart - b, a.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="System.Single"/> number from a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Single"/> number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(ComplexNumber a, System.Single b)
        {
            return new ComplexNumber(a.RealPart - b, a.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="System.Int16"/> integer from a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int16"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(ComplexNumber a, Int16 b)
        {
            return new ComplexNumber(a.RealPart - b, a.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="System.Int32"/> integer from a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int32"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(ComplexNumber a, Int32 b)
        {
            return new ComplexNumber(a.RealPart - b, a.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="System.Int64"/> integer from a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int64"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(ComplexNumber a, Int64 b)
        {
            return new ComplexNumber(a.RealPart - b, a.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="ComplexNumber"/> from a real number.
        /// </summary>
        /// <param name="a">A real number.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(double a, ComplexNumber b)
        {
            return new ComplexNumber(a - b.RealPart, -b.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="ComplexNumber"/> from a <see cref="System.Single"/> number.
        /// </summary>
        /// <param name="a">A <see cref="System.Single"/> number.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(System.Single a, ComplexNumber b)
        {
            return new ComplexNumber(a - b.RealPart, -b.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="ComplexNumber"/> from a <see cref="System.Int16"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="System.Int16"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(Int16 a, ComplexNumber b)
        {
            return new ComplexNumber(a - b.RealPart, -b.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="ComplexNumber"/> from a <see cref="System.Int32"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="System.Int32"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(Int32 a, ComplexNumber b)
        {
            return new ComplexNumber(a - b.RealPart, -b.ImaginaryPart);
        }

        /// <summary>Subtracts a <see cref="ComplexNumber"/> from a <see cref="System.Int64"/> integer.
        /// </summary>
        /// <param name="a">A <see cref="System.Int64"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals <paramref name="a"/> minus <paramref name="b"/>.</returns>
        public static ComplexNumber operator -(Int64 a, ComplexNumber b)
        {
            return new ComplexNumber(a - b.RealPart, -b.ImaginaryPart);
        }
        #endregion

        #region multiplicity methods

        /// <summary>Multiplies two <see cref="ComplexNumber"/>'s.
        /// </summary>
        /// <param name="a">The first <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The second <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.RealPart * b.RealPart - a.ImaginaryPart * b.ImaginaryPart, a.ImaginaryPart * b.RealPart + a.RealPart * b.ImaginaryPart);
        }

        /// <summary>Multiplies a real number and a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The real number.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(double a, ComplexNumber b)
        {
            return new ComplexNumber(a * b.RealPart, a * b.ImaginaryPart);
        }

        /// <summary>Multiplies a <see cref="System.Single"/> number and a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Single"/> number.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(System.Single a, ComplexNumber b)
        {
            return new ComplexNumber(a * b.RealPart, a * b.ImaginaryPart);
        }

        /// <summary>Multiplies a <see cref="System.Int16"/> integer and a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Int16"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(System.Int16 a, ComplexNumber b)
        {
            return new ComplexNumber(a * b.RealPart, a * b.ImaginaryPart);
        }

        /// <summary>Multiplies a <see cref="System.Int32"/> integer and a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Int32"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(System.Int32 a, ComplexNumber b)
        {
            return new ComplexNumber(a * b.RealPart, a * b.ImaginaryPart);
        }

        /// <summary>Multiplies a <see cref="System.Int64"/> integer and a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Int64"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(System.Int64 a, ComplexNumber b)
        {
            return new ComplexNumber(a * b.RealPart, a * b.ImaginaryPart);
        }

        /// <summary>Multiplies a <see cref="ComplexNumber"/> and a real number.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The real number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(ComplexNumber a, double b)
        {
            return new ComplexNumber(a.RealPart * b, a.ImaginaryPart * b);
        }

        /// <summary>Multiplies a <see cref="ComplexNumber"/> and a <see cref="System.Single"/> number.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Single"/> number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(ComplexNumber a, System.Single b)
        {
            return new ComplexNumber(a.RealPart * b, a.ImaginaryPart * b);
        }

        /// <summary>Multiplies a <see cref="ComplexNumber"/> and a <see cref="System.Int16"/> integer.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int16"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(ComplexNumber a, System.Int16 b)
        {
            return new ComplexNumber(a.RealPart * b, a.ImaginaryPart * b);
        }

        /// <summary>Multiplies a <see cref="ComplexNumber"/> and a <see cref="System.Int32"/> integer.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int32"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(ComplexNumber a, System.Int32 b)
        {
            return new ComplexNumber(a.RealPart * b, a.ImaginaryPart * b);
        }

        /// <summary>Multiplies a <see cref="ComplexNumber"/> and a <see cref="System.Int64"/> integer.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int64"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ComplexNumber operator *(ComplexNumber a, System.Int64 b)
        {
            return new ComplexNumber(a.RealPart * b, a.ImaginaryPart * b);
        }
        #endregion

        #region division methods

        /// <summary>Divides a <see cref="ComplexNumber"/> by a real number.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The real number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if  <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(ComplexNumber a, double b)
        {
            return new ComplexNumber(a.RealPart / b, a.ImaginaryPart / b);
        }

        /// <summary>Divides a <see cref="ComplexNumber"/> by a <see cref="System.Single"/> number.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Single"/> number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if  <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(ComplexNumber a, System.Single b)
        {
            return new ComplexNumber(a.RealPart / b, a.ImaginaryPart / b);
        }

        /// <summary>Divides a <see cref="ComplexNumber"/> by a <see cref="System.Int16"/> integer.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int16"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if  <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(ComplexNumber a, Int16 b)
        {
            return new ComplexNumber(a.RealPart / b, a.ImaginaryPart / b);
        }

        /// <summary>Divides a <see cref="ComplexNumber"/> by a <see cref="System.Int32"/> integer.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int32"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if  <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(ComplexNumber a, Int32 b)
        {
            return new ComplexNumber(a.RealPart / b, a.ImaginaryPart / b);
        }

        /// <summary>Divides a <see cref="ComplexNumber"/> by a <see cref="System.Int64"/> integer.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="System.Int64"/> integer.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if  <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(ComplexNumber a, Int64 b)
        {
            return new ComplexNumber(a.RealPart / b, a.ImaginaryPart / b);
        }

        /// <summary>Divides a <see cref="ComplexNumber"/> by another.
        /// </summary>
        /// <param name="a">The <see cref="ComplexNumber"/>.</param>
        /// <param name="b">The <see cref="ComplexNumber"/> which is the divisor.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if  <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
        {
            /* this is the naive implementation with little robustness against overflow: 
             * 
             * double norm = b.RealPart * b.RealPart + b.ImaginaryPart * b.ImaginaryPart;
             * return new ComplexNumber((a.RealPart * b.RealPart + a.ImaginaryPart * b.ImaginaryPart) / norm, (a.ImaginaryPart * b.RealPart - a.RealPart * b.ImaginaryPart) / norm);
             *
             * Here we use the approach given in:
             * 'Smith, R.L., Algorithm 116: ComplexNumber division. Commun.ACM 5,8 (1962),435'.
             */

            double r, temp;
            if (Math.Abs(b.RealPart) >= Math.Abs(b.ImaginaryPart))
            {
                r = b.ImaginaryPart / b.RealPart;
                temp = b.RealPart + b.ImaginaryPart * r;
                return new ComplexNumber((a.RealPart + a.ImaginaryPart * r) / temp, (a.ImaginaryPart - a.RealPart * r) / temp);
            }
            else
            {
                r = b.RealPart / b.ImaginaryPart;
                temp = b.ImaginaryPart + b.RealPart * r;
                return new ComplexNumber((a.ImaginaryPart + a.RealPart * r) / temp, -(a.RealPart - a.ImaginaryPart * r) / temp);
            }
        }

        /// <summary>Divides a real number by a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The real number.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(double a, ComplexNumber b)
        {
            /* this is the naive implementation with little robustness against overflow: 
             *  
             * double norm = b.RealPart * b.RealPart + b.ImaginaryPart * b.ImaginaryPart;
             * return new ComplexNumber((a * b.RealPart) / norm, (-a * b.ImaginaryPart) / norm);
             * 
             * Here we use the approach given in:
             * 'Smith, R.L., Algorithm 116: Complex division. Commun.ACM 5,8 (1962), p. 435'.
             * */

            double r, temp;
            if (Math.Abs(b.RealPart) >= Math.Abs(b.ImaginaryPart))
            {
                r = b.ImaginaryPart / b.RealPart;
                temp = b.RealPart + b.ImaginaryPart * r;
                return new ComplexNumber(a / temp, -a * r / temp);
            }
            else
            {
                r = b.RealPart / b.ImaginaryPart;
                temp = b.ImaginaryPart + b.RealPart * r;
                return new ComplexNumber(a * r / temp, -a / temp);
            }
        }

        /// <summary>Divides a <see cref="System.Single"/> number by a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Single"/> number.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(System.Single a, ComplexNumber b)
        {
            /* this is the naive implementation with little robustness against overflow: 
             *  
             * double norm = b.RealPart * b.RealPart + b.ImaginaryPart * b.ImaginaryPart;
             * return new ComplexNumber((a * b.RealPart) / norm, (-a * b.ImaginaryPart) / norm);
             * 
             * Here we use the approach given in:
             * 'Smith, R.L., Algorithm 116: Complex division. Commun.ACM 5,8 (1962), p. 435'.
             * */

            double r, temp;
            if (Math.Abs(b.RealPart) >= Math.Abs(b.ImaginaryPart))
            {
                r = b.ImaginaryPart / b.RealPart;
                temp = b.RealPart + b.ImaginaryPart * r;
                return new ComplexNumber(a / temp, -a * r / temp);
            }
            else
            {
                r = b.RealPart / b.ImaginaryPart;
                temp = b.ImaginaryPart + b.RealPart * r;
                return new ComplexNumber(a * r / temp, -a / temp);
            }
        }

        /// <summary>Divides a <see cref="System.Int16"/> integer by a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Int16"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(System.Int16 a, ComplexNumber b)
        {
            /* this is the naive implementation with little robustness against overflow: 
             *  
             * double norm = b.RealPart * b.RealPart + b.ImaginaryPart * b.ImaginaryPart;
             * return new ComplexNumber((a * b.RealPart) / norm, (-a * b.ImaginaryPart) / norm);
             * 
             * Here we use the approach given in:
             * 'Smith, R.L., Algorithm 116: Complex division. Commun.ACM 5,8 (1962), p. 435'.
             * */

            double r, temp;
            if (Math.Abs(b.RealPart) >= Math.Abs(b.ImaginaryPart))
            {
                r = b.ImaginaryPart / b.RealPart;
                temp = b.RealPart + b.ImaginaryPart * r;
                return new ComplexNumber(a / temp, -a * r / temp);
            }
            else
            {
                r = b.RealPart / b.ImaginaryPart;
                temp = b.ImaginaryPart + b.RealPart * r;
                return new ComplexNumber(a * r / temp, -a / temp);
            }
        }

        /// <summary>Divides a <see cref="System.Int32"/> integer by a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Int32"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(System.Int32 a, ComplexNumber b)
        {
            /* this is the naive implementation with little robustness against overflow: 
             *  
             * double norm = b.RealPart * b.RealPart + b.ImaginaryPart * b.ImaginaryPart;
             * return new ComplexNumber((a * b.RealPart) / norm, (-a * b.ImaginaryPart) / norm);
             * 
             * Here we use the approach given in:
             * 'Smith, R.L., Algorithm 116: Complex division. Commun.ACM 5,8 (1962), p. 435'.
             * */

            double r, temp;
            if (Math.Abs(b.RealPart) >= Math.Abs(b.ImaginaryPart))
            {
                r = b.ImaginaryPart / b.RealPart;
                temp = b.RealPart + b.ImaginaryPart * r;
                return new ComplexNumber(a / temp, -a * r / temp);
            }
            else
            {
                r = b.RealPart / b.ImaginaryPart;
                temp = b.ImaginaryPart + b.RealPart * r;
                return new ComplexNumber(a * r / temp, -a / temp);
            }
        }

        /// <summary>Divides a <see cref="System.Int64"/> integer by a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="a">The <see cref="System.Int64"/> integer.</param>
        /// <param name="b">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that equals to <paramref name="a"/> divided by <paramref name="b"/>.</returns>
        /// <exception cref="DivideByZeroException">Thrown if <paramref name="b"/> is equal to zero.</exception>
        public static ComplexNumber operator /(System.Int64 a, ComplexNumber b)
        {
            /* this is the naive implementation with little robustness against overflow: 
             *  
             * double norm = b.RealPart * b.RealPart + b.ImaginaryPart * b.ImaginaryPart;
             * return new ComplexNumber((a * b.RealPart) / norm, (-a * b.ImaginaryPart) / norm);
             * 
             * Here we use the approach given in:
             * 'Smith, R.L., Algorithm 116: Complex division. Commun.ACM 5,8 (1962), p. 435'.
             * */

            double r, temp;
            if (Math.Abs(b.RealPart) >= Math.Abs(b.ImaginaryPart))
            {
                r = b.ImaginaryPart / b.RealPart;
                temp = b.RealPart + b.ImaginaryPart * r;
                return new ComplexNumber(a / temp, -a * r / temp);
            }
            else
            {
                r = b.RealPart / b.ImaginaryPart;
                temp = b.ImaginaryPart + b.RealPart * r;
                return new ComplexNumber(a * r / temp, -a / temp);
            }
        }
        #endregion

        #region unary methods

        /// <summary>The implementation of the unary operator which returns the additive inverse of some <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>The additive inverse of <paramref name="z"/>.</returns>
        public static ComplexNumber operator -(ComplexNumber z)
        {
            return new ComplexNumber(-z.RealPart, -z.ImaginaryPart);
        }
        #endregion

        #region (un-) equality methods

        /// <summary>Equality comparison for two complex numbers.
        /// </summary>
        /// <param name="a">The first complex number.</param>
        /// <param name="b">The second complex number.</param>
        /// <returns>A value indicating whether <paramref name="a"/> and <paramref name="b"/> represents the
        /// same complex number.</returns>
        /// <remarks>This method does not take into account any tolerance.</remarks>
        public static bool operator ==(ComplexNumber a, ComplexNumber b)
        {
            return (a.ImaginaryPart == b.ImaginaryPart) && (a.RealPart == b.RealPart);
        }

        /// <summary>Un-equality comparison for two complex numbers.
        /// </summary>
        /// <param name="a">The first complex number.</param>
        /// <param name="b">The second complex number.</param>
        /// <returns>A value indicating whether <paramref name="a"/> and <paramref name="b"/> represents different
        /// complex numbers.</returns>
        /// <remarks>This method does not take into account any tolerance.</remarks>
        public static bool operator !=(ComplexNumber a, ComplexNumber b)
        {
            return (a.ImaginaryPart != b.ImaginaryPart) || (a.RealPart != b.RealPart);
        }

        /// <summary>Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return RealPart.GetHashCode() ^ ImaginaryPart.GetHashCode();
        }
        #endregion

        #region implicit cast methods

        /// <summary>Performs an implicit conversion from <see cref="System.Double"/> to <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The real number.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ComplexNumber(double value)
        {
            return new ComplexNumber(value, 0.0);
        }

        /// <summary>Performs an implicit conversion from <see cref="System.Single"/> to <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The <see cref="System.Single"/> number.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ComplexNumber(System.Single value)
        {
            return new ComplexNumber(value, 0.0);
        }

        /// <summary>Performs an implicit conversion from <see cref="System.Int16"/> to <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The <see cref="System.Int16"/> integer.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ComplexNumber(System.Int16 value)
        {
            return new ComplexNumber(value, 0.0);
        }

        /// <summary>Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The <see cref="System.Int32"/> integer.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ComplexNumber(System.Int32 value)
        {
            return new ComplexNumber(value, 0.0);
        }

        /// <summary>Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The <see cref="System.Int64"/> integer.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ComplexNumber(System.Int64 value)
        {
            return new ComplexNumber(value, 0.0);
        }

        /// <summary>Performs an implicit conversion from <see cref="System.Numerics.Complex"/> to <see cref="Dodoni.MathLibrary.Basics.ComplexNumber"/>.
        /// </summary>
        /// <param name="value">The complex number in its <see cref="System.Numerics.Complex"/> representation.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ComplexNumber(System.Numerics.Complex value)
        {
            return new ComplexNumber(value.Real, value.Imaginary);
        }
        #endregion

        #endregion

        #region basic methods

        /// <summary>Returns the conjugate of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the conjugate of <paramref name="z"/>.</returns>
        /// <remarks>The conjugate of a complex number <c>z = a+ ib</c> is <c>a - ib</c>.</remarks>
        public static ComplexNumber Conjugate(ComplexNumber z)
        {
            return new ComplexNumber(z.RealPart, -z.ImaginaryPart);
        }

        /// <summary>Returns the absolute value of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/>.</param>
        /// <value>The modulus (= absolute value) of <paramref name="z"/>.</value>
        /// <remarks>The modulus of a complex number is the square root of the sum of the squares of its
        /// real and imaginary components and is equal to its absolute value.</remarks>
        public static double Abs(ComplexNumber z)
        {
            double x = Math.Abs(z.RealPart);
            double y = Math.Abs(z.ImaginaryPart);

            if (x <= MachineConsts.Epsilon)
            {
                return y;
            }
            else if (y <= MachineConsts.Epsilon)
            {
                return x;
            }
            else if (x > y)
            {
                double temp1 = y / x;
                return x * Math.Sqrt(1.0 + temp1 * temp1);
            }
            else
            {
                double temp2 = x / y;
                return y * Math.Sqrt(1.0 + temp2 * temp2);
            }
        }

        /// <summary>Returns the phase angle of a complex number
        /// </summary>
        /// <param name="z">The complex number.</param>
        /// <returns>The phase angle of <paramref name="z"/> or <see cref="System.Double.NaN"/> if
        /// <paramref name="z"/> is <c>0.0</c>.</returns>
        public static double Angle(ComplexNumber z)
        {
            if ((Math.Abs(z.RealPart) <= MachineConsts.Epsilon) && (Math.Abs(z.ImaginaryPart) <= MachineConsts.Epsilon))
            {
                return double.NaN;
            }
            return Math.Atan2(z.ImaginaryPart, z.RealPart);
        }

        /// <summary>Returns a value indicating whether a <see cref="ComplexNumber"/> is undefined, i.e.
        /// <c>Not a Number</c>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/>.</param>
        /// <returns>
        /// <c>true</c> if the real or imaginary part is <c>NaN</c>; otherwise <c>false</c>.
        /// </returns>
        public static bool IsNaN(ComplexNumber z)
        {
            if (Double.IsNaN(z.RealPart) || Double.IsNaN(z.ImaginaryPart))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region transcendental functions

        /// <summary>Returns the exponential of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the exponential of <paramref name="z"/>, i.e.
        /// if <c>z = a + ib</c> the number <c>exp(z) = exp(a) * exp(ib) = exp(a) * [cos b + i sin b]</c> will be returned.</returns>
        public static ComplexNumber Exp(ComplexNumber z)
        {
            double factor = Math.Exp(z.RealPart);
            return new ComplexNumber(factor * Math.Cos(z.ImaginaryPart), factor * Math.Sin(z.ImaginaryPart));
        }

        /// <summary>Returns the sinus of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>Sinus of <paramref name="z"/>.</returns>
        /// <remarks>The sinus of z is given by the euler equation:
        /// <para>(Exp(i * z) - Exp(-1.0 * i * z)) / (2.0 * i)</para>
        /// which is equal to 0.5 * sin(a) * [exp(b) + exp(-b) ] - 0.5 * cos(a) * [exp(-b) - exp(b)]* i,
        /// where z = a + b*i.
        /// </remarks>
        public static ComplexNumber Sin(ComplexNumber z)
        {
            double expOfImaginaryPart = Math.Exp(z.ImaginaryPart);
            double expOfMinusImaginaryPart = 1.0 / expOfImaginaryPart;

            return new ComplexNumber(0.5 * Math.Sin(z.RealPart) * (expOfImaginaryPart + expOfMinusImaginaryPart), -0.5 * Math.Cos(z.RealPart) * (expOfMinusImaginaryPart - expOfImaginaryPart));
        }

        /// <summary>Returns the cosinus of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>Cosinus of <paramref name="z"/>.</returns>
        /// <remarks>The cosinus of z is given by the euler equation:
        /// <para>0.5 * [exp(i z) + exp(-i z)]</para>
        /// which is equal to 0.5 * cos(a) * [exp(-b) + exp(b) ] + 0.5 * sin(a) * [exp(-b) - exp(b)]* i,
        /// where z = a + b*i.
        /// </remarks>
        public static ComplexNumber Cos(ComplexNumber z)
        {
            double expOfImaginaryPart = Math.Exp(z.ImaginaryPart);
            double expOfMinusImaginaryPart = 1.0 / expOfImaginaryPart;

            return new ComplexNumber(0.5 * Math.Cos(z.RealPart) * (expOfMinusImaginaryPart + expOfImaginaryPart), 0.5 * Math.Sin(z.RealPart) * (expOfMinusImaginaryPart - expOfImaginaryPart));
        }

        /// <summary>Returns the tangent of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>Tangent of <paramref name="z"/>, perhaps <see cref="ComplexNumber.NaN"/>.</returns>
        /// <remarks>The tangent of z is given by sin z / cos z.
        /// </remarks>
        public static ComplexNumber Tan(ComplexNumber z)
        {
            double expOfImaginaryPart = Math.Exp(z.ImaginaryPart);
            double minusExpOfImaginaryPart = 1.0 / expOfImaginaryPart;

            /* we implement 'sin z / cos z' explicit and we take into account the same
             * approach as above, see
             * 'Smith, R.L., Algorithm 116: Complex division. Commun.ACM 5,8 (1962),435'.
             * */
            double zRealPartCos = Math.Cos(z.RealPart);
            double zRealPartSin = Math.Sin(z.RealPart);

            double cosRealPart = 0.5 * zRealPartCos * (minusExpOfImaginaryPart + expOfImaginaryPart); // = Re( cos z)
            double cosImaginaryPart = 0.5 * zRealPartSin * (minusExpOfImaginaryPart - expOfImaginaryPart); // = Im( cos z)

            if ((Math.Abs(cosRealPart) <= MachineConsts.Epsilon) && (Math.Abs(cosImaginaryPart) <= MachineConsts.Epsilon))
            {
                return ComplexNumber.NaN;
            }

            double sinRealPart = 0.5 * zRealPartSin * (expOfImaginaryPart + minusExpOfImaginaryPart); // = Re( sin z)
            double sinImaginaryPart = -0.5 * zRealPartCos * (minusExpOfImaginaryPart - expOfImaginaryPart); // = Im( sin z)

            double r, temp;
            if (Math.Abs(cosRealPart) >= Math.Abs(cosImaginaryPart))
            {
                r = cosImaginaryPart / cosRealPart;
                temp = cosRealPart + cosImaginaryPart * r;
                return new ComplexNumber((sinRealPart + sinImaginaryPart * r) / temp, (sinImaginaryPart - sinRealPart * r) / temp);
            }
            else
            {
                r = cosRealPart / cosImaginaryPart;
                temp = cosImaginaryPart + cosRealPart * r;
                return new ComplexNumber((sinImaginaryPart + sinRealPart * r) / temp, -(sinRealPart - sinImaginaryPart * r) / temp);
            }
        }

        /// <summary>Returns the sinus hyperbolicus of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>Sinus hyperbolicus of <paramref name="z"/>.</returns>
        /// <remarks>The sinus hyperbolicus of z is equal to i*sin(- i * z).
        /// </remarks>
        public static ComplexNumber Sinh(ComplexNumber z)
        {
            // we use (almost) the same implementation as in the case of the sinus:
            double expOfImaginaryPart = Math.Exp(z.RealPart);  // = exp( Im(i * z) )
            double expOfMinusImaginaryPart = 1.0 / expOfImaginaryPart; // = exp( Im(-i * z) )

            return new ComplexNumber(0.5 * Math.Cos(z.ImaginaryPart) * (expOfImaginaryPart - expOfMinusImaginaryPart), 0.5 * Math.Sin(z.ImaginaryPart) * (expOfImaginaryPart + expOfMinusImaginaryPart));
        }

        /// <summary>Returns the cosinus hyperbolicus of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>Cosinus hyperbolicus of <paramref name="z"/>.</returns>
        /// <remarks>The cosinus hyperbolicus of z is equal to cos(- i * z).
        /// </remarks>
        public static ComplexNumber Cosh(ComplexNumber z)
        {
            // we use (almost) the same implementation as in the case of the cosinus:
            double expOfImaginaryPart = Math.Exp(z.RealPart); // = exp( Im(i * z) )
            double expOfMinusImaginaryPart = 1.0 / expOfImaginaryPart; // = exp( Im(-i* z) )

            return new ComplexNumber(0.5 * Math.Cos(z.ImaginaryPart) * (expOfMinusImaginaryPart + expOfImaginaryPart), 0.5 * Math.Sin(z.ImaginaryPart) * (expOfImaginaryPart - expOfMinusImaginaryPart));
        }

        /// <summary>Returns the tangens hyperbolicus of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/> argument.</param>
        /// <returns>Tangens hyperbolicus of <paramref name="z"/>, perhaps <see cref="ComplexNumber.NaN"/>.</returns>
        /// <remarks>The tangens hyperbolicus of z is equal to sinh(z) / cosh(z).
        /// </remarks>
        public static ComplexNumber Tanh(ComplexNumber z)
        {
            // see implementation of sinh and cosh above:

            double expOfImaginaryPart = Math.Exp(z.RealPart);  // = exp( Im(i * z) )
            double expOfMinusImaginaryPart = 1.0 / expOfImaginaryPart; // = exp( Im(-i * z) )

            ComplexNumber cosh = new ComplexNumber(0.5 * Math.Cos(z.ImaginaryPart) * (expOfMinusImaginaryPart + expOfImaginaryPart), 0.5 * Math.Sin(z.ImaginaryPart) * (expOfImaginaryPart - expOfMinusImaginaryPart));
            if ((Math.Abs(cosh.RealPart) <= MachineConsts.Epsilon) && (Math.Abs(cosh.ImaginaryPart) <= MachineConsts.Epsilon))  // cosh(z) == 0?
            {
                return ComplexNumber.NaN;
            }
            return (new ComplexNumber(0.5 * Math.Cos(z.ImaginaryPart) * (expOfImaginaryPart - expOfMinusImaginaryPart), 0.5 * Math.Sin(z.ImaginaryPart) * (expOfImaginaryPart + expOfMinusImaginaryPart))) / cosh;
        }

        /// <summary>Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="a">A double-precision floating-point number to be raised to a power.</param>
        /// <param name="z">A <see cref="ComplexNumber"/> that specifies a power.</param>
        /// <returns><paramref name="a"/> to the power of <paramref name="z"/>, i.e. <c>a^z = exp( log(a) * z)</c>.</returns>
        public static ComplexNumber Pow(double a, ComplexNumber z)
        {
            double log = Math.Log(a);
            double factor = Math.Exp(z.RealPart * log);
            return new ComplexNumber(factor * Math.Cos(z.ImaginaryPart * log), factor * Math.Sin(z.ImaginaryPart * log));
        }

        /// <summary>Returns the square root of a real number.
        /// </summary>
        /// <param name="a">A real number.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the square root of <paramref name="a"/>.</returns>
        public static ComplexNumber Sqrt(double a)
        {
            if (a >= 0)
            {
                return new ComplexNumber(Math.Sqrt(a));
            }
            else
            {
                return new ComplexNumber(0, Math.Sqrt(-a));
            }
        }

        /// <summary>Returns a square root of a <see cref="ComplexNumber"/>.
        /// </summary>
        /// <param name="z">The <see cref="ComplexNumber"/>.</param>
        /// <returns>A <see cref="ComplexNumber"/> that is the square root of <paramref name="z"/>.</returns>
        /// <remarks>The square root of a complex number is not uniquely defined. This method
        /// returns the square root in the right half of the complex plane.</remarks>
        public static ComplexNumber Sqrt(ComplexNumber z)
        {
            double x = Math.Abs(z.RealPart);
            double y = Math.Abs(z.ImaginaryPart);

            if ((x <= MachineConsts.Epsilon) && (y <= MachineConsts.Epsilon))
            {
                return new ComplexNumber(0, 0);
            }
            else
            {
                double w, r;

                if (x >= y)
                {
                    r = y / x;
                    w = Math.Sqrt(x * 0.5 * (1.0 + Math.Sqrt(1.0 + r * r)));
                }
                else
                {
                    r = x / y;
                    w = Math.Sqrt(y * 0.5 * (r + Math.Sqrt(1.0 + r * r)));
                }

                if (z.RealPart >= 0.0)
                {
                    return new ComplexNumber(w, z.ImaginaryPart / (2.0 * w));
                }
                else
                {
                    w = (z.ImaginaryPart >= 0) ? w : -w;
                    return new ComplexNumber(z.ImaginaryPart / (2.0 * w), w);
                }
            }
        }
        #endregion

        /// <summary>Creates a new <see cref="ComplexNumber"/> object.
        /// </summary>
        /// <param name="realPart">The real part.</param>
        /// <returns>A specified <see cref="ComplexNumber"/> object.</returns>
        public static ComplexNumber Create(double realPart)
        {
            return new ComplexNumber(realPart);
        }

        /// <summary>Creates a new <see cref="ComplexNumber"/> object.
        /// </summary>
        /// <param name="realPart">The real part.</param>
        /// <param name="imaginaryPart">The imaginary part.</param>
        /// <returns>A specified <see cref="ComplexNumber"/> object.</returns>
        public static ComplexNumber Create(double realPart, double imaginaryPart)
        {
            return new ComplexNumber(realPart, imaginaryPart);
        }
        #endregion
    }
}