using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DapperExamples.Service
{
    /// <summary>
    /// This class provides all the methods for accessing the database
    /// </summary>
    public class DataAccessService
    {
        /// <summary>
        /// Connection string builder
        /// </summary>
        private static readonly SqliteConnectionStringBuilder _connectionStringBuilder = new SqliteConnectionStringBuilder()
        {
            DataSource = "./SqliteDB.db",
        };

        /// <summary>
        /// Executes a sql statement/ stored procedure without output asynchronously
        /// </summary>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task ExecuteAsync(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
        {
            using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        if (isStoredProcedure)
                        {
                            Console.WriteLine("Executing stored procedure: [" + sql + "]");
                            await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                        }

                        else
                        {
                            Console.WriteLine("Executing query: [" + sql + "]");
                            await connection.ExecuteAsync(sql, parameters);
                        }
                        if (parameters != null)
                        {
                            foreach (var param in parameters.ParameterNames.AsList())
                            {

                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine("Error! Transaction rolled back, stacktrace: " + ex.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        /// Executes a sql statement/ stored procedure with output (single result set) asynchronously
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, List of T where T : class
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<T> Result)> QueryAsync<T>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null) where T : class
        {
            try
            {
                List<T> list = new List<T>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();

                    IEnumerable<T> result;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        result = await connection.QueryAsync<T>(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        result = await connection.QueryAsync<T>(sql, parameters);
                    }
                    if (parameters != null)
                    {
                        foreach (var param in parameters.ParameterNames.AsList())
                        {
                            //TODO: find a way to log both parameter names and value
                        }
                    }
                    list = result.AsList();
                }
                return (true, list);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (false, null);
            }
        }

        /// <summary>
        /// Executes a sql statement/ stored procedure with output (multiple result set) asynchronously
        /// Suports two input types
        /// *Note: though Dapper calls it "Query", it supports all CRUD operations with a return type
        /// 
        /// Type parameters:
        ///   T1:
        ///     The first type in the recordset.
        ///
        ///   T2:
        ///     The second type in the recordset.
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, List of T1, List of T2
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2)> QueryMultipleAsync
            <T1, T2>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
        {
            try
            {
                List<T1> listT1 = new List<T1>();
                List<T2> listT2 = new List<T2>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();
                    GridReader resultSet;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters);
                    }
                    IEnumerable<T1> t1 = await resultSet.ReadAsync<T1>();
                    IEnumerable<T2> t2 = await resultSet.ReadAsync<T2>();
                    listT1 = t1.AsList();
                    listT2 = t2.AsList();
                }
                return (true, listT1, listT2);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (false, null, null);
            }
        }

        /// <summary>
        /// Executes a sql statement/ stored procedure with output (multiple result set) asynchronously
        /// Suports two input types
        /// *Note: though Dapper calls it "Query", it supports all CRUD operations with a return type
        /// 
        /// Type parameters:
        ///   T1:
        ///     The first type in the recordset.
        ///   T2:
        ///     The second type in the recordset.
        ///   T3:
        ///     The third type in the recordset.
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, List of T1, List of T2, List of T3
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3)> QueryMultipleAsync
            <T1, T2, T3>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            try
            {
                List<T1> listT1 = new List<T1>();
                List<T2> listT2 = new List<T2>();
                List<T3> listT3 = new List<T3>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();
                    GridReader resultSet;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters);
                    }
                    IEnumerable<T1> t1 = await resultSet.ReadAsync<T1>();
                    IEnumerable<T2> t2 = await resultSet.ReadAsync<T2>();
                    IEnumerable<T3> t3 = await resultSet.ReadAsync<T3>();
                    listT1 = t1.AsList();
                    listT2 = t2.AsList();
                    listT3 = t3.AsList();
                }
                return (true, listT1, listT2, listT3);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (false, null, null, null);
            }
        }

        /// <summary>
        /// Executes a sql statement/ stored procedure with output (multiple result set) asynchronously
        /// Suports two input types
        /// *Note: though Dapper calls it "Query", it supports all CRUD operations with a return type
        /// 
        /// Type parameters:
        ///   T1:
        ///     The first type in the recordset.
        ///   T2:
        ///     The second type in the recordset.
        ///   T3:
        ///     The third type in the recordset. 
        ///   T4:
        ///     The fourth type in the recordset.
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, List of T1, List of T2, List of T3, List of T4
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4)> QueryMultipleAsync
            <T1, T2, T3, T4>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            try
            {
                List<T1> listT1 = new List<T1>();
                List<T2> listT2 = new List<T2>();
                List<T3> listT3 = new List<T3>();
                List<T4> listT4 = new List<T4>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();
                    GridReader resultSet;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters);
                    }
                    IEnumerable<T1> t1 = await resultSet.ReadAsync<T1>();
                    IEnumerable<T2> t2 = await resultSet.ReadAsync<T2>();
                    IEnumerable<T3> t3 = await resultSet.ReadAsync<T3>();
                    IEnumerable<T4> t4 = await resultSet.ReadAsync<T4>();
                    listT1 = t1.AsList();
                    listT2 = t2.AsList();
                    listT3 = t3.AsList();
                    listT4 = t4.AsList();
                }
                return (true, listT1, listT2, listT3, listT4);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (false, null, null, null, null);
            }
        }

        /// <summary>
        /// Executes a sql statement/ stored procedure with output (multiple result set) asynchronously
        /// Suports two input types
        /// *Note: though Dapper calls it "Query", it supports all CRUD operations with a return type
        /// 
        /// Type parameters:
        ///   T1:
        ///     The first type in the recordset.
        ///   T2:
        ///     The second type in the recordset.
        ///   T3:
        ///     The third type in the recordset. 
        ///   T4:
        ///     The fourth type in the recordset.
        ///   T5:
        ///     The fifth type in the recordset.
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, List of T1, List of T2, List of T3, List of T4, List of T5
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4, List<T5> ListT5)> QueryMultipleAsync
            <T1, T2, T3, T4, T5>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            try
            {
                List<T1> listT1 = new List<T1>();
                List<T2> listT2 = new List<T2>();
                List<T3> listT3 = new List<T3>();
                List<T4> listT4 = new List<T4>();
                List<T5> listT5 = new List<T5>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();
                    GridReader resultSet;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters);
                    }
                    IEnumerable<T1> t1 = await resultSet.ReadAsync<T1>();
                    IEnumerable<T2> t2 = await resultSet.ReadAsync<T2>();
                    IEnumerable<T3> t3 = await resultSet.ReadAsync<T3>();
                    IEnumerable<T4> t4 = await resultSet.ReadAsync<T4>();
                    IEnumerable<T5> t5 = await resultSet.ReadAsync<T5>();
                    listT1 = t1.AsList();
                    listT2 = t2.AsList();
                    listT3 = t3.AsList();
                    listT4 = t4.AsList();
                    listT5 = t5.AsList();
                }
                return (true, listT1, listT2, listT3, listT4, listT5);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (false, null, null, null, null, null);
            }
        }

        /// <summary>
        /// Executes a sql statement/ stored procedure with output (multiple result set) asynchronously
        /// Suports two input types
        /// *Note: though Dapper calls it "Query", it supports all CRUD operations with a return type
        /// 
        /// Type parameters:
        ///   T1:
        ///     The first type in the recordset.
        ///   T2:
        ///     The second type in the recordset.
        ///   T3:
        ///     The third type in the recordset. 
        ///   T4:
        ///     The fourth type in the recordset.
        ///   T5:
        ///     The fifth type in the recordset.
        ///   T6:
        ///     The sixth type in the recordset.
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, List of T1, List of T2, List of T3, List of T4, List of T5, List of T6
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4, List<T5> ListT5, List<T6> ListT6)> QueryMultipleAsync
            <T1, T2, T3, T4, T5, T6>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            try
            {
                List<T1> listT1 = new List<T1>();
                List<T2> listT2 = new List<T2>();
                List<T3> listT3 = new List<T3>();
                List<T4> listT4 = new List<T4>();
                List<T5> listT5 = new List<T5>();
                List<T6> listT6 = new List<T6>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();
                    GridReader resultSet;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters);
                    }
                    IEnumerable<T1> t1 = await resultSet.ReadAsync<T1>();
                    IEnumerable<T2> t2 = await resultSet.ReadAsync<T2>();
                    IEnumerable<T3> t3 = await resultSet.ReadAsync<T3>();
                    IEnumerable<T4> t4 = await resultSet.ReadAsync<T4>();
                    IEnumerable<T5> t5 = await resultSet.ReadAsync<T5>();
                    IEnumerable<T6> t6 = await resultSet.ReadAsync<T6>();
                    listT1 = t1.AsList();
                    listT2 = t2.AsList();
                    listT3 = t3.AsList();
                    listT4 = t4.AsList();
                    listT5 = t5.AsList();
                    listT6 = t6.AsList();
                }
                return (true, listT1, listT2, listT3, listT4, listT5, listT6);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (false, null, null, null, null, null, null);
            }
        }

        /// <summary>
        /// Executes a sql statement/ stored procedure with output (multiple result set) asynchronously
        /// Suports two input types
        /// *Note: though Dapper calls it "Query", it supports all CRUD operations with a return type
        /// 
        /// Type parameters:
        ///   T1:
        ///     The first type in the recordset.
        ///   T2:
        ///     The second type in the recordset.
        ///   T3:
        ///     The third type in the recordset. 
        ///   T4:
        ///     The fourth type in the recordset.
        ///   T5:
        ///     The fifth type in the recordset.
        ///   T6:
        ///     The sixth type in the recordset.
        ///   T7:
        ///     The seventh type in the recordset.
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, List of T1, List of T2, List of T3, List of T4, List of T5, List of T6, List of T7
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4, List<T5> ListT5, List<T6> ListT6, List<T7> ListT7)> QueryMultipleAsync
            <T1, T2, T3, T4, T5, T6, T7>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
        {
            try
            {
                List<T1> listT1 = new List<T1>();
                List<T2> listT2 = new List<T2>();
                List<T3> listT3 = new List<T3>();
                List<T4> listT4 = new List<T4>();
                List<T5> listT5 = new List<T5>();
                List<T6> listT6 = new List<T6>();
                List<T7> listT7 = new List<T7>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();
                    GridReader resultSet;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters);
                    }
                    IEnumerable<T1> t1 = await resultSet.ReadAsync<T1>();
                    IEnumerable<T2> t2 = await resultSet.ReadAsync<T2>();
                    IEnumerable<T3> t3 = await resultSet.ReadAsync<T3>();
                    IEnumerable<T4> t4 = await resultSet.ReadAsync<T4>();
                    IEnumerable<T5> t5 = await resultSet.ReadAsync<T5>();
                    IEnumerable<T6> t6 = await resultSet.ReadAsync<T6>();
                    IEnumerable<T7> t7 = await resultSet.ReadAsync<T7>();
                    listT1 = t1.AsList();
                    listT2 = t2.AsList();
                    listT3 = t3.AsList();
                    listT4 = t4.AsList();
                    listT5 = t5.AsList();
                    listT6 = t6.AsList();
                    listT7 = t7.AsList();
                }
                return (true, listT1, listT2, listT3, listT4, listT5, listT6, listT7);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (false, null, null, null, null, null, null, null);
            }
        }

        /// <summary>
        /// To support querying of a single record set to List of IDictionary
        /// Useful for retrieval of system parameters which does not require a seperate Dto class to be created
        /// Conversion to class can be done via DictionaryListToObjectListAsync method if required
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, Lisf of IDictionary (which IDictionary is raw return type of Dapper casted to IDictionary)
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="numberOfResultSets">Integer - number of record sets that will be returned</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, List<IDictionary<string, object>> Result)> QueryToListAsync
            (string sql, int numberOfResultSets, bool isStoredProcedure = false, DynamicParameters parameters = null)
        {
            try
            {
                List<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        result = await connection.QueryAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        result = await connection.QueryAsync(sql, parameters);
                    }
                    foreach (var row in result)
                    {
                        list.Add(row as IDictionary<string, object>);
                    }
                }
                return (true, list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return (false, null);
            }
        }

        /// <summary>
        /// To support querying of Multiple ResultSet when # of resultSet is more than seven
        /// Conversion to class can be done via DictionaryListToObjectListAsync method if required
        /// </summary>
        /// <returns>
        /// Is the transaction successful?, Dictionary of key: #result-set-n, value: object (Where object = raw return type of Dapper casted to IDictionary)
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="numberOfResultSets">Integer - number of record sets that will be returned</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<(bool IsSuccess, Dictionary<string, List<IDictionary<string, object>>> Result)> QueryMultipleToDictionaryAsync
            (string sql, int numberOfResultSets, bool isStoredProcedure = false, DynamicParameters parameters = null)
        {
            try
            {
                var dictionary = new Dictionary<string, List<IDictionary<string, object>>>();
                using (var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString))
                {
                    await connection.OpenAsync();

                    GridReader resultSet;
                    if (isStoredProcedure)
                    {
                        Console.WriteLine("Executing stored procedure: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        Console.WriteLine("Executing query: [" + sql + "]");
                        resultSet = await connection.QueryMultipleAsync(sql, parameters);
                    }

                    for (int i = 1; i <= numberOfResultSets; i++)
                    {
                        string key = "#result-set-" + i;
                        var result = await resultSet.ReadAsync();
                        var tableResult = new List<IDictionary<string, object>>();
                        foreach (var row in result)
                        {
                            tableResult.Add(row as IDictionary<string, object>);
                        }
                        dictionary.Add(key, tableResult);
                    }
                }
                return (true, dictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.StackTrace);
                return (false, null);
            }
        }

        /// <summary>
        /// To convert IDictionary to class using Reflection
        /// [MapTo] attribute may be added to variable(s) in a class to match dictionary key (e.g. when column name in database is is_success but variable is names IsSuccess)
        /// See MapToAttribute.cs
        /// This method is used when IDictionary return type is preferred from dapper
        /// </summary>
        /// <returns>
        /// List of T
        /// </returns>
        /// <param name="sql">String - A sql query or stored procedure name</param>
        /// <param name="isStoredProcedure">Boolean - Whether query is a stored procedure</param>
        /// <param name="parameters">DynamicParameters - sql parameters</param>
        public async Task<List<T>> DictionaryListToObjectListAsync<T>(List<IDictionary<string, object>> dictionaryList) where T : class, new()
        {
            return await Task.Run(() =>
            {
                List<T> list = new List<T>();
                foreach (var dictionary in dictionaryList)
                {
                    T obj = new T();
                    foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                    {
                        string key;
                        if (Attribute.IsDefined(propertyInfo, typeof(MapToAttribute)))
                        {
                            var mapTo = propertyInfo.GetCustomAttribute(typeof(MapToAttribute), false) as MapToAttribute;
                            key = mapTo.Column;
                        }
                        else
                        {
                            key = propertyInfo.Name;
                        }
                        var containsKey = dictionary.TryGetValue(key, out var dictionaryObject);
                        if (containsKey)
                        {
                            propertyInfo.SetValue(obj, Convert.ChangeType(dictionaryObject, propertyInfo.PropertyType), null);
                        }
                        else
                        {
                            //graceful handling if key is not found, log it
                            Console.WriteLine("Attribute not found in model: " + key);
                        }
                    }
                    list.Add(obj);
                }
                return list;
            });
        }
    }
}
