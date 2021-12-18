using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Aguacongas.DynamicConfiguration.Formatters
{
    /// <summary>
    /// Formatter that allows content of type text/plain and application/octet stream
    /// or no content type to be parsed to raw data. Allows for a single input parameter
    /// in the form of:
    /// 
    /// public string RawString([FromBody] string data)
    /// public byte[] RawData([FromBody] byte[] data)
    /// </summary>
    public class RawRequestBodyFormatter : InputFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public const string CONTENTTYPE = "text/plain";

        /// <summary>
        /// Initialize a new instance of <see cref="RawRequestBodyFormatter"/>
        /// </summary>
        public RawRequestBodyFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(CONTENTTYPE));
        }


        /// <summary>
        /// Allow text/plain, application/octet-stream and no content type to
        /// be processed
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanRead(InputFormatterContext context)
        => context?.HttpContext?.Request?.ContentType?.Contains(CONTENTTYPE) == true;

        /// <summary>
        /// Handle text/plain or no content type for string results
        /// Handle application/octet-stream for byte[] results
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;

            using var reader = new StreamReader(request.Body);
            var content = await reader.ReadToEndAsync().ConfigureAwait(false);
            return await InputFormatterResult.SuccessAsync(content).ConfigureAwait(false);
        }
    }
}
