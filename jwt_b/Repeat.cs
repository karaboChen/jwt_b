
using Dapper;
using Microsoft.Data.SqlClient;

namespace jwt_b
{

    public class Repeat : BackgroundService
    {
        private readonly IConfiguration _configuration;

        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        public Repeat(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {

                if(i == 3)
                {
                    timer.Dispose();
                }
                else
                {
                    await DoworkAsync();
                    i ++;
                    //0 +1  =1
                    // 1+1 =2
                }


            }

        }

        private  async Task DoworkAsync()
        {
            try
            {

                using (var cn = new SqlConnection(_configuration.GetConnectionString("TodoDatabase")))
                {
                    string sql = @"insert into  [3_line] values(777,888,99)";
                    await cn.ExecuteAsync(sql);
                }
            }
            catch (Exception ex)
            {

                throw ;
            }

        }

    }
}
