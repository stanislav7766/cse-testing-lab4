using System;
using Xunit;
using IIG.CoSFE.DatabaseUtils;
using IIG.BinaryFlag;
using IIG.DatabaseConnectionUtils;

namespace Test.BinaryFlag
{
    public class UnitTest1
    {
        private const string Server = @"DESKTOP-OU6O2N4";
        private const string Database = @"IIG.CoSWE.FlagpoleDB";
        private const bool IsTrusted = true;
        private const string Login = @"sa";
        private const string Password = @"testinglab4";
        private const int ConnectionTime = 75;
        readonly FlagpoleDatabaseUtils flagDatabaseUtils = new FlagpoleDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);
        private bool ClearDB()
        {
            return flagDatabaseUtils.ExecSql("DELETE FROM dbo.MultipleBinaryFlags");
        }
        [Fact]
        public void TestGetFlagFalse()
        {
            //По скольку addFlag - проблемный, добавили флаг в бд ручками с id:4, view: "dd", value: false
            int flagId = 4;
            Assert.True(flagDatabaseUtils.GetFlag(flagId, out string flagView, out bool? flagVal));
            Assert.Equal("dd", flagView);
            Assert.False(flagVal);
        }
        [Fact]
        public void TestGetFlagTrue()
        {
            //По скольку addFlag - проблемный, добавили флаг в бд ручками с id:5, view: "aa", value: true
            int flagId = 5;
            Assert.True(flagDatabaseUtils.GetFlag(flagId, out string flagView, out bool? flagVal));
            Assert.Equal("aa", flagView);
            Assert.True(flagVal);
        }
        [Fact]
        public void TestAddFlagTrue()
        {
            MultipleBinaryFlag mbf = new MultipleBinaryFlag(2);
            bool actual = mbf.GetFlag();
            Assert.True(flagDatabaseUtils.AddFlag("ss", actual));
            ClearDB();
        }
        [Fact]
        public void TestAddFlagFalse()
        {
            MultipleBinaryFlag mbf = new MultipleBinaryFlag(2, false);
            bool actual = mbf.GetFlag();
            Assert.True(flagDatabaseUtils.AddFlag("ss", actual));
            ClearDB();
        }
    }
}
