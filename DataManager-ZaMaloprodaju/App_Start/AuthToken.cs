using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace DataManager_ZaMaloprodaju.App_Start
{
    public class AuthToken : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/token", new PathItem
            {
                post = new Operation
                {   
                    tags = new List<string> { "Auth" },
                    //What type of type of data
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    //definition of paramteres, we get boxes to fill in grant_type, username, password
                    parameters = new List<Parameter>
                    {
                        new Parameter {
                            type = "string",
                            name = "grant_type",
                            required = true,
                            @in = "formData",
                            @default = "password"
                        },
                        new Parameter {
                            type = "string",
                            name = "username",
                            required = false,
                            @in = "formData"
                        },
                        new Parameter {
                            type = "string",
                            name = "password",
                            required = false,
                            @in = "formData"
                        }
                    }
                }
            });
        }
    }
}