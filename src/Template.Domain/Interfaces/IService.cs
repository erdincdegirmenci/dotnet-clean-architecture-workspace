using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Interfaces
{
    /// <summary>
    /// Business katmanında servis sınıflarının implement edeceği Interface
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Servisi database managera eklemek ve çağrılan metodların veritabanı seviyesinde transaction bütünlüğünde çalıştırılmasını sağlar 
        /// </summary>
        /// <param name="databaseManager">Veritabanı işlemini yapacak database manager objesi</param>
        void AddToExternalTransaction(IDatabaseManager databaseManager);
    }
}
