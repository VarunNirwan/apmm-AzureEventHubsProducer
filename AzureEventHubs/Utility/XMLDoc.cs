using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AzureEventHubs.Utility
{
    public static class XMLDoc
    {
        public static bool IsValidXMLString(this string xmlString)
        {
            try
            {
                var xmlData = XElement.Parse(xmlString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static XElement GetXML(this string xmlString)
        {
            XElement xmlData = null;
            if (xmlString.IsValidXMLString())
            {
                xmlData = XElement.Parse(xmlString);
                return xmlData;
            }
            else
            {
                return xmlData;
            }
        }

    }
}
