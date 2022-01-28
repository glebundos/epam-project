﻿using System.Xml.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Writing <see cref="FileCabinetRecord"/> to the .xml file.
    /// </summary>
    public class FileCabinetRecordXmlWriter : IDisposable
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Contains <see cref="TextWriter"/> to use.</param>
        public FileCabinetRecordXmlWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        ~FileCabinetRecordXmlWriter() => this.Dispose(false);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Write a collection of <see cref="FileCabinetRecord"/> sequences to XML file and save it.
        /// </summary>
        /// <param name="records">Collection of actual <see cref="FileCabinetRecord"/> data.</param>
        /// <param name="append">Append text if true, rewrite if false.</param>
        public void Write(IReadOnlyCollection<FileCabinetRecord> records, bool append)
        {
            if (!append)
            {
                this.writer.WriteLine("Id, FirstName, LastName, DateOfBirth, Height, Weigth, Temperament.");
            }

            var document = new XElement("Records", records.Select(record =>
                new XElement(
                    "Record",
                    new XAttribute("Id", record.Id),
                    new XElement(
                        "Name",
                        new XAttribute("First", record.FirstName),
                        new XAttribute("Last", record.LastName)),
                    new XElement("DateOfBirth", record.DateOfBirth.ToString("MM/dd/yyyy", new System.Globalization.CultureInfo("en-US"))),
                    new XElement("Height", record.Height),
                    new XElement("Gender", record.Weigth),
                    new XElement("Money", record.Temperament))));
            this.writer.WriteLine(document);
            this.writer.Flush();
            this.writer.Dispose();
        }

        private void Dispose(bool disposing)
        {
            this.writer?.Dispose();
        }
    }
}
