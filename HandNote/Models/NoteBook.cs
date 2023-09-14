using System;
using System.Collections.Generic;

namespace HandNote.Models
{
    [Serializable]
    public sealed class NoteBook
    {
        public NoteBook() {
            Items = new List<NoteBookItem>();
        }

        public List<NoteBookItem> Items { get; set; }

        public string SelectedNoteBookId { get; set; }
    }
}
