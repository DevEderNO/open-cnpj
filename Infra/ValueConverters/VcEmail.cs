using System;
using Infra.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infra.ValueConverters;

public static class VcEmail
{
  public static ValueConverter<VoEmail, string> Converter =>
    new(
      email => email.Address,
      address => new VoEmail(address)
    );
}
