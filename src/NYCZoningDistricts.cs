using Elements;
using Elements.Geometry;
using System.Collections.Generic;

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
            /// Your code here.
            var height = 1.0;
            var volume = input.Length * input.Width * height;
            var output = new NYCZoningDistrictsOutputs(volume);
            var rectangle = Polygon.Rectangle(input.Length, input.Width);
            var mass = new Mass(rectangle, height);
            output.model.AddElement(mass);
            return output;
        }
      }
}