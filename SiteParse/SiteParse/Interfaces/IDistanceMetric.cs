namespace SiteParse.Interfaces
{
    public interface IDistanceMetric
    {
        double GetDistance(object[] set1, object[] set2);
    }
}
