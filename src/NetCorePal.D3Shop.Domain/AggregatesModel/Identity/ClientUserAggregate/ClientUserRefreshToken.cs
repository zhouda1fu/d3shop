﻿using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

public partial record ClientUserRefreshTokenId : IInt64StronglyTypedId;

public class ClientUserRefreshToken : Entity<ClientUserRefreshTokenId>
{
    protected ClientUserRefreshToken()
    {
    }

    public ClientUserRefreshToken(string token)
    {
        Token = token;
        CreatedTime = DateTimeOffset.Now;
        ExpiresTime = CreatedTime.AddDays(1);
    }

    public ClientUserId UserId { get; private set; } = null!;
    public string Token { get; private set; } = string.Empty;
    public DateTimeOffset CreatedTime { get; init; }
    public DateTimeOffset ExpiresTime { get; init; }
    public bool IsUsed { get; private set; }
    public bool IsRevoked { get; private set; }

    public void Use()
    {
        if (IsUsed ||
            IsRevoked ||
            ExpiresTime < DateTimeOffset.Now)
            throw new KnownException("无效的刷新令牌");

        IsUsed = true;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}