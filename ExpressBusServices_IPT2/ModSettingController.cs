﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ExpressBusServices_IPT2
{
    public class ModSettingController
    {
        public static readonly string pathToConfigXml = "ExpressBusServices_IPT2_Config.xml";

        public static void Touch()
        {
            // with JSON being so tedious in C# I can understand why everyone opted for XML setting files
            ReadSettings();
            WriteSettings();
        }

        public static void ReadSettings()
        {
            // default interpretation is first principles, but try to see if we have any config files around.
            IPT2UnbunchingRuleReader.InterpretationMode interpretation = IPT2UnbunchingRuleReader.InterpretationMode.FIRST_PRINCIPLES;

            if (File.Exists(pathToConfigXml))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(pathToConfigXml);
                    XmlNode root = document.FirstChild;
                    if (root.HasChildNodes)
                    {
                        var interestedNode = root.ChildNodes[0];
                        if (interestedNode.Name == "ExpressBusServices_IPT2_Config")
                        {
                            XmlNodeList configNodes = interestedNode.ChildNodes;
                            for (int i = 0; i < configNodes.Count; i++)
                            {
                                XmlNode currentNode = configNodes[i];
                                if (currentNode.Name == "SelectedIndex")
                                {
                                    string tempIndex = currentNode.InnerText;
                                    int selectedIndex = Convert.ToInt32(tempIndex);
                                    interpretation = (IPT2UnbunchingRuleReader.InterpretationMode)selectedIndex;
                                }
                            }
                        }
                    }
                }
                catch (Exception x)
                {
                    Debug.Log($"Could not load config file: {x}");
                }
            }

            IPT2UnbunchingRuleReader.CurrentRuleInterpretation = interpretation;
        }

        public static void WriteSettings()
        {
            var interpretation = IPT2UnbunchingRuleReader.CurrentRuleInterpretation;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.CloseOutput = true;

                XmlWriter writer = XmlWriter.Create(pathToConfigXml, settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("ExpressBusServices_IPT2_Config");

                writer.WriteStartElement("SelectedIndex");
                writer.WriteString(((int) interpretation).ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Close();
            }
            catch (Exception x)
            {
                Debug.Log($"Could not write to config file: {x}");
            }
        }
    }
}
