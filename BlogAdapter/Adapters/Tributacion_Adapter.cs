using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using supERPro_DLL.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
//using Microsoft.Owin.Security.OAuth;
//using System.Net.Http;

namespace supERPro_DLL.Adapters
{
    public class Tributacion_Adapter
    {
        public string access_token = "";
        public string refresh_token = "";
        ArrayList laListaData;
        
        public List<Tributacion> All_Usuarios_Tributacion(double Id_TribUser)
        {
            List<Tributacion> ListaTributacion = new List<Tributacion>();
            string Procedimiento_Almacenado = "All_Usuarios_Tributacion";

            using (var LaConexion = new SqlConnection(new Connection_Adapter().ConnString))
            {
                using (SqlCommand SQL_Command = new SqlCommand(Procedimiento_Almacenado, LaConexion))
                {
                    SQL_Command.CommandType = CommandType.StoredProcedure;
                    SQL_Command.Parameters.AddWithValue("@Id_TribUser", Id_TribUser);
                    LaConexion.Open();

                    using (DbDataReader dataReader = SQL_Command.ExecuteReader())
                    {
                        if (dataReader != null)
                        {
                            while (dataReader.Read())
                            {
                                ListaTributacion.Add(new Tributacion
                                {
                                    Id_TribUser = Convert.ToDouble( dataReader["Id_TribUser"]),
                                    Username = dataReader["Username"].ToString(),
                                    Password = dataReader["Password"].ToString(),
                                    Id_Empresa = Convert.ToInt32(dataReader["Id_Empresa"])
                                });
                            }//Fin while   
                        }//Fin If
                    }
                }
            }
            return ListaTributacion;
        }//Fin All_Usuarios_Tributacion ------------------------------------------------------------

        public void Guarda_Usuarios_Tributacion(double Id_TribUser, string Username, string Password, int Id_Empresa)
        {
            string Procedimiento_Almacenado = "Guarda_Usuarios_Tributacion";

            using (var LaConexion = new SqlConnection(new Connection_Adapter().ConnString))
            {
                using (SqlCommand SQL_Command = new SqlCommand(Procedimiento_Almacenado, LaConexion))
                {
                    SQL_Command.CommandType = CommandType.StoredProcedure;
                    SQL_Command.Parameters.AddWithValue("@Id_TribUser", Id_TribUser);
                    SQL_Command.Parameters.AddWithValue("@Username", Username);
                    SQL_Command.Parameters.AddWithValue("@Password", Password);
                    SQL_Command.Parameters.AddWithValue("@Id_Empresa", Id_Empresa);
                    LaConexion.Open();
                    SQL_Command.ExecuteNonQuery();
                }
            }
        }//Fin Guarda_Usuarios_Tributacion ------------------------------------------------------------
        
        public void ObtienElToken()
        {
             Uri authorizationServerTokenIssuerUri = new Uri("https://idp.comprobanteselectronicos.go.cr/auth/realms/rut/protocol/openid-connect/token");
            string clientId = "ClientIdThatCanOnlyRead";    
            string clientSecret = "secret1";
            string scope = "scope.readaccess";

            //access token request
            string rawJwtToken = RequestTokenToAuthorizationServer(
                 authorizationServerTokenIssuerUri,
                 clientId, 
                 scope, 
                 clientSecret)
                .GetAwaiter()
                .GetResult();
        }//Fin ObtienElToken ------------------------------------------------------------

        private static async Task<string> RequestTokenToAuthorizationServer(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret)
        {
            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                HttpContent httpContent = new FormUrlEncodedContent(
                    new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("scope", scope),
                    new KeyValuePair<string, string>("client_secret", clientSecret)
                    });
                tokenRequest.Content = httpContent;
                responseMessage = await client.SendAsync(tokenRequest);
            }
            return await responseMessage.Content.ReadAsStringAsync();
        }//Fin RequestTokenToAuthorizationServer ------------------------------------------------------------

    }
}
