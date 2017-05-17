using System.IO;
using NUnit.Framework;
using reviews_service.infrastructure;

namespace reviews_service.test.acceptance
{
    public class Database
    {
        private const string databaseFile = DatabaseWriter.DatabaseFile;

        public void Reset()
        {
            if (!DbExists) return;
            File.Delete(databaseFile);
        }

        public void AssertWasSaved(ReviewDto reviewDto)
        {
            Assert.That(DbExists, Is.True, $"Database file not found - {databaseFile}");
            var rows = Read();
            Assert.That(rows.Length, Is.EqualTo(1), "unexpected count of rows");
            AssertRowsMatch(reviewDto, rows[0]);
        }

        private bool DbExists => File.Exists(databaseFile);

        private string[] Read()
        {
            return DbExists ? File.ReadAllLines(databaseFile) : new string[0];
        }

        private void AssertRowsMatch(ReviewDto expectedDto, string actual)
        {
            var expected = string.Join("|",
                expectedDto.ISBN, expectedDto.Reviewer, expectedDto.Text, expectedDto.Uri);

            Assert.That(actual, Is.EqualTo(expected), "Invalid record data");
        }

        public void AssertWasNotSaved()
        {
            Assert.That(DbExists, Is.False, "Unexpected write to database");
        }
    }
}