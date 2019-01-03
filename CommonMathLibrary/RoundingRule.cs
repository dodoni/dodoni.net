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
using System.Globalization;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;
using Dodoni.MathLibrary.Miscellaneous;

namespace Dodoni.MathLibrary
{
    /// <summary>Serves as factory for <see cref="IRoundingRule"/> instances and represents a collection of <see cref="IRoundingRule"/>.
    /// </summary>
    public static class RoundingRule
    {
        #region public static (readonly) members

        /// <summary>A multithread-safe instance of a no-rounding implementation.
        /// </summary>
        public static readonly IRoundingRule NoRounding = new NoRounding();

        /// <summary>A factory for the banker's rounding, i.e. follows IEEE Standard 754, section 4.
        /// </summary>
        public static readonly BankersRounding.Factory BankersRounding = new BankersRounding.Factory();

        /// <summary>A factory for the rounding rule that returns the next multiple of a specified <c>unit</c> with respect to a double-precision floating-point argument. If the argument is positiv the result will
        /// be greater or equal than this number; otherwise the result will be smaller.
        /// </summary>
        public static readonly CeilingRounding.Factory Ceiling = new CeilingRounding.Factory();

        /// <summary>A factory for the rounding rule that returns the next multiple a specified <c>unit</c> with respect to a double-precision floating-point argument. If the argument is positiv the result will
        /// be less or equal than this number; otherwise the result will be greater.
        /// </summary>
        public static readonly FloorRounding.Factory Floor = new FloorRounding.Factory();

        /// <summary>A factory for the rounding rule that returns the argument truncated with respect to a specific number of digits.
        /// </summary>
        public static readonly TruncateRounding.Factory Truncate = new TruncateRounding.Factory();

        /// <summary>Rounds a double-precision floating-point value to the nearest integral value.
        /// </summary>
        /// <remarks>If the fractional component of a is halfway between two integers, one of which is even and the other odd, then the even number is returned.</remarks>
        public static readonly IRoundingRule NearestIntegralValue = new NearestIntegralValueRounding();
        #endregion

        #region private static members

        /// <summary>The mapping between the name of the rounding rule and the type of the class, needed if a rounding rule can have additional parameters.
        /// </summary>
        private static Dictionary<String, Type> sm_RoundingRuleTypes;

        /// <summary>The collection of the rounding rules.
        /// </summary>
        private static IdentifierNameableDictionary<IRoundingRule> sm_Pool;
        #endregion

        #region static constructors

        /// <summary>Initializes a new instance of the <see cref="RoundingRule"/> class.
        /// </summary>
        static RoundingRule()
        {
            sm_RoundingRuleTypes = new Dictionary<string, Type>();
            sm_RoundingRuleTypes.Add(Miscellaneous.BankersRounding.RoundingName.IDString, typeof(BankersRounding));
            sm_RoundingRuleTypes.Add(CeilingRounding.RoundingName.IDString, typeof(CeilingRounding));
            sm_RoundingRuleTypes.Add(TruncateRounding.RoundingName.IDString, typeof(TruncateRounding));
            sm_RoundingRuleTypes.Add(FloorRounding.RoundingName.IDString, typeof(FloorRounding));

            sm_Pool = GetInitialCollection();
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of rounding rules.
        /// </summary>
        /// <value>The number of rounding rules.</value>
        public static int Count
        {
            get { return sm_Pool.Count; }
        }

        /// <summary>Gets the rounding rules.
        /// </summary>
        /// <value>The rounding rules.</value>
        public static IEnumerable<IRoundingRule> Values
        {
            get { return sm_Pool.Values; }
        }
        #endregion

        #region public static methods

        /// <summary>Gets the names of the rounding rules.
        /// </summary>
        /// <returns>A collection which contains the <see cref="IdentifierString"/> representations of the rounding rules.</returns>
        public static IEnumerable<IdentifierString> GetNames()
        {
            return sm_Pool.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets a specific rounding rule with respect to its <see cref="System.String"/> representation.
        /// </summary>
        /// <param name="name">The name of the rounding rule.</param>
        /// <param name="value">The rounding rule (output).</param>
        /// <param name="addIntoPool">If a individual rounding rule has been created, the corresponding 
        /// <paramref name="value"/> object will be added to the <see cref="RoundingRule"/> if set to <c>true</c>.</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(string name, out IRoundingRule value, bool addIntoPool = false)
        {
            if (sm_Pool.TryGetValue(name, out value) == true)
            {
                return true;
            }

            // Format: "Rulename:Unit=23.1" or "Rulename:Digits=5" etc.
            string[] ruleNameSplit = name.Split(':', '=');
            if ((ruleNameSplit != null) && (ruleNameSplit.Length == 3))
            {
                Type roundingRuleType;
                if (sm_RoundingRuleTypes.TryGetValue(ruleNameSplit[0].ToIDString(), out roundingRuleType) == true)
                {
                    try
                    {
                        /* assume that the rounding rule needs exactly one argument in the constructor:
                         */
                        Type parameterType = roundingRuleType.GetConstructors()[0].GetParameters()[0].ParameterType;
                        if (parameterType.Equals(typeof(Int32)) == true)
                        {
                            value = (IRoundingRule)Activator.CreateInstance(roundingRuleType, Int32.Parse(ruleNameSplit[2]));
                        }
                        else if (parameterType.Equals(typeof(Double)) == true)
                        {
                            value = (IRoundingRule)Activator.CreateInstance(roundingRuleType, Double.Parse(ruleNameSplit[2], CultureInfo.InvariantCulture));
                        }
                        else if (parameterType.Equals(typeof(String)) == true)
                        {
                            value = (IRoundingRule)Activator.CreateInstance(roundingRuleType, ruleNameSplit[2]);
                        }
                        if (value != null)
                        {
                            if (addIntoPool == true)
                            {
                                sm_Pool.Add(value);
                            }
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>Adds the specified rounding rule.
        /// </summary>
        /// <param name="roundingRule">The rounding rule.</param>
        /// <returns>A value indicating whether <paramref name="roundingRule"/> has been inserted.</returns>
        public static ItemAddedState Add(IRoundingRule roundingRule)
        {
            return sm_Pool.Add(roundingRule);
        }
        #endregion

        #region private static methods

        /// <summary>Gets the initial collection of rounding rules.
        /// </summary>
        /// <returns>A collection of rounding rules.</returns>
        private static IdentifierNameableDictionary<IRoundingRule> GetInitialCollection()
        {
            return new IdentifierNameableDictionary<IRoundingRule>(NoRounding, BankersRounding.TwoDigits, Ceiling.Unit10, Floor.Unit10, Truncate.TwoDigits, NearestIntegralValue);
        }
        #endregion
    }
}