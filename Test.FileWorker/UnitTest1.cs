using System;
using System.IO;
using Xunit;
using IIG.FileWorker;
using IIG.CoSFE.DatabaseUtils;
using System.Text;

namespace Test.FileWorker
{
    public class UnitTest1
    {
        private const string Server = @"DESKTOP-OU6O2N4";
        private const string Database = @"IIG.CoSWE.StorageDB";
        private const bool IsTrusted = true;
        private const string Login = @"sa";
        private const string Password = @"testinglab4";
        private const int ConnectionTime = 75;
        readonly StorageDatabaseUtils storageDatabaseUtils = new StorageDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);
        static readonly string workingDirectory = Environment.CurrentDirectory;
        private readonly string testsDirFullPath = Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\tests-dir";

        private bool ClearDB()
        {
            return storageDatabaseUtils.ExecSql("DELETE FROM dbo.Files");
        }
        [Fact]
        public void TestAddFile()
        {
            string fileFullPath = testsDirFullPath + "\\file.txt";
            BaseFileWorker.Write("some data provided", fileFullPath);
            string data = BaseFileWorker.ReadAll(fileFullPath);
            Assert.True(storageDatabaseUtils.AddFile("13", Encoding.UTF8.GetBytes(data)));
        }
        [Fact]
        public void TestAddFileEmptyContent()
        {
            string fileFullPath = testsDirFullPath + "\\file-empty.txt";
            BaseFileWorker.Write("", fileFullPath);
            string data = BaseFileWorker.ReadAll(fileFullPath);
            Assert.True(storageDatabaseUtils.AddFile("14", Encoding.UTF8.GetBytes(data)));
            
        }
        [Fact]
        public void TestDeleteFile()
        {
            int fileId = 43;//по скольку в апи нет механизма получения ид файла, берем ид с базы
            Assert.True(storageDatabaseUtils.DeleteFile(fileId));
        }
        [Fact]
        public void TestDeleteFileRandomId()
        {
            int fileId = 3900;
            Assert.False(storageDatabaseUtils.DeleteFile(fileId));
        }
        [Fact]
        public void TestGetFile()
        {
            int fileId = 46;//по скольку в апи нет механизма получения ид файла, берем ид с базы
            Assert.True(storageDatabaseUtils.GetFile(fileId, out string fileName, out byte[] fileContent));
            Assert.Equal("1212", fileName);
            Assert.Equal("", Encoding.UTF8.GetString(fileContent));

        }
        [Fact]
        public void TestGetFileRandomId()
        {
            int fileId = 4111;
            Assert.False(storageDatabaseUtils.GetFile(fileId, out _, out byte[] _));
        }
        [Fact]
        public void TestGetFiles()
        {
            string fileName = "11";
            System.Data.DataTable table = storageDatabaseUtils.GetFiles(fileName);
            Assert.Equal(3, table.Rows.Count);
        }
        [Fact]
        public void TestGetFilesRandomName()
        {
            string fileName = "113234435";
            System.Data.DataTable table = storageDatabaseUtils.GetFiles(fileName);
            Assert.Equal(0, table.Rows.Count);
        }
    }
}
