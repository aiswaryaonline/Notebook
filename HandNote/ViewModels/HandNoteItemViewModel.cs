using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HandNote.ViewModels
{
    public class HandNoteItemViewModel : ViewModelBase
    {

        #region Private Members
        private string _id;
        private string _displayName;
        private string _editingMode;
        private InkCanvas _canvas;
        private string _penColor;
        private string _penColorForHighlighter;
        private bool _isHighlighter;
        private double _inkThickness;
        private double _inkThicknessForHighlighter;
        public ICommand _addImageCommand;
        private ICommand _saveAsCommand;
        #endregion

        public HandNoteItemViewModel()
        {
            PropertyChanged += HandNoteItemViewModel_PropertyChanged;
            Canvas = new InkCanvas();
            Canvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
            Canvas.ClipToBounds = true;
            Canvas.DefaultDrawingAttributes.FitToCurve = true;

            EditingMode = "Ink";
            InkThickness = 2;
            InkThicknessForHighlighter = 2;
            Canvas.EraserShape = new EllipseStylusShape(2, 2);
            PenColorForHighlighter = "Yellow";
            PenColor = "Black";



            DoStrokes = new Stack<DoStroke>();
            UndoStrokes = new Stack<DoStroke>();
            Canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
            UndoCommand = new Command(new Action<object>(Undo), CanUndo);
            RedoCommand = new Command(new Action<object>(Redo), CanRedo);
            AddImageCommand = new Command(new Action<object>(OnAddImage));
            SaveAsCommand = new Command(new Action<object>(OnSaveAs));
        }

        private void HandNoteItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EditingMode):
                    {
                        if (Enum.TryParse(_editingMode, out InkCanvasEditingMode editingMode))
                        {
                            Canvas.EditingMode = editingMode;
                            IsHighLighter = false;
                        }
                        break;
                    }
                case nameof(PenColor):
                    {
                        var penColor = ColorConverter.ConvertFromString(_penColor);
                        if (penColor != null)
                        {
                            Canvas.DefaultDrawingAttributes.Color = (Color)penColor;
                        }
                        break;
                    }

                case nameof(PenColorForHighlighter):
                    {
                        var penColorForHighlighter = ColorConverter.ConvertFromString(_penColorForHighlighter);
                        if (penColorForHighlighter != null)
                        {
                            Canvas.DefaultDrawingAttributes.Color = (Color)penColorForHighlighter;
                        }
                        break;
                    }
                case nameof(IsHighLighter):
                    {
                        Canvas.DefaultDrawingAttributes.IsHighlighter = _isHighlighter;
                        if (_isHighlighter)
                        {
                            Canvas.EditingMode = InkCanvasEditingMode.Ink;
                            RaisePropertyChanged(nameof(PenColorForHighlighter));
                            RaisePropertyChanged(nameof(InkThicknessForHighlighter));
                        }
                        else
                        {
                            RaisePropertyChanged(nameof(PenColor));
                            RaisePropertyChanged(nameof(InkThickness));
                        }
                        break;
                    }
                case nameof(InkThickness):
                    {
                        if (Canvas != null)
                        {
                            var drawingAttributes = Canvas.DefaultDrawingAttributes;
                            double newSize = Math.Round(_inkThickness, 0);
                            drawingAttributes.Width = newSize;
                            drawingAttributes.Height = newSize;
                        }
                        break;
                    }
                case nameof(InkThicknessForHighlighter):
                    {
                        if (Canvas != null)
                        {
                            var drawingAttributes = Canvas.DefaultDrawingAttributes;
                            double newSize = Math.Round(_inkThicknessForHighlighter, 0);
                            drawingAttributes.Width = newSize;
                            drawingAttributes.Height = newSize;
                        }
                        break;
                    }
            }
        }

        #region Public Members
        /// <summary>
        /// Gets or sets InkCanvas
        /// </summary>
        public InkCanvas Canvas
        {
            get => _canvas;
            set
            {
                SetProperty(ref _canvas, value);
            }
        }

        /// <summary>
        /// Gets or sets InkCanvas Editing mode
        /// </summary>
        public string EditingMode
        {
            get => _editingMode;
            set
            {
                SetProperty(ref _editingMode, value);
            }
        }

        /// <summary>
        /// Gets or sets Pen Color.
        /// </summary>
        public string PenColor
        {
            get => _penColor;
            set
            {
                SetProperty(ref _penColor, value);
            }
        }

        /// <summary>
        /// Gets or sets Pen Color for Highlighter.
        /// </summary>
        public string PenColorForHighlighter
        {
            get => _penColorForHighlighter;
            set
            {
                SetProperty(ref _penColorForHighlighter, value);
            }
        }

        /// <summary>
        /// Gets or sets whether IsHighlighter is enabled or not.
        /// </summary>
        public bool IsHighLighter
        {
            get => _isHighlighter;
            set
            {
                SetProperty(ref _isHighlighter, value);
            }
        }

        /// <summary>
        /// Ink thickness
        /// </summary>
        public double InkThickness
        {
            get => _inkThickness;
            set
            {
                SetProperty(ref _inkThickness, value);
            }
        }

        /// <summary>
        /// Ink thickness for highlighter;
        /// </summary>
        public double InkThicknessForHighlighter
        {
            get => _inkThicknessForHighlighter;
            set
            {
                SetProperty(ref _inkThicknessForHighlighter, value);
            }
        }

        /// <summary>
        /// Unique ID for the note item.
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
            }
        }

        /// <summary>
        /// Display Name for the note item.
        /// </summary>
        public string DisplayName
        {
            get => _displayName;
            set
            {
                SetProperty(ref _displayName, value);
            }
        }

        /// <summary>
        /// Command to add image
        /// </summary>
        public ICommand AddImageCommand
        {
            get => _addImageCommand;
            set => _addImageCommand = value;
        }

        /// <summary>
        /// Command to save selected note as Image.
        /// </summary>
        public ICommand SaveAsCommand
        {
            get => _saveAsCommand;
            set => _saveAsCommand = value;
        }
        #endregion

        // Add image to Canvas.
        private void OnAddImage(object sender)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                string filePath = openFileDlg.FileName;
                Image dynamicImage = new Image();

                // Create a BitmapSource  
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.EndInit();

                // Set Image.Source  
                dynamicImage.Source = bitmap;
                Canvas.Children.Add(dynamicImage);
            }
        }


        private void OnSaveAs(object sender)
        {
            if (Canvas == null)
            {
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image files (*.png)|*.png";

            if (saveFileDialog1.ShowDialog() == true)
            {
                SaveAsImage(saveFileDialog1.FileName, Canvas);
            }
        }

        private void SaveAsImage(string sPath, InkCanvas canvas)
        {
            double
               x1 = canvas.Margin.Left,
               x2 = canvas.Margin.Top,
               x3 = canvas.Margin.Right,
               x4 = canvas.Margin.Bottom;

            if (sPath == null) return;

            canvas.Margin = new Thickness(0, 0, 0, 0);

            Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            RenderTargetBitmap renderBitmap =
             new RenderTargetBitmap(
               (int)size.Width,
               (int)size.Height,
               96,
               96,
               PixelFormats.Default);
            renderBitmap.Render(canvas);
            using (FileStream fs = File.Open(sPath, FileMode.OpenOrCreate))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(fs);
            }
            canvas.Margin = new Thickness(x1, x2, x3, x4);
        }


        #region Undo/Redo
        public Stack<DoStroke> DoStrokes { get; set; }
        public Stack<DoStroke> UndoStrokes { get; set; }

        /// <summary>
        /// Stroke Structure
        /// </summary>
        public struct DoStroke
        {
            public string ActionFlag { get; set; }
            public Stroke Stroke { get; set; }
        }

        private bool handle = true;

        private ICommand _undoCommand;
        public ICommand UndoCommand
        {
            get => _undoCommand;
            set => _undoCommand = value;
        }

        private ICommand _redoCommand;
        public ICommand RedoCommand
        {
            get => _redoCommand;
            set => _redoCommand = value;
        }

        private void Strokes_StrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
        {
            if (handle)
            {
                if (e.Added.Count > 0)
                {
                    DoStrokes.Push(new DoStroke
                    {
                        ActionFlag = "ADD",
                        Stroke = e.Added[0]
                    });
                }
                else if (e.Removed.Count > 0)
                {
                    DoStrokes.Push(new DoStroke
                    {
                        ActionFlag = "REMOVE",
                        Stroke = e.Removed[0]
                    });
                }
            }
        }

        private bool CanUndo()
        {
            return DoStrokes.Count > 0;
        }

        public void Undo(object sender)
        {
            handle = false;

            if (DoStrokes.Count > 0)
            {
                DoStroke @do = DoStrokes.Pop();
                if (@do.ActionFlag.Equals("ADD"))
                {
                    Canvas.Strokes.Remove(@do.Stroke);
                }
                else
                {
                    Canvas.Strokes.Add(@do.Stroke);
                }

                UndoStrokes.Push(@do);
            }
            handle = true;
        }

        private bool CanRedo()
        {
            return UndoStrokes.Count > 0;
        }

        public void Redo(object sender)
        {
            handle = false;
            if (UndoStrokes.Count > 0)
            {
                DoStroke @do = UndoStrokes.Pop();
                if (@do.ActionFlag.Equals("ADD"))
                {
                    Canvas.Strokes.Add(@do.Stroke);
                }
                else
                {
                    Canvas.Strokes.Remove(@do.Stroke);
                }

                DoStrokes.Push(@do);
            }
            handle = true;
        }

        #endregion

    }
}
