using CsvHelper;
using CsvHelper.Configuration;
using MeterReadings.Repositories;

namespace MeterReadings.Database.SeedData
{
    public static class AccountsDatabaseSeed
    {
        private static readonly string PathToSeedData = "Database/SeedData/Test_Accounts.csv";

        public static void SeedDatabase(IRepository<Account> database)
        {
            using var reader = new StreamReader(Path.Join(AppDomain.CurrentDomain.BaseDirectory, PathToSeedData));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<AccountMap>();
            var records = csv.GetRecords<Account>();

            foreach (var record in records)
            {
                database.Add(record);
            }
        }

        private class AccountMap : ClassMap<Account>
        {
            public AccountMap()
            {
                Map(m => m.Id).Name("AccountId");
                Map(m => m.FirstName).Name("FirstName");
                Map(m => m.LastName).Name("LastName");
            }
        }
    }
}
