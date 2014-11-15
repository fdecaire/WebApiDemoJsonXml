using System.Configuration;
using System.IO;
using System.Net;
using log4net;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Xml.Serialization;

namespace WebApiRetreiver
{
	public class WebApiRetriever
	{
		private readonly string apiURLLocation = ConfigurationManager.AppSettings["ApiURLLocation"];
		private readonly string apiAuthorization = ConfigurationManager.AppSettings["ApiCredential"];

		public void JSONRetriever()
		{
			var serializer = new JsonSerializer();

			var apiRequest = new ApiRequest
			{
				StoreId = 1,
				ProductId = { 2, 3, 4 }
			};

			var request = (HttpWebRequest)WebRequest.Create(apiURLLocation + "");
			request.ContentType = "application/json; charset=utf-8";
			request.Accept = "application/json";
			request.Method = "POST";
			request.Headers.Add(HttpRequestHeader.Authorization, apiAuthorization);
			request.UserAgent = "ApiRequest";

			//Writes the ApiRequest Json object to request 
			using (var streamWriter = new StreamWriter(request.GetRequestStream()))
			{
				streamWriter.Write(JsonConvert.SerializeObject(apiRequest));
				streamWriter.Flush();
			}

			var httpResponse = (HttpWebResponse)request.GetResponse();

			using (var streamreader = new StreamReader(httpResponse.GetResponseStream()))
			using (var reader = new JsonTextReader(streamreader))
			{
				var storeInventory = serializer.Deserialize<ApiResponse>(reader);
			}
		}

		public void XMLRetriever()
		{
			var xmlSerializer = new XmlSerializer(typeof(ApiResponse));

			var apiRequest = new ApiRequest
			{
				StoreId = 1,
				ProductId = { 2, 3, 4 }
			};

			var request = (HttpWebRequest)WebRequest.Create(apiURLLocation + "");
			request.ContentType = "application/json; charset=utf-8";
			request.Accept = "text/xml; charset=utf-8";
			request.Method = "POST";
			request.Headers.Add(HttpRequestHeader.Authorization, apiAuthorization);
			request.UserAgent = "ApiRequest";

			//Writes the ApiRequest Json object to request 
			using (var streamWriter = new StreamWriter(request.GetRequestStream()))
			{
				streamWriter.Write(JsonConvert.SerializeObject(apiRequest));
				streamWriter.Flush();
			}

			var httpResponse = (HttpWebResponse)request.GetResponse();

			// receives xml data and deserializes it.
			using (var streamreader = new StreamReader(httpResponse.GetResponseStream()))
			{
				var storeInventory = (ApiResponse)xmlSerializer.Deserialize(streamreader);
			}
		}
	}
}
