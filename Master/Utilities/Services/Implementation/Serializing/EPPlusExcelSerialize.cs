namespace Master.Utilities.Services.Implementation.Serializing;

using System.Data;
using Extentions;
using Abstraction.Serializing;

public class EPPlusExcelSerialize : IExcelSerialize
{
    public EPPlusExcelSerialize() { }

    public DataTable ExcelToDataTable(byte[] source) =>
        source.ExcelToDataTable();

    public byte[] ToExcelBytes<T>(List<T> source, string sheet = "default") =>
        source.ToExcelByteArray<T>();

    public List<T> ExcelBytesToList<T>(byte[] source) =>
         ExcelToDataTable(source).ToList<T>();
}
