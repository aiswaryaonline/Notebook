using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows;
using HandNote.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandNote.Extensions
{ 
   /// <summary>
  /// PointArrayExtension class
  /// </summary>
    public static class PointArrayExtension
    {
        /// <summary>
        /// Method to generate stroke
        /// </summary>
        /// <param name="pointArray">point array</param>
        /// <param name="drawingMode">drawingMode</param>
        /// <returns></returns>
        public static Stroke GenerateStroke(this Point[] pointArray, DrawingMode drawingMode)
        {
            StylusPointCollection stylusCollection = new StylusPointCollection(pointArray);
            Stroke stroke = new Stroke(stylusCollection);

            var color = ColorConverter.ConvertFromString(drawingMode.Color);
            if (color != null)
            {
                stroke.DrawingAttributes.Color = (Color) color;
            }
            stroke.DrawingAttributes.IsHighlighter = drawingMode.IsHighlighter;
            stroke.DrawingAttributes.Width = drawingMode.Thickness;
            stroke.DrawingAttributes.Height = drawingMode.Thickness;
            return stroke;
        }
    }
}
