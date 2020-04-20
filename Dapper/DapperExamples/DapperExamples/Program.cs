using Dapper;
using DapperExamples.Dto;
using DapperExamples.Mapper;
using DapperExamples.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperExamples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Setup - Register the Dapper SqlMapper
            Console.WriteLine("Setting up...");
            CustomMapper customMapper = new CustomMapper();
            customMapper.RegisterDapperDtoMap();

            DataAccessService dataAccessService = new DataAccessService();

            //Example #1 - Async execution without return recordset 
            Console.WriteLine("Example #1: Running queries without return recordset");
            
            //Initialize the database using this example, which the data will be used for the rest of this demo
            await dataAccessService.ExecuteAsync("DROP TABLE IF EXISTS beer");
            await dataAccessService.ExecuteAsync("DROP TABLE IF EXISTS brewery");
            await dataAccessService.ExecuteAsync("CREATE TABLE beer(Name VARCHAR(50), Company VARCHAR(50), Style VARCHAR(50))");
            await dataAccessService.ExecuteAsync("CREATE TABLE brewery(Name VARCHAR(50), Country VARCHAR(50), CEO VARCHAR(50), Year_Est INT)");
            await dataAccessService.ExecuteAsync("INSERT INTO beer VALUES('Miller Lite', 'Molson Coors Beverage Company', 'Pale Lager')");
            await dataAccessService.ExecuteAsync("INSERT INTO beer VALUES('Corona Extra', 'AB InBev', 'Pale Lager')");
            await dataAccessService.ExecuteAsync("INSERT INTO beer VALUES('Somersby cider', 'Carlsberg A/S', 'Cider')");
            await dataAccessService.ExecuteAsync("INSERT INTO beer VALUES('Heineken Lager Beer', 'Heineken N.V', 'Pale Lager')");
            await dataAccessService.ExecuteAsync("INSERT INTO brewery VALUES('AB InBev', 'Belgium', 'Carlos Brito', 2008)");
            await dataAccessService.ExecuteAsync("INSERT INTO brewery VALUES('Heineken N.V', 'Netherlands', 'Jean-François van Boxmeer', 1864)");
            await dataAccessService.ExecuteAsync("INSERT INTO brewery VALUES('Carlsberg A/S', 'Denmark', 'Cees ''t Hart', 1847)");
            await dataAccessService.ExecuteAsync("INSERT INTO brewery VALUES('Molson Coors Beverage Company', 'USA', 'Gavin Hattersley', 2005)");

            //Example #2 - Async execution with return recordset mapped to a class
            Console.WriteLine("\n\n\n\n\n\n\n\n");
            Console.WriteLine("Example #2 - Async execution with return recordset mapped to a class");
            var example2Result = await dataAccessService.QueryAsync<Beer>("Select * from beer");
            if (example2Result.IsSuccess)
            {
                var result = example2Result.Result;
                result.ForEach(x => Console.WriteLine("Example 2 - Retrieved Beer: [" + "Name: " + x.Name + " | Company: " + x.Company + " | Style: " + x.Style + "]"));
            }

            /*
             * Example #3 - Async execution with return recordset mapped to a class 
             * This example shows how object is mapped to class when database column name is different than variable name in the class
             * Example uses brewery class, which has the variable YearEstablished, which is mapped to database column name Year_Est
             * 
             * This is accomplished by:
             * 1) Creating a custom Attribute class, see MapToAttribute.cs
             * 2) Init the mapping via CustomMapper.cs, which uses Dapper SqlMapper. Class variable will be scanned for MapTo attribute, 
             * which can override the default variable name when it is used to map against the database column
             */
            Console.WriteLine("\n\n\n\n\n\n\n\n");
            Console.WriteLine("Example #3 - Async execution with return recordset mapped to a class ");
            var example3Result = await dataAccessService.QueryAsync<Brewery>("Select * from brewery");
            if (example3Result.IsSuccess)
            {
                var result = example3Result.Result;
                result.ForEach(x => Console.WriteLine("Example 3 - Retrieved Beer: [" + "Name: " + x.Name + " | Country: " + x.Country
                    + " | CEO: " + x.Ceo + " | Year Established: " + x.YearEstablished + "]"));
            }

            //Example #4 - Async execution of query with parameter returning recordset mapped to a class
            Console.WriteLine("\n\n\n\n\n\n\n\n");
            Console.WriteLine("Example #4 - Async execution of query with parameter returning recordset mapped to a class");
            var example4parametersSet1 = new DynamicParameters();
            example4parametersSet1.Add("Style", "Cider");
            var example4Result = await dataAccessService.QueryAsync<Beer>("Select * from beer where Style = @Style", false, example4parametersSet1); //condition equals

            var example4parametersSet2 = new DynamicParameters();
            example4parametersSet2.Add("Name", "%er%");
            var example4Result2 = await dataAccessService.QueryAsync<Brewery>("Select * from brewery where Name like @Name", false, example4parametersSet2); //condition like

            if (example4Result.IsSuccess)
            {
                var result = example4Result.Result;
                result.ForEach(x => Console.WriteLine("Example 4 (Select Query; Condition equal) - Retrieved Beer: [" + "Name: " + x.Name + " | Company: " + x.Company + " | Style: " + x.Style + "]"));
            }
            if (example4Result2.IsSuccess)
            {
                var result = example4Result2.Result;
                result.ForEach(x => Console.WriteLine("Example 4 (Select Query; Condition like) - Retrieved Brewery: [" + "Name: " + x.Name + " | Country: " + x.Country
                    + " | CEO: " + x.Ceo + " | Year Established: " + x.YearEstablished + "]"));
            }

            //Example #5 - Async execution of query which returns multiple record set mapped to a tuple of classes
            Console.WriteLine("\n\n\n\n\n\n\n\n");
            Console.WriteLine("//Example #5 - Async execution of query which returns multiple record set mapped to a tuple of classes");
            var example5Result = await dataAccessService.QueryMultipleAsync<Beer, Brewery>("Select * from beer; Select * from brewery");
            if (example5Result.IsSuccess)
            {
                List<Beer> beerList = example5Result.ListT1;
                List<Brewery> breweryList = example5Result.ListT2;
                Console.WriteLine("Expanding the first record set - beer:");
                beerList.ForEach(x => Console.WriteLine("Example 5 - Retrieved Beer: [" + "Name: " + x.Name + " | Company: " + x.Company + " | Style: " + x.Style + "]"));
                Console.WriteLine("Expanding the second record set - brewery:");
                breweryList.ForEach(x => Console.WriteLine("Example 5 - Retrieved Brewery: [" + "Name: " + x.Name + " | Country: " + x.Country
                    + " | CEO: " + x.Ceo + " | Year Established: " + x.YearEstablished + "]"));
            }
			
			//Example #6 - Async execution of query which returns multiple record set mapped to a tuple of classes (with empty resultset)
            Console.WriteLine("\n\n\n\n\n\n\n\n");
            Console.WriteLine("//Example #6 - Async execution of query which returns multiple record set mapped to a tuple of classes (with empty resultset)");
            var example6Result = await dataAccessService.QueryMultipleAsync<Beer, Brewery, Brewery>("Select * from beer; Select * from brewery where 1=2; Select * from brewery");
            if (example6Result.IsSuccess)
            {
                List<Beer> beerList = example6Result.ListT1;
                List<Brewery> breweryList = example6Result.ListT2;
                List<Brewery> breweryList2 = example6Result.ListT3;
                Console.WriteLine("Expanding the first record set - beer:");
                beerList.ForEach(x => Console.WriteLine("Example 6 - Retrieved Beer: [" + "Name: " + x.Name + " | Company: " + x.Company + " | Style: " + x.Style + "]"));

                Console.WriteLine("Expanding the second record set (This should be an empty record set) - brewery:");
                breweryList.ForEach(x => Console.WriteLine("Example 6 - Retrieved Brewery: [" + "Name: " + x.Name + " | Country: " + x.Country
                    + " | CEO: " + x.Ceo + " | Year Established: " + x.YearEstablished + "]"));

                Console.WriteLine("Expanding the third record set - brewery:");
                breweryList2.ForEach(x => Console.WriteLine("Example 6 - Retrieved Brewery: [" + "Name: " + x.Name + " | Country: " + x.Country
                    + " | CEO: " + x.Ceo + " | Year Established: " + x.YearEstablished + "]"));
            }
        }
    }
}
