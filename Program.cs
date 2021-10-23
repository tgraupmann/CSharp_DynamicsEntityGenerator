using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsEntityGenerator
{
    class Program
    {

        static void GenerateEntity(string entity, Entity result)
        {
            //var myJSON = JsonConvert.SerializeObject(result);
            foreach (var fvc in result.FormattedValues)
            {
                Console.WriteLine("Key={0} Value={1}", fvc.Key, fvc.Value);
            }


            if (string.IsNullOrEmpty(entity) || entity.Length < 2)
            {
                return;
            }
            string ucEntity = string.Format("{0}{1}", entity.Substring(0, 1).ToUpper(), entity.Substring(1));

            string path = string.Format("{0}.cs", ucEntity);
            /*
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            */
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine("public class {0}", ucEntity);
                    sw.WriteLine("{0}", "{");
                    foreach (var key in result.Attributes.Keys)
                    {
                        sw.WriteLine("\tpublic string {0} {1}", key, "{ get; set; }");
                    }
                    sw.WriteLine("{0}", "}");
                    sw.Flush();
                }
            }
        }
        static void Main(string[] args)
        {
            IOrganizationService oServiceProxy;

            string connection = string.Format("AuthType=ClientSecret;Url={0};ClientId={1};ClientSecret={2}",
                Secrets.ServiceUrl,
                Secrets.ClientId,
                Secrets.Secret);

            //Create the Dynamics 365 Connection:
            CrmServiceClient oMSCRMConn = new Microsoft.Xrm.Tooling.Connector.CrmServiceClient(connection);

            //Create the IOrganizationService:
            oServiceProxy = (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient != null ? (IOrganizationService)oMSCRMConn.OrganizationWebProxyClient : (IOrganizationService)oMSCRMConn.OrganizationServiceProxy;


            if (oServiceProxy != null)
            {
                //Get the current user ID:
                Guid userid = ((WhoAmIResponse)oServiceProxy.Execute(new WhoAmIRequest())).UserId;

                if (userid != Guid.Empty)
                {
                    Console.WriteLine("Connection Successful!");
                }

                string[] entities =
                {
                    "account",
                };
                foreach (string entity in entities)
                {
                    var query = new QueryExpression(entity);
                    query.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true);
                    query.TopCount = 1;
                    var results = oServiceProxy.RetrieveMultiple(query);
                    if (results != null)
                    {
                        foreach (var result in results.Entities)
                        {
                            GenerateEntity(entity, result);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Connection failed...");
            }
        }
    }
}
