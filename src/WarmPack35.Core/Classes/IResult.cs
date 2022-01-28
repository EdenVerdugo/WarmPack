using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarmPack.Classes
{
    public interface IResult
    {
        bool Value { get; set; }
        string Message { get; set; }
        int Code { get; set; }
    }

    public interface IResultList<T> : IResult
    {
        List<T> Data { get; set; }        
    }    
}
