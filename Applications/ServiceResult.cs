using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Applications
{
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; set; }
    }


    public class ServiceResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; } = [];

        // 1. hali
        //public bool IsSuccessLegacy
        //{
        //    get
        //    {
        //        return Errors.Any();
        //    }
        //}
        // 2. hali
        //public bool IsSuccessLegacy
        //{
        //    get =>  Errors.Any();
        //}


        public bool IsSuccess => Errors.Any();

        public bool IsFail => !IsSuccess;
    }
}
