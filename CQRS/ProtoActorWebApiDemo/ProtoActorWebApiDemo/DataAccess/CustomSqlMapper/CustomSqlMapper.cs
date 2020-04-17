using Dapper;
using ProtoActorWebApiDemo.Domain.Model.Dto;
using System;
using System.Linq;
using System.Reflection;

namespace ProtoActorWebApiDemo.DataAccess.CustomSqlMapper
{
    public class CustomSqlMapper : ICustomSqlMapper
    {
        public CustomSqlMapper() 
        {
            RegisterDapperDtoMap();
        }
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
