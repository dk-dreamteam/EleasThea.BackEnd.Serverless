using Microsoft.WindowsAzure.Storage.Table;

namespace EleasThea.BackEnd.Contracts.TableStorageModels
{
    public class Feedback : TableEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Message { get; set; }
    }
}
