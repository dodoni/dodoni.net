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
using Dodoni.MathLibrary.Optimizer;
using Dodoni.BasicComponents.Utilities;
using Dodoni.MathLibrary.Optimizer.MultiDimensional;

namespace Dodoni.MathLibrary.Native.NLopt
{
    /// <summary>The optimization algorithms of NLopt library 2.4.x, see http://ab-initio.mit.edu/wiki/index.php/NLopt.
    /// </summary>
    /// <remarks> Naming conventions:
    /// {G/L}{D/N} global/local derivative/no-derivative optimization, respectively 
    ///          _RAND algorithms involve some randomization.
    /// 		 _NOSCAL algorithms are not scaled to a unit hypercube (i.e. they are sensitive to the units of x)
    /// The values have been taken from the header file nlopt.h of the nlopt library 2.4.1:
    /// Copyright (c) 2007-2012 Massachusetts Institute of Technology
    ///
    /// Permission is hereby granted, free of charge, to any person obtaining
    /// a copy of this software and associated documentation files (the
    /// "Software"), to deal in the Software without restriction, including
    /// without limitation the rights to use, copy, modify, merge, publish,
    /// distribute, sublicense, and/or sell copies of the Software, and to
    /// permit persons to whom the Software is furnished to do so, subject to
    /// the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be
    /// included in all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    /// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    /// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    /// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    /// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    /// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    /// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
    /// </remarks>
    public enum NLoptAlgorithm
    {
        /// <summary>The DIRECT (DIviding RECTangles) algorithm for (derivative-free) global optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        GN_DIRECT = 0,

        /// <summary>The locally biased DIRECT (DIviding RECTangles) algorithm for (derivative-free) global optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        GN_DIRECT_L,

        /// <summary>A randomized version of <see cref="NLoptAlgorithm.GN_DIRECT_L"/> (derivative-free, global optimization).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, IsRandomAlgorithm = true)]
        GN_DIRECT_L_RAND,

        /// <summary>An unscaled version of the DIRECT (DIviding RECTangles) algorithm for (derivative-free) global optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        GN_DIRECT_NOSCAL,

        /// <summary>An unscaled version of a locally biased DIRECT (DIviding RECTangles) algorithm for (derivative-free) global optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        GN_DIRECT_L_NOSCAL,

        /// <summary>A randomized version of <see cref="NLoptAlgorithm.GN_DIRECT_L_NOSCAL"/>.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, IsRandomAlgorithm = true)]
        GN_DIRECT_L_RAND_NOSCAL,

        /// <summary>The original Fortran implementation of the DIRECT (DIviding RECTangles) algorithm for (derivative-free) global optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        GN_ORIG_DIRECT,

        /// <summary>The original Fortran implementation of a locally biased DIRECT (DIviding RECTangles) algorithm for (derivative-free) global optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        GN_ORIG_DIRECT_L,

        /// <summary>A global optimization algorithm that works by systematically dividing the search space via branch-and-bound technique
        /// and search them by a gradient-based local-search algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.Required, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        GD_STOGO,

        /// <summary>A global optimization algorithm that works by systematically dividing the search space via branch-and-bound technique
        /// and search them by a gradient-based local-search algorithm. This is the randomized variant.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.Required, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, IsRandomAlgorithm = true)]
        GD_STOGO_RAND,

        /// <summary>The original L-BFGS (=low-storage Broyden–Fletcher–Goldfarb–Shanno) code by Nocedal et al.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required)]
        LD_LBFGS_NOCEDAL,

        /// <summary>The low-storage Broyden–Fletcher–Goldfarb–Shanno (BFGS) local, gradient-based optimization algorithm. Limited-memory.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Optional)]
        LD_LBFGS,

        /// <summary>The PRincipal AXIS gradient-free local optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Optional, IsRandomAlgorithm = true)]
        LN_PRAXIS,

        /// <summary>Limited-memory variable-metric, rank 1 (local, derivative-based).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required)]
        LD_VAR1,

        /// <summary>Limited-memory variable-metric, rank 2 (local, derative-based).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required)]
        LD_VAR2,

        /// <summary>Truncated Newton (local, derivate-based)
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required)]
        LD_TNEWTON,

        /// <summary>Truncated Newton with restart (local, derivative-based).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required)]
        LD_TNEWTON_RESTART,

        /// <summary>Preconditioned truncated Newton (local, derivative-based).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required)]
        LD_TNEWTON_PRECOND,

        /// <summary>Preconditioned truncated Newton with restarting (local, derivative-based).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required)]
        LD_TNEWTON_PRECOND_RESTART,

        /// <summary>A Controlled Random Search (CRS) with local mutation global optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, IsRandomAlgorithm = true)]
        GN_CRS2_LM,

        /// <summary>A Multi-Lebel-Single-Linkage (MLSL) global (derivative-free) optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, SubsidiaryRequirement = NLoptSubsidiaryRequirement.Optional)]
        GN_MLSL,

        /// <summary>A Multi-Lebel-Single-Linkage (MLSL) global (gradient-based) optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.Required, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, SubsidiaryRequirement = NLoptSubsidiaryRequirement.Optional)]
        GD_MLSL,

        /// <summary>A Multi-Lebel-Single-Linkage (MLSL) global (derivative-free) optimization algorithm,
        /// based on Low Discrepancy Sequences (LDS).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, SubsidiaryRequirement = NLoptSubsidiaryRequirement.Optional)]
        GN_MLSL_LDS,

        /// <summary>A Multi-Lebel-Single-Linkage (MLSL) global (gradient-based) optimization algorithm, based on Low Discrepancy Sequences (LDS).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.Required, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, SubsidiaryRequirement = NLoptSubsidiaryRequirement.Optional)]
        GD_MLSL_LDS,

        /// <summary>The Method of Moving Asymptotes, i.e. a local, gradient-based optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, NonlinearConstraintRequirement = NLoptNonlinearConstraintRequirement.SupportsInequalityConstraints)]
        LD_MMA,

        /// <summary>A Constrainted Optimization BY Linear Approximations (Cobyla) local (derivative-free) optimization algorithm.
        /// </summary>
        [String("NLoptCOBYLA")]
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, NonlinearConstraintRequirement = NLoptNonlinearConstraintRequirement.SupportsInequalityAndEqualityConstraints)]
        LN_COBYLA,

        /// <summary>The NEW Unconstrained Optimization Algorithm (NEWUOA) via quadratic models, i.e. a local, derivative-free, unconstraint optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.None)]
        LN_NEWUOA,

        /// <summary>The NEW Unconstrained Optimization Algorithm (NEWUOA) for bounded constraints, i.e. a local, derivative-free optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Optional)]
        LN_NEWUOA_BOUND,

        /// <summary>The Nelder-Mead Simplex derivative-free, local optimization algorithm.
        /// </summary>
        [String("NLoptNelderMead")]
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Optional)]
        LN_NELDERMEAD,

        /// <summary>Sbplx variant of Nelder-Mead (re-implementation of Rowan's Subplex) (local, no-derivative).
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Optional)]
        LN_SBPLX,

        /// <summary>Augmented Lagrangian method (local, no-derivative).
        /// </summary>
        LN_AUGLAG,

        /// <summary>Augmented Lagrangian method (local, derivative).
        /// </summary>
        LD_AUGLAG,

        /// <summary>Augmented Lagrangian method for equality constraints (local, no-derivative).
        /// </summary>
        LN_AUGLAG_EQ,

        /// <summary>Augmented Lagrangian method for equality constraints (local, derivative).
        /// </summary>
        LD_AUGLAG_EQ,

        /// <summary>The Bound Optimization BY Quadratic Approximation (BOBYQA) derivative-free local optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required)]
        LN_BOBYQA,

        /// <summary>The Improved Stochastic Evolution Strategy (ISRES) (derivative-free) global optimization algorithm.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Global, OrdinaryMultiDimOptimizer.GradientMethod.None, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Required, NonlinearConstraintRequirement = NLoptNonlinearConstraintRequirement.SupportsInequalityAndEqualityConstraints)]
        GN_ISRES,

        /// <summary>Augmented Lagrangian method (needs sub-algorithm).
        /// </summary>
        AUGLAG,

        /// <summary>Augmented Lagrangian method for equality constraints (needs sub-algorithm).
        /// </summary>
        AUGLAG_EQ,

        /// <summary>Multi-level single-linkage (MLSL), random (global, needs sub-algorithm).
        /// </summary>
        G_MLSL,

        /// <summary>Multi-level single-linkage (MLSL), quasi-random (global, needs sub-algorithm).
        /// </summary>
        G_MLSL_LDS,

        /// <summary>A sequental quadratic programming (SQP) algorithm for nonlinear constraint gradient-based optimization.
        /// </summary>
        [NLoptAlgorithm(NLoptProceeding.Local, OrdinaryMultiDimOptimizer.GradientMethod.Required, BoxConstraintRequirement = NLoptBoundConstraintRequirement.Optional, NonlinearConstraintRequirement = NLoptNonlinearConstraintRequirement.SupportsInequalityAndEqualityConstraints)]
        LD_SLSQP,

        /// <summary>CCSA (Conservative Convex Separable Approximations) with simple quadratic approximations (local, derivative).
        /// </summary>
        LD_CCSAQ,

        /// <summary>ESCH evolutionary strategy.
        /// </summary>
        GN_ESCH
    }
}