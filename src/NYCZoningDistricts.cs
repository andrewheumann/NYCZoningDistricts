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

            var json = File.ReadAllText("../../NycZoningDistricts.json");
            var zoningInfo = JsonConvert.DeserializeObject<ZoningInfo>(json);

            return new NYCZoningDistrictsOutputs();
        }
    }
}