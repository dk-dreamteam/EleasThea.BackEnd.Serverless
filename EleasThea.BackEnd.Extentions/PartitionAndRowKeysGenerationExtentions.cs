using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EleasThea.BackEnd.Extentions
{
    public static class PartitionAndRowKeysGenerationExtentions
    {
        /// <summary>
        /// Generate unique partition and row keys. If preset partition or row keys are provided, there are going to be used.
        /// </summary>
        /// <param name="tableEntity">TableEntity derived class object to assign partition and row key.</param>
        /// <param name="presetPartitionKey">Optional preset PartitionKey to add. Default is Guid.NewGuid().</param>
        /// <param name="presetRowKey">Optional preset RowKey to add. Default is Guid.NewGuid().</param>
        /// <returns>TableEntity derived class object with Partition and Row keys.</returns>
        public static TableEntity GeneratePartitionAndRowKeys(this TableEntity tableEntity, string presetPartitionKey = null, string presetRowKey = null)
        {
            var newGuid = Guid.NewGuid();
            tableEntity.PartitionKey = presetPartitionKey ?? newGuid.ToString();
            tableEntity.RowKey = presetRowKey ?? newGuid.ToString();
            return tableEntity;
        }
    }
}
