using Application.Dependencies.Services;
using Application.UseCases.Commands.Seeding.Dtos;
using ExcelDataReader;
using Infrastructure.Helpers;
using System.Runtime.CompilerServices;

namespace Infrastructure.ApplicationDependencies.Services;

public sealed class ExcelCustomersReader : IExcelCustomersReader
{
    public async IAsyncEnumerable<List<CustomerDto>> ReadCustomersDataAsync(string filePath, int batchSize, [EnumeratorCancellation] CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var batch = new List<CustomerDto>(batchSize);
        var isTitleSkip = false;

        while (reader.Read())
        {
            token.ThrowIfCancellationRequested();

            if (!isTitleSkip)
            {
                isTitleSkip = true;
                continue;
            }

            var customer = new CustomerDto
            {
                CardCode = reader.GetValue(0)?.ToString(),

                LastName = RowDataParsingHelper.NormalizeName(reader.GetValue(1)?.ToString()),
                FirstName = RowDataParsingHelper.NormalizeName(reader.GetValue(2)?.ToString()),
                SurName = RowDataParsingHelper.NormalizeName(reader.GetValue(3)?.ToString()),

                Gender = RowDataParsingHelper.ParseGender(reader.GetValue(6)?.ToString()),
                Birthday = RowDataParsingHelper.ParseDate(reader.GetValue(7)?.ToString()),
                City = RowDataParsingHelper.NormalizeCity(reader.GetValue(8)?.ToString()),

                Contacts = new ContactsDto
                {
                    Email = reader.GetValue(5)?.ToString(),
                    PhoneMobile = RowDataParsingHelper.ParsePhoneMobile(reader.GetValue(4)?.ToString())
                },

                FinancialProfile = new FinancialProfileDto
                {
                    Pincode = reader.GetValue(9)?.ToString(),
                    Bonus = RowDataParsingHelper.ParseDecimal(reader.GetValue(10)?.ToString()),
                    Turnover = RowDataParsingHelper.ParseDecimal(reader.GetValue(11)?.ToString())
                }
            };

            token.ThrowIfCancellationRequested();

            batch.Add(customer);

            if (batch.Count >= batchSize)
            {
                await Task.Yield();
                yield return batch;
                batch = new List<CustomerDto>(batchSize);
            }
        }

        if (batch.Count > 0)
        {
            yield return batch;
            await Task.Yield();
        }
    }
}
