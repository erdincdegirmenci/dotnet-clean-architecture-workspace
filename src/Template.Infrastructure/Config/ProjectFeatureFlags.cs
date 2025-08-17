using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Config
{
    /// <summary>
    /// Feature flag'ler ile opsiyonel modüller (Kafka, ElasticSearch) açılıp kapatılabilir.
    /// </summary>
    public class ProjectFeatureFlags
    {
        public bool UseKafka { get; set; }
        public bool UseRedis { get; set; }
        public bool UseElastic { get; set; }
        public bool UseOpenTelemetry { get; set; }
        public bool UseSwagger { get; set; }
    }
}
