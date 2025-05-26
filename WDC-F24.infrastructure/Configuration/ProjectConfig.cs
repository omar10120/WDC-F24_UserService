//using Microsoft.Extensions.Configuration;
//using System;

//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;



//namespace WDC_F24.infrastructure.Configuration
//{
//    public class ProjectConfig
//    {
//        private static readonly Lazy<ProjectConfig> _instance =
//           new Lazy<ProjectConfig>(() => new ProjectConfig());

//        public static ProjectConfig Instance => _instance.Value;

//        public JwtSettings JwtSettings { get; private set; }
//        public SmtpSettings SmtpSettings { get; private set; }
//        public string ConnectionString { get; private set; }
//        public string BaseUrl { get; private set; }

//        private ProjectConfig()
//        {
//            // تحديد البيئة الحالية
//            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

//            // إنشاء منشئ الإعدادات
//            var configBuilder = new ConfigurationBuilder()
//                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // الملف الأساسي
//                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true); // ملف البيئة

//            IConfiguration configuration = configBuilder.Build();

//            // تحميل الأقسام المختلفة من الإعدادات
//            JwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
//            SmtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
//            ConnectionString = configuration.GetConnectionString("DefaultConnection");
//            BaseUrl = configuration["FileSettings:BaseUrl"];
//        }
//    }
//}
