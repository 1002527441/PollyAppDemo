using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollyClientApp.Policies
{
    public class ClientPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> ImmdiateHttpRetry { get; }

        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }


        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; }

        public ClientPolicy()
        {
            ImmdiateHttpRetry = Policy
                .HandleResult<HttpResponseMessage>
                (res => !res.IsSuccessStatusCode)
                .RetryAsync(5);


            LinearHttpRetry = Policy
              .HandleResult<HttpResponseMessage>
              (res => !res.IsSuccessStatusCode)
              .WaitAndRetryAsync(5, retryAttempt=>TimeSpan.FromSeconds(3));


            ExponentialHttpRetry = Policy
            .HandleResult<HttpResponseMessage>
            (res => !res.IsSuccessStatusCode)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));


        }
    }
}
