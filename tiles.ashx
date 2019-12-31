using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

namespace VectorTile
{
    /// <summary>
    /// tiles 的摘要说明
    /// </summary>
    public class tiles : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/octet-stream";
            if (context.Request.HttpMethod.ToLower() == "get")
            {
                if (context.Request.QueryString["m"] == "getVectorTile")
                {
                    context.Response.BinaryWrite(getVectorTile(context));
                }
            }
        }
        public byte[] getVectorTile(HttpContext context)
        {
            int x = Convert.ToInt32( context.Request["x"]);
            int y = Convert.ToInt32(context.Request["y"]);
            int z = Convert.ToInt32(context.Request["z"]);
            List<double> lons = Util.getLon(x, z);
            List<double> lats = Util.getLat(y, z);
            double lonmin = lons[0];
            double lonmax = lons[1];
            double latmin = lats[1];
            double latmax = lats[0];

            string sql = "SELECT ST_AsMVT(tile, 'lines', 4096, 'geom') tile FROM(SELECT fs_name, ST_AsMVTGeom(geom, ST_Transform(ST_MakeEnvelope(:lonmin,:latmin,:lonmax,:latmax, 4326),3857),4096, 256, true) AS geom FROM  public.pipesectionmpa_4326_3857 ) AS tile";
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter(":lonmin", lonmin),
                new NpgsqlParameter(":lonmax", lonmax),
                new NpgsqlParameter(":latmin", latmin),
                new NpgsqlParameter(":latmax", latmax)
            };
           DataTable dataTable= PgsqlHelper.ExecuteQuery(sql, parameters);
           byte[] result = (Byte[])dataTable.Rows[0]["tile"];
            return result;
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
