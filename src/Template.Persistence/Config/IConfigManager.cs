using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Persistence.Config
{
    /// <summary>
    /// Uygulama konfigurasyonlarını yöneten sınıfların implemente edeceği interface
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Configurasyon bilgisini okuma işlemi yapar
        /// </summary>
        /// <param name="configName">Konfigurasyon adı bilgisi</param>
        /// <returns>Konfigurasyon verisini döner</returns>
        string GetConfig(string configName);


        string GetConfig(string configName, int tenant);
    }
}
