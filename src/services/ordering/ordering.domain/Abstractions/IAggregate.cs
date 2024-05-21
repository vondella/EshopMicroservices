﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.domain.Abstractions
{
    public interface IAggregate<T>: IAggregate, IEntity<T>
    {

    }

    public  interface IAggregate:IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IDomainEvent[] ClearDomainEvents();

    }
}
