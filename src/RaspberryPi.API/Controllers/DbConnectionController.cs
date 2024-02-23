using Dapper;
using Fetchgoods.Text.Json.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaspberryPi.Infrastructure.Data.Dapper.Connection;
using System.ComponentModel.DataAnnotations;

namespace RaspberryPi.API.Controllers
{
    /// <summary>
    /// DbConnection related methods
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DbConnectionController : ControllerBase
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DbConnectionController(IDbConnectionFactory dbConnectionFactory)
        {
            _connectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// Executes a sql query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [HttpPost("execute")]
        [Authorize(Roles = "root")]
        public int Execute([FromHeader][Required] string sql)
        {
            using var connection = _connectionFactory.CreateConnection();
            return connection.Execute(sql);
        }

        /// <summary>
        /// Executes a sql query and returns data
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [HttpPost("executeReader")]
        [Authorize(Roles = "root")]
        public string? ExecuteReader([FromHeader] string sql)
        {
            using var connection = _connectionFactory.CreateConnection();
            var dataReader = connection.ExecuteReader(sql);

            if (dataReader is null) return null;

            var jsonArray = new List<Dictionary<string, object>>();
            while (dataReader.Read())
            {
                var rowData = new Dictionary<string, object>();

                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    string fieldName = dataReader.GetName(i);
                    object fieldValue = dataReader[i];
                    rowData[fieldName] = fieldValue;
                }

                jsonArray.Add(rowData);
            }

            return jsonArray.ToJson();
        }
    }
}