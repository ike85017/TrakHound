﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;

using TH_Configuration;

namespace TH_Shifts
{
    public class ShiftConfiguration
    {
        public ShiftConfiguration()
        {
            shifts = new List<Shift>();
            generatedEvents = new List<GeneratedEventConfiguration>();
        }

        public List<Shift> shifts;

        public List<GeneratedEventConfiguration> generatedEvents;


        public static ShiftConfiguration Read(XmlDocument configXML)
        {
            var result = new ShiftConfiguration();

            XmlNodeList nodes = configXML.SelectNodes("/Settings/ShiftData");

            if (nodes != null)
            {
                if (nodes.Count > 0)
                {

                    XmlNode node = nodes[0];

                    foreach (XmlNode Child in node.ChildNodes)
                    {
                        if (Child.NodeType == XmlNodeType.Element)
                        {

                            Type Setting = typeof(ShiftConfiguration);
                            PropertyInfo info = Setting.GetProperty(Child.Name);

                            if (info != null)
                            {
                                Type t = info.PropertyType;
                                info.SetValue(result, Convert.ChangeType(Child.InnerText, t), null);
                            }
                            else
                            {
                                switch (Child.Name.ToLower())
                                {
                                    case "shifts":

                                        foreach (XmlNode shiftNode in Child.ChildNodes)
                                        {
                                            if (shiftNode.Name.ToLower() == "shift")
                                            {
                                                if (shiftNode.NodeType == XmlNodeType.Element)
                                                {
                                                    result.shifts.Add(ProcessShift(shiftNode));
                                                }
                                            }
                                        }

                                        break;

                                    case "generatedevents":

                                        foreach (XmlNode eventNode in Child.ChildNodes)
                                        {

                                            if (eventNode.Name.ToLower() == "event")
                                            {
                                                if (eventNode.NodeType == XmlNodeType.Element)
                                                {
                                                    result.generatedEvents.Add(ProcessGeneratedEvent(eventNode));
                                                }
                                            }

                                        }

                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static ShiftConfiguration Get(Configuration configuration)
        {
            ShiftConfiguration result = null;

            var customClass = configuration.CustomClasses.Find(x => x.GetType() == typeof(ShiftConfiguration));
            if (customClass != null) result = (ShiftConfiguration)customClass;

            return result;
        }

        #region "Private"

        static Shift ProcessShift(XmlNode node)
        {
            Shift Result = null;

            if (node.Attributes != null)
            {
                if (node.Attributes["name"] != null &&
                    node.Attributes["id"] != null &&
                    node.Attributes["begintime"] != null &&
                    node.Attributes["endtime"] != null
                    )
                {

                    Result = new Shift();

                    int id;
                    int.TryParse(node.Attributes["id"].Value, out id);
                    Result.id = id;

                    Result.name = node.Attributes["name"].Value;

                    DateTime beginTime = DateTime.MinValue;
                    if (DateTime.TryParse(node.Attributes["begintime"].Value, out beginTime))
                    {
                        Result.beginTime = new ShiftTime(beginTime, false);
                    }

                    DateTime endTime = DateTime.MinValue;
                    if (DateTime.TryParse(node.Attributes["endtime"].Value, out endTime))
                    {
                        Result.endTime = new ShiftTime(endTime, false);
                    }

                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.NodeType == XmlNodeType.Element)
                        {
                            switch (child.Name.ToLower())
                            {
                                case "segments":

                                    Result.segments = ProcessSegments(child, Result);

                                    break;

                                case "days":

                                    Result.days = ProcessDays(child);

                                    break;
                            }
                        }
                    }
                }
            }

            return Result;
        }

        static List<Segment> ProcessSegments(XmlNode node, Shift shift)
        {
            List<Segment> Result = new List<Segment>();

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element && child.Attributes != null)
                {
                    if (child.Attributes["id"] != null && child.Attributes["begintime"] != null && child.Attributes["endtime"] != null)
                    {
                        Segment sc = new Segment();
                        int id;
                        int.TryParse(child.Attributes["id"].Value, out id);
                        sc.id = id;
                        sc.type = child.Name;
                        sc.shift = shift;

                        ShiftTime begin;
                        ShiftTime end;

                        if (ShiftTime.TryParse(child.Attributes["begintime"].Value, out begin) &&
                        ShiftTime.TryParse(child.Attributes["endtime"].Value, out end))
                        {
                            sc.beginTime = begin;
                            sc.endTime = end;

                            Result.Add(sc);
                        }
                    }
                }
            }

            return Result;
        }

        static List<DayOfWeek> ProcessDays(XmlNode node)
        {
            List<DayOfWeek> Result = new List<DayOfWeek>();

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element && child.InnerText != null)
                {
                    if (child.Name.ToLower() == "day")
                    {
                        switch (child.InnerText.ToLower())
                        {
                            case "sunday":
                            case "sun": Result.Add(DayOfWeek.Sunday); break;
                            case "monday":
                            case "mon": Result.Add(DayOfWeek.Monday); break;
                            case "tuesday":
                            case "tue": Result.Add(DayOfWeek.Tuesday); break;
                            case "wednesday":
                            case "wed": Result.Add(DayOfWeek.Wednesday); break;
                            case "thursday":
                            case "thur": Result.Add(DayOfWeek.Thursday); break;
                            case "friday":
                            case "fri": Result.Add(DayOfWeek.Friday); break;
                            case "saturday":
                            case "sat": Result.Add(DayOfWeek.Saturday); break;
                        }
                    }
                }
            }

            return Result;
        }

        static GeneratedEventConfiguration ProcessGeneratedEvent(XmlNode node)
        {
            GeneratedEventConfiguration Result = null;

            if (node.InnerText != null)
            {
                Result = new GeneratedEventConfiguration();
                Result.name = node.InnerText.ToUpper();
            }

            return Result;
        }

        #endregion
    }

    public class Shift
    {
        public Shift()
        {
            segments = new List<Segment>();
            days = new List<DayOfWeek>();
        }

        public int id { get; set; }

        public string name { get; set; }

        public ShiftTime beginTime { get; set; }
        public ShiftTime endTime { get; set; }

        public List<Segment> segments;

        public List<DayOfWeek> days;
    }

    public class Segment
    {
        public int id { get; set; }

        public string type { get; set; }

        public ShiftTime beginTime { get; set; }
        public ShiftTime endTime { get; set; }

        public Shift shift { get; set; }

        public int beginDayOffset { get; set; }
        public int endDayOffset { get; set; }

        #region "operator overrides"

        public static bool operator ==(Segment sc1, Segment sc2)
        {
            if (ReferenceEquals(sc1, null) && ReferenceEquals(sc2, null)) return true;
            else if (ReferenceEquals(sc1, null) && !ReferenceEquals(sc2, null)) return false;
            else if (!ReferenceEquals(sc1, null) && ReferenceEquals(sc2, null)) return false;
            else if (sc1.id == sc2.id) return true;
            else return false;
        }

        public static bool operator !=(Segment sc1, Segment sc2)
        {
            if (ReferenceEquals(sc1, null) && ReferenceEquals(sc2, null)) return false;
            else if (ReferenceEquals(sc1, null) && !ReferenceEquals(sc2, null)) return true;
            else if (!ReferenceEquals(sc1, null) && ReferenceEquals(sc2, null)) return true;
            else if (sc1.id != sc2.id) return true;
            else return false;
        }

        #endregion
    }
}
