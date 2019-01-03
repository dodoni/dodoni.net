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
using System.Linq;
using System.Threading;
using System.Resources;
using System.Collections.Generic;

using Dodoni.Finance;
using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.BasicComponents.Utilities;
using Dodoni.BasicComponents.Containers;
using Dodoni.Finance.CommonMarketUsages.HolidayCalendars;
using Microsoft.Extensions.Logging;

namespace Dodoni.Finance.CommonMarketUsages
{
    /// <summary>Serves as factory for holiday calendars.
    /// </summary>
    public static class HolidayCalendar
    {
        #region nested stuff

        /// <summary>Handles the initialization event of <see cref="HolidayCalendar"/> which will be raised before starting to query a specific holiday calendar.
        /// </summary>
        public class InitializeEventArgs : EventArgs
        {
            #region internal constructors

            /// <summary>Initializes a new instance of the <see cref="InitializeEventArgs"/> class.
            /// </summary>
            internal InitializeEventArgs()
            {
            }
            #endregion

            #region public methods

            /// <summary>Registers a specific <see cref="IHolidayCalendar"/> object.
            /// </summary>
            /// <param name="value">The <see cref="IHolidayCalendar"/> object to register.</param>
            /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
            public ItemAddedState Add(IHolidayCalendar value)
            {
                return HolidayCalendar.Add(value);
            }

            /// <summary>Registers a collection of <see cref="IHolidayCalendar"/> objects.
            /// </summary>
            /// <param name="values">The collection of <see cref="IHolidayCalendar"/> objects to register.</param>
            /// <returns>For each element of <paramref name="values"/> a value indicating whether the <see cref="IHolidayCalendar"/>
            /// object has been inserted.</returns>
            public ItemAddedState[] Add(params IHolidayCalendar[] values)
            {
                if (values == null)
                {
                    return null;
                }
                IList<ItemAddedState> stateList = new List<ItemAddedState>();
                for (int j = 0; j < values.Length; j++)
                {
                    stateList.Add(HolidayCalendar.Add(values[j]));
                }
                return stateList.ToArray();
            }
            #endregion
        }
        #endregion

        #region public static (readonly) members

        /// <summary>Represents the 'dummy' holiday calendar, where each day is a business day, i.e. the set of holidays is empty.
        /// </summary>
        public static readonly IHolidayCalendar EmptyHolidayCalendar = new EmptyHolidayCalendar();

        /// <summary>Represents the holiday calendar, which is given by weekend days, i.e. saturdays and sundays are holidays
        /// and mondays up to fridays are business days.
        /// </summary>
        public static readonly IHolidayCalendar WeekendCalendar = new WeekendHolidayCalendar();

        /// <summary>The collection of holiday calendars with respect to 'Africa'.
        /// </summary>
        public static readonly HolidayCalendarCollectionAfrica Africa = new HolidayCalendarCollectionAfrica();

        /// <summary>The collection of holiday calendars with respect to 'America'.
        /// </summary>
        public static readonly HolidayCalendarCollectionAmerica America = new HolidayCalendarCollectionAmerica();

        /// <summary>The collection of holiday calendars with respect to 'Asia'.
        /// </summary>
        public static readonly HolidayCalendarCollectionAsia Asia = new HolidayCalendarCollectionAsia();

        /// <summary>The collection of holiday calendars with respect to 'Europe'.
        /// </summary>
        public static readonly HolidayCalendarCollectionEurope Europe = new HolidayCalendarCollectionEurope();

        /// <summary>The collection of holiday calendars with respect to 'Oceania'.
        /// </summary>
        public static readonly HolidayCalendarCollectionOceania Oceania = new HolidayCalendarCollectionOceania();
        #endregion

        #region private static members

        /// <summary>The internal pool of holiday calendars.
        /// </summary>
        private static IdentifierNameableDictionary<IHolidayCalendar> sm_Pool;

        /// <summary>Occurs before quering <see cref="IHolidayCalendar"/> objects.
        /// </summary>
        private static event Action<InitializeEventArgs> sm_Initialize;

        /// <summary>A value indicating whether the user has added or removed at least one event to the <see cref="HolidayCalendar.Initialize"/> event handler
        /// since the last call of the event-handler.
        /// </summary>
        /// <remarks>This member is used for performance reason only.</remarks>
        private static bool sm_InitializeChanged = true;

        /// <summary>The logger.
        /// </summary>
        private static ILogger sm_Logger;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="HolidayCalendar"/> class.
        /// </summary>
        static HolidayCalendar()
        {
//            sm_Logger = Logger.Stream.Create("Pool", typeof(HolidayCalendar), "Holiday calendar", "Pool");

            sm_Pool = new IdentifierNameableDictionary<IHolidayCalendar>();

            Initialize += Africa.RegisterHolidayCalendars;
            Initialize += America.RegisterHolidayCalendars;
            Initialize += Asia.RegisterHolidayCalendars;
            Initialize += Europe.RegisterHolidayCalendars;
            Initialize += Oceania.RegisterHolidayCalendars;
        }
        #endregion

        #region public static properties

        /// <summary>Gets the number of holiday calendar.
        /// </summary>
        /// <value>The number of holiday calendar.</value>
        public static int Count
        {
            get
            {
                OnInitialize();
                return sm_Pool.Count;
            }
        }

        /// <summary>Gets the holiday calendar.
        /// </summary>
        /// <value>The holiday calendar.</value>
        public static IEnumerable<IHolidayCalendar> Values
        {
            get
            {
                OnInitialize();
                return sm_Pool.Values;
            }
        }

        /// <summary>Gets the holiday calendar names.
        /// </summary>
        /// <value>The holiday calendar names.</value>
        public static IEnumerable<string> Names
        {
            get
            {
                OnInitialize();
                return sm_Pool.Names;
            }
        }

        /// <summary>Occurs before quering a specific <see cref="IHolidayCalendar"/> object.
        /// </summary>
        public static event Action<InitializeEventArgs> Initialize
        {
            add
            {
                sm_InitializeChanged = true;
                sm_Initialize += value;
            }
            remove
            {
                sm_InitializeChanged = true;
                sm_Initialize -= value;
            }
        }
        #endregion

        #region public static methods

        #region pool query methods

        /// <summary>Gets the holiday calendar names in its <see cref="IdentifierString"/> representation. 
        /// </summary>
        /// <returns>A collection of holiday calendar names in its <see cref="IdentifierString"/> representation.</returns>
        public static IEnumerable<IdentifierString> GetIdentifierStringNames()
        {
            OnInitialize();
            return sm_Pool.GetNamesAsIdentifierStrings();
        }

        /// <summary>Gets a specified holiday calendar.
        /// </summary>
        /// <param name="name">The name of the holiday calendar.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(IdentifierString name, out IHolidayCalendar value)
        {
            OnInitialize();
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets a specified holiday calendar.
        /// </summary>
        /// <param name="name">The name of the holiday calendar.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        public static bool TryGetValue(string name, out IHolidayCalendar value)
        {
            OnInitialize();
            return sm_Pool.TryGetValue(name, out value);
        }

        /// <summary>Gets the collection of holiday calendars with respect to a specific region.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns>The collection of <see cref="IHolidayCalendar"/> instances with respect to <paramref name="region"/>.</returns>
        public static IEnumerable<IHolidayCalendar> GetCalendars(HolidayCalendarRegion region)
        {
            OnInitialize();
            if (region != HolidayCalendarRegion.Unspecified)
            {
                return from holidayCalendar in sm_Pool.Values
                       where (holidayCalendar.Region & region) == region
                       select holidayCalendar;
            }
            else  // region = 0x00
            {
                return from holidayCalendar in sm_Pool.Values
                       where holidayCalendar.Region == region
                       select holidayCalendar;
            }
        }

        /// <summary>Gets the collection of holiday calendars.
        /// </summary>
        /// <returns>The collection of <see cref="IHolidayCalendar"/>.</returns>
        public static IEnumerable<IHolidayCalendar> GetCalendars()
        {
            OnInitialize();
            return sm_Pool.Values;
        }

        /// <summary>Adds the specified holiday calendar.
        /// </summary>
        /// <param name="value">The holiday calendar.</param>
        /// <returns>A value indicating whether <paramref name="value"/> has been inserted.</returns>
        public static ItemAddedState Add(IHolidayCalendar value)
        {
            ItemAddedState state = sm_Pool.Add(value);
//            sm_Logger.Add_PoolItemState(state, (value != null) ? value.Name : null);
            return state;
        }
        #endregion

        #region IHolidayCalendar factory methods

        /// <summary>Gets the holiday calendar from a specific enumeration, where the elements of the enumeration has to 
        /// have some <see cref="HolidayAttribute"/> attribute containing the holiday information and the enumeration
        /// has to have a <see cref="CustomHolidayCalendarAttribute"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="System.Type"/> of the enumeration where each element
        /// contains some <see cref="HolidayAttribute"/> attribute.</param>
        /// <returns>A holiday calendar with respect to <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="enumType"/> does not represent an enumeration or no attributes are given.</exception>
        public static IHolidayCalendar Create(Type enumType)
        {
            if ((enumType == null) || (!enumType.IsEnum))
            {
                throw new ArgumentException("Enumeration types allowed only.", "enumType");
            }
            CustomHolidayCalendarAttribute enumTypeAttribute = EnumAttribute.Create<CustomHolidayCalendarAttribute>(enumType);

            if ((enumTypeAttribute == null) || (enumTypeAttribute.FullResourceName == null) || (enumTypeAttribute.FullResourceName.Length == 0))
            {
                throw new InvalidOperationException("The enumeration type '" + enumType.Name + "' does not contains the 'CustomHolidayCalendarAttribute' with a valid resource name.");
            }
            /* now get the list of holidays with respect to the holiday calendar: */
            List<IHoliday> holidayList = new List<IHoliday>();
            ResourceManager resourceManager = new ResourceManager(enumTypeAttribute.FullResourceName, enumType.Assembly);
            foreach (Enum holiday in Enum.GetValues(enumType))
            {
                HolidayAttribute holidayAttribute = EnumAttribute.Create<HolidayAttribute>(holiday);
                if (holidayAttribute != null)
                {
                    holidayList.Add(holidayAttribute.GetHoliday(resourceManager));
                }
            }
            IdentifierString calendarName = new IdentifierString(enumTypeAttribute.CalendarName);

            /* try to get some (language depending) description of the holiday calendar: */
            string description = null;

            if ((enumTypeAttribute.Description != null) && (String.IsNullOrEmpty(enumTypeAttribute.Description) == false))
            {
                description = resourceManager.GetString(enumTypeAttribute.Description, Thread.CurrentThread.CurrentUICulture);
            }
            return new CustomHolidayCalendar(calendarName, enumTypeAttribute.Region, holidayList, enumTypeAttribute.FirstDate, enumTypeAttribute.LastDate, description);
        }

        ///<summary>Creates a new <see cref="IHolidayCalendar"/> object.</summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="holidayCalendarRegion">The region with respect to the holiday calendar.</param>
        /// <param name="holidayList">The reference (shallow copy) of the list of holidays taken into account for the holiday calendar.</param>
        /// <param name="firstDate">The earliest date for which holiday informations are available.</param>
        /// <param name="lastDate">The lastest date for which holiday informations are available.</param>
        /// <param name="annotation">The (optional) annotation, i.e. description, of the holiday calendar.</param>
        /// <param name="weekendRepresentation">The weekend representation, if <c>null</c> Saturdays and Sundays are always non-working days.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="calendarName"/> or <paramref name="holidayList"/> is <c>null</c>.</exception>
        /// <returns>A <see cref="IHolidayCalendar"/> object with respect to the list of holidays.</returns>
        public static IHolidayCalendar Create(IdentifierString calendarName, HolidayCalendarRegion holidayCalendarRegion, IEnumerable<IHoliday> holidayList, DateTime firstDate, DateTime lastDate, string annotation = null, IWeekendRepresentation weekendRepresentation = null)
        {
            return new CustomHolidayCalendar(calendarName, holidayCalendarRegion, holidayList, firstDate, lastDate, annotation, weekendRepresentation);
        }

        ///<summary>Creates a new <see cref="IHolidayCalendar"/> object.</summary>
        /// <param name="calendarName">The name of the calendar.</param>
        /// <param name="holidayCalendarRegion">The region with respect to the holiday calendar.</param>
        /// <param name="holidayList">The reference (shallow copy) of the list of holidays taken into account for the holiday calendar.</param>
        /// <param name="annotation">The (optional) annotation, i.e. description, of the holiday calendar.</param>
        /// <param name="weekendRepresentation">The weekend representation, if <c>null</c> Saturdays and Sundays are always non-working days.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="calendarName"/> or <paramref name="holidayList"/> is <c>null</c>.</exception>
        /// <returns>A <see cref="IHolidayCalendar"/> object with respect to the list of holidays.</returns>
        /// <remarks>
        /// <see cref="IHolidayCalendar.FirstDate"/> will be set to <see cref="System.DateTime.MinValue"/> and <see cref="IHolidayCalendar.LastDate"/> will 
        /// be set to <see cref="System.DateTime.MaxValue"/>.
        /// </remarks>
        public static IHolidayCalendar Create(IdentifierString calendarName, HolidayCalendarRegion holidayCalendarRegion, IEnumerable<IHoliday> holidayList, string annotation = null, IWeekendRepresentation weekendRepresentation = null)
        {
            return new CustomHolidayCalendar(calendarName, holidayCalendarRegion, holidayList, DateTime.MinValue, DateTime.MaxValue, annotation, weekendRepresentation);
        }

        /// <summary>Joins a list of holiday calendar.
        /// </summary>
        /// <param name="calendarName">The name of the holiday calendar.</param>
        /// <param name="holidayCalendarJoinRule">The holiday calendar join rule.</param>
        /// <param name="holidayCalendarList">The holiday calendar list.</param>
        /// <returns>A <see cref="IHolidayCalendar"/> object that represents a holiday calendar with respect to <paramref name="holidayCalendarList"/>
        /// and a specific <paramref name="holidayCalendarJoinRule"/>.</returns>
        public static IHolidayCalendar Join(IdentifierString calendarName, HolidayCalendarJointRule holidayCalendarJoinRule, IEnumerable<IHolidayCalendar> holidayCalendarList)
        {
            switch (holidayCalendarJoinRule)
            {
                case HolidayCalendarJointRule.JoinHolidays:
                    return new UnionHolidaysJointHolidayCalendar(calendarName, holidayCalendarList);

                case HolidayCalendarJointRule.JoinBusinessDays:
                    return new UnionBusinessDaysJoinHolidayCalendar(calendarName, holidayCalendarList);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Joins two holiday calendar.
        /// </summary>
        /// <param name="calendarName">The name of the holiday calendar.</param>
        /// <param name="holidayCalendarJoinRule">The holiday calendar join rule.</param>
        /// <param name="firstHolidayCalendar">The first holiday calendar.</param>
        /// <param name="secondHolidayCalendar">The second holiday calendar.</param>
        /// <returns>A <see cref="IHolidayCalendar"/> object that represents a holiday calendar with respect to <paramref name="firstHolidayCalendar"/>
        /// <paramref name="secondHolidayCalendar"/> and a specific <paramref name="holidayCalendarJoinRule"/>.</returns>
        public static IHolidayCalendar Join(IdentifierString calendarName, HolidayCalendarJointRule holidayCalendarJoinRule, IHolidayCalendar firstHolidayCalendar, IHolidayCalendar secondHolidayCalendar)
        {
            switch (holidayCalendarJoinRule)
            {
                case HolidayCalendarJointRule.JoinHolidays:
                    return new UnionHolidaysJointHolidayCalendar(calendarName, firstHolidayCalendar, secondHolidayCalendar);

                case HolidayCalendarJointRule.JoinBusinessDays:
                    return new UnionBusinessDaysJoinHolidayCalendar(calendarName, firstHolidayCalendar, secondHolidayCalendar);

                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #endregion

        #region private (static) methods

        /// <summary>Raises the <see cref="Initialize"/> event.
        /// </summary>
        private static void OnInitialize()
        {
            if (sm_InitializeChanged == true)
            {
                /* first remove all IHolidayCalendar objects */
                sm_Pool = new IdentifierNameableDictionary<IHolidayCalendar>(100, isReadOnlyExceptAdding: true);  // clear is not allowed, it is readonly
                sm_Pool.Add(EmptyHolidayCalendar);
                sm_Pool.Add(WeekendCalendar);

                /* add all IHolidayCalendar objects */
                if (sm_Initialize != null)
                {
                    InitializeEventArgs eventArgs = new InitializeEventArgs();
                    sm_Initialize(eventArgs);
                }
                sm_InitializeChanged = false;
            }
        }
        #endregion
    }
}