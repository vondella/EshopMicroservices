﻿namespace Play.Identity.Contract
{
    public record DebitGil(Guid UserId, decimal Gil, Guid CorrelationId);
    public record GilDebited(Guid CorrelationId);
    public record UserUpdated(
        Guid UserId,
        string Email,
        decimal NewTotalGil
    );
}
