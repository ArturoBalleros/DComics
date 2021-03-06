﻿using System;
using System.Xml;

using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace ComicsIDownload
{
    /// <summary>
    /// Layout that formats the log events as XML elements compatible with the log4j schema
    /// </summary>
    /// <remarks>
    /// <para>
    /// Formats the log events according to the http://logging.apache.org/log4j schema.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    public class XmlLayoutSchemaLog4j : XmlLayoutBase
    {
        #region Static Members

        /// <summary>
        /// The 1st of January 1970 in UTC
        /// </summary>
        private static readonly DateTime s_date1970 = new DateTime(1970, 1, 1);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an XMLLayoutSchemaLog4j
        /// </summary>
        public XmlLayoutSchemaLog4j() : base()
        {
        }

        /// <summary>
        /// Constructs an XMLLayoutSchemaLog4j.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <b>LocationInfo</b> option takes a boolean value. By
        /// default, it is set to false which means there will be no location
        /// information output by this layout. If the the option is set to
        /// true, then the file name and line number of the statement
        /// at the origin of the log statement will be output. 
        /// </para>
        /// <para>
        /// If you are embedding this layout within an SMTPAppender
        /// then make sure to set the <b>LocationInfo</b> option of that 
        /// appender as well.
        /// </para>
        /// </remarks>
        public XmlLayoutSchemaLog4j(bool locationInfo) : base(locationInfo)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The version of the log4j schema to use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only version 1.2 of the log4j schema is supported.
        /// </para>
        /// </remarks>
        public string Version
        {
            get { return "1.2"; }
            set
            {
                if (value != "1.2")
                {
                    throw new ArgumentException("Only version 1.2 of the log4j schema is currently supported");
                }
            }
        }

        #endregion

        /// <summary>
        /// Actually do the writing of the xml
        /// </summary>
        /// <param name="writer">the writer to use</param>
        /// <param name="loggingEvent">the event to write</param>
        /// <remarks>
        /// <para>
        /// Generate XML that is compatible with the log4j schema.
        /// </para>
        /// </remarks>
        override protected void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
        {
            // Translate logging events for log4j

            // Translate hostname property
            if (loggingEvent.LookupProperty(LoggingEvent.HostNameProperty) != null && loggingEvent.LookupProperty("log4jmachinename") == null)
                loggingEvent.GetProperties()["log4jmachinename"] = loggingEvent.LookupProperty(LoggingEvent.HostNameProperty);


            // translate appdomain name
            if (loggingEvent.LookupProperty("log4japp") == null && loggingEvent.Domain != null && loggingEvent.Domain.Length > 0)
                loggingEvent.GetProperties()["log4japp"] = loggingEvent.Domain;


            // translate identity name
            if (loggingEvent.Identity != null && loggingEvent.Identity.Length > 0 && loggingEvent.LookupProperty(LoggingEvent.IdentityProperty) == null)
                loggingEvent.GetProperties()[LoggingEvent.IdentityProperty] = loggingEvent.Identity;


            // translate user name
            if (loggingEvent.UserName != null && loggingEvent.UserName.Length > 0 && loggingEvent.LookupProperty(LoggingEvent.UserNameProperty) == null)
                loggingEvent.GetProperties()[LoggingEvent.UserNameProperty] = loggingEvent.UserName;


            // Write the start element
            writer.WriteStartElement("log4j", "event", "log4j");
            writer.WriteAttributeString("logger", loggingEvent.LoggerName);

            // Calculate the timestamp as the number of milliseconds since january 1970
            TimeSpan timeSince1970 = loggingEvent.TimeStampUtc - s_date1970;

            writer.WriteAttributeString("timestamp", XmlConvert.ToString((long)timeSince1970.TotalMilliseconds));
            writer.WriteAttributeString("level", loggingEvent.Level.DisplayName);
            writer.WriteAttributeString("thread", loggingEvent.ThreadName);

            // Append the message text
            writer.WriteStartElement("log4j", "message", "log4j");
            Transform.WriteEscapedXmlString(writer, loggingEvent.RenderedMessage, InvalidCharReplacement);
            writer.WriteEndElement();

            object ndcObj = loggingEvent.LookupProperty("NDC");
            if (ndcObj != null)
            {
                string valueStr = loggingEvent.Repository.RendererMap.FindAndRender(ndcObj);

                if (valueStr != null && valueStr.Length > 0)
                {
                    // Append the NDC text
                    writer.WriteStartElement("log4j", "NDC", "log4j");
                    Transform.WriteEscapedXmlString(writer, valueStr, InvalidCharReplacement);
                    writer.WriteEndElement();
                }
            }

            // Append the properties text
            PropertiesDictionary properties = loggingEvent.GetProperties();
            if (properties.Count > 0)
            {
                writer.WriteStartElement("log4j", "properties", "log4j");
                foreach (System.Collections.DictionaryEntry entry in properties)
                {
                    writer.WriteStartElement("log4j", "data", "log4j");
                    writer.WriteAttributeString("name", (string)entry.Key);

                    // Use an ObjectRenderer to convert the object to a string
                    string valueStr = loggingEvent.Repository.RendererMap.FindAndRender(entry.Value);
                    writer.WriteAttributeString("value", valueStr);

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            string exceptionStr = loggingEvent.GetExceptionString();
            if (exceptionStr != null && exceptionStr.Length > 0)
            {
                // Append the stack trace line
                writer.WriteStartElement("log4j", "throwable", "log4j");
                Transform.WriteEscapedXmlString(writer, exceptionStr, InvalidCharReplacement);
                writer.WriteEndElement();
            }

            if (LocationInfo)
            {
                LocationInfo locationInfo = loggingEvent.LocationInformation;

                writer.WriteStartElement("log4j", "locationInfo", "log4j");
                writer.WriteAttributeString("class", locationInfo.ClassName);
                writer.WriteAttributeString("method", locationInfo.MethodName);
                writer.WriteAttributeString("file", locationInfo.FileName);
                writer.WriteAttributeString("line", locationInfo.LineNumber);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
