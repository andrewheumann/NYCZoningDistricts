using System;
using System.Collections.Generic;
using Elements.Geometry;
using Newtonsoft.Json;
public class ZoningInfo
{
    List<ZoningDistrictRaw> ZoningDistricts { get; set; }
}

public class ZoningDistrictRaw
{
    public string ZoningDist { get; set; }

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

    private void GenerateBoundary()
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < VerticesRaw.Count; i += 2)
        {
            points.Add(new Vector3(VerticesRaw[i], VerticesRaw[i + 1]));
        }
        _boundary = new Polygon(points);
    }
}