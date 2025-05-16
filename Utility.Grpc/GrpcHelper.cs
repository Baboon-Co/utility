﻿using FluentResults;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Utility.Grpc.ResultErrors;
using Status = Google.Rpc.Status;
using FieldViolation = Google.Rpc.BadRequest.Types.FieldViolation;

namespace Utility.Grpc;

public static class GrpcHelper
{
    public static RpcException CreateRpcException<T>(Result<T> result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Result is not failed.");

        var grpcResultError = result.Errors.OfType<GrpcResultError>().FirstOrDefault();
        if (grpcResultError is null)
            throw new InvalidOperationException("Result MUST have one GrpcResultError!");
        
        var errorDetail = new Status
        {
            Code = (int) grpcResultError.ErrorType,
            Message = grpcResultError.Message,
            Details =
            {
                Any.Pack(new BadRequest
                {
                    FieldViolations = {ResultToFieldViolations(result)}
                })
            }
        };

        return errorDetail.ToRpcException();
    }

    private static RepeatedField<FieldViolation> ResultToFieldViolations<T>(Result<T> result)
    {
        return new RepeatedField<FieldViolation>
        {
            result.Errors
                .OfType<FieldError>()
                .Select(err => new FieldViolation
                {
                    Field = err.Field,
                    Description = err.Message
                })
        };
    }
}