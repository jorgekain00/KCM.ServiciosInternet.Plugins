/*
 *  @Author:  Ing. Jorge Flores Miguel      KCUS\C84818
 *  @Email :  jorgekain00@gmail.com
 *  @Date  :  January 2021
 *
 */


namespace KCM.ServiciosInternet.Common.Library.Xml
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System;
    using System.Text.RegularExpressions;
    /// <summary>
    /// Helper Class with a variety of operations related to XML Documents
    /// </summary>
    /// <list type="bullet">
    /// </list>
    /// /// <item>
    /// <term>Add</term>
    /// <description>Addition Operation</description>
    /// </item>
    internal class XmlHelper
    {
        public Dictionary<string, string> getDictionaryFromXML(string strPath, string strRoot, string strXQuery)
        {
            string strAttrValuePattern = @"\(.)+[(.)+\]";
            string lstAttr = string.Empty;
            string strElement = string.Empty;
            int inIndex = 0;
            Dictionary<string, string> objDic = new Dictionary<string, string>();
            string strCheckParams = string.IsNullOrEmpty(strXQuery) ? "strXQuery" : string.IsNullOrEmpty(strPath) ? "strPath" : string.Empty;

            if (strCheckParams.Length > 0)
            {
                throw new ArgumentException("{0} param is missing", strCheckParams);
            }

            string[] strLevels = strXQuery.Split('/');
            XDocument objDocXML = XDocument.Load(strPath);

            var objRoots = (from r in objDocXML.Descendants(strRoot) select r);

            //    if (objRoot != null)
            //    {
            //        foreach (Match objmatch in Regex.Matches(strLevels[inIndex], strAttrValuePattern, RegexOptions.IgnoreCase))
            //        {
            //            strElement = objmatch.Groups[1].Value;
            //            lstAttr = objmatch.Groups[2].Value;
            //        }

            //        var objNode = (from lev in objRoot.Descendants(strElement) select lev);

            //    }

            //    foreach (var objRoot in objRoots)
            //    {

            //    }

            //    foreach (var xmlElem in objLinqArchivoElemento)                     // Obtiene los subnodos y sus valores en formato diccionario.
            //    {
            //        objListaModulos.Add((from nodo in xmlElem.Descendants()
            //                             select nodo).ToDictionary<XElement, string, string>((xElem) => xElem.Name.ToString(), (xElem) => xElem.Value));
            //    }

            //    return objListaModulos;
            throw new NotImplementedException();
        }
    }
}
