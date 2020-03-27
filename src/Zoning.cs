using System;
using System.Collections.Generic;
using System.Linq;
using Elements;
using Elements.Geometry;
using Newtonsoft.Json;
namespace NYCZoningDistricts
{
    public class ZoningInfo : List<ZoningDistrictRaw>
    {

    }

    public class ZoningDistrictRaw
    {
        public string ZoningDist { get; set; }

       public List<ZoningBoundary> Boundaries { get; set; }

       public bool Contains(Vector3 coords) {
           return false; // TODO
       }
    }

    public class ZoningBoundary
    {
        [JsonProperty("Vertices")]
        public List<double> VerticesRaw { get; set; }

        private Polygon _boundary = null;
        public Polygon Boundary
        {
            get
            {
                if (_boundary != null) return _boundary;
                GenerateBoundary();
                return _boundary;
            }
        }
        private const double _earthRadius = 6378137;
        private const double _originShift = 2 * Math.PI * _earthRadius / 2;
        internal static Vector3 LatLonToMeters(double lat, double lon)
        {
            var posx = lon * _originShift / 180.0;
            var posy = Math.Log(Math.Tan((90.0 + lat) * Math.PI / 360.0)) / (Math.PI / 180.0);
            posy = posy * _originShift / 180.0;
            return new Vector3(posx, posy);
        }

        public Polygon GetWorldSpaceBoundary(Origin origin)
        {
            var worldOrigin = LatLonToMeters(origin.Position.Latitude, origin.Position.Longitude);
            return new Polygon(Boundary.Vertices.Select(v => LatLonToMeters(v.Y/ 1000.0, v.X / 1000.0) - worldOrigin).ToList());
        }

        private void GenerateBoundary()
        {
            var scaleXform = new Transform();
            List<Vector3> points = new List<Vector3>();
            try
            {
                for (int i = 0; i < VerticesRaw.Count; i += 2)
                {
                    points.Add(new Vector3(VerticesRaw[i], VerticesRaw[i + 1]));
                }
                _boundary = new Polygon(points.ToList());

                _boundary.Transform(scaleXform);

            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
            }
        }
    }
}