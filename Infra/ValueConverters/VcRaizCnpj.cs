using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Infra.ValueObjects;

namespace Infra.ValueConverters;

public class VcRaizCnpj
{
  public static ValueConverter<VoRaizCnpj, string> Converter =>
    new(
      raizCnpj => raizCnpj.RaizCnpj,
      raizCnpj => new VoRaizCnpj(raizCnpj)
    );
}
