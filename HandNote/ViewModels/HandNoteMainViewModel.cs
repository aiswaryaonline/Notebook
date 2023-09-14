using HandNote.Extensions;
using HandNote.Models;
using HandNote.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;

namespace HandNote.ViewModels
{
    /// <summary>
    /// The view model class which handles list of HandNotes and handnote functions.
    /// </summary>
    public class HandNoteMainViewModel : ViewModelBase
    {
        #region Private Members
        private static readonly string FileName = "handNote.xml";
        private int _uniqueNumber = 1;
        private ObservableCollection<HandNoteItemViewModel> _handNoteItems = new ObservableCollection<HandNoteItemViewModel>();
        private HandNoteItemViewModel _selectedHandNote;
        private ICommand _addNoteItemCommand;
        private ICommand _exitCommand;
        private string _header;
        #endregion

        public HandNoteMainViewModel() {
            AddNoteItemCommand = new Command(new Action<object>(OnAddNewHandNoteItem));
            ExitCommand = new Command(new Action<object>(OnExit));
            Header = "HandNote";
            SelectedHandNote = null; 
        }

        #region Properties
        /// <summary>
        /// Gets or sets collction of HandNote Items.
        /// </summary>
        public ObservableCollection<HandNoteItemViewModel> HandNoteItems
        {
            get => _handNoteItems;
            set
            {
                if (_handNoteItems != value)
                {
                    _handNoteItems = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets selected hand note.
        /// </summary>
        public HandNoteItemViewModel SelectedHandNote
        {
            get => _selectedHandNote;
            set
            {
               if(SetProperty(ref _selectedHandNote, value))
                {
                    Header = $"HandNote - {_selectedHandNote.DisplayName}";
                }
            }
        }

        public string Header
        {
            get => _header;
            set
            {
                SetProperty(ref _header, value);
            }
        }

        /// <summary>
        /// Command to add new note item.
        /// </summary>
        public ICommand AddNoteItemCommand
        {
            get => _addNoteItemCommand;
            set => _addNoteItemCommand = value;
        }

        /// <summary>
        /// Command to exit the application.
        /// </summary>
        public ICommand ExitCommand
        {
            get => _exitCommand;
            set => _exitCommand = value;
        }

        #endregion

        private void OnAddNewHandNoteItem(object commandParameter)
        {
            string serialNumber = $"Note {_uniqueNumber}";

            while (HandNoteItems.Any(x => x.Id == serialNumber))
            {
                _uniqueNumber++;
            }
            serialNumber = $"Note {_uniqueNumber}";

            HandNoteItemViewModel item = new HandNoteItemViewModel() { Id = serialNumber, DisplayName = serialNumber };
            HandNoteItems.Add(item);
            _uniqueNumber++;

            SelectedHandNote = item;
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        /// <param name="sender"></param>
        private void OnExit(object sender)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Method to save the canvas strokes
        /// </summary>
        public void SaveSettings()
        {
            NoteBook noteBook = new NoteBook();

            foreach(HandNoteItemViewModel handNoteItemVm in HandNoteItems)
            {
                NoteBookItem noteBookItem = new NoteBookItem()
                {
                    Id = handNoteItemVm.Id,
                    DisplayName = handNoteItemVm.DisplayName,
                    EditingMode = handNoteItemVm.EditingMode,
                    InkThickness = handNoteItemVm.InkThickness,
                    InkThicknessForHighlighter = handNoteItemVm.InkThicknessForHighlighter,
                    IsHighLighter = handNoteItemVm.IsHighLighter,
                    PenColor = handNoteItemVm.PenColor,
                    PenColorForHighlighter = handNoteItemVm.PenColorForHighlighter
                };

                // Call the extension method to convert the strokes in to point array
                noteBookItem.Points = handNoteItemVm.Canvas.Strokes.GeneratePointArray();
                noteBookItem.DrawingModes = handNoteItemVm.Canvas.Strokes.GetDrawingModes();

                noteBook.Items.Add(noteBookItem);
            }

            noteBook.SelectedNoteBookId = SelectedHandNote?.Id;

            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\" + FileName;
            HandNoteService handNoteService = new HandNoteService();

            // call the service method to serialize and save the the contents
            handNoteService.Write(fileName, noteBook);
        }

        /// <summary>
        /// Method to load xml settings.
        /// </summary>
        public void LoadSettings()
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\" + FileName;
            // Call service method to deserialize the file contents into Model
            HandNoteService handNoteService = new HandNoteService();
            NoteBook noteBook = handNoteService.Read(fileName);

            if(noteBook == null)
            {
                return;
            }

            HandNoteItems = new ObservableCollection<HandNoteItemViewModel>();

            foreach(NoteBookItem noteBookItem in noteBook.Items) 
            {
                HandNoteItemViewModel noteItemViewModel = new HandNoteItemViewModel()
                {
                    Id = noteBookItem.Id,
                    DisplayName = noteBookItem.DisplayName,
                    EditingMode = noteBookItem.EditingMode,
                    InkThickness = noteBookItem.InkThickness,
                    InkThicknessForHighlighter = noteBookItem.InkThicknessForHighlighter,
                    IsHighLighter = noteBookItem.IsHighLighter,
                    PenColor = noteBookItem.PenColor,
                    PenColorForHighlighter = noteBookItem.PenColorForHighlighter
                };

                for (int i = 0; i < noteBookItem.Points.Length; i++)
                {
                    if (noteBookItem.Points[i] != null)
                    {
                        // Call the extension method to convet the points array to storke
                        var strokes = noteBookItem.Points[i].GenerateStroke(noteBookItem.DrawingModes[i]);

                        // add the stroke to the collection
                        noteItemViewModel.Canvas.Strokes.Add(strokes);
                    }
                }

                HandNoteItems.Add(noteItemViewModel);

            }

            SelectedHandNote = HandNoteItems.FirstOrDefault(x => x.Id == noteBook.SelectedNoteBookId);
            _uniqueNumber = HandNoteItems.Count() + 1;
        }
    }
}
