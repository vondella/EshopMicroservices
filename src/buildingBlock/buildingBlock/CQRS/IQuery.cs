﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buildingBlock.CQRS
{
    public  interface IQuery<out TResponse>:IRequest<TResponse>  where TResponse: notnull
    {
    }
}