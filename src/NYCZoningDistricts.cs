using Elements;
using Elements.Geometry;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;
using Elements.Geometry.Solids;
using System.Reflection;

namespace NYCZoningDistricts
{
    public static class NYCZoningDistricts
    {
        /// <summary>
        /// The NYCZoningDistricts function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A NYCZoningDistrictsOutputs instance containing computed results and the model with any new elements.</returns>
        public static NYCZoningDistrictsOutputs Execute(Dictionary<string, Model> inputModels, NYCZoningDistrictsInputs input)
        {
            inputModels.TryGetValue("location", out Model originModel);
            if (originModel == null)
            {
                throw new Exception("No location model present.");
            }
            var origin = originModel.AllElementsOfType<Origin>().FirstOrDefault();
            if (origin == null)
            {
                var allKnownTypes = originModel.Elements.Values.Select(e => e.GetType().ToString()).Distinct();
                var joined = String.Join(", ", allKnownTypes);
                throw new Exception($"Unable to locate the site origin. Keys are {String.Join(", ", inputModels.Keys)}. " +
                $"There are {originModel.Elements.Count} elements. The only types found in the location were: {joined}");
            }


            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("ZoningDistricts.json"));

            string json = "";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            //var json = File.ReadAllText("ZoningDistricts.json");
            var zoningInfo = JsonConvert.DeserializeObject<ZoningInfo>(json);
            var originAsPoint = new Vector3(origin.Position.Longitude * 1000.0, origin.Position.Latitude * 1000.0);
            Console.WriteLine(originAsPoint);
            var model = new Model();
            int i = 0;
            foreach (var boundarySet in zoningInfo)
            {
                foreach (var boundary in boundarySet.Boundaries)
                {
                    var crv = boundary.Boundary;
                    if (crv == null) continue;
                    if (crv.Contains(originAsPoint))
                    {
                        var worldCrv = boundary.GetWorldSpaceBoundary(origin);
                        var regionLamina = new Lamina(worldCrv, false);
                        var rep = new Representation(new SolidOperation[] { regionLamina });
                        var mat = new Material("Zoning Region", Guid.NewGuid());
                        var zoneFirstChar = boundarySet.ZoningDist.First().ToString();
                        mat.Color = ZoningInfo.ZoneColorCodes.ContainsKey(zoneFirstChar) ? ZoningInfo.ZoneColorCodes[zoneFirstChar] : new Color(0.7, 0.2, 1.0, 0.3);
                        Console.WriteLine(boundarySet.ZoningDist);
                        model.AddElement(new ModelCurve(worldCrv));
                        model.AddElement(new ZoningDistrict(boundarySet.ZoningDist, worldCrv, origin.Position.Latitude, origin.Position.Longitude, new Transform(0, 0, 5), mat, rep, false, Guid.NewGuid(), ""));
                    }

                }
                i++;
            }
            var outputs = new NYCZoningDistrictsOutputs();
            outputs.model = model;
            return outputs;
        }
    }
}