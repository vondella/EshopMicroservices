﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record OrderItemDto(Guid OrderId,Guid ProductId,int Quantity, decimal Price);
