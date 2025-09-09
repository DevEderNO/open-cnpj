using System;
using Infra.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infra.ValueConverters;

public class VcCnpj
{
  public static ValueConverter<VoCnpj, string> Converter =>
    new(
      cnpj => cnpj.Numero,
      numero => new VoCnpj(numero)
    );
}
