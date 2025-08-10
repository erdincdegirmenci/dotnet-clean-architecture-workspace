using Template.Domain.Interfaces;

namespace Template.Infrastructure
{
    public interface IService
    {
        /// <summary>
        /// Servisi database managera eklemek ve çağrılan metodların veritabanı seviyesinde transaction bütünlüğünde çalıştırılmasını sağlar 
        /// </summary>
        /// <param name="databaseManager">Veritabanı işlemini yapacak database manager objesi</param>
        void AddToExternalTransaction(IDatabaseManager databaseManager);
    }
}
