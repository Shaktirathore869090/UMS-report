using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
namespace DocxTemplater.Processors
{
    public class TableProcessor
    {
        private static readonly Regex VMergePattern = new Regex(@"\{\{vmerge}}(.+)", RegexOptions.IgnoreCase);
        private static readonly Regex HMergePattern = new Regex(@"\{\{hmerge}}(.+)", RegexOptions.IgnoreCase);

        public void ProcessTable_Old_1(XElement tableElement)
        {
            var rows = tableElement.Elements().Where(e => e.Name.LocalName == "tr").ToList();
            string lastMergedValue = null;  // Track the previous row's merge value

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var cells = row.Elements().Where(tc => tc.Name.LocalName == "tc").ToList();

                if (cells.Count > 0)
                {
                    var firstCell = cells[0]; // First column where merging occurs
                    string cellText = firstCell.Value.Trim();

                    var match = VMergePattern.Match(cellText);
                    if (match.Success)
                    {
                        string mergeValue = match.Groups[1].Value; // Extract number after `{{vmerge}}`

                        if (!string.IsNullOrEmpty(mergeValue) && lastMergedValue == mergeValue)
                        {
                            ApplyVerticalMerge(firstCell, continueMerge: true);
                            firstCell.Value = ""; // Remove duplicate text
                        }
                        else
                        {
                            ApplyVerticalMerge(firstCell, continueMerge: false);
                            lastMergedValue = mergeValue;
                        }
                    }
                    else
                    {
                        lastMergedValue = null; // Reset when there's no `{{vmerge}}`
                    }
                }
            }
        }

        public void ProcessTable_Old(XElement tableElement)
        {
            var rows = tableElement.Elements().Where(e => e.Name.LocalName == "tr").ToList();
            string lastMergedValue = null;  // Track the last seen merge value

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var cells = row.Elements().Where(tc => tc.Name.LocalName == "tc").ToList();

                if (cells.Count > 0)
                {
                    var firstCell = cells[0]; // First column where merging occurs
                    string cellText = firstCell.Value.Trim();

                    var match = VMergePattern.Match(cellText);
                    if (match.Success)
                    {
                        string mergeValue = match.Groups[1].Value; // Extract the number from `{{vmergeX}}`

                        if (!string.IsNullOrEmpty(mergeValue) && lastMergedValue == mergeValue)
                        {
                            ApplyVerticalMerge(firstCell, continueMerge: true);
                            ClearCellText(firstCell); // Remove duplicate text from merged rows
                        }
                        else
                        {
                            ApplyVerticalMerge(firstCell, continueMerge: false);
                            CleanupVMergeTag(firstCell); // Set the actual value
                            lastMergedValue = mergeValue;
                        }
                    }
                    else
                    {
                        lastMergedValue = null; // Reset when there's no vmerge
                    }
                }
            }
        }

        public void ProcessTable(XElement tableElement)
        {
            ProcessVMerge(tableElement);
            ProcessHMerge(tableElement);
        }

        internal void ProcessVMerge(XElement tableElement)
        {
            var rows = tableElement.Elements().Where(e => e.Name.LocalName == "tr").ToList();

            // Dictionary to track last seen merge values per column
            Dictionary<int, string> lastMergedValues = new Dictionary<int, string>();

            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var cells = row.Elements().Where(tc => tc.Name.LocalName == "tc").ToList();

                for (int colIndex = 0; colIndex < cells.Count; colIndex++) // Process each column
                {
                    var cell = cells[colIndex];
                    string cellText = cell.Value.Trim();

                    var match = VMergePattern.Match(cellText);
                    if (match.Success)
                    {
                        string mergeValue = match.Groups[1].Value; // Extract the number from `{{vmergeX}}`

                        if (!string.IsNullOrEmpty(mergeValue) &&
                            lastMergedValues.ContainsKey(colIndex) &&
                            lastMergedValues[colIndex] == mergeValue)
                        {
                            ApplyVerticalMerge(cell, continueMerge: true);
                            ClearCellText(cell); // Remove duplicate text from merged rows
                        }
                        else
                        {
                            ApplyVerticalMerge(cell, continueMerge: false);
                            CleanupVMergeTag(cell); // Remove `{{vmerge}}` but keep the rest of the content
                            lastMergedValues[colIndex] = mergeValue; // Update last seen merge value
                        }
                    }
                    else
                    {
                        lastMergedValues[colIndex] = null; // Reset merge tracking for this column
                    }
                }
            }
        }
        internal void ProcessHMerge(XElement tableElement)
        {
            var rows = tableElement.Elements().Where(e => e.Name.LocalName == "tr").ToList();

            foreach (var row in rows)
            {
                var cells = row.Elements().Where(tc => tc.Name.LocalName == "tc").ToList();
                string lastMergedValue = null; // Track the last seen merge value for this row
                XElement firstMergedCell = null; // Track the first cell in the merge sequence

                for (int colIndex = 0; colIndex < cells.Count; colIndex++)
                {
                    var cell = cells[colIndex];
                    string cellText = cell.Value.Trim();

                    var match = HMergePattern.Match(cellText);
                    if (match.Success)
                    {
                        string mergeValue = match.Groups[1].Value; // Extract number from `{{hmergeX}}`

                        if (!string.IsNullOrEmpty(mergeValue) && lastMergedValue == mergeValue)
                        {
                            ApplyHorizontalMerge(cell, continueMerge: true);
                            ClearCellText(cell); // Remove duplicate text from merged columns
                        }
                        else
                        {
                            ApplyHorizontalMerge(cell, continueMerge: false);
                            CleanupHMergeTag(cell); // Remove `{{hmerge}}` but keep rest of content
                            lastMergedValue = mergeValue;
                            firstMergedCell = cell; // Set the first merged cell reference
                        }
                    }
                    else
                    {
                        lastMergedValue = null; // Reset when no `hmerge` is found
                        firstMergedCell = null;
                    }
                }
            }
        }


        private static void ClearCellText(XElement cell)
        {
            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

            // Find all <w:t> elements inside <w:r>
            foreach (var textElement in cell.Descendants(w + "t"))
            {
                textElement.Value = ""; // Clear text while preserving structure
            }
        }

        private static void CleanupVMergeTag(XElement cell, string newValue="")
        {
            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

            foreach (var textElement in cell.Descendants(w + "t"))
            {
                string originalText = textElement.Value;

                // Replace "{{vmerge}}" but keep the rest of the text
                if (originalText.Contains("{{vMerge}}") 
                    || originalText.Contains("{{vmerge}}")
                    )
                {
                    textElement.Value = originalText
                        .Replace("{{vMerge}}", "")
                        .Replace("{{vmerge}}", "")
                        .Trim() + newValue;
                }
            }
        }
        private static void CleanupHMergeTag(XElement cell, string newValue = "")
        {
            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

            foreach (var textElement in cell.Descendants(w + "t"))
            {
                string originalText = textElement.Value;

                // Replace "{{hmerge}}" but keep the rest of the text
                if (originalText.Contains("{{hMerge}}")
                    || originalText.Contains("{{hmerge}}")
                    )
                {
                    textElement.Value = originalText
                        .Replace("{{hMerge}}", "")
                        .Replace("{{hmerge}}", "")
                        .Trim() + newValue;
                }
            }
        }
        private void ApplyVerticalMerge(XElement cell, bool continueMerge)
        {
            // Ensure the namespace is correctly handled
            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

            // Get or create <w:tcPr>
            XElement tcPr = cell.Element(w + "tcPr");
            if (tcPr == null)
            {
                tcPr = new XElement(w + "tcPr");
                cell.AddFirst(tcPr); // Ensure it's added before the text content
            }

            // Remove any existing <w:vMerge> if present
            XElement existingVMerge = tcPr.Element(w + "vMerge");
            existingVMerge?.Remove();

            // Add the <w:vMerge> element with appropriate attribute
            XElement vMerge = new XElement(w + "vMerge", 
                !continueMerge ? 
                new XAttribute(w + "val", "restart") : "");

            tcPr.Add(vMerge);
        }

        private void ApplyHorizontalMerge(XElement cell, bool continueMerge)
        {
            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

            // Get or create <w:tcPr>
            XElement tcPr = cell.Element(w + "tcPr");
            if (tcPr == null)
            {
                tcPr = new XElement(w + "tcPr");
                cell.AddFirst(tcPr);
            }

            // Remove any existing <w:hMerge> if present
            XElement existingHMerge = tcPr.Element(w + "hMerge");
            existingHMerge?.Remove();

            // Add <w:hMerge> element
            XElement hMerge = new XElement(w + "hMerge",
                !continueMerge ?
                new XAttribute(w + "val", "restart") : "");
            tcPr.Add(hMerge);
        }


        private static XElement GetOrCreateChildElement(XElement parent, string localName)
        {
            var element = parent.Elements().FirstOrDefault(e => e.Name.LocalName == localName);
            if (element == null)
            {
                element = new XElement(XName.Get(localName, parent.Name.NamespaceName));
                parent.AddFirst(element);
            }
            return element;
        }
    }
}
