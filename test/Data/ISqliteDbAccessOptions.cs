using Microsoft.Extensions.Options;

namespace CoreRepository.Test.Data
{
    public interface ISqliteDbAccessOptions : IOptions<DbAccessOptions>
    {
    }
}
