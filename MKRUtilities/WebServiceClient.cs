using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MKRUtilities
{
    public class WebServiceClient
    {
        private readonly HttpClient client;

        public WebServiceClient(string proxyUri = null)
        {
            var httpClientHandler = new HttpClientHandler();
            if (!string.IsNullOrEmpty(proxyUri))
            {
                httpClientHandler.Proxy = new WebProxy(proxyUri, false);
                httpClientHandler.UseProxy = true;
            }

            client = new HttpClient(httpClientHandler)
            {
                Timeout = TimeSpan.FromSeconds(15) // 설정 가능한 타임아웃
            };
        }
        public string CallWebServiceSync(string requestUri, string proxyUri, string jsonContent)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                if (!string.IsNullOrEmpty(proxyUri))
                {
                    httpClientHandler.Proxy = new WebProxy(proxyUri, false);
                    httpClientHandler.UseProxy = true;
                }

                using (var client = new HttpClient(httpClientHandler))
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
                    {
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };

                    var response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();

                    return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
        }


        public string CallWebService(string requestUri, string jsonContent)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };

                var response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode(); // 상태 코드가 200-299가 아니면 예외를 던집니다.

                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (HttpRequestException e)
            {
                // HTTP 요청 관련 오류 처리
                //Console.Error.WriteLine($"Error sending request: {e.Message}");
                return null; // 또는 적절한 오류 메시지 또는 코드 반환
            }
            catch (TaskCanceledException e)
            {
                // 타임아웃 오류 처리
                //Console.Error.WriteLine("Request timed out.");
                return null; // 또는 적절한 오류 메시지 또는 코드 반환
            }
            catch (Exception e)
            {
                // 기타 예외 처리
                //Console.Error.WriteLine($"Unexpected error: {e.Message}");
                return null; // 또는 적절한 오류 메시지 또는 코드 반환
            }
        }
        public string CallWebService_OCR(string requestUri, string jsonContent, string OCR_Key)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };
                if (!string.IsNullOrEmpty(OCR_Key))
                {
                    requestMessage.Headers.Add("X-OCR-SECRET", OCR_Key);
                }
                else
                {
                    throw new Exception();
                }
                var response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode(); // 상태 코드가 200-299가 아니면 예외를 던집니다.

                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (HttpRequestException e)
            {
                // HTTP 요청 관련 오류 처리
                //Console.Error.WriteLine($"Error sending request: {e.Message}");
                return null; // 또는 적절한 오류 메시지 또는 코드 반환
            }
            catch (TaskCanceledException e)
            {
                // 타임아웃 오류 처리
                //Console.Error.WriteLine("Request timed out.");
                return null; // 또는 적절한 오류 메시지 또는 코드 반환
            }
            catch (Exception e)
            {
                // 기타 예외 처리
                //Console.Error.WriteLine($"Unexpected error: {e.Message}");
                return null; // 또는 적절한 오류 메시지 또는 코드 반환
            }
        }
    }
}
