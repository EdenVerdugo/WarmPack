using System;
using System.Collections.Generic;

namespace WarmPack.Classes
{
    public class ResultList<T> : IResultList<T>
    {        
        public bool Value { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        public List<T> Data { get; set; }

        public ResultList()
        {

        }

        public ResultList(Result<List<T>> result)
        {
            Value = result.Value;
            Message = result.Message;
            Data = result.Data;
        }

        public ResultList(bool value, string msg)
        {
            Value = value;
            Message = msg;
        }

        public ResultList(bool value, string msg, List<T> data)
        {
            Value = value;
            Message = msg;
            Data = data;
        }

        public ResultList(Exception ex)
        {
            this.Value = false;
            this.Message = ex.Message;
        }

        public static ResultList<T> Create(bool value, string msg)
        {
            return new ResultList<T>(value, msg);
        }

        public static ResultList<T> Create(bool value, string msg, List<T> data)
        {
            return new ResultList<T>(value, msg, data);
        }

        public static ResultList<T> Create(Exception ex)
        {
            return new ResultList<T>(ex);
        }

        public static ResultList<T> Create(Result<List<T>> result)
        {
            return new ResultList<T>(result);
        }
    }
}
