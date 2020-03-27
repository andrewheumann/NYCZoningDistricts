using Elements;
using Elements.Geometry;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;

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
                throw new Exception("Unable to locate the site origin.");
            }

            var json = File.ReadAllText("ZoningDistricts.json");
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
                        Console.WriteLine(boundarySet.ZoningDist);
                        model.AddElement(new ModelCurve(boundary.GetWorldSpaceBoundary(origin)));
                        model.AddElement(new ZoningDistrict(boundarySet.ZoningDist, crv, Guid.NewGuid(), ""));
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