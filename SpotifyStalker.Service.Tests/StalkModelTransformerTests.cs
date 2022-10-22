namespace SpotifyStalker.Service.Tests;

[ExcludeFromCodeCoverage]
public class StalkModelTransformerTests : TestBase
{
    [Theory]
    [InlineData(-24, 423.5)]
    public void CalculateMarkerPosition_Expected_Behavior_For_Metric_With_Negative_Min_And_Positive_Max(
        double input,
        double expectedResult
        )
    {
        // arrange
        var metric = new Metric()
        {
            Min = -50,
            Max = 2
        };

        var sut = Fixture.Create<StalkModelTransformer>();

        // act
        var result = sut.CalculateMarkerPosition(metric, input);

        // assert
        result.Should().BeApproximately(expectedResult, 0.01);
    }
}
