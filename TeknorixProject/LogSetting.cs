using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace TeknorixProject
{
    public class LogSetting
    {
        private readonly string _connectionString;
        Serilog.ILogger _logger;

        public LogSetting()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.Level);
            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn { ColumnName = "FunctionName", DataType = SqlDbType.NVarChar, DataLength = 255 }
            };

            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(
                    connectionString: _connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Log",
                        AutoCreateSqlTable = true
                    },
                    columnOptions: columnOptions
                )
                .CreateLogger();

        }
        public string GetConnectionString(string name)
        {
            return _connectionString;
        }
        public void LogError(string error, string FunctionName)
        {
            var Loge = _logger.ForContext("FunctionName", FunctionName);
            Loge.Error(error);
          
        }

        public void LogInformation(string info, string FunctionName)
        {
            var Logi = _logger.ForContext("FunctionName", FunctionName);
            Logi.Information(info);
        }

    }


}
