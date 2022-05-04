using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using MonolitoApi.Controllers;
using Microsoft.Extensions.Options;
using FluentAssertions;

namespace MonolitoApi.Tests;

public class PersonControllerTest
{
    private MonolitoDbContext _DbContext;
    private int _InstancesCounter = 0;
    
    public PersonControllerTest()
    {
        var dbContextOption = new Microsoft.EntityFrameworkCore.DbContextOptions<MonolitoDbContext>();
        var options = Options.Create<Settings.DbContextConnection>(new Settings.DbContextConnection() {ConnectionString = "server=localhost;database=db;user=user;password=password"});
        _DbContext = new MonolitoDbContext(dbContextOption, options);
        _InstancesCounter++;
    }

    [Fact]
    public async void Test1()
    {
        //Arrange
        var person = new PersonController(_DbContext);

        //Ac
        var result = await person.GetPerson();

        //Assert
        result.Should().NotBeNull();
        Assert.True(result.Value.Any(), "El arregle no contiene información");

        // Assert.Null(result);
        // Assert.True(true);
    }

    // [Fact]
    // public async void CreatePersonMethodPost()
    // {
    //     var person = new PersonController(_DbContext);
    //     var newPerson = new Models.Person {
    //         FirtName = "Juan",
    //         LastName = "Diaz"
    //         };

    //     var resultAction = await person.PostPerson(newPerson);
    //     var result = resultAction.Result as Microsoft.AspNetCore.Mvc.CreatedAtActionResult;

    //     result.Should().NotBeNull();
    //     result?.StatusCode.Should().BeGreaterThanOrEqualTo(200);
    //     result?.StatusCode.Should().BeLessThan(300);
    // }

    [Fact]
    public async void FindAllPersonMethodGet()
    {
        Assert.True(true);
    }

    [Fact]
    public async void FindOnePersonMethodGet()
    {
        Assert.True(true);
    } 

    [Fact]
    public async void UpdatePersonMethodPut()
    {
        Assert.Equal(1, _InstancesCounter);
        Assert.True(true);
    } 

    [Fact]
    public async void DeletePersonMethodDelete()
    {
        Assert.Equal(1, _InstancesCounter);
        Assert.True(true);
    }

}