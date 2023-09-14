using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace HandNote.Models
{
    [Serializable]
    public class NoteBookItem
    {
        /// <summary>
        /// Gets or sets InkCanvas Editing mode
        /// </summary>
        public string EditingMode { get; set; }

        /// <summary>
        /// Gets or sets Pen Color.
        /// </summary>
        public string PenColor { get; set; }

        /// <summary>
        /// Gets or sets Pen Color for Highlighter.
        /// </summary>
        public string PenColorForHighlighter { get; set; }

        /// <summary>
        /// Gets or sets whether IsHighlighter is enabled or not.
        /// </summary>
        public bool IsHighLighter { get; set; }

        /// <summary>
        /// Ink thickness
        /// </summary>
        public double InkThickness { get; set; }

        /// <summary>
        /// Ink thickness for highlighter;
        /// </summary>
        public double InkThicknessForHighlighter { get; set; }

        /// <summary>
        /// Unique ID for the note item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Display Name for the note item.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Variable for point array
        /// </summary>
        public Point[][] Points { get; set; }

        public List<DrawingMode> DrawingModes { get; set; }

        public NoteBookItem()
        {
            DrawingModes = new List<DrawingMode>();
        }
    }
}
