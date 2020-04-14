using Dapper;
using DapperExamples.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DapperExamples.Mapper
{
    public class CustomMapper
    {
        public void RegisterDapperDtoMap()
        {
            //Register all Data Access Object classes here
            SqlMapper.SetTypeMap(typeof(Beer), new CustomPropertyTypeMap(
                typeof(Beer), (type, columnName) =>
                    type.GetProperties().FirstOrDefault(property =>
                        GetDescriptionFromAttribute(property) == columnName
                    )
                )
            );

            SqlMapper.SetTypeMap(typeof(Brewery), new CustomPropertyTypeMap(
                typeof(Brewery), (type, columnName) =>
                    type.GetProperties().FirstOrDefault(property =>
                        GetDescriptionFromAttribute(property) == columnName
                    )
                )
            );
        }

        private static string GetDescriptionFromAttribute(PropertyInfo propertyInfo)
        {
            return !(Attribute.GetCustomAttribute(propertyInfo, typeof(MapToAttribute), false) is MapToAttribute attribute) ? propertyInfo.Name : attribute.Column;
        }
    }
}
