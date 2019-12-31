using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorTile
{
    public class Util
    {
        /// <summary>
        /// 瓦片转经度
        /// </summary>
        /// <param name="x">列号</param>
        /// <param name="z">缩放级别</param>
        /// <returns></returns>
        public static double  tile2lon(int x, int z)
        {
            return x / Math.Pow(2.0, z) * 360.0 - 180;
        }
        /// <summary>
        /// 瓦片转纬度
        /// </summary>
        /// <param name="y">行号</param>
        /// <param name="z">缩放级别</param>
        /// <returns></returns>
        public static double tile2lat(int y, int z)
        {
            double n = Math.PI - (2.0 * Math.PI * y) / Math.Pow(2.0, z);
            return Math.Atan(Math.Sinh(n))*180/Math.PI;
        }
        /// <summary>
        /// 获取经度最大值和经度最小值
        /// </summary>
        /// <param name="x">列号</param>
        /// <param name="z">缩放级别</param>
        /// <returns></returns>
        public  static List<double> getLon(int x, int z)
        {
            List<double> lonExtent = new List<double>();
            lonExtent.Add(tile2lon(x, z));
            lonExtent.Add(tile2lon(x + 1, z));
            return lonExtent;
        }
        /// <summary>
        /// 获取纬度最大值和经度最小值
        /// </summary>
        /// <param name="y">列号</param>
        /// <param name="z">缩放级别</param>
        /// <returns></returns>
        public static List<double> getLat(int y, int z)
        {
            List<double> latExtent = new List<double>();
            latExtent.Add(tile2lat(y, z));
            latExtent.Add(tile2lat(y + 1, z));
            return latExtent;
        }

    }
}
