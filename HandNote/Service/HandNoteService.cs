using HandNote.Models;
using System;
using System.IO;
using System.Xml.Serialization;

namespace HandNote.Service
{
    public class HandNoteService
    {
        /// <summary>
        /// Method to read and deserialize the data
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>canvas model</returns>
        public NoteBook Read(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(NoteBook));
                StreamReader reader = new StreamReader(fs);
                NoteBook noteBook = (NoteBook)serializer.Deserialize(reader);
                fs.Close();
                return noteBook;
            }
            catch
            {
                return null;
            }
           
        }

        /// <summary>
        /// Method to write the serialized data in the given location
        /// </summary>
        /// <param name="fileName">file Name</param>
        /// <param name="canvasModel">canvas model</param>
        public void Write(string fileName, NoteBook noteBook)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(noteBook.GetType());
            StreamWriter writer = new StreamWriter(fs);
            serializer.Serialize(writer, noteBook);
            fs.Close();
        }
    }
}
