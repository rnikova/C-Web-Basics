using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.QueryData = new Dictionary<string, object>();
            this.FormData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private bool IsValidRequestLine(string[] requestLineParams)
        {
            if (requestLineParams.Length != 3
                || requestLineParams[2] != GlobalConstants.HttpOneProtocolFragment)
            {
                return false;
            }

            return true;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            if (string.IsNullOrEmpty(queryString) || queryParameters.Length < 1)
            {
                return false;
            }

            return true;
        }

        private IEnumerable<string> ParsePlainRequestHeaders(string[] requestLines)
        {
            for (int i = 1; i < requestLines.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(requestLines[i]))
                {
                    yield return requestLines[i];
                }
            }
        }

        private void ParseRequestMethod(string[] requestLineParams)
        {
            bool parseMethod = Enum.TryParse(requestLineParams[0], true, out HttpRequestMethod requestMethod);

            if (!parseMethod)
            {
                throw new BadRequestException(string.Format(
                    GlobalConstants.UnsupportedHttpMethodException,
                    requestLineParams[0]));
            }

            this.RequestMethod = requestMethod;
        }

        private void ParseRequestUrl(string[] requestLineParams)
        {
            this.Url = requestLineParams[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }

        private void ParseRequestHeaders(string[] requestContent)
        {
            requestContent
                .Select(rh => rh.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(kvp => this.Headers.AddHeader(new HttpHeader(kvp[0], kvp[1])));
        }

        //private void ParseCookies()
        //{

        //}

        private void ParseRequestQueryParameters()
        {
            this.Url.Split(new[] { '?', '#' })[1]
                .Split('&')
                .Select(qp => qp.Split('='))
                .ToList()
                .ForEach(kvp => this.QueryData.Add(kvp[0], kvp[1]));
        }

        private void ParseRequestFormDataParameters(string requestBody)
        {

            //TODO: Parse multiple parameters by name

            requestBody
                .Split('&')
                .Select(qp => qp.Split('='))
                .ToList()
                .ForEach(kvp => this.FormData.Add(kvp[0], kvp[1]));
        }

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameters();
            this.ParseRequestFormDataParameters(requestBody);
        }

        private void ParseRequest(string requestString)
        {
            string[] splitedRequsetContent = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            string[] requestLineParams = splitedRequsetContent[0]
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (this.IsValidRequestLine(requestLineParams))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLineParams);
            this.ParseRequestUrl(requestLineParams);
            this.ParseRequestPath();
            this.ParseRequestHeaders(this.ParsePlainRequestHeaders(splitedRequsetContent).ToArray());
            //this.ParseCookies();
            this.ParseRequestParameters(splitedRequsetContent[splitedRequsetContent.Length - 1]);
        }
    }
}
