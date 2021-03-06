﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dodoni.Finance.CommonMarketUsages.DayCountConventions {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DayCountConventionResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DayCountConventionResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Dodoni.Finance.CommonMarketUsages.DayCountConventions.DayCountConventionResources" +
                            "", typeof(DayCountConventionResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This day count fraction is defined as the actual numbers of days in the period over 360..
        /// </summary>
        internal static string Actual360 {
            get {
                return ResourceManager.GetString("Actual360", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Act/360.
        /// </summary>
        internal static string Actual360LongName {
            get {
                return ResourceManager.GetString("Actual360LongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This day count fraction is defined as the actual numbers of days in the period over 365..
        /// </summary>
        internal static string Actual365 {
            get {
                return ResourceManager.GetString("Actual365", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This day count fraction is defined as the actual numbers of days in the period over 365.25..
        /// </summary>
        internal static string Actual365_25 {
            get {
                return ResourceManager.GetString("Actual365_25", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Act/365.25.
        /// </summary>
        internal static string Actual365_25LongName {
            get {
                return ResourceManager.GetString("Actual365_25LongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Act/365.
        /// </summary>
        internal static string Actual365LongName {
            get {
                return ResourceManager.GetString("Actual365LongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The numerator is the actual number of days, the denominator is either 365 or 366 depending on whether or not the period inclues a  29 February..
        /// </summary>
        internal static string ActualActualAFB {
            get {
                return ResourceManager.GetString("ActualActualAFB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to AFB Actual/Actual (Euro).
        /// </summary>
        internal static string ActualActualAFBLongName {
            get {
                return ResourceManager.GetString("ActualActualAFBLongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Split the period into the years in which it occurs. For each year, divide the number of actual days in the period by the number of days in that year..
        /// </summary>
        internal static string ActualActualISDA {
            get {
                return ResourceManager.GetString("ActualActualISDA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ISDA Actual/Actual (historical), Actual/Actual, Act/Act, and according to ISDA also Actual/365, Act/365, A/365..
        /// </summary>
        internal static string ActualActualISDALongName {
            get {
                return ResourceManager.GetString("ActualActualISDALongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The actual number of days, divided by the product of the number of days in the period and the number of periods in the year..
        /// </summary>
        internal static string ActualActualISMA {
            get {
                return ResourceManager.GetString("ActualActualISMA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ISMA Actual/Actual (Bond).
        /// </summary>
        internal static string ActualActualISMALongName {
            get {
                return ResourceManager.GetString("ActualActualISMALongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of business days between the start date of the interest period (inclusive) and the end date of the interest period (exclusive) over 252..
        /// </summary>
        internal static string Bu252 {
            get {
                return ResourceManager.GetString("Bu252", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bu/252 [{0}].
        /// </summary>
        internal static string Bu252LongName {
            get {
                return ResourceManager.GetString("Bu252LongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Returns 1.00 for a period of a year regardless of the number of days within the year, especially year(d2) - year(d1) + (month(d2) - month(d1)) /12, where d1.m1.y1 is the first date and d2.m2.y2 is the second date..
        /// </summary>
        internal static string OneOverOne {
            get {
                return ResourceManager.GetString("OneOverOne", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1/1.
        /// </summary>
        internal static string OneOverOneLongName {
            get {
                return ResourceManager.GetString("OneOverOneLongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All months have 30 days and each year has 360 days with an exception that if the last day is 31st and the first day is not 30th or 31ts then that month has 31 days..
        /// </summary>
        internal static string Thirty360 {
            get {
                return ResourceManager.GetString("Thirty360", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 30/360 (Bond Basis).
        /// </summary>
        internal static string Thirty360LongName {
            get {
                return ResourceManager.GetString("Thirty360LongName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All months have 30 days and each year has 360 days. If the start or end day is on the 31th, it will moved to the 30th..
        /// </summary>
        internal static string ThirtyE360 {
            get {
                return ResourceManager.GetString("ThirtyE360", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 30E/360 (Eurobond Basis).
        /// </summary>
        internal static string ThirtyE360LongName {
            get {
                return ResourceManager.GetString("ThirtyE360LongName", resourceCulture);
            }
        }
    }
}
