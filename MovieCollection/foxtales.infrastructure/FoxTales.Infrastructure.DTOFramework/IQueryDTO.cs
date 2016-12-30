using System.Text;

namespace FoxTales.Infrastructure.DTOFramework
{
    public interface IQueryDTO
    {
        StringBuilder GenerateSelectStatement();
    }
}