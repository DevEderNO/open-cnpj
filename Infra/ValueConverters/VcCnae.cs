using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Infra.ValueObjects;

namespace Infra.ValueConverters;

public class VcCnae
{
  public static ValueConverter<VoCnae, string> Converter =>
    new(
      cnae => cnae.Code,
      cnae => new VoCnae(cnae)
    );
}
