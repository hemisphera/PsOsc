namespace Hsp.PsOsc.Extensibility
{
  
  public interface IRegion
  {

    string Name { get; }

    int? Id { get; }

    float StartTime { get; }

    float? Duration { get; }

  }

}