using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoActorWebApiDemo.DataAccess
{
    public interface IDataAccessService
    {
        Task InitData();
        Task<(bool IsSuccess, int RowsAffected)> ExecuteAsync(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null);
        Task<(bool IsSuccess, List<T> Result)> QueryAsync<T>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null) where T : class;
        Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2)> QueryMultipleAsync
            <T1, T2>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class;
        Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3)> QueryMultipleAsync
            <T1, T2, T3>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class;
        Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4)> QueryMultipleAsync
            <T1, T2, T3, T4>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class;
        Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4, List<T5> ListT5)> QueryMultipleAsync
            <T1, T2, T3, T4, T5>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class;
        Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4, List<T5> ListT5, List<T6> ListT6)> QueryMultipleAsync
            <T1, T2, T3, T4, T5, T6>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class;
        Task<(bool IsSuccess, List<T1> ListT1, List<T2> ListT2, List<T3> ListT3, List<T4> ListT4, List<T5> ListT5, List<T6> ListT6, List<T7> ListT7)> QueryMultipleAsync
            <T1, T2, T3, T4, T5, T6, T7>(string sql, bool isStoredProcedure = false, DynamicParameters parameters = null)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class;
        Task<(bool IsSuccess, List<IDictionary<string, object>> Result)> QueryToListAsync
            (string sql, int numberOfResultSets, bool isStoredProcedure = false, DynamicParameters parameters = null);
        Task<(bool IsSuccess, Dictionary<string, List<IDictionary<string, object>>> Result)> QueryMultipleToDictionaryAsync
            (string sql, int numberOfResultSets, bool isStoredProcedure = false, DynamicParameters parameters = null);
        Task<List<T>> DictionaryListToObjectListAsync<T>(List<IDictionary<string, object>> dictionaryList) where T : class, new();

    }
}
