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

using NUnit.Framework;

using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary.Optimizer.MultiDimensional
{
    /// <summary>Serves as unit test class for <see cref="QuadraticProgram"/> objects.
    /// </summary>
    [TestFixture]
    public class QuadraticProgramTests
    {
        /// <summary>Serves as unit test for <see cref="QuadraticProgram"/>.
        /// </summary>
        [Test]
        public void FindMinimum_TestFunction1NoConstraints_AnalyticResult()
        {
            var optimizer = new GoldfarbIdanaQuadraticProgram();
            var algorithm = optimizer.Create(2);

            var A = new DenseMatrix(2, 2, new[] { 4.0, -2.0, -2.0, 4 - 0 });  // = (4 & -2 \\ -2 & 4)
            var b = new[] { 6.0, 0.0 };

            algorithm.Function = optimizer.Function.Create(A, b);

            var actualArgMin = new double[2];
            double actualMinimum;

            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { -2.0, -1.0 };
            var expectedMinimum = -6.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>Serves as unit test for <see cref="QuadraticProgram"/>.
        /// </summary>
        [Test]
        public void FindMinimum_TestFunction1WithConstraints_AnalyticResult()
        {
            /* Constraints: 
             * x_0 + x_1 = 3, 
             * x_0 >= 0, 
             * x_1 >= 0, 
             * x_0 + x_2 >= 2 */

            var optimizer = new GoldfarbIdanaQuadraticProgram();
            var boxConstraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(2, new[] { 0.0, 0.0 }, new[] { Double.NaN, Double.NaN }));
            var equalityConstraint = optimizer.Constraint.Create(new MultiDimRegion.LinearEquality(new DenseMatrix(2, 1, new[] { 1.0, 1.0 }), new[] { 3.0 }));
            var inequalityConstraint = optimizer.Constraint.Create(new MultiDimRegion.LinearInequality(new DenseMatrix(2, 1, new[] { 1.0, 1.0 }), new[] { 2.0 }));

            var algorithm = optimizer.Create(boxConstraint, equalityConstraint, inequalityConstraint);

            var A = new DenseMatrix(2, 2, new[] { 4.0, -2.0, -2.0, 4 - 0 });  // = (4 & -2 \\ -2 & 4)
            var b = new[] { 6.0, 0.0 };

            algorithm.Function = optimizer.Function.Create(A, b);

            var actualArgMin = new double[2];
            double actualMinimum;

            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { 1.0, 2.0 };
            var expectedMinimum = 12.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>Serves as unit test for <see cref="QuadraticProgram"/>.
        /// </summary>
        [Test]
        public void FindMinimum_TestFunction2NoConstraints_AnalyticResult()
        {
            var optimizer = new GoldfarbIdanaQuadraticProgram();
            var algorithm = optimizer.Create(3);

            var A = new DenseMatrix(3, 3, new[] { 5.0, -2.0, -1.0, -2.0, 4.0, 3.0, -1.0, 3.0, 5.0 });
            var b = new[] { 2.0, -35.0, -47.0 };

            algorithm.Function = optimizer.Function.Create(A, b);

            var actualArgMin = new double[3];
            double actualMinimum;

            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { 3.0, 5.0, 7.0 };
            var expectedMinimum = -249.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>Serves as unit test for <see cref="QuadraticProgram"/>.
        /// </summary>
        [Test]
        public void FindMinimum_TestFunction3WithConstraints_AnalyticResult()
        {
            /* minimize:
             *    f(x) = - 8 * x_0 - 16 * x_1 + x_0^2 + 4 * x_1^2 
             *    
             * subject to x_0 + x_1 <= 5, x_0 <? 3, x_0 >= 0, x_1 >= 0 */

            var optimizer = new GoldfarbIdanaQuadraticProgram();
            var boxConstraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(2, new[] { 0.0, 0.0 }, new[] { 3.0, Double.NaN }));
            var inequalityConstraint = optimizer.Constraint.Create(new MultiDimRegion.LinearInequality(new DenseMatrix(2, 1, new[] { -1.0, -1.0 }), new[] { -5.0 }));

            var algorithm = optimizer.Create(boxConstraint, inequalityConstraint);

            var A = new DenseMatrix(2, 2, new[] { 2.0, 0.0, 0.0, 8.0 });
            var b = new[] { -8.0, -16.0 };

            algorithm.Function = optimizer.Function.Create(A, b);

            var actualArgMin = new double[2];
            double actualMinimum;

            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { 3.0, 2.0 };
            var expectedMinimum = -31.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }

        /// <summary>Serves as unit test for <see cref="QuadraticProgram"/>.
        /// </summary>
        [Test]
        public void FindMinimum_TestFunction4WithConstraints_AnalyticResult()
        {
            /* Constraints: 
             * x_0 + x_1 + x_2 >= 6, 
             * -x_0 - x_1 + 2* x_2 >= 2, 
             * x_0, x_1, x_2  >= 0, 
             * */

            var optimizer = new GoldfarbIdanaQuadraticProgram();
            var boxConstraint = optimizer.Constraint.Create(MultiDimRegion.Interval.Create(3, new[] { 0.0, 0.0, 0.0 }, new[] { Double.NaN, Double.NaN, Double.NaN }));
            var inequalityConstraint = optimizer.Constraint.Create(new MultiDimRegion.LinearInequality(new DenseMatrix(3, 2, new[] { 1.0, 1.0, 1.0, -1.0, -1.0, 2.0 }), new[] { 6.0, 2.0 }));

            var algorithm = optimizer.Create(boxConstraint, inequalityConstraint);

            var A = new DenseMatrix(3, 3, new[] { 2.0, 1.0, 0.0, 1.0, 4.0, 2.0, 0.0, 2.0, 4.0 });
            var b = new[] { 4.0, 6.0, 12.0 };

            algorithm.Function = optimizer.Function.Create(A, b);

            var actualArgMin = new double[3];
            double actualMinimum;

            var state = algorithm.FindMinimum(actualArgMin, out actualMinimum);

            var expectedArgMin = new[] { 3 + 1.0 / 3.0, 0.0, 2 + 2.0 / 3.0 };
            var expectedMinimum = 70.0 + 2.0 / 3.0;
            Assert.That(actualMinimum, Is.EqualTo(expectedMinimum).Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
            Assert.That(actualArgMin, Is.EqualTo(expectedArgMin).AsCollection.Within(1E-7), String.Format("State: {0}; actual Minimum: {1}; expected Minimum: {2}; actual argMin: [{3}; {4}]; expected argMin: [{5}; {6}].", state, actualMinimum, expectedMinimum, actualArgMin[0], actualArgMin[1], expectedArgMin[0], expectedArgMin[1]));
        }
    }
}