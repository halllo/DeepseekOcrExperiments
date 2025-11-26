namespace DeepseekOcrExperiments;

public class BoundingBox
{
    public record BoxCoordinates(int X1, int Y1, int X2, int Y2);
    
    public BoundingBox(BoxCoordinates Coordinates, string Type, string? Value = null)
    {
        this.Coordinates = Coordinates;
        this.Type = Type;
        this.Value = Value;
    }

    public BoxCoordinates Coordinates { get; }
    public string Type { get; }
    public string? Value { get; }
}
