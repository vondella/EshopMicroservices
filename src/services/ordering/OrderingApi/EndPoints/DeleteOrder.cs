﻿
		{
			var result = await sender.Send(new DeleteOrderCommand(id));
			var response = result.Adapt<DeleteOrderResponse>();
			return Results.Ok(response);
		}).WithName("delete Order")
         .Produces<DeleteOrderResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("delete Order")
         .WithDescription("delete Order");
    }