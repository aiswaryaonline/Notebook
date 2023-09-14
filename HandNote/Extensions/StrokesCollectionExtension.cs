using HandNote.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Ink;

namespace HandNote.Extensions
{
    public static class StrokesCollectionExtension
    {
        /// <summary>
        /// Method to generate point array
        /// </summary>
        /// <param name="strokes">stroke collection</param>
        /// <returns>point array</returns>
        public static Point[][] GeneratePointArray(this StrokeCollection strokes)
        {
            Point[][] pointArray = new Point[strokes.Count][];
            // canvasModel.Modes = new DrawingMode[strokes.Count];

            for (int i = 0; i < strokes.Count; i++)
            {
                pointArray[i] = new Point[strokes[i].StylusPoints.Count];

                for (int j = 0; j < strokes[i].StylusPoints.Count; j++)
                {
                    pointArray[i][j] = new Point();
                    pointArray[i][j].X = strokes[i].StylusPoints[j].X;
                    pointArray[i][j].Y = strokes[i].StylusPoints[j].Y;
                }
            }
            return pointArray;
        }

        /// <summary>
        /// Method to get drawing modes with color value
        /// </summary>
        /// <param name="strokes">strokes array</param>
        /// <returns>Array of DrawingMode</returns>
        public static List<DrawingMode> GetDrawingModes(this StrokeCollection strokes)
        {
            List<DrawingMode> drawingModeList = new List<DrawingMode>();

            for (int i = 0; i < strokes.Count; i++)
            {
                string color = strokes[i].DrawingAttributes.Color.ToString();
                DrawingMode drawingMode = new DrawingMode()
                {
                    Color = color,
                    IsHighlighter = strokes[i].DrawingAttributes.IsHighlighter,
                    Thickness = strokes[i].DrawingAttributes.Width
                };
                drawingModeList.Add(drawingMode);
            }
            return drawingModeList;
        }
    }
}
