namespace SpaceCourier.Test;

public class GameEngineTests
{
    private class PirateFactory : EventFactory
    {
        public override ISpaceEvent CreateEvent() => new PirateAttackEvent();
    }

    [Fact]
    public void Player_Should_SpendFuel_When_Travelling()
    {
        // Arrange
        var start = new SafePlanet("Earth");
        var destination = new SafePlanet("Mars");
        var player = new Player(start, initialFuel: 100);
        var route = new Route(start, destination, fuelCost: 20, risk: 0);
        var engine = new GameEngine();

        // Act
        var result = engine.TryTravel(player, route);

        // Assert
        Assert.Equal(80, player.Fuel);
        Assert.Equal(destination, player.Current);
    }

    [Fact]
    public void Player_Should_LoseCargo_When_PirateAttackOccurs()
    {
        // Arrange
        var start = new SafePlanet("Earth");
        var destination = new SafePlanet("Alpha");
        var player = new Player(start, initialFuel: 100);
        var route = new Route(start, destination, fuelCost: 10, risk: 100);
        
        var eventService = new EventService(new PirateFactory());
        var engine = new GameEngine(eventService);

        // Act
        var result = engine.TryTravel(player, route);

        // Assert
        Assert.True(player.IsCargoLost);
        Assert.True(result.IsGameOver);
    }

    [Fact]
    public void Player_Should_Refuel_When_VisitingFuelStation()
    {
        // Arrange
        var start = new SafePlanet("Earth");
        var fuelStation = new FuelStationPlanet("FuelStation");
        var player = new Player(start, initialFuel: 50);
        var route = new Route(start, fuelStation, fuelCost: 10, risk: 0);
        var engine = new GameEngine();

        // Act
        engine.TryTravel(player, route);

        // Assert
        Assert.Equal(80, player.Fuel);
    }

    [Fact]
    public void DangerousPlanet_Should_DeductFuel_OnVisit()
    {
        // Arrange
        var start = new SafePlanet("Earth");
        var dangerous = new DangerousPlanet("AsteroidField", danger: 5);
        var player = new Player(dangerous, initialFuel: 100);
        var route = new Route(start, dangerous, fuelCost: 10, risk: 0);
        var engine = new GameEngine();

        // Act
        engine.TryTravel(player, route);

        // Assert
        Assert.Equal(65, player.Fuel);
    }

    [Fact]
    public void Player_Should_ReachDestination_Successfully()
    {
        // Arrange
        var start = new SafePlanet("Earth");
        var destination = new SafePlanet("Destination");
        var player = new Player(start, initialFuel: 100);
        var route = new Route(start, destination, fuelCost: 20, risk: 0);
        var engine = new GameEngine();

        // Act
        var result = engine.TryTravel(player, route);

        // Assert
        Assert.True(result.ReachedDestination);
        Assert.True(result.IsGameOver);
        Assert.Equal(destination, player.Current);
    }
}
