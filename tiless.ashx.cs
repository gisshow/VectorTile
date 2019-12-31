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
    /// tiless 的摘要说明
    /// </summary>
    public class tiless : IHttpHandler
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
                else if (context.Request.QueryString["m"] == "getVectorTilePolygon")
                {
                    context.Response.BinaryWrite(getVectorTilePolygon(context));

                }
            }
        }
        /// <summary>
        /// 面
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public byte[] getVectorTilePolygon(HttpContext context)
        {
            int x = Convert.ToInt32(context.Request["x"]);
            int y = Convert.ToInt32(context.Request["y"]);
            int z = Convert.ToInt32(context.Request["z"]);
            List<double> lons = Util.getLon(x, z);
            List<double> lats = Util.getLat(y, z);
            double lonmin = lons[0];
            double lonmax = lons[1];
            double latmin = lats[1];
            double latmax = lats[0];

            // string sql = "SELECT ST_AsMVT(tile, 'lines', 4096, 'geom') AS tile FROM(SELECT  w.gid,      ST_AsMVTGeom(w.geom, ST_Transform(ST_MakeEnvelope(:lonmin,:latmin,:lonmax,:latmax, 4326),3857), 4096, 256, true) AS geom FROM  public.gis_osm_railways_free w )   tile WHERE tile.geom IS NOT NULL";
            // string sql = " SELECT ST_AsMVT(tile, 'polygons', 4096, 'geom') AS tile FROM(SELECT w.gid, ST_AsMVTGeom(w.geom,        Box2D(ST_MakeEnvelope(:lonmin,:latmin,:lonmax,:latmax, 4326)     ), 4096, 0,   true) AS geom FROM gis_osm_buildings_a_free_1 w) tile WHERE tile.geom IS NOT NULL";

            /// 
            string sql = " SELECT ST_AsMVT(tile, 'polygons', 4096, 'geom') AS tile FROM(SELECT w.name, ST_AsMVTGeom(w.geom,        Box2D(ST_MakeEnvelope(:lonmin,:latmin,:lonmax,:latmax, 4326)     ), 4096, 0,   true) AS geom FROM public.\"szbud\" w) tile WHERE tile.geom IS NOT NULL";



            NpgsqlParameter[] parameters = {
                new NpgsqlParameter(":lonmin", lonmin),
                new NpgsqlParameter(":lonmax", lonmax),
                new NpgsqlParameter(":latmin", latmin),
                new NpgsqlParameter(":latmax", latmax)
            };
            DataTable dataTable = PgsqlHelper.ExecuteQuery(sql, parameters);
            byte[] result = (Byte[])dataTable.Rows[0]["tile"];
            return result;
        }

        public byte[] getVectorTile(HttpContext context)
        {
            int x = Convert.ToInt32(context.Request["x"]);
            int y = Convert.ToInt32(context.Request["y"]);
            int z = Convert.ToInt32(context.Request["z"]);
            List<double> lons = Util.getLon(x, z);
            List<double> lats = Util.getLat(y, z);
            double lonmin = lons[0];
            double lonmax = lons[1];
            double latmin = lats[1];
            double latmax = lats[0];

           // string sql = "SELECT ST_AsMVT(tile, 'lines', 4096, 'geom') AS tile FROM(SELECT  w.gid,      ST_AsMVTGeom(w.geom, ST_Transform(ST_MakeEnvelope(:lonmin,:latmin,:lonmax,:latmax, 4326),3857), 4096, 256, true) AS geom FROM  public.gis_osm_railways_free w )   tile WHERE tile.geom IS NOT NULL";
           // string sql = " SELECT ST_AsMVT(tile, 'polygons', 4096, 'geom') AS tile FROM(SELECT w.gid, ST_AsMVTGeom(w.geom,        Box2D(ST_MakeEnvelope(:lonmin,:latmin,:lonmax,:latmax, 4326)     ), 4096, 0,   true) AS geom FROM gis_osm_buildings_a_free_1 w) tile WHERE tile.geom IS NOT NULL";

            /// 
            string sql = " SELECT ST_AsMVT(tile, 'lines', 4096, 'geom') AS tile FROM(SELECT w.name, ST_AsMVTGeom(w.geom,        Box2D(ST_MakeEnvelope(:lonmin,:latmin,:lonmax,:latmax, 4326)     ), 4096, 0,   true) AS geom FROM public.\"New_Shapefile\" w) tile WHERE tile.geom IS NOT NULL";



            NpgsqlParameter[] parameters = {
                new NpgsqlParameter(":lonmin", lonmin),
                new NpgsqlParameter(":lonmax", lonmax),
                new NpgsqlParameter(":latmin", latmin),
                new NpgsqlParameter(":latmax", latmax)
            };
            DataTable dataTable = PgsqlHelper.ExecuteQuery(sql, parameters);
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