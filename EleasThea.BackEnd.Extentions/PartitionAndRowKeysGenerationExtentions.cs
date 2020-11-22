using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EleasThea.BackEnd.Extentions
{
    public static class PartitionAndRowKeysGenerationExtentions
    {
        /// <summary>
        /// Generate partition and row keys.
        /// </summary>
        /// <param name="tableEntity">TableEntity derived class object to assign partition and row key.</param>
        /// <param name="partitionKey">Optional PartitionKey to add. Default is Guid.NewGuid().</param>
        /// <param name="rowKey">Optional RowKey to add. Default is Guid.NewGuid().</param>
        /// <returns>TableEntity derived class object with Partition and Row keys.</returns>
        public static TableEntity GeneratePartitionAndRowKeys(this TableEntity tableEntity, string partitionKey = null, string rowKey = null)
        {
            var newGuid = Guid.NewGuid();
            tableEntity.PartitionKey = partitionKey ?? newGuid.ToString();
            tableEntity.RowKey = rowKey ?? newGuid.ToString();
            return tableEntity;
        }

        /// <summary>
        /// Generate partition and row keys.
        /// </summary>
        /// <param name="tableEntity">TableEntity derived class object to assign partition and row key.</param>
        /// <param name="commonKey">Key to be used as PartionKey and RowKey</param>
        /// <returns>TableEntity derived class object with Partition and Row keys.</returns>
        public static TableEntity GeneratePartitionAndRowKeys(this TableEntity tableEntity, string commonKey)
        {
            tableEntity.PartitionKey = commonKey;
            tableEntity.RowKey = commonKey;
            return tableEntity;
        }
    }
}
