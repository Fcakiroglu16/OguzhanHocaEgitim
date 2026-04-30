using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Applications
{
}


public class ServiceResult
{
        public HttpStatusCode StatusCode { get; set; }
        public ProblemDetails? ProblemDetails { get; set; }


        public bool IsSuccess => ProblemDetails is null;

        public bool IsFail => !IsSuccess;


        // static factory methods
        public static ServiceResult Success(HttpStatusCode statusCode)
        {
            return new ServiceResult()
            {
                StatusCode = statusCode
            };
        }

        public static ServiceResult Failure(HttpStatusCode statusCode, string title)
        {
            return new ServiceResult()
            {
                StatusCode = statusCode,
                ProblemDetails = new ProblemDetails()
                {
                    Status = (int)statusCode,
                    Title = title
                }
            };
        }

        public static ServiceResult Failure(HttpStatusCode statusCode, string title, string detail)
        {
            return new ServiceResult()
            {
                StatusCode = statusCode,
                ProblemDetails = new ProblemDetails()
                {
                    Status = (int)statusCode,
                    Title = title,
                    Detail = detail
                }
            };
        }
    }


    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ServiceResult<T>()
            {
                Data = data,
                StatusCode = statusCode
            };
        }


        public new static ServiceResult<T> Failure(HttpStatusCode statusCode, string title)
        {
            return new ServiceResult<T>()
            {
                StatusCode = statusCode,
                ProblemDetails = new ProblemDetails()
                {
                    Status = (int)statusCode,
                    Title = title
                }
            };
        }

        public new static ServiceResult<T> Failure(HttpStatusCode statusCode, string title, string detail)
        {
            return new ServiceResult<T>()
            {
                StatusCode = statusCode,
                ProblemDetails = new ProblemDetails()
                {
                    Status = (int)statusCode,
                    Title = title,
                    Detail = detail
                }
            };
        }
    }


    public static class ServiceResultExtensions
    {
        public static IResult ToActionResult<T>(this ServiceResult<T> serviceResult)
        {
            return serviceResult.StatusCode switch
            {
                HttpStatusCode.OK => Results.Ok(serviceResult.Data),
                HttpStatusCode.Created => Results.Created(string.Empty, serviceResult.Data),
                HttpStatusCode.NoContent => Results.NoContent(),
                _ => Results.Problem(serviceResult.ProblemDetails!)
            };
        }


        public static IResult ToActionResult(this ServiceResult serviceResult)
        {
            return serviceResult.StatusCode switch
            {
                HttpStatusCode.NoContent => Results.NoContent(),
                HttpStatusCode.Created => Results.Created(),
                _ => Results.Problem(serviceResult.ProblemDetails!)
            };
        }
    }
