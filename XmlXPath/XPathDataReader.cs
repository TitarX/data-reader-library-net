//Подготовка xml-документа с возможностью проверки соответствия его xml-схеме,
//и получение данных из подготовленного xml-документа посредством XPath-выражений

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Collections;

namespace XmlXPath
{
    public class XPathDataReader
    {
        private XPathNavigator xPathNavigator;

        private XPathDataReader(XPathNavigator xPathNavigator)
        {
            this.xPathNavigator = xPathNavigator;
        }

        //Создать объект этого класса, содержащий xml-документ из xml-файла
        public static XPathDataReader newInstance(FileInfo xmlFileInfo)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileInfo.FullName);
            return new XPathDataReader(xmlDocument.CreateNavigator());
        }

        //Создать объект этого класса, содержащий xml-документ из строки
        public static XPathDataReader newInstance(String xmlString)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            return new XPathDataReader(xmlDocument.CreateNavigator());
        }

        //Создать объект этого класса, содержащий xml-документ из xml-файла,
        //проверив соответствие xml-схемам из xsd-файлов
        public static XPathDataReader newInstance(FileInfo xmlFileInfo, params KeyValuePair<String, FileInfo>[] targetNamespacesAndSchemaFiles)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            foreach (KeyValuePair<String, FileInfo> targetNamespaceAndSchemaFile in targetNamespacesAndSchemaFiles)
            {
                xmlReaderSettings.Schemas.Add(targetNamespaceAndSchemaFile.Key, targetNamespaceAndSchemaFile.Value.FullName);
            }

            XmlReader xmlReader = XmlReader.Create(xmlFileInfo.FullName, xmlReaderSettings);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            xmlReader.Close();

            return new XPathDataReader(xmlDocument.CreateNavigator());
        }

        //Создать объект этого класса, содержащий xml-документ из xml-файла,
        //проверив соответствие xml-схемам из строк
        public static XPathDataReader newInstance(FileInfo xmlFileInfo, params KeyValuePair<String, String>[] targetNamespacesAndSchemaFiles)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            foreach (KeyValuePair<String, String> targetNamespaceAndSchemaFile in targetNamespacesAndSchemaFiles)
            {
                xmlReaderSettings.Schemas.Add(targetNamespaceAndSchemaFile.Key, XmlReader.Create(new StringReader(targetNamespaceAndSchemaFile.Value)));
            }

            XmlReader xmlReader = XmlReader.Create(xmlFileInfo.FullName, xmlReaderSettings);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            xmlReader.Close();

            return new XPathDataReader(xmlDocument.CreateNavigator());
        }

        //Создать объект этого класса, содержащий xml-документ из строки,
        //проверив соответствие xml-схемам из xsd-файлов
        public static XPathDataReader newInstance(String xmlString, params KeyValuePair<String, FileInfo>[] targetNamespacesAndSchemaFiles)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            foreach (KeyValuePair<String, FileInfo> targetNamespaceAndSchemaFile in targetNamespacesAndSchemaFiles)
            {
                xmlReaderSettings.Schemas.Add(targetNamespaceAndSchemaFile.Key, targetNamespaceAndSchemaFile.Value.FullName);
            }

            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlString), xmlReaderSettings);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            xmlReader.Close();

            return new XPathDataReader(xmlDocument.CreateNavigator());
        }

        //Создать объект этого класса, содержащий xml-документ из строки,
        //проверив соответствие xml-схемам из строк
        public static XPathDataReader newInstance(String xmlString, params KeyValuePair<String, String>[] targetNamespacesAndSchemaFiles)
        {
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            foreach (KeyValuePair<String, String> targetNamespaceAndSchemaFile in targetNamespacesAndSchemaFiles)
            {
                xmlReaderSettings.Schemas.Add(targetNamespaceAndSchemaFile.Key, XmlReader.Create(new StringReader(targetNamespaceAndSchemaFile.Value)));
            }

            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlString), xmlReaderSettings);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            xmlReader.Close();

            return new XPathDataReader(xmlDocument.CreateNavigator());
        }

        //Получить данные из xml-документа этого класса по XPath-выражению
        public ArrayList ReadData(String xPathString, params KeyValuePair<String, String>[] aliasesAndNamespaces)
        {
            ArrayList resultArrayList = new ArrayList();

            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
            foreach (KeyValuePair<String, String> aliasAndNamespace in aliasesAndNamespaces)
            {
                xmlNamespaceManager.AddNamespace(aliasAndNamespace.Key, aliasAndNamespace.Value);
            }

            XPathNodeIterator xPathNodeIterator = xPathNavigator.Select(xPathString, xmlNamespaceManager);
            while (xPathNodeIterator.MoveNext())
            {
                resultArrayList.Add(xPathNodeIterator.Current.Value);
            }

            return resultArrayList;
        }
    }
}
