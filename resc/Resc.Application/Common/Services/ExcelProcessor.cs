using EnumsNET;
using OfficeOpenXml;
using Resc.Application.Applications.Dtos.Search;
using Resc.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Resc.Application.Common.Services
{
    public class ExcelProcessor : IExcelProcessor
    {
        private readonly IEnumUtility utility;

        public ExcelProcessor(IEnumUtility utility)
        {
            this.utility = utility;
        }
        public MemoryStream ExportReports<T, TResult>(SearchReportFilter filter, IEnumerable<T> list, params Expression<Func<T, TResult>>[] expr)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                var headers = new List<string>();
                var memberExpressions = new List<MemberExpression>();
                GetHeadersAndMembers(out headers, out memberExpressions, expr);

                bool[] isFormatedMaxCols = new bool[headers.Count];

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("GeneratedWorksheet");

                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells[1, 1].Value = "СПРАВКА ЗА УЧЕБНАТА " + filter.SchoolYearName + " ГОДИНА";
                worksheet.Cells[1, 1].Style.Font.Bold = true;

                worksheet.Cells["A2:E2"].Merge = true;
                worksheet.Cells[2, 1].Value = "Вид справка: " + filter.ReportType.AsString(EnumFormat.Description);

                worksheet.Cells["A3:E3"].Merge = true;
                worksheet.Cells[3, 1].Value = filter.ResearchAreaName != null ? "Професионално направление: " + filter.ResearchAreaName : "Професионално направление: Всички";

                worksheet.Cells["A4:E4"].Merge = true;
                worksheet.Cells[4, 1].Value = filter.SpecialityName != null ? "Специалност: " + filter.SpecialityName : "Специалност: Всички";

                worksheet.Cells["A5:E5"].Merge = true;
                worksheet.Cells[5, 1].Value = filter.InstitutionName != null ? "Висше училище: " + filter.InstitutionName : "Висше училище: Всички";

                worksheet.Cells["A6:E6"].Merge = true;
                worksheet.Cells[6, 1].Value = filter.EducationalQualificationName != null ? "ОКС: " + filter.EducationalQualificationName : "ОКС: Всички";

                worksheet.Cells["A7:E7"].Merge = true;
                worksheet.Cells[7, 1].Value = "Дата на справката: " + filter.CreatedReportDate + "ч.";

                worksheet.Cells["A8:E8"].Merge = true;

                int col = 1, row = 9;
                foreach (var header in headers)
                {
                    worksheet.Cells[row, col].Value = header;
                    worksheet.Cells[row, col].Style.Font.Bold = true;
                    col++;
                }

                foreach (var item in list)
                {
                    col = 1;
                    row++;
                    foreach (var memberExpression in memberExpressions)
                    {
                        object value = null;
                        if (memberExpression.Expression.Type == typeof(T))
                        {
                            value = item.GetType().GetProperty(memberExpression.Member.Name).GetValue(item, null);
                        }
                        else
                        {
                            var resultValue = GetNestedProperties(item, memberExpression.Expression.ToString().Substring(2));
                            if (resultValue == null)
                            {
                                value = null;
                            }
                            else
                            {
                                value = resultValue.GetType().GetProperty(memberExpression.Member.Name).GetValue(resultValue, null);
                            }
                        }

                        if (value != null
                            && value.GetType().BaseType == typeof(Enum))
                        {
                            worksheet.Cells[row, col].Value = utility.GetDescription(value);
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = value;
                        }

                        var fieldType = memberExpression.Type;
                        if (fieldType == typeof(Boolean)
                            || fieldType == typeof(Boolean?))
                        {
                            var obj = (bool)worksheet.Cells[row, col].Value;
                            string boolValue = "Не";
                            if (obj)
                            {
                                boolValue = "Да";
                            }
                            worksheet.Cells[row, col].Value = boolValue;
                        }

                        worksheet.Cells[row, col].Style.Numberformat.Format = GetCellFormatting(fieldType);

                        if (!isFormatedMaxCols[col - 1]
                            && worksheet.Cells[row, col].Value != null)
                        {
                            int cellSize = worksheet.Cells[row, col].Value.ToString().Length;
                            if (cellSize > 80)
                            {
                                worksheet.Column(col).Width = 80;
                                worksheet.Column(col).Style.WrapText = true;
                                isFormatedMaxCols[col - 1] = true;
                            }
                        }
                        col++;
                    }

                }

                for (int i = 0; i <= headers.Count - 1; i++)
                {
                    if (!isFormatedMaxCols[i])
                    {
                        worksheet.Column(i + 1).AutoFit();
                    }
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }

        private object GetNestedProperties(object original, string properties)
        {

            string[] namesOfProperties = properties.Split('.');
            int size = namesOfProperties.Length - 1;

            PropertyInfo property = original.GetType().GetProperty(namesOfProperties[0]);
            object propValue = property.GetValue(original, null);

            for (int i = 1; i <= size; i++)
            {
                property = propValue.GetType().GetProperty(namesOfProperties[i]);
                propValue = property.GetValue(propValue, null);
            }

            return propValue;
        }

        private void GetHeadersAndMembers<T, TResult>(out List<string> headers, out List<MemberExpression> memberExpressions, params Expression<Func<T, TResult>>[] expressions)
        {
            headers = new List<string>();
            memberExpressions = new List<MemberExpression>();

            foreach (var item in expressions)
            {
                var expression = item.Body as MemberInitExpression;
                var bindings = expression.Bindings;

                foreach (var binding in bindings)
                {
                    dynamic obj = binding;

                    var member = obj.Expression as MemberExpression;
                    var unary = obj.Expression as UnaryExpression;
                    var result = member ?? (unary != null ? unary.Operand as MemberExpression : null);

                    if (result == null)
                    {
                        headers.Add(obj.Expression.Value);
                    }
                    else
                    {
                        memberExpressions.Add(result);
                    }
                }
            }
        }

        private string GetCellFormatting(Type fieldType)
        {
            if (fieldType == typeof(DateTime)
                || fieldType == typeof(DateTime?))
            {
                return "dd-mm-yyyy";
            }
            else if (fieldType == typeof(Double)
                || fieldType == typeof(Double?)
                || fieldType == typeof(Decimal)
                || fieldType == typeof(Decimal?))
            {
                return "0.00";
            }

            return null;
        }
    }
}
