using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsEntityGenerator
{
    class Program
    {
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
                            foreach (string key in result.Attributes.Keys)
                            {
                                Console.WriteLine("Key={0} Value={1}", key, result.Attributes[key]);
                            }
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
