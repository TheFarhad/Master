using System.Data;

namespace Master.Utilities.Services.Abstraction.Serializing;

public interface IExcelSerialize
{
    byte[] ToExcelBytes<T>(List<T> source, string sheet = "default");
    DataTable ExcelToDataTable(byte[] source);
    List<T> ExcelBytesToList<T>(byte[] source);
}