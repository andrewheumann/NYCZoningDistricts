//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.1.4.0 (Newtonsoft.Json v12.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------
using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Elements.Geometry.Solids;
using Elements.Properties;
using Elements.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using Line = Elements.Geometry.Line;
using Polygon = Elements.Geometry.Polygon;

namespace Elements
{
    #pragma warning disable // Disable all warnings

    /// <summary>Represents the geometry of a zoning district</summary>
    [Newtonsoft.Json.JsonConverter(typeof(Elements.Serialization.JSON.JsonInheritanceConverter), "discriminator")]
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.4.0 (Newtonsoft.Json v12.0.0.0)")]
    [UserElement]
	public partial class ZoningDistrict : Element
    {
        [Newtonsoft.Json.JsonConstructor]
        public ZoningDistrict(string @zone_Name, Polygon @boundary, System.Guid @id, string @name)
            : base(id, name)
        {
            var validator = Validator.Instance.GetFirstValidatorForType<ZoningDistrict>();
            if(validator != null)
            {
                validator.PreConstruct(new object[]{ @zone_Name, @boundary, @id, @name});
            }
        
            this.Zone_Name = @zone_Name;
            this.Boundary = @boundary;
        
            if(validator != null)
            {
                validator.PostConstruct(this);
            }
        }
    
        /// <summary>The name of the Zone</summary>
        [Newtonsoft.Json.JsonProperty("Zone Name", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Zone_Name { get; set; }
    
        /// <summary>The boundary of the zone, encoded in Lat/Long coordinates</summary>
        [Newtonsoft.Json.JsonProperty("Boundary", Required = Newtonsoft.Json.Required.AllowNull)]
        public Polygon Boundary { get; set; }
    
    
    }
}